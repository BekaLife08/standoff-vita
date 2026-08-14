using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGUIMisc
	{
		public enum ToastLength
		{
			Short,
			Long
		}

		private const int SYSTEM_UI_FLAG_LAYOUT_STABLE = 256;

		private const int SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION = 512;

		private const int SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN = 1024;

		private const int SYSTEM_UI_FLAG_HIDE_NAVIGATION = 2;

		private const int SYSTEM_UI_FLAG_FULLSCREEN = 4;

		private const int SYSTEM_UI_FLAG_IMMERSIVE = 2048;

		private const int SYSTEM_UI_FLAG_IMMERSIVE_STICKY = 4096;

		private const int ImmersiveFlagNonSticky = 3846;

		private const int ImmersiveFlagSticky = 5894;

		public static void ShowToast(string text, ToastLength length = ToastLength.Short)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					using (AndroidJavaClass ajc = new AndroidJavaClass("android.widget.Toast"))
					{
						AndroidJavaObject androidJavaObject = ajc.CallStaticAJO("makeText", AGUtils.Activity, text, (int)length);
						androidJavaObject.Call("show");
					}
				});
			}
		}

		public static void EnableImmersiveMode(bool sticky = true)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				GoodiesSceneHelper.IsInImmersiveMode = true;
				int mode = (!sticky) ? 3846 : 5894;
				AGUtils.RunOnUiThread(delegate
				{
					using (AndroidJavaObject androidJavaObject = AGUtils.ActivityDecorView)
					{
						androidJavaObject.Call("setSystemUiVisibility", mode);
					}
				});
			}
		}
	}
}
