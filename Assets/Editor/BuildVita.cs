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
        bool prevBakedGI = UnityEditor.Lightmapping.bakedGI;
        UnityEditor.Lightmapping.bakedGI = false;
        string error = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
        UnityEditor.Lightmapping.bakedGI = prevBakedGI;
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
