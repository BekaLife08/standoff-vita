using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class AndroidDialogUtils
	{
		private const int Theme_DeviceDefault_Dialog_Alert = 16974545;

		private const int Theme_DeviceDefault_Light_Dialog_Alert = 16974546;

		private const int Theme_Material_Dialog_Alert = 16974374;

		private const int Theme_Material_Light_Dialog_Alert = 16974394;

		private const int THEME_DEVICE_DEFAULT_DARK = 4;

		private const int THEME_DEVICE_DEFAULT_LIGHT = 5;

		private const int THEME_HOLO_DARK = 2;

		private const int THEME_HOLO_LIGHT = 3;

		private const int ThemeDefault = -1;

		public static bool IsDefault(int theme)
		{
			return theme == -1;
		}

		public static int GetDialogTheme(AGDialogTheme theme)
		{
			if (theme == AGDialogTheme.Default)
			{
				return -1;
			}
			int sDK_INT = AGDeviceInfo.SDK_INT;
			if (sDK_INT >= 21)
			{
				return (theme != 0) ? 16974374 : 16974394;
			}
			if (sDK_INT >= 14)
			{
				return (theme != 0) ? 4 : 5;
			}
			if (sDK_INT >= 11)
			{
				return (theme != 0) ? 2 : 3;
			}
			return -1;
		}

		public static void ShowWithImmersiveModeWorkaround(AndroidJavaObject dialog)
		{
			AndroidJavaObject androidJavaObject = dialog.CallAJO("getWindow");
			androidJavaObject.Call("setFlags", 8, 8);
			dialog.Call("show");
			int num = AGUtils.Window.CallInt("getSystemUiVisibility");
			androidJavaObject.CallAJO("getDecorView").Call("setSystemUiVisibility", num);
			androidJavaObject.Call("clearFlags", 8);
		}
	}
}
