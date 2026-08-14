using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class AGUtils
	{
		private static AndroidJavaObject _activity;

		public static AndroidJavaObject Activity
		{
			get
			{
				if (_activity == null)
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					_activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
				return _activity;
			}
		}

		public static AndroidJavaObject ActivityDecorView => Window.Call<AndroidJavaObject>("getDecorView", Array.Empty<object>());

		public static AndroidJavaObject Window => Activity.Call<AndroidJavaObject>("getWindow", Array.Empty<object>());

		public static AndroidJavaObject PackageManager => Activity.CallAJO("getPackageManager");

		public static AndroidJavaObject ContentResolver => Activity.CallAJO("getContentResolver");

		public static long CurrentTimeMillis
		{
			get
			{
				using (AndroidJavaClass ajc = new AndroidJavaClass("java.lang.System"))
				{
					return ajc.CallStaticLong("currentTimeMillis");
				}
			}
		}

		public static bool HasSystemFeature(string feature)
		{
			using (AndroidJavaObject ajo = PackageManager)
			{
				return ajo.CallBool("hasSystemFeature", feature);
			}
		}

		public static AndroidJavaObject ClassForName(string className)
		{
			using (AndroidJavaClass ajc = new AndroidJavaClass("java.lang.Class"))
			{
				return ajc.CallStaticAJO("forName", className);
			}
		}

		public static AndroidJavaObject Cast(this AndroidJavaObject source, string destClass)
		{
			using (AndroidJavaObject androidJavaObject = ClassForName(destClass))
			{
				return androidJavaObject.Call<AndroidJavaObject>("cast", new object[1]
				{
					source
				});
			}
		}

		public static bool IsJavaNull(this AndroidJavaObject ajo)
		{
			return ajo.GetRawObject().ToInt32() == 0;
		}

		public static bool IsNotAndroidCheck()
		{
			bool flag = Application.platform == RuntimePlatform.Android;
			if (flag)
			{
				GoodiesSceneHelper.Init();
			}
			return !flag;
		}

		public static void RunOnUiThread(Action action)
		{
			Activity.Call("runOnUiThread", new AndroidJavaRunnable(action.Invoke));
		}

		public static void StartActivity(AndroidJavaObject intent, Action fallback = null)
		{
			try
			{
				Activity.Call("startActivity", intent);
			}
			catch (AndroidJavaException ex)
			{
				if (Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + ex.Message);
				}
				fallback?.Invoke();
			}
			finally
			{
				intent.Dispose();
			}
		}

		public static void StartActivityWithChooser(AndroidJavaObject intent, string chooserTitle)
		{
			try
			{
				AndroidJavaObject androidJavaObject = intent.CallStaticAJO("createChooser", intent, chooserTitle);
				Activity.Call("startActivity", androidJavaObject);
			}
			catch (AndroidJavaException ex)
			{
				UnityEngine.Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + ex.Message);
			}
			finally
			{
				intent.Dispose();
			}
		}

		public static void SendBroadcast(AndroidJavaObject intent)
		{
			Activity.Call("sendBroadcast", intent);
		}

		public static AndroidJavaObject GetMainActivityClass()
		{
			string applicationPackage = AGDeviceInfo.GetApplicationPackage();
			using (PackageManager)
			{
				AndroidJavaObject ajo = PackageManager.CallAJO("getLaunchIntentForPackage", applicationPackage);
				try
				{
					string className = ajo.CallAJO("getComponent").CallStr("getClassName");
					return ClassForName(className);
				}
				catch (Exception ex)
				{
					if (Debug.isDebugBuild)
					{
						UnityEngine.Debug.LogWarning("Unable to find Main Activity Class: " + ex.Message);
					}
					return null;
				}
			}
		}

		public static AndroidJavaObject NewJavaFile(string path)
		{
			return new AndroidJavaObject("java.io.File", path);
		}

		public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D tex2D, ImageFormat format = ImageFormat.PNG)
		{
			byte[] array = tex2D.Encode(format);
			return "android.graphics.BitmapFactory".AJCCallStaticOnceAJO("decodeByteArray", array, 0, array.Length);
		}

		public static Texture2D TextureFromUriInternal(string uri)
		{
			if (string.IsNullOrEmpty(uri))
			{
				return null;
			}
			using (AndroidJavaObject ajo = ContentResolver)
			{
				AndroidJavaObject androidJavaObject = AndroidUri.Parse(uri);
				try
				{
					AndroidJavaObject androidJavaObject2 = ajo.CallAJO("openInputStream", androidJavaObject);
					AndroidJavaObject bitmapAjo = "android.graphics.BitmapFactory".AJCCallStaticOnceAJO("decodeStream", androidJavaObject2);
					return Texture2DFromBitmap(bitmapAjo);
				}
				catch (Exception exception)
				{
					if (Debug.isDebugBuild)
					{
						UnityEngine.Debug.LogException(exception);
					}
					return null;
				}
			}
		}

		private static Texture2D Texture2DFromBitmap(AndroidJavaObject bitmapAjo)
		{
			AndroidJavaObject @static = new AndroidJavaClass("android.graphics.Bitmap$CompressFormat").GetStatic<AndroidJavaObject>("PNG");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.io.ByteArrayOutputStream");
			bitmapAjo.CallBool("compress", @static, 100, androidJavaObject);
			byte[] data = androidJavaObject.Call<byte[]>("toByteArray", Array.Empty<object>());
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return texture2D;
		}

		public static Texture2D Texture2DFromFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			try
			{
				AndroidJavaObject bitmap = "android.graphics.BitmapFactory".AJCCallStaticOnceAJO("decodeFile", path);
				bitmap = RotateBitmap(bitmap, path);
				return Texture2DFromBitmap(bitmap);
			}
			catch (Exception exception)
			{
				if (Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogException(exception);
				}
				return null;
			}
		}

		private static AndroidJavaObject RotateBitmap(AndroidJavaObject bitmap, string photoPath)
		{
			try
			{
				AndroidJavaObject ajo = new AndroidJavaObject("android.media.ExifInterface", photoPath);
				switch (ajo.CallInt("getAttributeInt", "Orientation", 0))
				{
				case 6:
					return RotateBitmap(bitmap, 90f);
				case 3:
					return RotateBitmap(bitmap, 180f);
				case 8:
					return RotateBitmap(bitmap, 270f);
				default:
					return bitmap;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError("Failed rotating bitmap");
				UnityEngine.Debug.LogException(exception);
				return bitmap;
			}
		}

		private static AndroidJavaObject RotateBitmap(AndroidJavaObject bitmap, float angle)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.graphics.Matrix"))
			{
				androidJavaObject.CallBool("postRotate", angle);
				return "android.graphics.Bitmap".AJCCallStaticOnceAJO("createBitmap", bitmap, 0, 0, bitmap.CallInt("getWidth"), bitmap.CallInt("getHeight"), androidJavaObject, true);
			}
		}
	}
}
