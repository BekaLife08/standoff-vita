using System;
using System.IO;
using UnityEngine;

// =============================================================================
// VitaLogger.cs — safe crash-proof logger for PS Vita (Unity 2017.4 / .NET 3.5)
// Attach to an empty GameObject in the FIRST scene (Welcome.unity):
//   1. Hierarchy > Create Empty > name it "VitaLogger", attach this script.
//   2. Script Execution Order: set VitaLogger to -1000 (before everything).
// Log file: Path.Combine(Application.persistentDataPath, "game_log.txt")
//   (= ux0:data/<TITLE_ID>/game_log.txt on real Vita hardware)
//
// CRASH-SAFETY RULES (IL2CPP C2-12828-1):
//   - Startup uses ONLY Debug.Log (Unity native API) + File.WriteAllText /
//     File.AppendAllText wrapped in try { } catch { }.
//   - NO StreamWriter kept open (a held handle dies with the runtime and can
//     take the FS with it), NO System.Threading.Tasks, NO threads, NO queues.
//   - Every line is opened-written-closed synchronously, so whatever reached
//     the disk survives even a hard native crash.
//   - NEVER call Debug.Log from inside OnLogMessageReceived (infinite loop).
// Compatible with: Unity 2017.4, C# 4 (.NET 3.5), Mono 2.0, IL2CPP PSP2.
// =============================================================================
public class VitaLogger : MonoBehaviour
{
    private static VitaLogger instance;

    private static string logFilePath;

    void Awake()
    {
        // FIRST operation: prove the filesystem works, before anything else.
        // try/catch: persistentDataPath itself can throw on Vita (no card).
        try
        {
            logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");
            File.WriteAllText(logFilePath, "=== CRITICAL START LOG ===\n");
        }
        catch
        {
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");
                File.WriteAllText(logFilePath, "=== CRITICAL START LOG ===\n");
            }

            // Native Unity API only — safe before any subscription exists.
            Debug.Log("[VitaLogger] start. File: " + logFilePath);

            Application.logMessageReceived += OnLogMessageReceived;

            AppendLine("[LOG] VitaLogger initialized. File: " + logFilePath);
        }
        catch
        {
        }
    }

    void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        // NOTE: no Debug.Log here — it would re-enter this handler forever.
        try
        {
            string time = DateTime.Now.ToString("HH:mm:ss.fff");
            AppendLine("[" + time + "] [" + type.ToString() + "] " + logString);

            // StackTrace only for errors/exceptions — keeps the log small.
            if ((type == LogType.Error || type == LogType.Exception) && !string.IsNullOrEmpty(stackTrace))
            {
                AppendLine("StackTrace:\n" + stackTrace);
            }
        }
        catch
        {
            // Swallow: logging must never throw.
        }
    }

    /// <summary>Manual checkpoint from game code: VitaLogger.LogStep("Menu loaded").</summary>
    public static void LogStep(string message)
    {
        try
        {
            AppendLine("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] [STEP] " + message);
        }
        catch
        {
        }
    }

    /// <summary>Last-resort error line with a managed stack dump.</summary>
    public static void LogFatal(string message)
    {
        try
        {
            AppendLine("[FATAL] " + message);
            AppendLine("[FATAL] StackTrace: " + Environment.StackTrace);
        }
        catch
        {
        }
    }

    // Single write path: open-append-close per line, each in try/catch so an
    // FS error can never propagate into the IL2CPP runtime.
    private static void AppendLine(string line)
    {
        if (string.IsNullOrEmpty(logFilePath))
        {
            return;
        }
        try
        {
            File.AppendAllText(logFilePath, line + "\n");
        }
        catch
        {
        }
    }

    void OnDestroy()
    {
        try
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }
        catch
        {
        }
    }

    void OnApplicationQuit()
    {
        try
        {
            AppendLine("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] [LOG] Application quit.");
        }
        catch
        {
        }
    }
}
