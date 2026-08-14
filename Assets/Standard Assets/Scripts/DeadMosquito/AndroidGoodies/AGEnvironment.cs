using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGEnvironment
	{
		public const string MEDIA_BAD_REMOVAL = "bad_removal";

		public const string MEDIA_CHECKING = "checking";

		public const string MEDIA_MOUNTED = "mounted";

		public const string MEDIA_MOUNTED_READ_ONLY = "mounted_ro";

		public const string MEDIA_NOFS = "nofs";

		public const string MEDIA_REMOVED = "removed";

		public const string MEDIA_SHARED = "shared";

		public const string MEDIA_UNKNOWN = "unknown";

		public const string MEDIA_UNMOUNTABLE = "unmountable";

		public const string MEDIA_UNMOUNTED = "unmounted";

		public static string DirectoryAlarms => GetStringProperty("DIRECTORY_ALARMS");

		public static string DirectoryDCIM => GetStringProperty("DIRECTORY_DCIM");

		public static string DirectoryDocuments => GetStringProperty("DIRECTORY_DOCUMENTS");

		public static string DirectoryDownloads => GetStringProperty("DIRECTORY_DOWNLOADS");

		public static string DirectoryMovies => GetStringProperty("DIRECTORY_MOVIES");

		public static string DirectoryMusic => GetStringProperty("DIRECTORY_MUSIC");

		public static string DirectoryNotifications => GetStringProperty("DIRECTORY_NOTIFICATIONS");

		public static string DirectoryPictures => GetStringProperty("DIRECTORY_PICTURES");

		public static string DirectoryPodcasts => GetStringProperty("DIRECTORY_PODCASTS");

		public static string DirectoryRingtones => GetStringProperty("DIRECTORY_RINGTONES");

		public static string DataDirectoryPath => GetFileDirectory("getDataDirectory");

		public static string DownloadCacheDirectoryPath => GetFileDirectory("getDownloadCacheDirectory");

		public static string ExternalStorageDirectoryPath => GetFileDirectory("getExternalStorageDirectory");

		public static string RootDirectoryPath => GetFileDirectory("getRootDirectory");

		public static string ExternalStorageState => EnvClassCallStatic<string>("getExternalStorageState", Array.Empty<object>());

		public static bool IsExternalStorageEmulated => EnvClassCallStatic<bool>("isExternalStorageEmulated", Array.Empty<object>());

		public static bool IsExternalStorageRemovable => EnvClassCallStatic<bool>("isExternalStorageRemovable", Array.Empty<object>());

		public static string GetExternalStoragePublicDirectoryPath(string type)
		{
			return GetFileDirectory("getExternalStoragePublicDirectory", type);
		}

		private static string GetFileDirectory(string methodName, params object[] args)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			try
			{
				using (AndroidJavaClass ajc = new AndroidJavaClass("android.os.Environment"))
				{
					return ajc.CallStaticAJO(methodName, args).CallStr("getAbsolutePath");
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(string.Format("Failed to get directory {0} on {1}, Error: {2}", methodName, "android.os.Environment", ex.Message));
				return null;
			}
		}

		private static T EnvClassCallStatic<T>(string methodName, params object[] args)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return default(T);
			}
			try
			{
				return "android.os.Environment".AJCCallStaticOnce<T>(methodName, args);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(string.Format("Failed to invoke {0} on {1}, Error: {2}", methodName, "android.os.Environment", ex.Message));
				return default(T);
			}
		}

		private static string GetStringProperty(string propertyName)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}
			try
			{
				using (AndroidJavaClass ajc = new AndroidJavaClass("android.os.Environment"))
				{
					return ajc.GetStaticStr(propertyName);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("Could not get the property: " + propertyName + ". Check the device API level if the property is present, reason: " + ex.Message);
				return string.Empty;
			}
		}
	}
}
