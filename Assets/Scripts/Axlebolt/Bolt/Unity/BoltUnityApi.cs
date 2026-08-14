using Axlebolt.Bolt.Player;
using Axlebolt.RpcSupport;
using System;
using UnityEngine;

namespace Axlebolt.Bolt.Unity
{
	public class BoltUnityApi
	{
		private class BoltUnityBridge : MonoBehaviour, ILogStream
		{
			private readonly ILogger _logger = UnityEngine.Debug.unityLogger;

			private const string Tag = "Bolt";

			private void Awake()
			{
				Axlebolt.RpcSupport.Logger.Stream = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				InitArgs();
			}

			private static void InitArgs()
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getIntent", Array.Empty<object>());
					if (androidJavaObject.Call<bool>("hasExtra", new object[1]
					{
						"bolt"
					}))
					{
						AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getExtras", Array.Empty<object>());
						string text = androidJavaObject2.Call<string>("getString", new object[1]
						{
							"bolt"
						});
						_boltNotification = ((!string.IsNullOrEmpty(text)) ? JsonUtility.FromJson<BoltNotification>(text) : null);
					}
				}
			}

			public void Debug(object msg)
			{
				_logger.Log("Bolt", msg);
			}

			public void Error(object msg)
			{
				_logger.LogError("Bolt", msg);
			}

			private void OnApplicationPause(bool pauseStatus)
			{
				if (BoltApi.Instance.IsConnectedAndReady)
				{
					try
					{
						if (pauseStatus)
						{
							_logger.Log("SetAwayStatus");
							BoltService<BoltPlayerService>.Instance.SetAwayStatus().Wait();
						}
						else
						{
							_logger.Log("SetOnlineStatus");
							BoltService<BoltPlayerService>.Instance.SetOnlineStatus().Wait();
						}
					}
					catch (ConnectionFailedException message)
					{
						_logger.Log("Bolt", message);
					}
					catch (Exception message2)
					{
						_logger.LogError("Bolt", message2);
					}
				}
			}

			private void OnDestroy()
			{
				BoltApi.Destroy();
			}
		}

		private static BoltNotification _boltNotification;

		public static void Init()
		{
			GameObject gameObject = new GameObject("BoltUnityApi");
			gameObject.AddComponent<BoltUnityBridge>();
			BoltApi.TempPath = Application.temporaryCachePath;
			BoltApi.Init(BoltSettings.Instance.GameId, Application.version);
			BoltApi.Instance.Ip = "165.227.154.6";
		}

		public static bool IsLaunchedViaNotification()
		{
			return _boltNotification != null;
		}

		public static BoltNotification GetNotification()
		{
			BoltNotification boltNotification = _boltNotification;
			_boltNotification = null;
			return boltNotification;
		}
	}
}
