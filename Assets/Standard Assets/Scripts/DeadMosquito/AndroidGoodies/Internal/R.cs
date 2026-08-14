using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class R
	{
		private const string DrawableRes = "drawable";

		private const string UnityAppIconDefaultName = "app_icon";

		public static int UnityLauncherIcon => GetAppDrawableId("app_icon");

		public static int GetAppDrawableId(string drawableName)
		{
			return GetAppResourceId(drawableName, "drawable");
		}

		public static int GetAppResourceId(string variableName, string resourceName)
		{
			try
			{
				return AGUtils.Activity.CallAJO("getResources").Call<int>("getIdentifier", new object[3]
				{
					variableName,
					resourceName,
					AGDeviceInfo.GetApplicationPackage()
				});
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public static int GetAndroidDrawableId(string name)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.R$drawable"))
			{
				return androidJavaClass.GetStatic<int>(name);
			}
		}
	}
}
