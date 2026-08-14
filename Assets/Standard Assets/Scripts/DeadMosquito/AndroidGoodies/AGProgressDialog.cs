using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public class AGProgressDialog
	{
		private enum Style
		{
			Horizontal = 1,
			Spinner = 0
		}

		private const string ProgressDialogClass = "android.app.ProgressDialog";

		private AndroidJavaObject _dialog;

		private AGProgressDialog(Style style, string title, string message, AGDialogTheme theme, int maxValue = 100)
		{
			AGUtils.RunOnUiThread(delegate
			{
				AndroidJavaObject androidJavaObject = CreateDialog(theme);
				androidJavaObject.Call("setProgressStyle", (int)style);
				androidJavaObject.Call("setCancelable", false);
				androidJavaObject.Call("setTitle", title);
				androidJavaObject.Call("setMessage", message);
				androidJavaObject.Call("setCancelable", false);
				if (style == Style.Spinner)
				{
					androidJavaObject.Call("setIndeterminate", true);
				}
				else
				{
					androidJavaObject.Call("setIndeterminate", false);
					androidJavaObject.Call("setProgress", 0);
					androidJavaObject.Call("setMax", maxValue);
				}
				_dialog = androidJavaObject;
			});
		}

		private static AndroidJavaObject CreateDialog(AGDialogTheme theme)
		{
			int dialogTheme = AndroidDialogUtils.GetDialogTheme(theme);
			return (!AndroidDialogUtils.IsDefault(dialogTheme)) ? new AndroidJavaObject("android.app.ProgressDialog", AGUtils.Activity, dialogTheme) : new AndroidJavaObject("android.app.ProgressDialog", AGUtils.Activity);
		}

		public static AGProgressDialog CreateSpinnerDialog(string title, string message, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			return new AGProgressDialog(Style.Spinner, title, message, theme);
		}

		public static AGProgressDialog CreateHorizontalDialog(string title, string message, int maxValue, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			return new AGProgressDialog(Style.Horizontal, title, message, theme, maxValue);
		}

		public void Show()
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					_dialog.Call("show");
				});
			}
		}

		public void Dismiss()
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					_dialog.Call("dismiss");
					_dialog.Dispose();
				});
			}
		}

		public void SetProgress(int value)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					_dialog.Call("setProgress", value);
				});
			}
		}
	}
}
