using DeadMosquito.AndroidGoodies.Internal;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGShare
	{
		private const string SmsUriFormat = "sms:{0}";

		private const string TwitterPackage = "com.twitter.android";

		private const string TweetActivity = "com.twitter.android.composer.ComposerShareActivity";

		private const string FbMessengerPackage = "com.facebook.orca";

		private const string WhatsAppPackage = "com.whatsapp";

		private const string InstagramPackage = "com.instagram.android";

		private const string TelegramChatPackage = "org.telegram.messenger";

		private const string ViberPackage = "com.viber.voip";

		private const string SnapChatPackage = "com.snapchat.android";

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache4;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache5;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache6;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache7;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache8;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cache9;

		[CompilerGenerated]
		private static Func<bool> _003C_003Ef__mg_0024cacheA;

		public static void ShareText(string subject, string body, bool withChooser = true, string chooserTitle = "Share via...")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SEND").SetType("text/plain");
				androidIntent.PutExtra("android.intent.extra.SUBJECT", subject);
				androidIntent.PutExtra("android.intent.extra.TEXT", body);
				if (withChooser)
				{
					AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
				}
				else
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		public static void ShareTextWithImage(string subject, string body, Texture2D image, bool withChooser = true, string chooserTitle = "Share via...")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				AndroidIntent androidIntent = new AndroidIntent().SetAction("android.intent.action.SEND").SetType("image/jpeg").PutExtra("android.intent.extra.SUBJECT", subject)
					.PutExtra("android.intent.extra.TEXT", body);
				AndroidJavaObject value = AndroidPersistanceUtilsInternal.SaveShareImageToExternalStorage(image);
				androidIntent.PutExtra("android.intent.extra.STREAM", value);
				if (withChooser)
				{
					AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
				}
				else
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		public static void ShareScreenshot(bool withChooser = true, string chooserTitle = "Share via...")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				GoodiesSceneHelper.Instance.SaveScreenshotToGallery(delegate(string uri)
				{
					AndroidIntent androidIntent = new AndroidIntent().SetAction("android.intent.action.SEND").SetType("image/jpeg");
					androidIntent.PutExtra("android.intent.extra.STREAM", AndroidUri.Parse(uri));
					if (withChooser)
					{
						AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
					}
					else
					{
						AGUtils.StartActivity(androidIntent.AJO);
					}
				});
			}
		}

		public static bool UserHasSmsApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return CreateSmsIntent("123123123", "dummy").ResolveActivity();
		}

		public static void SendSms(string phoneNumber, string message, bool withChooser = true, string chooserTitle = "Send SMS...")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = CreateSmsIntent(phoneNumber, message);
				if (withChooser)
				{
					AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
				}
				else
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		private static AndroidIntent CreateSmsIntent(string phoneNumber, string message)
		{
			AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW");
			if (AGDeviceInfo.SDK_INT >= 19)
			{
				AndroidJavaObject data = AndroidUri.Parse($"sms:{phoneNumber}");
				androidIntent.SetData(data);
			}
			else
			{
				androidIntent.SetType("vnd.android-dir/mms-sms");
				androidIntent.PutExtra("address", phoneNumber);
			}
			androidIntent.PutExtra("sms_body", message);
			return androidIntent;
		}

		public static void SendMms(string phoneNumber, string message, Texture2D image = null, bool withChooser = true, string chooserTitle = "Send MMS...")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SEND");
				androidIntent.PutExtra("address", phoneNumber);
				androidIntent.PutExtra("sms_body", message);
				if (image != null)
				{
					AndroidJavaObject value = AndroidPersistanceUtilsInternal.SaveShareImageToExternalStorage(image);
					androidIntent.PutExtra("android.intent.extra.STREAM", value);
					androidIntent.SetType("image/png");
				}
				if (withChooser)
				{
					AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
				}
				else
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		public static bool UserHasEmailApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return CreateEmailIntent(new string[1]
			{
				"dummy@gmail.com"
			}, "dummy", "dummy").ResolveActivity();
		}

		public static void SendEmail(string[] recipients, string subject, string body, Texture2D attachment = null, bool withChooser = true, string chooserTitle = "Send mail...", string[] cc = null, string[] bcc = null)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AndroidIntent androidIntent = CreateEmailIntent(recipients, subject, body, attachment, cc, bcc);
				if (withChooser)
				{
					AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
				}
				else
				{
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		private static AndroidIntent CreateEmailIntent(string[] recipients, string subject, string body, Texture2D attachment = null, string[] cc = null, string[] bcc = null)
		{
			AndroidJavaObject data = AndroidUri.Parse("mailto:");
			AndroidIntent androidIntent = new AndroidIntent().SetAction("android.intent.action.SENDTO").SetData(data).PutExtra("android.intent.extra.EMAIL", recipients)
				.PutExtra("android.intent.extra.SUBJECT", subject)
				.PutExtra("android.intent.extra.TEXT", body);
			if (cc != null)
			{
				androidIntent.PutExtra("android.intent.extra.CC", cc);
			}
			if (bcc != null)
			{
				androidIntent.PutExtra("android.intent.extra.BCC", bcc);
			}
			if (attachment != null)
			{
				AndroidJavaObject value = AndroidPersistanceUtilsInternal.SaveShareImageToExternalStorage(attachment);
				androidIntent.PutExtra("android.intent.extra.STREAM", value);
			}
			return androidIntent;
		}

		public static bool IsTwitterInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("com.twitter.android");
		}

		public static void Tweet(string tweet, Texture2D image = null)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}
			if (IsTwitterInstalled())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SEND").SetType("text/plain").PutExtra("android.intent.extra.TEXT", tweet).SetClassName("com.twitter.android", "com.twitter.android.composer.ComposerShareActivity");
				if (image != null)
				{
					AndroidJavaObject value = AndroidPersistanceUtilsInternal.SaveShareImageToExternalStorage(image);
					androidIntent.PutExtra("android.intent.extra.STREAM", value);
				}
				AGUtils.StartActivity(androidIntent.AJO);
			}
			else
			{
				Application.OpenURL("https://twitter.com/intent/tweet?text=" + WWW.EscapeURL(tweet));
			}
		}

		public static bool IsFacebookMessengerInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("com.facebook.orca");
		}

		public static void SendFacebookMessageText(string text)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				SendTextMessageGeneric(text, IsFacebookMessengerInstalled, "com.facebook.orca");
			}
		}

		public static void SendFacebookMessageImage(Texture2D image)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				SendImageGeneric(image, IsFacebookMessengerInstalled, "com.facebook.orca");
			}
		}

		public static bool IsWhatsAppInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("com.whatsapp");
		}

		public static void SendWhatsAppTextMessage(string text)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				SendTextMessageGeneric(text, IsWhatsAppInstalled, "com.whatsapp");
			}
		}

		public static void SendWhatsAppImageMessage(Texture2D image)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				SendImageGeneric(image, IsWhatsAppInstalled, "com.whatsapp");
			}
		}

		public static bool IsInstagramInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("com.instagram.android");
		}

		public static void ShareInstagramPhoto(Texture2D image)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				SendImageGeneric(image, IsInstagramInstalled, "com.instagram.android");
			}
		}

		public static bool IsTelegramInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("org.telegram.messenger");
		}

		public static void SendTelegramTextMessage(string text)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				SendTextMessageGeneric(text, IsTelegramInstalled, "org.telegram.messenger");
			}
		}

		public static void SendTelegramImageMessage(Texture2D image)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				SendImageGeneric(image, IsTelegramInstalled, "org.telegram.messenger");
			}
		}

		public static bool IsViberInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("com.viber.voip");
		}

		public static void SendViberTextMessage(string text)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				SendTextMessageGeneric(text, IsViberInstalled, "com.viber.voip");
			}
		}

		public static void SendViberImageMessage(Texture2D image)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				SendImageGeneric(image, IsViberInstalled, "com.viber.voip");
			}
		}

		public static bool IsSnapChatInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.IsPackageInstalled("com.snapchat.android");
		}

		public static void SendSnapChatTextMessage(string text)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				SendTextMessageGeneric(text, IsSnapChatInstalled, "com.snapchat.android");
			}
		}

		public static void SendSnapChatImageMessage(Texture2D image)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (image == null)
				{
					throw new ArgumentNullException("image", "Image must not be null");
				}
				SendImageGeneric(image, IsSnapChatInstalled, "com.snapchat.android");
			}
		}

		public static void ShareVideo(string videoPathOnDisc, string title, string chooserTitle = "Share Video")
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (!File.Exists(videoPathOnDisc))
				{
					UnityEngine.Debug.LogError("File (video) does not exist to share: " + videoPathOnDisc);
				}
				else
				{
					AGUtils.RunOnUiThread(delegate
					{
						AndroidPersistanceUtilsInternal.ScanFile(videoPathOnDisc, delegate(string path, AndroidJavaObject uri)
						{
							AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SEND");
							androidIntent.SetType("video/*");
							androidIntent.PutExtra("android.intent.extra.SUBJECT", title);
							androidIntent.PutExtra("android.intent.extra.STREAM", uri);
							AGUtils.StartActivityWithChooser(androidIntent.AJO, chooserTitle);
						});
					});
				}
			}
		}

		public static void SendTextMessageGeneric(string text, Func<bool> isInstalled, string package)
		{
			if (isInstalled())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SEND").SetType("text/plain").PutExtra("android.intent.extra.TEXT", text).SetPackage(package);
				AGUtils.StartActivity(androidIntent.AJO);
			}
			else
			{
				UnityEngine.Debug.Log($"Can't send message because [0] is not installed");
			}
		}

		public static void SendImageGeneric(Texture2D image, Func<bool> isInstalled, string package)
		{
			if (isInstalled())
			{
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.SEND").SetType("image/*").SetPackage(package);
				if (image != null)
				{
					AndroidJavaObject value = AndroidPersistanceUtilsInternal.SaveShareImageToExternalStorage(image);
					androidIntent.PutExtra("android.intent.extra.STREAM", value);
				}
				AGUtils.StartActivity(androidIntent.AJO);
			}
			else
			{
				UnityEngine.Debug.Log($"Can't send image because {package} is not installed");
			}
		}
	}
}
