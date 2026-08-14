using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public class AGAlertDialog
	{
		private class Builder
		{
			internal string _title;

			internal string _message;

			internal AGDialogTheme _theme;

			internal bool _isPositiveButtonSet;

			internal string _positiveButtonText;

			internal Action _positiveButtonCallback;

			internal bool _isNegativeButtonSet;

			internal string _negativeButtonText;

			internal Action _negativeButtonCallback;

			internal bool _isNeutralButtonSet;

			internal string _neutralButtonText;

			internal Action _neutralButtonCallback;

			internal bool _areItemsSet;

			internal string[] _items;

			internal Action<int> _itemClickCallback;

			internal bool _areSingleChoiceItemsSet;

			internal string[] _singleChoiceItems;

			internal int _singleChoiceCheckedItem;

			internal Action<int> _singleChoiceItemClickCallback;

			internal bool _areMultiChoiceItemsSet;

			internal string[] _multiChoiceItems;

			internal bool[] _multiChoiceCheckedItems;

			internal Action<int, bool> _multiChoiceItemClickCallback;

			internal Action _onCancelCallback;

			internal Action _onDismissCallback;

			public Builder SetTitle(string title)
			{
				_title = title;
				return this;
			}

			public Builder SetMessage(string message)
			{
				_message = message;
				return this;
			}

			public Builder SetTheme(AGDialogTheme theme)
			{
				_theme = theme;
				return this;
			}

			public Builder SetPositiveButton(string buttonText, Action callback)
			{
				_isPositiveButtonSet = true;
				_positiveButtonText = buttonText;
				_positiveButtonCallback = callback;
				return this;
			}

			public Builder SetNegativeButton(string buttonText, Action callback)
			{
				_isNegativeButtonSet = true;
				_negativeButtonText = buttonText;
				_negativeButtonCallback = callback;
				return this;
			}

			public Builder SetNeutralButton(string buttonText, Action callback)
			{
				_isNeutralButtonSet = true;
				_neutralButtonText = buttonText;
				_neutralButtonCallback = callback;
				return this;
			}

			public Builder SetItems(string[] items, Action<int> onItemClick)
			{
				_areItemsSet = true;
				_items = items;
				_itemClickCallback = onItemClick;
				return this;
			}

			public Builder SetMultiChoiceItems(string[] items, bool[] checkedItems, Action<int, bool> onItemClick)
			{
				_areMultiChoiceItemsSet = true;
				_multiChoiceItems = items;
				_multiChoiceCheckedItems = checkedItems;
				_multiChoiceItemClickCallback = onItemClick;
				return this;
			}

			public Builder SetSingleChoiceItems(string[] items, int checkedItem, Action<int> onItemClick)
			{
				_areSingleChoiceItemsSet = true;
				_singleChoiceItems = items;
				_singleChoiceCheckedItem = checkedItem;
				_singleChoiceItemClickCallback = onItemClick;
				return this;
			}

			public Builder SetOnCancelListener(Action onCancel)
			{
				_onCancelCallback = onCancel;
				return this;
			}

			public Builder SetOnDismissListener(Action onDismiss)
			{
				_onDismissCallback = onDismiss;
				return this;
			}

			public AGAlertDialog Create()
			{
				return new AGAlertDialog(this);
			}
		}

		private readonly AndroidJavaObject _dialog;

		private const string AlertDialogBuilderClass = "android.app.AlertDialog$Builder";

		private AGAlertDialog(Builder builder)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
			AndroidJavaObject ajo = CreateDialogBuilder(builder._theme);
			if (!string.IsNullOrEmpty(builder._title))
			{
				ajo.CallAJO("setTitle", builder._title);
			}
			if (!string.IsNullOrEmpty(builder._message))
			{
				ajo.CallAJO("setMessage", builder._message);
			}
			if (builder._isPositiveButtonSet)
			{
				ajo.CallAJO("setPositiveButton", builder._positiveButtonText, new DialogOnClickListenerProxy(builder._positiveButtonCallback));
			}
			if (builder._isNegativeButtonSet)
			{
				ajo.CallAJO("setNegativeButton", builder._negativeButtonText, new DialogOnClickListenerProxy(builder._negativeButtonCallback));
			}
			if (builder._isNeutralButtonSet)
			{
				ajo.CallAJO("setNeutralButton", builder._neutralButtonText, new DialogOnClickListenerProxy(builder._neutralButtonCallback));
			}
			if (builder._areItemsSet)
			{
				ajo.CallAJO("setItems", builder._items, new DialogOnClickListenerProxy(builder._itemClickCallback, dismissOnClick: true));
			}
			if (builder._areSingleChoiceItemsSet)
			{
				ajo.CallAJO("setSingleChoiceItems", builder._singleChoiceItems, builder._singleChoiceCheckedItem, new DialogOnClickListenerProxy(builder._singleChoiceItemClickCallback));
			}
			if (builder._areMultiChoiceItemsSet)
			{
				ajo.CallAJO("setMultiChoiceItems", builder._multiChoiceItems, builder._multiChoiceCheckedItems, new DialogOnMultiChoiceClickListenerProxy(builder._multiChoiceItemClickCallback));
			}
			if (builder._onCancelCallback != null)
			{
				ajo.CallAJO("setOnCancelListener", new DialogOnCancelListenerPoxy(builder._onCancelCallback));
			}
			if (builder._onDismissCallback != null)
			{
				ajo.CallAJO("setOnDismissListener", new DialogOnDismissListenerProxy(builder._onDismissCallback));
			}
			_dialog = ajo.CallAJO("create");
		}

		private static AndroidJavaObject CreateDialogBuilder(AGDialogTheme theme)
		{
			int dialogTheme = AndroidDialogUtils.GetDialogTheme(theme);
			return (!AndroidDialogUtils.IsDefault(dialogTheme)) ? new AndroidJavaObject("android.app.AlertDialog$Builder", AGUtils.Activity, dialogTheme) : new AndroidJavaObject("android.app.AlertDialog$Builder", AGUtils.Activity);
		}

		private void Show()
		{
			AndroidDialogUtils.ShowWithImmersiveModeWorkaround(_dialog);
		}

		public static void ShowMessageDialog(string title, string message, string positiveButtonText, Action onPositiveButtonClick, Action onDismiss = null, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (onPositiveButtonClick == null)
				{
					throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
				}
				AGUtils.RunOnUiThread(delegate
				{
					Builder builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss).SetTheme(theme);
					AGAlertDialog aGAlertDialog = builder.Create();
					aGAlertDialog.Show();
				});
			}
		}

		public static void ShowMessageDialog(string title, string message, string positiveButtonText, Action onPositiveButtonClick, string negativeButtonText, Action onNegativeButtonClick, Action onDismiss = null, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (onPositiveButtonClick == null)
				{
					throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
				}
				if (onNegativeButtonClick == null)
				{
					throw new ArgumentNullException("onNegativeButtonClick", "Button callback cannot be null");
				}
				AGUtils.RunOnUiThread(delegate
				{
					Builder builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss).SetTheme(theme);
					builder.SetNegativeButton(negativeButtonText, onNegativeButtonClick);
					AGAlertDialog aGAlertDialog = builder.Create();
					aGAlertDialog.Show();
				});
			}
		}

		public static void ShowMessageDialog(string title, string message, string positiveButtonText, Action onPositiveButtonClick, string negativeButtonText, Action onNegativeButtonClick, string neutralButtonText, Action onNeutralButtonClick, Action onDismiss = null, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (onPositiveButtonClick == null)
				{
					throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
				}
				if (onNegativeButtonClick == null)
				{
					throw new ArgumentNullException("onNegativeButtonClick", "Button callback cannot be null");
				}
				if (onNeutralButtonClick == null)
				{
					throw new ArgumentNullException("onNeutralButtonClick", "Button callback cannot be null");
				}
				AGUtils.RunOnUiThread(delegate
				{
					Builder builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss).SetTheme(theme);
					builder.SetNegativeButton(negativeButtonText, onNegativeButtonClick);
					builder.SetNeutralButton(neutralButtonText, onNeutralButtonClick);
					AGAlertDialog aGAlertDialog = builder.Create();
					aGAlertDialog.Show();
				});
			}
		}

		private static Builder CreateMessageDialogBuilder(string title, string message, string positiveButtonText, Action onPositiveButtonClick, Action onDismiss)
		{
			Builder builder = new Builder().SetTitle(title).SetMessage(message);
			builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
			if (onDismiss != null)
			{
				builder.SetOnDismissListener(onDismiss);
			}
			return builder;
		}

		public static void ShowChooserDialog(string title, string[] items, Action<int> onClickCallback, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGUtils.RunOnUiThread(delegate
				{
					Builder builder = new Builder().SetTitle(title).SetTheme(theme);
					builder.SetItems(items, onClickCallback);
					AGAlertDialog aGAlertDialog = builder.Create();
					aGAlertDialog.Show();
				});
			}
		}

		public static void ShowSingleItemChoiceDialog(string title, string[] items, int checkedItem, Action<int> onItemClicked, string positiveButtonText, Action onPositiveButtonClick, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (items == null)
				{
					throw new ArgumentNullException("items", "Items can't be null");
				}
				if (onItemClicked == null)
				{
					throw new ArgumentNullException("onItemClicked", "Item click callback cannot be null");
				}
				if (onPositiveButtonClick == null)
				{
					throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
				}
				AGUtils.RunOnUiThread(delegate
				{
					Builder builder = CreateSingleChoiceDialogBuilder(title, items, checkedItem, onItemClicked).SetTheme(theme);
					builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
					AGAlertDialog aGAlertDialog = builder.Create();
					aGAlertDialog.Show();
				});
			}
		}

		private static Builder CreateSingleChoiceDialogBuilder(string title, string[] items, int checkedItem, Action<int> onItemClicked)
		{
			Builder builder = new Builder().SetTitle(title);
			builder.SetSingleChoiceItems(items, checkedItem, onItemClicked);
			return builder;
		}

		public static void ShowMultiItemChoiceDialog(string title, string[] items, bool[] checkedItems, Action<int, bool> onItemClicked, string positiveButtonText, Action onPositiveButtonClick, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (items == null)
				{
					throw new ArgumentNullException("items", "Items can't be null");
				}
				if (checkedItems == null)
				{
					throw new ArgumentNullException("checkedItems", "Checked items can't be null");
				}
				if (onItemClicked == null)
				{
					throw new ArgumentNullException("onItemClicked", "Item click callback can't be null");
				}
				if (onPositiveButtonClick == null)
				{
					throw new ArgumentNullException("onPositiveButtonClick", "Button click callback can;t be null");
				}
				AGUtils.RunOnUiThread(delegate
				{
					Builder builder = CreateMultiChoiceDialogBuilder(title, items, checkedItems, onItemClicked).SetTheme(theme);
					builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
					AGAlertDialog aGAlertDialog = builder.Create();
					aGAlertDialog.Show();
				});
			}
		}

		private static Builder CreateMultiChoiceDialogBuilder(string title, string[] items, bool[] checkedItems, Action<int, bool> onItemClicked)
		{
			Builder builder = new Builder().SetTitle(title);
			builder.SetMultiChoiceItems(items, checkedItems, onItemClicked);
			return builder;
		}
	}
}
