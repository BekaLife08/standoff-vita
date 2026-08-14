using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGDialer
	{
		public static bool UserHasPhoneApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.DIAL"))
			{
				return androidIntent.ResolveActivity();
			}
		}

		public static void OpenDialer(string phoneNumber)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.DIAL"))
				{
					androidIntent.SetData(ParsePhoneNumber(phoneNumber));
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		public static void PlacePhoneCall(string phoneNumber)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				using (AndroidIntent androidIntent = new AndroidIntent("android.intent.action.CALL"))
				{
					androidIntent.SetData(ParsePhoneNumber(phoneNumber));
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}

		private static AndroidJavaObject ParsePhoneNumber(string phoneNumber)
		{
			return AndroidUri.Parse("tel:" + phoneNumber);
		}
	}
}
