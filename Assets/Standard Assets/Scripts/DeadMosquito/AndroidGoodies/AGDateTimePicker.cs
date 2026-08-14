using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGDateTimePicker
	{
		public delegate void OnDatePicked(int year, int month, int day);

		public delegate void OnTimePicked(int hourOfDay, int minute);

		private class OnDateSetListenerPoxy : AndroidJavaProxy
		{
			private readonly OnDatePicked _onDatePicked;

			public OnDateSetListenerPoxy(OnDatePicked onDatePicked)
				: base("android.app.DatePickerDialog$OnDateSetListener")
			{
				_onDatePicked = onDatePicked;
			}

			private void onDateSet(AndroidJavaObject view, int year, int month, int dayOfMonth)
			{
				GoodiesSceneHelper.Queue(delegate
				{
					_onDatePicked(year, month + 1, dayOfMonth);
				});
			}
		}

		private class OnTimeSetListenerPoxy : AndroidJavaProxy
		{
			private readonly OnTimePicked _onTimePicked;

			public OnTimeSetListenerPoxy(OnTimePicked onTimePicked)
				: base("android.app.TimePickerDialog$OnTimeSetListener")
			{
				_onTimePicked = onTimePicked;
			}

			private void onTimeSet(AndroidJavaObject view, int hourOfDay, int minute)
			{
				GoodiesSceneHelper.Queue(delegate
				{
					_onTimePicked(hourOfDay, minute);
				});
			}
		}

		private const string DatePickerClass = "android.app.DatePickerDialog";

		private const string TimePickerClass = "android.app.TimePickerDialog";

		public static void ShowDatePicker(int year, int month, int day, OnDatePicked onDatePicked, Action onCancel, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					OnDateSetListenerPoxy onDateSetListenerPoxy = new OnDateSetListenerPoxy(onDatePicked);
					int dialogTheme = AndroidDialogUtils.GetDialogTheme(theme);
					AndroidJavaObject androidJavaObject = (!AndroidDialogUtils.IsDefault(dialogTheme)) ? new AndroidJavaObject("android.app.DatePickerDialog", AGUtils.Activity, dialogTheme, onDateSetListenerPoxy, year, month - 1, day) : new AndroidJavaObject("android.app.DatePickerDialog", AGUtils.Activity, onDateSetListenerPoxy, year, month - 1, day);
					androidJavaObject.Call("setOnCancelListener", new DialogOnCancelListenerPoxy(onCancel));
					AndroidDialogUtils.ShowWithImmersiveModeWorkaround(androidJavaObject);
				});
			}
		}

		public static void ShowTimePicker(int hourOfDay, int minute, OnTimePicked onTimePicked, Action onCancel, AGDialogTheme theme = AGDialogTheme.Default, bool is24HourFormat = true)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					OnTimeSetListenerPoxy onTimeSetListenerPoxy = new OnTimeSetListenerPoxy(onTimePicked);
					int dialogTheme = AndroidDialogUtils.GetDialogTheme(theme);
					AndroidJavaObject androidJavaObject = (!AndroidDialogUtils.IsDefault(dialogTheme)) ? new AndroidJavaObject("android.app.TimePickerDialog", AGUtils.Activity, dialogTheme, onTimeSetListenerPoxy, hourOfDay, minute, is24HourFormat) : new AndroidJavaObject("android.app.TimePickerDialog", AGUtils.Activity, onTimeSetListenerPoxy, hourOfDay, minute, is24HourFormat);
					androidJavaObject.Call("setOnCancelListener", new DialogOnCancelListenerPoxy(onCancel));
					AndroidDialogUtils.ShowWithImmersiveModeWorkaround(androidJavaObject);
				});
			}
		}
	}
}
