using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGLocalNotifications
	{
		private const string GoodiesNotificationManagerClass = "com.deadmosquitogames.notifications.GoodiesNotificationManager";

		public static void ShowNotification(string title, string text, DateTime? when, string tickerText = null, string iconName = null)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (!when.HasValue)
				{
					when = DateTime.Now;
				}
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.deadmosquitogames.notifications.GoodiesNotificationManager"))
				{
					Color32 color = Color.black;
					androidJavaClass.CallStatic("setNotification", AGUtils.Activity, new System.Random().Next(), when.Value.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch(), title, text, text, 1, 1, 1, string.Empty, "notify_icon_small", color.ToAndroidColor(), Application.identifier);
				}
			}
		}

		public static void ShowNotification(int id, DateTime when, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.deadmosquitogames.notifications.GoodiesNotificationManager"))
				{
					androidJavaClass.CallStatic("setNotification", AGUtils.Activity, id, when.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch(), title, message, message, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.ToAndroidColor(), Application.identifier);
				}
			}
		}

		public static void ShowNotificationRepeating(int id, DateTime when, long intervalMillis, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.deadmosquitogames.notifications.GoodiesNotificationManager"))
				{
					androidJavaClass.CallStatic("setRepeatingNotification", AGUtils.Activity, id, when.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch(), title, message, message, intervalMillis, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.ToAndroidColor(), Application.identifier);
				}
			}
		}

		public static void CancelNotification(int notificationId)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.deadmosquitogames.notifications.GoodiesNotificationManager"))
				{
					androidJavaClass.CallStatic("cancelNotification", AGUtils.Activity, notificationId);
				}
			}
		}
	}
}
