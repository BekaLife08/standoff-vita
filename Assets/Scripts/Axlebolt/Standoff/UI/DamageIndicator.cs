using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
    public class DamageIndicator : MonoBehaviour
    {
        private static DamageIndicator _instance;
        private Image _overlay;
        private Canvas _canvas;
        private float _showUntil;
        private bool _isShowing;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (_instance != null) return;
            var go = new GameObject("DamageIndicator");
            _instance = go.AddComponent<DamageIndicator>();
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            CreateOverlay();
        }

        private void OnEnable()
        {
            // HitManager may not be initialized yet, retry in Start
            Invoke(nameof(TrySubscribe), 0.5f);
        }

        private void OnDisable()
        {
        }

        private void TrySubscribe()
        {
            try
            {
                var hm = Singleton<HitManager>.Instance;
                if (hm != null)
                {
                    hm.HitEvent.AddListener(OnHit);
                }
            }
            catch { }
        }

        private void CreateOverlay()
        {
            // Create canvas if not exists
            _canvas = FindObjectOfType<Canvas>();
            if (_canvas == null)
            {
                var canvasGo = new GameObject("DamageIndicatorCanvas");
                _canvas = canvasGo.AddComponent<Canvas>();
                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _canvas.sortingOrder = 1000;
                canvasGo.AddComponent<CanvasScaler>();
                canvasGo.AddComponent<GraphicRaycaster>();
                DontDestroyOnLoad(canvasGo);
            }

            var overlayGo = new GameObject("DamageOverlay");
            overlayGo.transform.SetParent(_canvas.transform, false);
            _overlay = overlayGo.AddComponent<Image>();
            _overlay.color = new Color(1f, 0f, 0f, 0f);
            _overlay.raycastTarget = false;
            var rt = _overlay.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private void OnHit(HitEventArgs args)
        {
            if (args == null) return;
            var victim = args.Victim;
            if (victim == null) return;
            // Check if victim is local player
            bool isLocal = false;
            try
            {
                isLocal = victim.isLocal;
            }
            catch
            {
                // Fallback: compare with PhotonNetwork.player
                try { isLocal = (PhotonNetwork.player == victim); } catch { }
            }
            if (!isLocal) return;

            // Show damage indicator
            _showUntil = Time.time + 0.6f;
            _isShowing = true;
            if (_overlay != null)
            {
                _overlay.color = new Color(1f, 0f, 0f, 0.35f);
            }

            // Optional: directional indicator via rotation (if shooter available)
            try
            {
                var shooter = args.Shooter;
                if (shooter != null && !shooter.isLocal)
                {
                    // Try to get positions to compute direction
                    // Use Camera.main as victim view
                    var cam = Camera.main;
                    var victimCtrl = FindLocalPlayerController();
                    var shooterCtrl = FindPlayerController(shooter);
                    if (cam != null && victimCtrl != null && shooterCtrl != null)
                    {
                        Vector3 dir = shooterCtrl.transform.position - victimCtrl.transform.position;
                        dir.y = 0;
                        float angle = Vector3.SignedAngle(cam.transform.forward, dir, Vector3.up);
                        // Rotate overlay slightly to hint direction (optional, simple flash for now)
                        _overlay.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
                    }
                }
            }
            catch { }
        }

        private PlayerController FindLocalPlayerController()
        {
            try
            {
                var players = FindObjectsOfType<PlayerController>();
                foreach (var p in players)
                {
                    if (p.PhotonView != null && p.PhotonView.isMine) return p;
                }
            }
            catch { }
            return null;
        }

        private PlayerController FindPlayerController(PhotonPlayer player)
        {
            try
            {
                var players = FindObjectsOfType<PlayerController>();
                foreach (var p in players)
                {
                    if (p.PhotonView != null && p.PhotonView.owner == player) return p;
                }
            }
            catch { }
            return null;
        }

        private void Update()
        {
            if (!_isShowing || _overlay == null) return;
            float remaining = _showUntil - Time.time;
            if (remaining <= 0f)
            {
                _overlay.color = new Color(1f, 0f, 0f, 0f);
                _overlay.rectTransform.rotation = Quaternion.identity;
                _isShowing = false;
            }
            else
            {
                float alpha = Mathf.Clamp01(remaining / 0.6f) * 0.35f;
                var c = _overlay.color;
                c.a = alpha;
                _overlay.color = c;
            }
        }
    }
}
