using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// =============================================================================
// VitaOfflineBypass.cs — PS Vita offline bootstrap (Unity 2017.4 / .NET 3.5)
//
// CRASH-SAFETY RULES (IL2CPP C2-12828-1):
//   - NO [RuntimeInitializeOnLoadMethod] (fires before the engine is ready).
//   - NO System.Threading.Tasks, NO async/await, NO Task, NO Thread.
//   - Deferred work ONLY via classic Unity coroutines (IEnumerator +
//     yield return null). Plain MonoBehaviour, created late via EnsureCreated.
//   - Every external call (Photon / SceneManager / VitaLogger) in try/catch.
//
// LIFECYCLE: BoltController.Awake() (Main scene, engine fully up) calls
//   VitaOfflineBypass.EnsureCreated(), which builds the classic singleton:
//     new GameObject("VitaOfflineBypass").AddComponent<VitaOfflineBypass>()
//   Start() then launches two coroutines:
//     1. BootRoutine()   — waits 2 frames (engine settles, PhotonHandler
//                          awake), forces PhotonNetwork.offlineMode, creates
//                          the in-memory Offline Profile, fires OnUserDataLoaded.
//     2. SaveWatchdog()  — 3-second timeout: if the Main Menu still is not
//                          confirmed after 3 s, re-fires ConnectToBolt()
//                          (instant success on PSP2) and re-fires the event.
// Companion #if UNITY_PSP2 stubs (synchronous, Task-free):
//   AuthController.Authenticate -> instant CallbackResult()
//   TestAuthService             -> instant Action callback / null (unreached)
//   BoltController              -> skips BoltUnityApi.Init, instant connect
//   MainController.Init         -> PrefsStorage instead of BoltFileStorage
//   MainController.OnConnectedToBolt -> VitaOfflineBypass.NotifyMenuReady()
// Compatible with: Unity 2017.4, C# 4 (.NET 3.5), IL2CPP PSP2.
// =============================================================================
public class VitaOfflineBypass : MonoBehaviour
{
    // ---- Fake offline profile (in-memory, never touches network/disk) ----
    public static readonly string OfflinePlayerName = "VitaPlayer";
    public static readonly string OfflinePlayerId = "vita-offline-001";

    public static bool IsOfflineMode
    {
        get
        {
#if UNITY_PSP2
            return true;
#else
            return false;
#endif
        }
    }

    // Fired the moment local user data is ready (BootRoutine + watchdog).
    public static event Action OnUserDataLoaded;

    // Decoupling hook: VitaOfflineBypass NEVER references BoltController by
    // type (avoids all CS0246/CS1061 namespace issues). BoltController.Awake()
    // (PSP2) registers its own instant ConnectToBolt here; the watchdog only
    // invokes the delegate. Set to null when the controller is destroyed.
    public static Action ForceConnectAction;

    public static bool UserDataReady
    {
        get { return userDataReady; }
    }

    public static bool MenuConfirmed
    {
        get { return menuConfirmed; }
    }

    private static bool userDataReady;
    private static bool menuConfirmed;
    private static VitaOfflineBypass instance;

    private const float SAVE_TIMEOUT_SECONDS = 3f;

    // Classic singleton entry point. Safe to call from any controller Awake():
    // pure Unity API (Find / new GameObject / AddComponent), no network, no
    // file IO, no threads. Second call is a no-op returning the existing one.
    public static VitaOfflineBypass EnsureCreated()
    {
        if (instance != null)
        {
            return instance;
        }
        try
        {
            instance = FindObjectOfType<VitaOfflineBypass>();
        }
        catch
        {
            instance = null;
        }
        if (instance != null)
        {
            return instance;
        }
        GameObject go;
        try
        {
            go = new GameObject("VitaOfflineBypass");
        }
        catch
        {
            return null;
        }
        try
        {
            DontDestroyOnLoad(go);
        }
        catch
        {
        }
        try
        {
            instance = go.AddComponent<VitaOfflineBypass>();
        }
        catch
        {
            instance = null;
        }
        return instance;
    }

    // Called by MainController.OnConnectedToBolt (PSP2) once the menu is open.
    // Tells the watchdog the boot flow completed — no forcing needed.
    public static void NotifyMenuReady()
    {
        menuConfirmed = true;
    }

    void Start()
    {
        StartCoroutine(BootRoutine());
        StartCoroutine(SaveWatchdog());
    }

    // Lets the engine settle (PhotonHandler.Awake creates networkingPeer),
    // then applies offline state — coroutine-only, zero threads.
    private IEnumerator BootRoutine()
    {
        yield return null;
        yield return null;

        TryApplyPhotonOffline();
        MarkUserDataReady("BootRoutine");
    }

    // 3-second save-load timeout: if Main is active but the menu was never
    // confirmed, force the instant PSP2 connect path and re-fire the event.
    private IEnumerator SaveWatchdog()
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - start < SAVE_TIMEOUT_SECONDS)
        {
            yield return null;
        }

        if (!IsOfflineMode || menuConfirmed)
        {
            yield break;
        }

        string sceneName = string.Empty;
        try
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
        catch
        {
        }

        if (sceneName == "Main" || sceneName == string.Empty)
        {
            ForceMainMenu();
        }
    }

    private static void TryApplyPhotonOffline()
    {
#if UNITY_PSP2
        try
        {
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.offlineMode = true;
            }
            if (PhotonNetwork.offlineMode)
            {
                VitaLogger.LogStep("VitaOfflineBypass: PhotonNetwork.offlineMode = true");
            }
        }
        catch (Exception ex)
        {
            // Peer not ready yet — never crash boot over the logger path.
            try
            {
                VitaLogger.LogStep("VitaOfflineBypass: Photon offline deferred (" + ex.GetType().Name + ")");
            }
            catch
            {
            }
        }
#endif
    }

    private static void MarkUserDataReady(string from)
    {
        if (userDataReady)
        {
            return;
        }
        userDataReady = true;
        try
        {
            VitaLogger.LogStep("VitaOfflineBypass: Offline Profile ready (" + OfflinePlayerName + ") via " + from);
        }
        catch
        {
        }
        try
        {
            if (OnUserDataLoaded != null)
            {
                OnUserDataLoaded();
            }
        }
        catch
        {
        }
    }

    private void ForceMainMenu()
    {
        try
        {
            VitaLogger.LogStep("VitaOfflineBypass: 3s timeout — forcing Main Menu");
        }
        catch
        {
        }

        try
        {
            // Instant success path (registered by BoltController.Awake on
            // PSP2): hides splash, opens the menu. Fully synchronous — no
            // Task, no await, and no BoltController type reference here.
            VitaOfflineBypass.EnsureCreated();
            Action force = ForceConnectAction;
            if (force != null)
            {
                force();
            }
            else if (SceneManager.GetActiveScene().name != "Main")
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
        catch
        {
            try
            {
                if (SceneManager.GetActiveScene().name != "Main")
                {
                    SceneManager.LoadScene("Main", LoadSceneMode.Single);
                }
            }
            catch
            {
            }
        }

        userDataReady = false;
        MarkUserDataReady("Watchdog");
    }
}
