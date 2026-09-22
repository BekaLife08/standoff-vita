using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class SilentFileLogger : MonoBehaviour
{
    private string logFilePath;
    private readonly Queue<string> logQueue = new Queue<string>();
    private Thread loggingThread;
    private readonly AutoResetEvent logEvent = new AutoResetEvent(false);
    private bool isRunning = true;

    void Awake()
    {
        // Не уничтожать логгер при смене сцен
        DontDestroyOnLoad(gameObject);

        logFilePath = Path.Combine(Application.persistentDataPath, "game_silent.log");

        // Запуск фонового потока для записи на диск (не нагружает главный поток игры)
        loggingThread = new Thread(WriteLogWorker)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal
        };
        loggingThread.Start();

        // Записываем заголовок
        EnqueueLog($"=== Log Started: {DateTime.Now} ===");
    }

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += OnLogReceived;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= OnLogReceived;
    }

    private void OnLogReceived(string logString, string stackTrace, LogType type)
    {
        // Фильтр: для Warning и Log пишем только сообщение, для Error/Exception — со стек-трейсом
        string logEntry;
        if (type == LogType.Exception || type == LogType.Error)
        {
            logEntry = $"[{DateTime.Now:HH:mm:ss}] [{type}] {logString}\nStackTrace:\n{stackTrace}";
        }
        else
        {
            logEntry = $"[{DateTime.Now:HH:mm:ss}] [{type}] {logString}";
        }

        EnqueueLog(logEntry);
    }

    private void EnqueueLog(string message)
    {
        lock (logQueue)
        {
            logQueue.Enqueue(message);
        }
        // Сигнализируем фоновому потоку, что появились новые данные
        logEvent.Set();
    }

    // Этот метод выполняется полностью в отдельном потоке
    private void WriteLogWorker()
    {
        using (StreamWriter writer = new StreamWriter(logFilePath, true, Encoding.UTF8))
        {
            while (isRunning)
            {
                // Ждем сигнала о новых логах, чтобы не загружать CPU в пустом цикле
                logEvent.WaitOne(1000);

                List<string> logsToWrite = null;

                lock (logQueue)
                {
                    if (logQueue.Count > 0)
                    {
                        logsToWrite = new List<string>(logQueue);
                        logQueue.Clear();
                    }
                }

                if (logsToWrite != null && logsToWrite.Count > 0)
                {
                    foreach (var log in logsToWrite)
                    {
                        writer.WriteLine(log);
                    }
                    // Моментальный сброс на диск для сохранения при фатальном краше
                    writer.Flush();
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        logEvent.Set(); // Будим поток для завершения
        if (loggingThread != null && loggingThread.IsAlive)
        {
            loggingThread.Join(500); // Даем 0.5сек на дозапись оставшихся логов
        }
    }
}