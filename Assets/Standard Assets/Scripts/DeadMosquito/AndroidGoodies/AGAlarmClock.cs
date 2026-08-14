using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGAlarmClock
	{
		public enum AlarmDays
		{
			Sunday = 1,
			Monday,
			Tuesday,
			Wednesday,
			Thursday,
			Friday,
			Saturday
		}

		private const string ACTION_SET_ALARM = "android.intent.action.SET_ALARM";

		private const string ACTION_SHOW_ALARMS = "android.intent.action.SHOW_ALARMS";

		private const string ACTION_SET_TIMER = "android.intent.action.SET_TIMER";

		private const string EXTRA_HOUR = "android.intent.extra.alarm.HOUR";

		private const string EXTRA_MINUTES = "android.intent.extra.alarm.MINUTES";

		private const string EXTRA_MESSAGE = "android.intent.extra.alarm.MESSAGE";

		private const string EXTRA_DAYS = "android.intent.extra.alarm.DAYS";

		private const string EXTRA_VIBRATE = "android.intent.extra.alarm.VIBRATE";

		private const string EXTRA_RINGTONE = "android.intent.extra.alarm.RINGTONE";

		private const string EXTRA_SKIP_UI = "android.intent.extra.alarm.SKIP_UI";

		private const string EXTRA_LENGTH = "android.intent.extra.alarm.LENGTH";

		public static bool CanShowListOfAlarms()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SHOW_ALARMS"))
			{
				return androidIntent.ResolveActivity();
			}
		}

		public static bool CanSetAlarm()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SET_ALARM"))
			{
				return androidIntent.ResolveActivity();
			}
		}

		public static bool CanSetTimer()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SET_TIMER"))
			{
				return androidIntent.ResolveActivity();
			}
		}

		public static void ShowAllAlarms()
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SHOW_ALARMS"))
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		public static void SetAlarm(int hour, int minute, string message, AlarmDays[] days = null, bool vibrate = true, bool skipUI = false)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SET_ALARM"))
				{
					androidIntent.PutExtra("android.intent.extra.alarm.HOUR", hour);
					androidIntent.PutExtra("android.intent.extra.alarm.MINUTES", minute);
					androidIntent.PutExtra("android.intent.extra.alarm.MESSAGE", message);
					if (days != null)
					{
						androidIntent.PutExtra("android.intent.extra.alarm.DAYS", CreateDaysArrayList(days));
					}
					androidIntent.PutExtra("android.intent.extra.alarm.VIBRATE", vibrate);
					androidIntent.PutExtra("android.intent.extra.alarm.SKIP_UI", skipUI);
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		private static AndroidJavaObject CreateDaysArrayList(AlarmDays[] days)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
			foreach (AlarmDays alarmDays in days)
			{
				androidJavaObject.Call<bool>("add", new object[1]
				{
					new AndroidJavaObject("java.lang.Integer", (int)alarmDays)
				});
			}
			return androidJavaObject;
		}

		public static bool UserHasTimerApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SET_TIMER"))
			{
				return androidIntent.ResolveActivity();
			}
		}

		public static void SetTimer(int lengthInSeconds, string message, bool skipUi = false)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SET_TIMER"))
				{
					androidIntent.PutExtra("android.intent.extra.alarm.LENGTH", lengthInSeconds);
					androidIntent.PutExtra("android.intent.extra.alarm.MESSAGE", message);
					androidIntent.PutExtra("android.intent.extra.alarm.SKIP_UI", skipUi);
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}
	}
}
