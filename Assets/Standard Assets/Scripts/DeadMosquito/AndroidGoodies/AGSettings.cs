using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGSettings
	{
		public const string ACTION_SETTINGS = "android.settings.SETTINGS";

		public const string ACTION_ACCESSIBILITY_SETTINGS = "android.settings.ACCESSIBILITY_SETTINGS";

		public const string ACTION_ADD_ACCOUNT = "android.settings.ADD_ACCOUNT_SETTINGS";

		public const string ACTION_AIRPLANE_MODE_SETTINGS = "android.settings.AIRPLANE_MODE_SETTINGS";

		public const string ACTION_APN_SETTINGS = "android.settings.APN_SETTINGS";

		public const string ACTION_APPLICATION_DETAILS_SETTINGS = "android.settings.APPLICATION_DETAILS_SETTINGS";

		public const string ACTION_APPLICATION_DEVELOPMENT_SETTINGS = "android.settings.APPLICATION_DEVELOPMENT_SETTINGS";

		public const string ACTION_APPLICATION_SETTINGS = "android.settings.APPLICATION_SETTINGS";

		public const string ACTION_BATTERY_SAVER_SETTINGS = "android.settings.BATTERY_SAVER_SETTINGS";

		public const string ACTION_BLUETOOTH_SETTINGS = "android.settings.BLUETOOTH_SETTINGS";

		public const string ACTION_CAPTIONING_SETTINGS = "android.settings.CAPTIONING_SETTINGS";

		public const string ACTION_CAST_SETTINGS = "android.settings.CAST_SETTINGS";

		public const string ACTION_DATA_ROAMING_SETTINGS = "android.settings.DATA_ROAMING_SETTINGS";

		public const string ACTION_DATE_SETTINGS = "android.settings.DATE_SETTINGS";

		public const string ACTION_DEVICE_INFO_SETTINGS = "android.settings.DEVICE_INFO_SETTINGS";

		public const string ACTION_DISPLAY_SETTINGS = "android.settings.DISPLAY_SETTINGS";

		public const string ACTION_DREAM_SETTINGS = "android.settings.DREAM_SETTINGS";

		public const string ACTION_HOME_SETTINGS = "android.settings.HOME_SETTINGS";

		public const string ACTION_INPUT_METHOD_SETTINGS = "android.settings.INPUT_METHOD_SETTINGS";

		public const string ACTION_INPUT_METHOD_SUBTYPE_SETTINGS = "android.settings.INPUT_METHOD_SUBTYPE_SETTINGS";

		public const string ACTION_INTERNAL_STORAGE_SETTINGS = "android.settings.INTERNAL_STORAGE_SETTINGS";

		public const string ACTION_LOCALE_SETTINGS = "android.settings.LOCALE_SETTINGS";

		public const string ACTION_LOCATION_SOURCE_SETTINGS = "android.settings.LOCATION_SOURCE_SETTINGS";

		public const string ACTION_MANAGE_ALL_APPLICATIONS_SETTINGS = "android.settings.MANAGE_ALL_APPLICATIONS_SETTINGS";

		public const string ACTION_MANAGE_APPLICATIONS_SETTINGS = "android.settings.MANAGE_APPLICATIONS_SETTINGS";

		public const string ACTION_MANAGE_OVERLAY_PERMISSION = "android.settings.MANAGE_APPLICATIONS_SETTINGS";

		public const string ACTION_MANAGE_WRITE_SETTINGS = "android.settings.MANAGE_WRITE_SETTINGS";

		public const string ACTION_MEMORY_CARD_SETTINGS = "android.settings.MEMORY_CARD_SETTINGS";

		public const string ACTION_NETWORK_OPERATOR_SETTINGS = "android.settings.NETWORK_OPERATOR_SETTINGS";

		public const string ACTION_NFCSHARING_SETTINGS = "android.settings.NFCSHARING_SETTINGS";

		public const string ACTION_NFC_PAYMENT_SETTINGS = "android.settings.NFC_PAYMENT_SETTINGS";

		public const string ACTION_NFC_SETTINGS = "android.settings.NFC_SETTINGS";

		public const string ACTION_NOTIFICATION_LISTENER_SETTINGS = "android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS";

		public const string ACTION_NOTIFICATION_POLICY_ACCESS_SETTINGS = "android.settings.NOTIFICATION_POLICY_ACCESS_SETTINGS";

		public const string ACTION_PRINT_SETTINGS = "android.settings.ACTION_PRINT_SETTINGS";

		public const string ACTION_PRIVACY_SETTINGS = "android.settings.PRIVACY_SETTINGS";

		public const string ACTION_QUICK_LAUNCH_SETTINGS = "android.settings.QUICK_LAUNCH_SETTINGS";

		public const string ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS = "android.settings.REQUEST_IGNORE_BATTERY_OPTIMIZATIONS";

		public const string ACTION_SEARCH_SETTINGS = "android.settings.SEARCH_SETTINGS";

		public const string ACTION_SECURITY_SETTINGS = "android.settings.SECURITY_SETTINGS";

		public const string ACTION_SHOW_REGULATORY_INFO = "android.settings.SHOW_REGULATORY_INFO";

		public const string ACTION_SOUND_SETTINGS = "android.settings.SOUND_SETTINGS";

		public const string ACTION_SYNC_SETTINGS = "android.settings.SYNC_SETTINGS";

		public const string ACTION_USAGE_ACCESS_SETTINGS = "android.settings.USAGE_ACCESS_SETTINGS";

		public const string ACTION_USER_DICTIONARY_SETTINGS = "android.settings.USER_DICTIONARY_SETTINGS";

		public const string ACTION_VOICE_INPUT_SETTINGS = "android.settings.VOICE_INPUT_SETTINGS";

		public const string ACTION_WIFI_IP_SETTINGS = "android.settings.WIFI_IP_SETTINGS";

		public const string ACTION_WIFI_SETTINGS = "android.settings.WIFI_SETTINGS";

		public const string ACTION_WIRELESS_SETTINGS = "android.settings.WIRELESS_SETTINGS";

		public static bool CanOpenSettingsScreen(string action)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			AndroidIntent androidIntent = new AndroidIntent(action);
			return androidIntent.ResolveActivity();
		}

		public static void OpenSettings()
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				OpenSettingsScreen("android.settings.SETTINGS");
			}
		}

		public static void OpenSettingsScreen(string action)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = new AndroidIntent(action);
				if (androidIntent.ResolveActivity())
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Could not launch " + action + " settings. Check the device API level");
				}
			}
		}

		public static void OpenApplicationDetailsSettings(string package)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.settings.APPLICATION_DETAILS_SETTINGS");
				androidIntent.SetData(AndroidUri.Parse($"package:{package}"));
				if (androidIntent.ResolveActivity())
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Could not open application details settings for package " + package + ". Most likely application is not installed.");
				}
			}
		}

		public static bool CanWriteSystemSettings()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AndroidSettings.System.CanWrite();
		}

		public static void OpenModifySystemSettingsActivity(Action onFailure)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidJavaObject aJO = new AndroidIntent("android.settings.action.MANAGE_WRITE_SETTINGS").AJO;
				AGUtils.StartActivity(aJO, onFailure);
			}
		}

		public static float GetSystemScreenBrightness()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0f;
			}
			int @int = AndroidSettings.System.GetInt("screen_brightness", 1);
			return (float)@int / 255f;
		}

		public static void SetSystemScreenBrightness(float newBrightness)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				newBrightness = Mathf.Clamp01(newBrightness);
				if (!CanWriteSystemSettings())
				{
					UnityEngine.Debug.LogError("The application does not have the permission to modify system settings. Check before invoking this method by invoking 'CanWriteSystemSettings()' and use 'OpenModifySystemSettingsActivity()' to prompt the use to allow");
				}
				else
				{
					try
					{
						AndroidSettings.System.PutInt("screen_brightness_mode", 0);
						int value = (int)(newBrightness * 255f);
						AndroidSettings.System.PutInt("screen_brightness", value);
					}
					catch (Exception exception)
					{
						if (Debug.isDebugBuild)
						{
							UnityEngine.Debug.LogException(exception);
						}
					}
				}
			}
		}

		private static void SetWindowScreenBrightness(float newBrightness)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				newBrightness = Mathf.Clamp01(newBrightness);
				AndroidJavaObject window = AGUtils.Window;
				AndroidJavaObject androidJavaObject = window.CallAJO("getAttributes");
				androidJavaObject.SetStatic("screenBrightness", newBrightness);
				window.Call("setAttributes", androidJavaObject);
			}
		}
	}
}
