using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal class GoodiesSceneHelper : MonoBehaviour
	{
		private static GoodiesSceneHelper _instance;

		private static readonly object InitLock = new object();

		private readonly object _queueLock = new object();

		private readonly List<Action> _queuedActions = new List<Action>();

		private readonly List<Action> _executingActions = new List<Action>();

		public static GoodiesSceneHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}
				return _instance;
			}
		}

		public static bool IsInImmersiveMode
		{
			private get;
			set;
		}

		public Texture2D LastTekenScreenshot
		{
			get;
			private set;
		}

		private GoodiesSceneHelper()
		{
		}

		internal static void Init()
		{
			lock (InitLock)
			{
				if (object.ReferenceEquals(_instance, null))
				{
					GoodiesSceneHelper[] array = UnityEngine.Object.FindObjectsOfType<GoodiesSceneHelper>();
					if (array.Length > 1)
					{
						UnityEngine.Debug.LogError(typeof(GoodiesSceneHelper) + " Something went really wrong  - there should never be more than 1 " + typeof(GoodiesSceneHelper) + " Reopening the scene might fix it.");
					}
					else if (array.Length == 0)
					{
						GameObject gameObject = new GameObject();
						_instance = gameObject.AddComponent<GoodiesSceneHelper>();
						gameObject.name = "GoodiesSceneHelper";
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
						UnityEngine.Debug.Log("[Singleton] An _instance of " + typeof(GoodiesSceneHelper) + " is needed in the scene, so '" + gameObject.name + "' was created with DontDestroyOnLoad.");
					}
					else
					{
						UnityEngine.Debug.Log("[Singleton] Using _instance already created: " + _instance.gameObject.name);
					}
				}
			}
		}

		internal static void Queue(Action action)
		{
			if (action == null)
			{
				UnityEngine.Debug.LogWarning("Trying to queue null action");
			}
			else
			{
				lock (_instance._queueLock)
				{
					_instance._queuedActions.Add(action);
				}
			}
		}

		private void OnApplicationFocus(bool focusStatus)
		{
			if (focusStatus && IsInImmersiveMode)
			{
				AGUIMisc.EnableImmersiveMode();
			}
		}

		private void Update()
		{
			MoveQueuedActionsToExecuting();
			while (_executingActions.Count > 0)
			{
				Action action = _executingActions[0];
				_executingActions.RemoveAt(0);
				action();
			}
		}

		private void MoveQueuedActionsToExecuting()
		{
			lock (_queueLock)
			{
				while (_queuedActions.Count > 0)
				{
					Action item = _queuedActions[0];
					_executingActions.Add(item);
					_queuedActions.RemoveAt(0);
				}
			}
		}

		public void SaveScreenshotToGallery(Action<string> onScreenSaved)
		{
			StartCoroutine(TakeScreenshot(Screen.width, Screen.height, onScreenSaved));
		}

		public IEnumerator TakeScreenshot(int width, int height, Action<string> onScreenSaved)
		{
			yield return new WaitForEndOfFrame();
			Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);
			texture.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			texture.Apply();
			LastTekenScreenshot = texture;
			string uri = AndroidPersistanceUtilsInternal.InsertImage(title: "Screenshot-" + DateTime.Now.ToString("yy-MM-dd-hh-mm-ss"), texture2D: LastTekenScreenshot, description: "My awesome screenshot");
			onScreenSaved(uri);
		}

		public void OnPickGalleryImageSuccess(string message)
		{
			AGGallery.OnSuccessTrigger(message);
		}

		public void OnPickGalleryImageError(string message)
		{
			AGGallery.OnErrorTrigger(message);
		}

		public void OnPickPhotoImageSuccess(string message)
		{
			AGCamera.OnSuccessTrigger(message);
		}

		public void OnPickPhotoImageError(string message)
		{
			AGCamera.OnErrorTrigger(message);
		}

		public void OnPickContactSuccess(string message)
		{
			AGContacts.OnSuccessTrigger(message);
		}

		public void OnPickContactError(string message)
		{
			AGContacts.OnErrorTrigger(message);
		}

		public void OnRequestPermissionsResult(string message)
		{
			AGPermissions.OnRequestPermissionsResult(message);
		}

		public void OnPickAudioSuccess(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Audio picker success: " + message);
			}
			AGFilePicker.OnAudioSuccessTrigger(message);
		}

		public void OnPickAudioError(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Audio picker error: " + message);
			}
			AGFilePicker.OnAudioErrorTrigger(message);
		}

		public void OnPickVideoSuccess(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Video picker success: " + message);
			}
			AGFilePicker.OnVideoSuccessTrigger(message);
		}

		public void OnPickVideoError(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Video picker error: " + message);
			}
			AGFilePicker.OnVideoErrorTrigger(message);
		}

		public void OnRecordVideoSuccess(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Video picker success: " + message);
			}
			AGCamera.OnVideoSuccessTrigger(message);
		}

		public void OnRecordVideoError(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("Video picker error: " + message);
			}
			AGCamera.OnVideoErrorTrigger(message);
		}

		public void OnPickFileError(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("File picker error: " + message);
			}
			AGFilePicker.OnFileErrorTrigger(message);
		}

		public void OnPickFileSuccess(string message)
		{
			if (Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("File picker success: " + message);
			}
			AGFilePicker.OnFileSuccessTrigger(message);
		}
	}
}
