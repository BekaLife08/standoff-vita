using System;
using Axlebolt.Standoff.Core;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Messages
{
	public class ChatUtility
	{
		private static readonly Log Log = Log.Create(typeof(ChatUtility));

		public static DateTime FromUnixTime(long timestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)timestamp / 1000.0).ToLocalTime();
		}

		public static string LocalizeMonth(int month)
		{
			switch (month)
			{
			case 1:
				return ScriptLocalization.DateTime.January;
			case 2:
				return ScriptLocalization.DateTime.February;
			case 3:
				return ScriptLocalization.DateTime.March;
			case 4:
				return ScriptLocalization.DateTime.April;
			case 5:
				return ScriptLocalization.DateTime.May;
			case 6:
				return ScriptLocalization.DateTime.June;
			case 7:
				return ScriptLocalization.DateTime.July;
			case 8:
				return ScriptLocalization.DateTime.August;
			case 9:
				return ScriptLocalization.DateTime.September;
			case 10:
				return ScriptLocalization.DateTime.October;
			case 11:
				return ScriptLocalization.DateTime.November;
			case 12:
				return ScriptLocalization.DateTime.December;
			default:
				Log.Error(string.Format("Can't localize month ({0})", month));
				return "Unkown";
			}
		}

		public static string LocalizeMonthShort(int month)
		{
			string text = LocalizeMonth(month);
			if (text.Length > 3)
			{
				return text.Substring(0, 3);
			}
			return text;
		}

		public static string LocalizeChatTimestamp(long timestamp)
		{
			DateTime dateTime = FromUnixTime(timestamp);
			if (dateTime >= DateTime.Today)
			{
				return FormatHourMinutes(dateTime);
			}
			if (IsDayEquals(dateTime, DateTime.Today.AddDays(-1.0)))
			{
				return ScriptLocalization.DateTime.Yesterday;
			}
			if (dateTime.Year == DateTime.Today.Year)
			{
				return dateTime.Day + " " + LocalizeMonthShort(dateTime.Month);
			}
			return dateTime.Day + " " + LocalizeMonthShort(dateTime.Month) + " " + dateTime.Year;
		}

		public static string FormatHourMinutes(DateTime dateTime)
		{
			if (dateTime.Minute > 9)
			{
				return dateTime.Hour + ":" + dateTime.Minute;
			}
			return dateTime.Hour + ":0" + dateTime.Minute;
		}

		public static string LocalizeDayMonthYear(DateTime dateTime)
		{
			if (IsDayEquals(dateTime, DateTime.Today))
			{
				return ScriptLocalization.DateTime.Today;
			}
			return dateTime.Day + " " + LocalizeMonth(dateTime.Month) + " " + dateTime.Year;
		}

		public static bool IsDayEquals(DateTime a, DateTime b)
		{
			return a.Year == b.Year && a.DayOfYear == b.DayOfYear;
		}

		public static long GetCurrentTimeMillis()
		{
			return (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}
