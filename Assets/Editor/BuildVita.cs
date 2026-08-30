using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildVita
{
    private static readonly string TempDllDir = @"F:\Standoff 2 vita project\_psp2_dlls_temp";

    [MenuItem("Build/Build PS Vita")]
    public static void Build()
    {
        Environment.SetEnvironmentVariable("SCE_PSP2_SDK_DIR", @"F:\Standoff 2 vita project\PSVITA\sdk");
        Environment.SetEnvironmentVariable("SCE_ROOT_DIR", @"F:\Standoff 2 vita project\PSVITA\SCE");

        string pluginsDir = Path.Combine(Application.dataPath, "Plugins");

        string[] nonPsp2Dlls = new string[] {
            "Purchasing.Common.dll", "Stores.dll", "ChannelPurchase.dll", "Apple.dll",
            "FacebookStore.dll", "Security.dll", "Tizen.dll", "UnityStore.dll", "winrt.dll"
        };
        if (!Directory.Exists(TempDllDir)) Directory.CreateDirectory(TempDllDir);
        foreach (string dll in nonPsp2Dlls)
        {
            string dllPath = Path.Combine(pluginsDir, dll);
            string tempPath = Path.Combine(TempDllDir, dll);
            if (File.Exists(dllPath))
            {
                File.Copy(dllPath, tempPath, true);
                File.Delete(dllPath);
                File.Delete(dllPath + ".meta");
                Debug.Log("Moved out non-PSP2 DLL: " + dll);
            }
        }

        string scriptAssembliesDir = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Library", "ScriptAssemblies");
        if (Directory.Exists(scriptAssembliesDir))
        {
            Directory.Delete(scriptAssembliesDir, true);
            Debug.Log("Deleted ScriptAssemblies to force recompile");
        }

        string libraryPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Library");
        string libraryCache = Path.Combine(libraryPath, "metadata");
        string scriptAsmCache = Path.Combine(libraryPath, "ScriptAssemblies");

        var scenes = EditorBuildSettings.scenes
            .Where(s => !string.IsNullOrEmpty(s.path))
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found!");
            RestoreDlls(pluginsDir);
            EditorApplication.Exit(1);
            return;
        }

        string buildPath = @"F:\Standoff 2 vita project\Build\PSP2";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        PlayerSettings.productName = "Standoff Vita";

        Debug.Log("Building PSP2 to: " + buildPath);
        Debug.Log("Scenes: " + string.Join(", ", scenes));

        string error = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.PSP2, BuildOptions.None);

        RestoreDlls(pluginsDir);

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

    private static void RestoreDlls(string pluginsDir)
    {
        if (Directory.Exists(TempDllDir))
        {
            string[] files = Directory.GetFiles(TempDllDir, "*.dll");
            foreach (string file in files)
            {
                string dest = Path.Combine(pluginsDir, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }
            Directory.Delete(TempDllDir, true);
        }

        string settingsPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ProjectSettings", "ProjectSettings.asset");
        if (File.Exists(settingsPath))
        {
            string content = File.ReadAllText(settingsPath);
            if (content.Contains("PSP2_BUILD"))
            {
                content = content.Replace("PSP2_BUILD;", "").Replace(" PSP2_BUILD", "").Replace("PSP2_BUILD", "");
                File.WriteAllText(settingsPath, content);
                Debug.Log("Removed PSP2_BUILD define");
            }
        }

        AssetDatabase.Refresh();
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
        foreach (string scenePath in scenes)
        {
            try
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                var probes = UnityEngine.Object.FindObjectsOfType<ReflectionProbe>();
                foreach (var probe in probes)
                {
                    UnityEngine.Object.DestroyImmediate(probe);
                }
                var lights = UnityEngine.Object.FindObjectsOfType<Light>();
                foreach (var l in lights)
                {
                    l.shadows = LightShadows.None;
                }
                EditorSceneManager.SaveScene(scene);
                Debug.Log("Prepared scene: " + scenePath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to prepare scene " + scenePath + ": " + ex.Message);
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
