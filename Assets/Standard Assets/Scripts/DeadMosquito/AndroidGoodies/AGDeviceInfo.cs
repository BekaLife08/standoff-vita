using DeadMosquito.AndroidGoodies.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGDeviceInfo
	{
		public static class VersionCodes
		{
			public const int BASE = 1;

			public const int BASE_1_1 = 2;

			public const int CUPCAKE = 3;

			public const int CUR_DEVELOPMENT = 10000;

			public const int DONUT = 4;

			public const int ECLAIR = 5;

			public const int ECLAIR_0_1 = 6;

			public const int ECLAIR_MR1 = 7;

			public const int FROYO = 8;

			public const int GINGERBREAD = 9;

			public const int GINGERBREAD_MR1 = 10;

			public const int HONEYCOMB = 11;

			public const int HONEYCOMB_MR1 = 12;

			public const int HONEYCOMB_MR2 = 13;

			public const int ICE_CREAM_SANDWICH = 14;

			public const int ICE_CREAM_SANDWICH_MR1 = 15;

			public const int JELLY_BEAN = 16;

			public const int JELLY_BEAN_MR1 = 17;

			public const int JELLY_BEAN_MR2 = 18;

			public const int KITKAT = 19;

			public const int KITKAT_WATCH = 20;

			public const int LOLLIPOP = 21;

			public const int LOLLIPOP_MR1 = 22;

			public const int M = 23;

			public const int N = 24;

			public const int N_MR1 = 25;

			public const int O = 26;
		}

		public static class SystemFeatures
		{
			public const string FEATURE_AUDIO_LOW_LATENCY = "android.hardware.audio.low_latency";

			public const string FEATURE_AUDIO_OUTPUT = "android.hardware.audio.output";

			public const string FEATURE_AUDIO_PRO = "android.hardware.audio.pro";

			public const string FEATURE_BLUETOOTH = "android.hardware.bluetooth";

			public const string FEATURE_BLUETOOTH_LE = "android.hardware.bluetooth_le";

			public const string FEATURE_CAMERA = "android.hardware.camera";

			public const string FEATURE_CAMERA_AUTOFOCUS = "android.hardware.camera.autofocus";

			public const string FEATURE_CAMERA_ANY = "android.hardware.camera.any";

			public const string FEATURE_CAMERA_EXTERNAL = "android.hardware.camera.external";

			public const string FEATURE_CAMERA_FLASH = "android.hardware.camera.flash";

			public const string FEATURE_CAMERA_FRONT = "android.hardware.camera.front";

			public const string FEATURE_CAMERA_LEVEL_FULL = "android.hardware.camera.level.full";

			public const string FEATURE_CAMERA_CAPABILITY_MANUAL_SENSOR = "android.hardware.camera.capability.manual_sensor";

			public const string FEATURE_CAMERA_CAPABILITY_MANUAL_POST_PROCESSING = "android.hardware.camera.capability.manual_post_processing";

			public const string FEATURE_CAMERA_CAPABILITY_RAW = "android.hardware.camera.capability.raw";

			public const string FEATURE_CONSUMER_IR = "android.hardware.consumerir";

			public const string FEATURE_LOCATION = "android.hardware.location";

			public const string FEATURE_LOCATION_GPS = "android.hardware.location.gps";

			public const string FEATURE_LOCATION_NETWORK = "android.hardware.location.network";

			public const string FEATURE_MICROPHONE = "android.hardware.microphone";

			public const string FEATURE_NFC = "android.hardware.nfc";

			public const string FEATURE_NFC_HCE = "android.hardware.nfc.hce";

			public const string FEATURE_NFC_HOST_CARD_EMULATION = "android.hardware.nfc.hce";

			public const string FEATURE_OPENGLES_EXTENSION_PACK = "android.hardware.opengles.aep";

			public const string FEATURE_SENSOR_ACCELEROMETER = "android.hardware.sensor.accelerometer";

			public const string FEATURE_SENSOR_BAROMETER = "android.hardware.sensor.barometer";

			public const string FEATURE_SENSOR_COMPASS = "android.hardware.sensor.compass";

			public const string FEATURE_SENSOR_GYROSCOPE = "android.hardware.sensor.gyroscope";

			public const string FEATURE_SENSOR_LIGHT = "android.hardware.sensor.light";

			public const string FEATURE_SENSOR_PROXIMITY = "android.hardware.sensor.proximity";

			public const string FEATURE_SENSOR_STEP_COUNTER = "android.hardware.sensor.stepcounter";

			public const string FEATURE_SENSOR_STEP_DETECTOR = "android.hardware.sensor.stepdetector";

			public const string FEATURE_SENSOR_HEART_RATE = "android.hardware.sensor.heartrate";

			public const string FEATURE_SENSOR_HEART_RATE_ECG = "android.hardware.sensor.heartrate.ecg";

			public const string FEATURE_SENSOR_RELATIVE_HUMIDITY = "android.hardware.sensor.relative_humidity";

			public const string FEATURE_SENSOR_AMBIENT_TEMPERATURE = "android.hardware.sensor.ambient_temperature";

			public const string FEATURE_HIFI_SENSORS = "android.hardware.sensor.hifi_sensors";

			public const string FEATURE_TELEPHONY = "android.hardware.telephony";

			public const string FEATURE_TELEPHONY_CDMA = "android.hardware.telephony.cdma";

			public const string FEATURE_TELEPHONY_GSM = "android.hardware.telephony.gsm";

			public const string FEATURE_USB_HOST = "android.hardware.usb.host";

			public const string FEATURE_USB_ACCESSORY = "android.hardware.usb.accessory";

			public const string FEATURE_SIP = "android.software.sip";

			public const string FEATURE_SIP_VOIP = "android.software.sip.voip";

			public const string FEATURE_CONNECTION_SERVICE = "android.software.connectionservice";

			public const string FEATURE_TOUCHSCREEN = "android.hardware.touchscreen";

			public const string FEATURE_TOUCHSCREEN_MULTITOUCH = "android.hardware.touchscreen.multitouch";

			public const string FEATURE_TOUCHSCREEN_MULTITOUCH_DISTINCT = "android.hardware.touchscreen.multitouch.distinct";

			public const string FEATURE_TOUCHSCREEN_MULTITOUCH_JAZZHAND = "android.hardware.touchscreen.multitouch.jazzhand";

			public const string FEATURE_FAKETOUCH = "android.hardware.faketouch";

			public const string FEATURE_FAKETOUCH_MULTITOUCH_DISTINCT = "android.hardware.faketouch.multitouch.distinct";

			public const string FEATURE_FAKETOUCH_MULTITOUCH_JAZZHAND = "android.hardware.faketouch.multitouch.jazzhand";

			public const string FEATURE_FINGERPRINT = "android.hardware.fingerprint";

			public const string FEATURE_SCREEN_PORTRAIT = "android.hardware.screen.portrait";

			public const string FEATURE_SCREEN_LANDSCAPE = "android.hardware.screen.landscape";

			public const string FEATURE_LIVE_WALLPAPER = "android.software.live_wallpaper";

			public const string FEATURE_APP_WIDGETS = "android.software.app_widgets";

			public const string FEATURE_VOICE_RECOGNIZERS = "android.software.voice_recognizers";

			public const string FEATURE_HOME_SCREEN = "android.software.home_screen";

			public const string FEATURE_INPUT_METHODS = "android.software.input_methods";

			public const string FEATURE_DEVICE_ADMIN = "android.software.device_admin";

			public const string FEATURE_LEANBACK = "android.software.leanback";

			public const string FEATURE_LEANBACK_ONLY = "android.software.leanback_only";

			public const string FEATURE_LIVE_TV = "android.software.live_tv";

			public const string FEATURE_WIFI = "android.hardware.wifi";

			public const string FEATURE_WIFI_DIRECT = "android.hardware.wifi.direct";

			public const string FEATURE_AUTOMOTIVE = "android.hardware.type.automotive";

			public const string FEATURE_WATCH = "android.hardware.type.watch";

			public const string FEATURE_PRINTING = "android.software.print";

			public const string FEATURE_BACKUP = "android.software.backup";

			public const string FEATURE_MANAGED_USERS = "android.software.managed_users";

			public const string FEATURE_MANAGED_PROFILES = "android.software.managed_users";

			public const string FEATURE_VERIFIED_BOOT = "android.software.verified_boot";

			public const string FEATURE_SECURELY_REMOVES_USERS = "android.software.securely_removes_users";

			public const string FEATURE_WEBVIEW = "android.software.webview";

			public const string FEATURE_ETHERNET = "android.hardware.ethernet";

			public const string FEATURE_HDMI_CEC = "android.hardware.hdmi.cec";

			public const string FEATURE_GAMEPAD = "android.hardware.gamepad";

			public const string FEATURE_MIDI = "android.software.midi";

			public static bool HasFlashlight => AGUtils.HasSystemFeature("android.hardware.camera.flash");

			public static bool HasSystemFeature(string feature)
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return false;
				}
				return AGUtils.HasSystemFeature(feature);
			}
		}

		public static string DEVICE => GetBuildClassStaticStr("DEVICE");

		public static string MODEL => GetBuildClassStaticStr("MODEL");

		public static string PRODUCT => GetBuildClassStaticStr("PRODUCT");

		public static string MANUFACTURER => GetBuildClassStaticStr("MANUFACTURER");

		public static string BASE_OS => GetDeviceStrProperty<string>("android.os.Build$VERSION", "BASE_OS");

		public static string CODENAME => GetDeviceStrProperty<string>("android.os.Build$VERSION", "CODENAME");

		public static string INCREMENTAL => GetDeviceStrProperty<string>("android.os.Build$VERSION", "INCREMENTAL");

		public static int PREVIEW_SDK_INT => GetDeviceStrProperty<int>("android.os.Build$VERSION", "PREVIEW_SDK_INT");

		public static string RELEASE => GetDeviceStrProperty<string>("android.os.Build$VERSION", "RELEASE");

		public static int SDK_INT => GetDeviceStrProperty<int>("android.os.Build$VERSION", "SDK_INT");

		public static string SECURITY_PATCH => GetDeviceStrProperty<string>("android.os.Build$VERSION", "SECURITY_PATCH");

		public static string GetAndroidId()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}
			try
			{
				using (AndroidJavaObject androidJavaObject = AGUtils.ContentResolver)
				{
					using (AndroidJavaClass ajc = new AndroidJavaClass("android.provider.Settings$Secure"))
					{
						string staticStr = ajc.GetStaticStr("ANDROID_ID");
						return ajc.CallStaticStr("getString", androidJavaObject, staticStr);
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("Failed to get Android Id, reasong: " + ex.Message);
				return string.Empty;
			}
		}

		private static string GetBuildClassStaticStr(string fieldName)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}
			try
			{
				using (AndroidJavaClass ajc = new AndroidJavaClass("android.os.Build"))
				{
					return ajc.GetStaticStr(fieldName);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("Failed to get property " + fieldName + ". Check device API level. " + ex.Message);
				return string.Empty;
			}
		}

		private static T GetDeviceStrProperty<T>(string className, string propertyName)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return default(T);
			}
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(className))
				{
					return androidJavaClass.GetStatic<T>(propertyName);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning($"Failed to get property: {propertyName} of class {className}, reason: {ex.Message}");
				return default(T);
			}
		}

		public static string GetApplicationPackage()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}
			return AGUtils.Activity.CallAJO("getApplicationContext").CallStr("getPackageName");
		}

		public static bool IsPackageInstalled(string package)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidJavaObject androidJavaObject = AGUtils.PackageManager)
			{
				try
				{
					androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[2]
					{
						package,
						1
					});
					return true;
				}
				catch (AndroidJavaException message)
				{
					UnityEngine.Debug.LogWarning(message);
					return false;
				}
			}
		}

		public static List<PackageInfo> GetInstalledPackages()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return new List<PackageInfo>();
			}
			using (AndroidJavaObject ajo = AGUtils.PackageManager)
			{
				List<PackageInfo> list = new List<PackageInfo>();
				List<AndroidJavaObject> list2 = ajo.CallAJO("getInstalledPackages", 0).FromJavaList<AndroidJavaObject>();
				foreach (AndroidJavaObject item in list2)
				{
					list.Add(PackageInfo.FromJavaObj(item));
				}
				return list;
			}
		}
	}
}
