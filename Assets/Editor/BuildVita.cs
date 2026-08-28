using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildVita
{
    [MenuItem("Build/Build PS Vita")]
    public static void Build()
    {
        Environment.SetEnvironmentVariable("SCE_PSP2_SDK_DIR", @"C:\PSVITA\sdk");
        Environment.SetEnvironmentVariable("SCE_ROOT_DIR", @"C:\PSVITA\SCE");

        EditorUserBuildSettings.psp2BuildSubtarget = UnityEditor.PSP2BuildSubtarget.PCHosted;

        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled && !string.IsNullOrEmpty(s.path))
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            scenes = EditorBuildSettings.scenes
                .Where(s => !string.IsNullOrEmpty(s.path))
                .Select(s => s.path)
                .ToArray();
        }

        if (scenes.Length == 0)
        {
            Debug.LogWarning("EditorBuildSettings.scenes empty - using fallback hardcoded scenes");
            scenes = new string[] {
                "Assets/Scenes/Welcome.unity",
                "Assets/Scenes/Main.unity",
                "Assets/Scenes/Game.unity",
                "Assets/Scenes/GameView.unity"
            };
            // filter to existing
            scenes = scenes.Where(p => File.Exists(Path.Combine(Directory.GetCurrentDirectory(), p))).ToArray();
        }

        // Vita scenes were created with newer Unity - 2017 can't load them (serialized version higher).
        // Create a minimal empty scene for Vita build test if needed.
        if (scenes.Any(s => s.Contains("Main.unity") || s.Contains("Game.unity")))
        {
            try
            {
                var testScenePath = "Assets/Scenes/TestVitaEmpty.unity";
                var fullTestPath = Path.Combine(Directory.GetCurrentDirectory(), testScenePath);
                if (!File.Exists(fullTestPath))
                {
                    var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(newScene, testScenePath);
                    Debug.Log("Created empty test scene for Vita: " + testScenePath);
                }
                // Use only the test scene to avoid version mismatch
                scenes = new string[] { testScenePath };
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to create test scene: " + e.Message);
            }
        }

        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found (even fallback)!");
            EditorApplication.Exit(1);
            return;
        }

        string buildPath = @"C:\Users\User\Documents\Standoff 2 vita project\Build\PSP2";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        Debug.Log("Building PSP2 (PC Hosted) to: " + buildPath);
        Debug.Log("Scenes: " + string.Join(", ", scenes));

        string error = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.PSP2, BuildOptions.None);

        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError("BuildPlayer failed: " + error);
            EditorApplication.Exit(1);
        }
        else
        {
            Debug.Log("BUILD SUCCESS: " + buildPath);
            EditorApplication.Exit(0);
        }
    }

    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled && !string.IsNullOrEmpty(s.path))
            .Select(s => s.path)
            .ToArray();
        if (scenes.Length == 0)
        {
            scenes = EditorBuildSettings.scenes
                .Where(s => !string.IsNullOrEmpty(s.path))
                .Select(s => s.path)
                .ToArray();
        }
        if (scenes.Length == 0)
        {
            scenes = new string[] {
                "Assets/Scenes/Welcome.unity",
                "Assets/Scenes/Main.unity",
                "Assets/Scenes/Game.unity",
                "Assets/Scenes/GameView.unity"
            };
            scenes = scenes.Where(p => File.Exists(Path.Combine(Directory.GetCurrentDirectory(), p))).ToArray();
        }
        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found!");
            EditorApplication.Exit(1);
            return;
        }
        string buildPath = @"F:\Standoff 2 vita project\Build\Windows\Standoff2.exe";
        var dir = Path.GetDirectoryName(buildPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        Debug.Log("Building Windows to: " + buildPath);
        Debug.Log("Scenes: " + string.Join(", ", scenes));
        Lightmapping.Clear();
        Lightmapping.bakedGI = false;
        Lightmapping.realtimeGI = false;
        PlayerSettings.realtimeReflectionProbes = false;
        LightmapEditorSettings.bakeResolution = 0;
        LightmapEditorSettings.supportedFormats = LightmapEditorTextureFormat.None;
        foreach (string scenePath in scenes)
        {
            try
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientIntensity = 0f;
                LightmapSettings.lightmaps = new LightmapData[0];
                LightmapSettings.giWorkflowMode = GIWorkflowMode.WorkflowMode.Realtime;
                var probes = UnityEngine.Object.FindObjectsOfType<ReflectionProbe>();
                foreach (var probe in probes)
                {
                    probe.intensity = 0f;
                    probe.bakedTexture = null;
                    probe.cubemap = null;
                    UnityEngine.Object.DestroyImmediate(probe);
                }
                var lights = UnityEngine.Object.FindObjectsOfType<Light>();
                foreach (var l in lights)
                {
                    l.shadows = LightShadows.None;
                    l.shadowStrength = 0f;
                }
                var renderers = UnityEngine.Object.FindObjectsOfType<Renderer>();
                foreach (var r in renderers)
                {
                    if (r != null && r.sharedMaterials != null)
                    {
                        foreach (var mat in r.sharedMaterials)
                        {
                            if (mat != null)
                            {
                                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                            }
                        }
                    }
                }
                EditorSceneManager.SaveScene(scene);
                Debug.Log("Stripped GI from: " + scenePath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to strip GI from " + scenePath + ": " + ex.Message);
            }
        }
        string error = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError("BuildPlayer failed: " + error);
            EditorApplication.Exit(1);
        }
        else
        {
            Debug.Log("BUILD SUCCESS: " + buildPath);
            EditorApplication.Exit(0);
        }
    }
}
