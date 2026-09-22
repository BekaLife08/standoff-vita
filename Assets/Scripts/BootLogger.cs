using System;
using System.IO;
using System.Text;
using UnityEngine;

// =============================================================================
// BootLogger.cs
// 
// Early-boot logging for PS Vita silent crash diagnosis.
// Captures ALL log output (Debug.Log, Debug.LogError, exceptions) and writes
// them to persistentDataPath/boot_log.txt with synchronous file I/O so logs
// survive hard crashes.
//
// SETUP:
//   1. Place this script on an EMPTY GameObject in your first scene.
//   2. Set Script Execution Order to -1000:
//      Edit > Project Settings > Script Execution Order
//      Click "+" and add BootLogger, set to -1000 (runs before everything).
//   3. The log file will appear at:
//      ux0:data/<APP_TITLE>/boot_log.txt  (PS Vita)
//      Application.persistentDataPath/boot_log.txt (Editor/other)
//
// Compatible with: Unity 2017.4, .NET 3.5, Mono 2.0
// =============================================================================
public class BootLogger : MonoBehaviour
{
    private static BootLogger instance;
    private static string logFilePath;
    private static StringBuilder logBuffer;
    private static int stepCounter;

    // MonoBehaviour is a class, so Awake() runs on the main thread - safe for file I/O
    void Awake()
    {
        // Singleton guard - prevent duplicates across scene loads
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            // Build log file path: persistentDataPath is ux0:data/<titleid> on Vita
            logFilePath = Path.Combine(Application.persistentDataPath, "boot_log.txt");

            // Clear old log on fresh boot so each crash dump is clean
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            // Initialize buffer for batched writes
            logBuffer = new StringBuilder(4096);
            stepCounter = 0;

            // Write header
            AppendLine("========================================");
            AppendLine("BOOT LOG - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            AppendLine("========================================");

            // Log system info immediately - identifies hardware/resource limits
            LogStep("BootLogger.Awake() started");
            AppendLine("Application.persistentDataPath: " + Application.persistentDataPath);
            AppendLine("Application.dataPath: " + Application.dataPath);
            AppendLine("Application.identifier: " + Application.identifier);
            AppendLine("Application.version: " + Application.version);
            AppendLine("Application.unityVersion: " + Application.unityVersion);
            LogStep("Application info logged");

            // SystemInfo - RAM, GPU, OS
            AppendLine("--- SystemInfo ---");
            AppendLine("SystemInfo.operatingSystem: " + SystemInfo.operatingSystem);
            AppendLine("SystemInfo.systemMemorySize: " + SystemInfo.systemMemorySize + " MB");
            AppendLine("SystemInfo.graphicsMemorySize: " + SystemInfo.graphicsMemorySize + " MB");
            AppendLine("SystemInfo.processorType: " + SystemInfo.processorType);
            AppendLine("SystemInfo.processorCount: " + SystemInfo.processorCount);
            AppendLine("SystemInfo.graphicsDeviceName: " + SystemInfo.graphicsDeviceName);
            AppendLine("SystemInfo.graphicsDeviceType: " + SystemInfo.graphicsDeviceType);
            AppendLine("SystemInfo.graphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion);
            AppendLine("SystemInfo.supportsImageEffects: " + SystemInfo.supportsImageEffects);
            AppendLine("SystemInfo.maxTextureSize: " + SystemInfo.maxTextureSize);
            LogStep("SystemInfo logged");

            // RAM warning - Vita has ~280MB usable
            if (SystemInfo.systemMemorySize < 300)
            {
                AppendLine("*** WARNING: systemMemorySize < 300 MB (" + SystemInfo.systemMemorySize + " MB) ***");
                AppendLine("*** WARNING: PS Vita RAM limit ~280MB. Close to threshold! ***");
            }

            Flush();
            LogStep("BootLogger.Awake() completed");

            // Subscribe to ALL log output after initialization
            Application.logMessageReceived += OnLogMessageReceived;
            LogStep("Subscribed to logMessageReceived");
        }
        catch (Exception ex)
        {
            // Last-resort: try to write the exception itself
            try
            {
                string emergencyPath = Path.Combine(Application.persistentDataPath, "boot_log.txt");
                File.AppendAllText(emergencyPath,
                    "[BootLogger] CRITICAL: Awake() exception: " + ex.GetType().Name + ": " + ex.Message + "\n" +
                    ex.StackTrace + "\n");
            }
            catch
            {
                // If file write fails, there is nothing more we can do
            }
        }
    }

    // Called for EVERY Debug.Log, Debug.LogWarning, Debug.LogError, and unhandled exception
    void OnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (logFilePath == null) return;

        try
        {
            string prefix;
            switch (type)
            {
                case LogType.Error:
                    prefix = "[ERROR]";
                    break;
                case LogType.Exception:
                    prefix = "[EXCEPTION]";
                    break;
                case LogType.Warning:
                    prefix = "[WARN]";
                    break;
                case LogType.Assert:
                    prefix = "[ASSERT]";
                    break;
                default:
                    prefix = "[LOG]";
                    break;
            }

            AppendLine(prefix + " " + condition);

            // Full stack trace for errors and exceptions - critical for crash diagnosis
            if (type == LogType.Error || type == LogType.Exception)
            {
                if (!string.IsNullOrEmpty(stackTrace))
                {
                    AppendLine("  StackTrace: " + stackTrace);
                }
            }

            Flush();
        }
        catch
        {
            // Swallow - logging must never cause additional crashes
        }
    }

    // Call this from other scripts to mark execution milestones
    public static void LogStep(string stepName)
    {
        if (logFilePath == null) return;

        try
        {
            stepCounter++;
            string timestamp = Time.realtimeSinceStartup.ToString("F3");
            AppendLine("[STEP " + stepCounter + "] (t+" + timestamp + "s) " + stepName);
            Flush();
        }
        catch
        {
            // Swallow
        }
    }

    // Log a fatal error right before potential crash
    public static void LogFatal(string message)
    {
        if (logFilePath == null) return;

        try
        {
            AppendLine("[FATAL] " + message);
            AppendLine("[FATAL] StackTrace: " + Environment.StackTrace);
            Flush();
        }
        catch
        {
            // Swallow
        }
    }

    // Synchronous file write - ensures data hits storage before crash
    private static void AppendLine(string line)
    {
        if (logBuffer != null)
        {
            logBuffer.AppendLine(line);
        }
    }

    private static void Flush()
    {
        if (logFilePath == null || logBuffer == null || logBuffer.Length == 0) return;

        try
        {
            File.AppendAllText(logFilePath, logBuffer.ToString());
            logBuffer.Length = 0;
        }
        catch
        {
            // Swallow - file I/O can fail on Vita if card is full/disconnected
        }
    }

    // Final flush on destroy and application quit
    void OnDestroy()
    {
        LogStep("BootLogger.OnDestroy()");
        Application.logMessageReceived -= OnLogMessageReceived;
        Flush();
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            LogStep("Application paused");
            Flush();
        }
        else
        {
            LogStep("Application resumed");
            Flush();
        }
    }

    void OnApplicationFocus(bool focused)
    {
        if (focused)
        {
            LogStep("Application focused");
            Flush();
        }
    }
}
