using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGCalendar
	{
		public enum EventAvailability
		{
			Default = -1,
			Busy,
			Free,
			Tentative
		}

		public enum EventAccessLevel
		{
			Default = 0,
			Private = 2,
			Public = 3
		}

		private static class Events
		{
			public const string TITLE = "title";

			public const string DESCRIPTION = "description";

			public const string EVENT_LOCATION = "eventLocation";

			public const string RRULE = "rrule";

			public const string AVAILABILITY = "availability";

			public const string ACCESS_LEVEL = "accessLevel";

			public static AndroidJavaObject CONTENT_URI
			{
				get
				{
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.provider.CalendarContract$Events"))
					{
						return androidJavaClass.GetStatic<AndroidJavaObject>("CONTENT_URI");
					}
				}
			}
		}

		private static class CalendarContract
		{
			public const string EXTRA_EVENT_BEGIN_TIME = "beginTime";

			public const string EXTRA_EVENT_END_TIME = "endTime";

			public const string EXTRA_EVENT_ALL_DAY = "allDay";
		}

		public class EventBuilder
		{
			internal string _title;

			internal long _beginTime;

			internal long _endTime;

			internal bool _allDay;

			internal string _location;

			internal string _description;

			internal string _emails;

			internal string _rrule;

			internal EventAvailability _availability = EventAvailability.Default;

			internal EventAccessLevel _accessLevel;

			public EventBuilder(string eventName, DateTime beginTime)
			{
				_title = eventName;
				_beginTime = beginTime.ToMillisSinceEpoch();
			}

			public EventBuilder SetEndTime(DateTime endTime)
			{
				_endTime = endTime.ToMillisSinceEpoch();
				return this;
			}

			public EventBuilder SetIsAllDay(bool allDay)
			{
				_allDay = allDay;
				return this;
			}

			public EventBuilder SetLocation(string location)
			{
				_location = location;
				return this;
			}

			public EventBuilder SetDescription(string description)
			{
				_description = description;
				return this;
			}

			public EventBuilder SetEmails(string[] emails)
			{
				_emails = string.Join(",", emails);
				return this;
			}

			public EventBuilder SetRRule(string rrule)
			{
				_rrule = rrule;
				return this;
			}

			public EventBuilder SetAvailability(EventAvailability availability)
			{
				_availability = availability;
				return this;
			}

			public EventBuilder SetAccessLevel(EventAccessLevel accessLevel)
			{
				_accessLevel = accessLevel;
				return this;
			}

			public void BuildAndShow()
			{
				if (!AGUtils.IsNotAndroidCheck())
				{
					AndroidIntent androidIntent = new AndroidIntent("android.intent.action.EDIT").SetData(Events.CONTENT_URI);
					androidIntent.SetType("vnd.android.cursor.item/event");
					androidIntent.PutExtra("title", _title);
					androidIntent.PutExtra("beginTime", _beginTime);
					androidIntent.PutExtra("allDay", _allDay);
					if (_endTime != 0)
					{
						androidIntent.PutExtra("endTime", _endTime);
					}
					if (!string.IsNullOrEmpty(_location))
					{
						androidIntent.PutExtra("eventLocation", _location);
					}
					if (!string.IsNullOrEmpty(_description))
					{
						androidIntent.PutExtra("description", _description);
					}
					if (!string.IsNullOrEmpty(_rrule))
					{
						androidIntent.PutExtra("rrule", _rrule);
					}
					if (!string.IsNullOrEmpty(_emails))
					{
						androidIntent.PutExtra("android.intent.extra.EMAIL", _emails);
					}
					if (_availability != EventAvailability.Default)
					{
						androidIntent.PutExtra("availability", (int)_availability);
					}
					androidIntent.PutExtra("accessLevel", (int)_accessLevel);
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		public static bool UserHasCalendarApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.INSERT").SetData(Events.CONTENT_URI))
			{
				androidIntent.PutExtra("title", "dummy_title");
				androidIntent.PutExtra("beginTime", 123L);
				return androidIntent.ResolveActivity();
			}
		}

		public static void OpenCalendarForDate(DateTime dateTime)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				long num = dateTime.ToMillisSinceEpoch();
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.provider.CalendarContract"))
				{
					using (AndroidJavaObject ajo = androidJavaClass.GetStatic<AndroidJavaObject>("CONTENT_URI"))
					{
						using (AndroidJavaObject androidJavaObject = ajo.CallAJO("buildUpon"))
						{
							androidJavaObject.CallAJO("appendPath", "time");
							using (AndroidJavaClass ajc = new AndroidJavaClass("android.content.ContentUris"))
							{
								ajc.CallStaticAJO("appendId", androidJavaObject, num);
							}
							AndroidJavaObject data = androidJavaObject.CallAJO("build");
							AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW").SetData(data);
							AGUtils.StartActivity(androidIntent.AJO);
						}
					}
				}
			}
		}
	}
}
