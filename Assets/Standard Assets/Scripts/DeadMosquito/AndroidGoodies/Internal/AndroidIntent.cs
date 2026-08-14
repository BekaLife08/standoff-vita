using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public class AndroidIntent : IDisposable
	{
		public const string MIMETypeTextPlain = "text/plain";

		public const string MIMETypeMessage = "message/rfc822";

		public const string MIMETypeImageJpeg = "image/jpeg";

		public const string MIMETypeImagePng = "image/png";

		public const string MIMETypeImageAll = "image/*";

		public const string ACTION_MAIN = "android.intent.action.MAIN";

		public const string ACTION_SEND = "android.intent.action.SEND";

		public const string ACTION_EDIT = "android.intent.action.EDIT";

		public const string ACTION_SENDTO = "android.intent.action.SENDTO";

		public const string ACTION_VIEW = "android.intent.action.VIEW";

		public const string ACTION_INSERT = "android.intent.action.INSERT";

		public const string ACTION_DELETE = "android.intent.action.DELETE";

		public const string ACTION_BATTERY_CHANGED = "android.intent.action.BATTERY_CHANGED";

		public const string ACTION_MEDIA_MOUNTED = "android.intent.action.MEDIA_MOUNTED";

		public const string ACTION_DIAL = "android.intent.action.DIAL";

		public const string ACTION_CALL = "android.intent.action.CALL";

		public const string EXTRA_TITLE = "android.intent.extra.TITLE";

		public const string EXTRA_SUBJECT = "android.intent.extra.SUBJECT";

		public const string EXTRA_TEXT = "android.intent.extra.TEXT";

		public const string EXTRA_EMAIL = "android.intent.extra.EMAIL";

		public const string EXTRA_CC = "android.intent.extra.CC";

		public const string EXTRA_BCC = "android.intent.extra.BCC";

		public const string EXTRA_STREAM = "android.intent.extra.STREAM";

		public const string CATEGORY_LAUNCHER = "android.intent.category.LAUNCHER";

		internal const string IntentClassSignature = "android.content.Intent";

		internal const string PutExtraMethodName = "putExtra";

		internal const string SetActionMethodName = "setAction";

		internal const string SetTypeMethodName = "setType";

		internal const string SetDataMethodName = "setData";

		internal const string SetDataAndTypeMethodName = "setDataAndType";

		internal const string SetClassNameMethodName = "setClassName";

		internal const string SetPackageMethodName = "setPackage";

		internal const string AddCategoryMethodName = "addCategory";

		private readonly AndroidJavaObject _intent;

		public AndroidJavaObject AJO => _intent;

		public AndroidIntent()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
			_intent = new AndroidJavaObject("android.content.Intent");
		}

		public AndroidIntent(string action)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
			_intent = new AndroidJavaObject("android.content.Intent", action);
		}

		public AndroidIntent(AndroidJavaObject cls)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
			_intent = new AndroidJavaObject("android.content.Intent", AGUtils.Activity, cls);
		}

		public AndroidIntent(string action, AndroidJavaObject uri)
			: this(action)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
			_intent = new AndroidJavaObject("android.content.Intent", action, uri);
		}

		public AndroidIntent(AndroidJavaObject activity, AndroidJavaObject next)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
			_intent = new AndroidJavaObject("android.content.Intent", activity, next);
		}

		public AndroidIntent PutExtra(string name, AndroidJavaObject value)
		{
			_intent.CallAJO("putExtra", name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, string value)
		{
			_intent.CallAJO("putExtra", name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, int value)
		{
			_intent.CallAJO("putExtra", name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, long value)
		{
			_intent.CallAJO("putExtra", name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, bool value)
		{
			_intent.CallAJO("putExtra", name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, string[] values)
		{
			_intent.CallAJO("putExtra", name, values);
			return this;
		}

		public AndroidIntent SetAction(string action)
		{
			_intent.CallAJO("setAction", action);
			return this;
		}

		public AndroidIntent SetType(string type)
		{
			_intent.CallAJO("setType", type);
			return this;
		}

		public AndroidIntent SetData(AndroidJavaObject uri)
		{
			_intent.CallAJO("setData", uri);
			return this;
		}

		public AndroidIntent SetDataAndType(AndroidJavaObject uri, string type)
		{
			_intent.CallAJO("setDataAndType", uri, type);
			return this;
		}

		public AndroidIntent AddCategory(string category)
		{
			_intent.CallAJO("addCategory", category);
			return this;
		}

		public AndroidIntent SetClassName(string packageName, string className)
		{
			_intent.CallAJO("setClassName", packageName, className);
			return this;
		}

		public AndroidIntent SetPackage(string packageName)
		{
			_intent.CallAJO("setPackage", packageName);
			return this;
		}

		public bool ResolveActivity()
		{
			using (AndroidJavaObject androidJavaObject = AGUtils.PackageManager)
			{
				try
				{
					_intent.CallAJO("resolveActivity", androidJavaObject).GetClassSimpleName();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public void Dispose()
		{
			AJO.Dispose();
		}
	}
}
