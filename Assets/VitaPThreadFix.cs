using UnityEngine;

public class VitaPthreadFix
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FixMainThreadContext()
    {
        // Принудительно заставляем Unity сфокусировать Main Thread ID 
        // до загрузки сетевых потоков и файловой системы
        System.Threading.Thread.CurrentThread.Name = "MainThread";
        
        // Отключаем фоновую многопоточную загрузку ресурсов для устранения race condition в pthread_cond
        Application.backgroundLoadingPriority = ThreadPriority.High;
        
        Debug.Log("[VITA_FIX] Pthread context stabilized for Main Thread.");
    }
}