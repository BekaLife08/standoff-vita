using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class AndroidSettings
	{
		public static class System
		{
			public const string SCREEN_BRIGHTNESS = "screen_brightness";

			public const string SCREEN_BRIGHTNESS_MODE = "screen_brightness_mode";

			public const int SCREEN_BRIGHTNESS_MODE_MANUAL = 0;

			private static AndroidJavaClass SettingsSystemClass => new AndroidJavaClass("android.provider.Settings$System");

			public static bool CanWrite()
			{
				using (AndroidJavaClass ajc = SettingsSystemClass)
				{
					return ajc.CallStaticBool("canWrite", AGUtils.Activity);
				}
			}

			public static bool PutInt(string name, int value)
			{
				using (AndroidJavaClass ajc = SettingsSystemClass)
				{
					return ajc.CallStaticBool("putInt", AGUtils.ContentResolver, name, value);
				}
			}

			public static int GetInt(string name, int defValue)
			{
				using (AndroidJavaClass ajc = SettingsSystemClass)
				{
					return ajc.CallStaticInt("getInt", AGUtils.ContentResolver, name, defValue);
				}
			}
		}

		public const string ACTION_MANAGE_WRITE_SETTINGS = "android.settings.action.MANAGE_WRITE_SETTINGS";
	}
}
