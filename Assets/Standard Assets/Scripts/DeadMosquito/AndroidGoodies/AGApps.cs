using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGApps
	{
		public static void WatchYoutubeVideo(string id)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW", AndroidUri.Parse("vnd.youtube:" + id));
				AGUtils.StartActivity(androidIntent.AJO, delegate
				{
					AndroidIntent androidIntent2 = new AndroidIntent("android.intent.action.VIEW", AndroidUri.Parse("http://www.youtube.com/watch?v=" + id));
					AGUtils.StartActivity(androidIntent2.AJO);
				});
			}
		}

		public static void OpenInstagramProfile(string profileId)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsStrNotNullOrEmpty(profileId, "profileId");
				string text = "http://instagram.com/_u/{0}";
				OpenProfileInternal("com.instagram.android", text, profileId, text);
			}
		}

		public static void OpenTwitterProfile(string profileId)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsStrNotNullOrEmpty(profileId, "profileId");
				OpenProfileInternal(null, "twitter://user?screen_name={0}", profileId, "https://twitter.com/{0}");
			}
		}

		public static void OpenFacebookProfile(string profileId)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsStrNotNullOrEmpty(profileId, "profileId");
				OpenProfileInternal(null, "fb://profile/{0}", profileId, "https://www.facebook.com/{0}");
			}
		}

		private static void OpenProfileInternal(string package, string formatUri, string profileId, string fallbackFormatUri)
		{
			AndroidIntent viewProfileIntent = GetViewProfileIntent(formatUri, profileId);
			if (package != null)
			{
				viewProfileIntent.SetPackage(package);
			}
			AGUtils.StartActivity(viewProfileIntent.AJO, delegate
			{
				AndroidIntent viewProfileIntent2 = GetViewProfileIntent(fallbackFormatUri, profileId);
				AGUtils.StartActivity(viewProfileIntent2.AJO);
			});
		}

		private static AndroidIntent GetViewProfileIntent(string uriFormat, string profileId)
		{
			string uriString = string.Format(uriFormat, profileId);
			return new AndroidIntent("android.intent.action.VIEW", AndroidUri.Parse(uriString));
		}

		public static void OpenOtherAppOnDevice(string package, Action onAppNotInstalled = null)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidJavaObject ajo = AGUtils.PackageManager)
				{
					try
					{
						AndroidJavaObject androidJavaObject = ajo.CallAJO("getLaunchIntentForPackage", package);
						androidJavaObject.CallAJO("addCategory", "android.intent.category.LAUNCHER");
						AGUtils.StartActivity(androidJavaObject);
					}
					catch (Exception ex)
					{
						if (Debug.isDebugBuild)
						{
							UnityEngine.Debug.Log("Could not find launch intent for package:" + package + ", Error: " + ex.StackTrace);
						}
						onAppNotInstalled?.Invoke();
					}
				}
			}
		}

		public static void UninstallApp(string package)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				try
				{
					AndroidJavaObject uri = AndroidUri.Parse($"package:{package}");
					AndroidIntent androidIntent = new AndroidIntent("android.intent.action.DELETE", uri);
					AGUtils.StartActivity(androidIntent.AJO);
				}
				catch
				{
				}
			}
		}

		public static void InstallApkFileFromSDCard(string apkPathOnDisc)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				try
				{
					AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW");
					androidIntent.SetDataAndType(AndroidUri.FromFile(apkPathOnDisc), "application/vnd.android.package-archive");
					AGUtils.StartActivity(androidIntent.AJO);
				}
				catch
				{
					if (Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log("Could not find apk file:" + apkPathOnDisc);
					}
				}
			}
		}
	}
}
