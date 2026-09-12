using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class BuildVita
{
    private static readonly string MapFileParserSrc = @"C:\Users\Rakhy\MapFileParser.exe";
    private static readonly string MapFileParserDst = @"C:\Program Files\Unity\Editor\Data\Tools\MapFileParser\MapFileParser.exe";

    private static readonly string[] nonPsp2Dlls = new string[]
    {
        "Purchasing.Common.dll", "Stores.dll", "ChannelPurchase.dll", "Apple.dll",
        "FacebookStore.dll", "Security.dll", "Tizen.dll", "UnityStore.dll", "winrt.dll",
        "Firebase.Analytics.dll", "Firebase.App.dll", "Firebase.Messaging.dll"
    };

    private static readonly string nonPsp2DllsBackupDir = @"C:\Users\Rakhy\vitasdk_temp\non_psp2_dlls";

    private static void MoveNonPsp2Dlls()
    {
        try
        {
            Directory.CreateDirectory(nonPsp2DllsBackupDir);
            string pluginsDir = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Assets", "Plugins");

            foreach (string dllName in nonPsp2Dlls)
            {
                string dllPath = Path.Combine(pluginsDir, dllName);
                string metaPath = dllPath + ".meta";

                if (File.Exists(dllPath))
                {
                    string dest = Path.Combine(nonPsp2DllsBackupDir, dllName);
                    File.Move(dllPath, dest);
                    Debug.Log("Moved non-PSP2 DLL: " + dllName);
                }
                if (File.Exists(metaPath))
                {
                    string dest = Path.Combine(nonPsp2DllsBackupDir, dllName + ".meta");
                    File.Move(metaPath, dest);
                }
            }

            string libsDir = Path.Combine(pluginsDir, "libs");
            if (Directory.Exists(libsDir))
            {
                string[] soFiles = Directory.GetFiles(libsDir, "*.so");
                string[] soMetaFiles = Directory.GetFiles(libsDir, "*.so.meta");
                foreach (string f in soFiles)
                {
                    string dest = Path.Combine(nonPsp2DllsBackupDir, Path.GetFileName(f));
                    File.Move(f, dest);
                    Debug.Log("Moved non-PSP2 native lib: " + Path.GetFileName(f));
                }
                foreach (string f in soMetaFiles)
                {
                    string dest = Path.Combine(nonPsp2DllsBackupDir, Path.GetFileName(f));
                    File.Move(f, dest);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("MoveNonPsp2Dlls: " + ex.Message);
        }
    }

    private static void RestoreNonPsp2Dlls()
    {
        try
        {
            if (!Directory.Exists(nonPsp2DllsBackupDir)) return;
            string pluginsDir = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Assets", "Plugins");

            foreach (string file in Directory.GetFiles(nonPsp2DllsBackupDir))
            {
                string fileName = Path.GetFileName(file);
                string dest;
                if (fileName.EndsWith(".so") || fileName.EndsWith(".so.meta"))
                    dest = Path.Combine(pluginsDir, "libs", fileName);
                else
                    dest = Path.Combine(pluginsDir, fileName);

                string destDir = Path.GetDirectoryName(dest);
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                File.Move(file, dest);
            }
            Directory.Delete(nonPsp2DllsBackupDir, true);
            Debug.Log("Restored non-PSP2 DLLs");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("RestoreNonPsp2Dlls: " + ex.Message);
        }
    }

    [MenuItem("Build/Build PS Vita")]
    public static void Build()
    {
        Environment.SetEnvironmentVariable("SCE_PSP2_SDK_DIR", @"C:\StandoffProj\PSVITA\sdk");
        Environment.SetEnvironmentVariable("SCE_ROOT_DIR", @"C:\StandoffProj\PSVITA\SCE");

        EnsureMapFileParser();

        string il2cppCacheDir = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Library", "il2cpp_cache");
        if (Directory.Exists(il2cppCacheDir))
        {
            Debug.Log("Reusing existing il2cpp_cache (not deleting)");
        }

        MoveNonPsp2Dlls();

        var scenes = EditorBuildSettings.scenes
            .Where(s => !string.IsNullOrEmpty(s.path))
            .Select(s => s.path)
            .ToArray();

        string buildPath = @"C:\StandoffProj\Build\PSP2";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        PlayerSettings.productName = "Standoff Vita";

        Debug.Log("Building PSP2 to: " + buildPath);
        Debug.Log("Scenes: " + string.Join(", ", scenes));

        string error = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.PSP2, BuildOptions.None);

        RestoreNonPsp2Dlls();

        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError("BuildPlayer failed: " + error);
            EditorApplication.Exit(1);
        }
        else
        {
            Debug.Log("BUILD SUCCESS: " + buildPath);
            FixParamSfo(buildPath);
            CopyLiveAreaAssets(buildPath);
            EditorApplication.Exit(0);
        }
    }

    private static void EnsureMapFileParser()
    {
        try
        {
            if (!File.Exists(MapFileParserSrc))
            {
                Debug.LogError("MapFileParser.exe not found at " + MapFileParserSrc);
                return;
            }
            if (!File.Exists(MapFileParserDst))
            {
                Debug.LogError("MapFileParser.exe target not found at " + MapFileParserDst);
                return;
            }

            var srcInfo = new FileInfo(MapFileParserSrc);
            var dstInfo = new FileInfo(MapFileParserDst);

            if (srcInfo.Length == dstInfo.Length)
            {
                Debug.Log("MapFileParser.exe is already correct (" + dstInfo.Length + " bytes)");
                return;
            }

            if (dstInfo.Length == 4096)
            {
                string batPath = @"C:\Users\Rakhy\fix_mapfileparser.bat";
                if (!File.Exists(batPath))
                {
                    string batContent =
                        "@echo off\r\n" +
                        "net session >nul 2>&1\r\n" +
                        "if %errorlevel% == 0 goto :fix\r\n" +
                        "echo Requesting Administrator privileges...\r\n" +
                        "powershell -Command \"Start-Process '%~f0' -Verb RunAs\"\r\n" +
                        "exit /b\r\n" +
                        ":fix\r\n" +
                        "echo Replacing MapFileParser.exe...\r\n" +
                        "copy /Y \"" + MapFileParserSrc + "\" \"" + MapFileParserDst + "\"\r\n" +
                        "if %errorlevel% == 0 (\r\n" +
                        "  echo SUCCESS! MapFileParser.exe replaced.\r\n" +
                        ") else (\r\n" +
                        "  echo FAILED to replace MapFileParser.exe!\r\n" +
                        ")\r\n" +
                        "echo.\r\n" +
                        "pause\r\n";
                    File.WriteAllText(batPath, batContent);
                }

                Debug.LogError("=== MapFileParser.exe IS BROKEN (" + dstInfo.Length + " bytes) ===");
                Debug.LogError("=== Without a working MapFileParser, IL2CPP produces dummy output ===");
                Debug.LogError("=== Game code will be MISSING from the build ===");
                Debug.LogError("=== FIX: Double-click 'C:\\Users\\Rakhy\\fix_mapfileparser.bat' as Administrator ===");
                Debug.LogError("=== Then run Build/Build PS Vita again ===");
                EditorUtility.DisplayDialog(
                    "MapFileParser.exe Needs Replacement",
                    "MapFileParser.exe is broken (" + dstInfo.Length + " bytes).\n\n" +
                    "This causes IL2CPP to produce dummy output — all game code will be missing.\n\n" +
                    "FIX: Double-click this file as Administrator:\n" +
                    batPath + "\n\n" +
                    "Then run Build/Build PS Vita again.",
                    "OK");
                EditorApplication.Exit(1);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("EnsureMapFileParser: " + ex.Message);
        }
    }

    private static void CopyLiveAreaAssets(string buildPath)
    {
        try
        {
            string liveAreaSrc = Path.Combine(Path.GetDirectoryName(Application.dataPath), "LiveAreaAssets");
            string liveAreaDst = Path.Combine(buildPath, "sce_sys", "livearea", "contents");
            if (!Directory.Exists(liveAreaSrc))
            {
                Debug.LogWarning("LiveAreaAssets not found at " + liveAreaSrc);
                return;
            }
            Directory.CreateDirectory(liveAreaDst);
            foreach (string srcFile in Directory.GetFiles(liveAreaSrc))
            {
                string dstFile = Path.Combine(liveAreaDst, Path.GetFileName(srcFile));
                File.Copy(srcFile, dstFile, true);
                Debug.Log("Copied livearea: " + Path.GetFileName(srcFile));
            }
            Debug.Log("LiveArea assets copied to " + liveAreaDst);
        }
        catch (Exception ex)
        {
            Debug.LogError("CopyLiveAreaAssets failed: " + ex.Message);
        }
    }

    private static void FixParamSfo(string buildPath)
    {
        try
        {
            string sfxContent =
                "sfxParams[CATEGORY] = gd\n" +
                "sfxParams[VERSION] = 01.00\n" +
                "sfxParams[APP_VER] = 01.00\n" +
                "sfxParams[CONTENT_ID] = IV0000-STDF00002_00-0123456789ABCDEF\n" +
                "sfxParams[TITLE_ID] = STDF00002\n" +
                "sfxParams[TITLE] = Standoff Vita\n" +
                "sfxParams[STITLE] = Standoff2 Vita\n" +
                "sfxParams[SAVEDATA_MAX_SIZE] = 10240\n" +
                "sfxParams[PARENTAL_LEVEL] = 1\n" +
                "sfxParams[ATTRIBUTE] = 0\n" +
                "sfxParams[ATTRIBUTE_MINOR] = 2\n" +
                "sfxParams[ATTRIBUTE2] = 0\n";

            string sfxPath = Path.Combine(Path.GetTempPath(), "vita_fix_params.sfx");
            string sfoPath = Path.Combine(Path.GetTempPath(), "vita_fix_param.sfo");
            File.WriteAllText(sfxPath, sfxContent);

            string psp2PubCmd = @"C:\StandoffProj\PSVITA\SCE\PSP2\Tools\Publishing Tools\bin\psp2pubcmd.exe";

            if (!File.Exists(psp2PubCmd)) { Debug.LogWarning("psp2pubcmd not found"); return; }

            ProcessStartInfo psi1 = new ProcessStartInfo
            {
                FileName = psp2PubCmd,
                Arguments = "-sc \"" + sfxPath + "\" \"" + sfoPath + "\"",
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            Process p1 = Process.Start(psi1);
            string stderr1 = p1.StandardError.ReadToEnd();
            p1.WaitForExit();
            Debug.Log("SFO generation: " + stderr1);

            if (!File.Exists(sfoPath)) { Debug.LogError("Failed to generate param.sfo"); return; }
            byte[] correctSfo = File.ReadAllBytes(sfoPath);
            Debug.Log("Generated correct param.sfo: " + correctSfo.Length + " bytes");

            string destSfo = Path.Combine(buildPath, "sce_sys", "param.sfo");
            File.WriteAllBytes(destSfo, correctSfo);
            Debug.Log("param.sfo written to: " + destSfo + " (" + correctSfo.Length + " bytes)");

            string pkgFile = Path.Combine(buildPath, "app.psvita");
            if (!File.Exists(pkgFile))
            {
                pkgFile = Directory.GetFiles(buildPath, "*.pkg").FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(pkgFile) && File.Exists(pkgFile))
            {
                string fixSfo = @"C:\Users\Rakhy\FixSfo.exe";
                if (File.Exists(fixSfo))
                {
                    ProcessStartInfo psi2 = new ProcessStartInfo
                    {
                        FileName = fixSfo,
                        Arguments = "\"" + pkgFile + "\" \"" + sfoPath + "\"",
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    Process p2 = Process.Start(psi2);
                    string stderr2 = p2.StandardError.ReadToEnd();
                    p2.WaitForExit();
                    Debug.Log("FixSfo: " + stderr2);
                }
            }
            Debug.Log("Param.sfo fixed!");
        }
        catch (Exception ex)
        {
            Debug.LogError("FixParamSfo failed: " + ex.Message);
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
        string buildPath = @"C:\StandoffProj\Build\Windows\Standoff2.exe";
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
