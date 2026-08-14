using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal static class AndroidPendingIntent
	{
		private const int FLAG_UPDATE_CURRENT = 134217728;

		public static AndroidJavaObject GetActivity(AndroidJavaObject intent)
		{
			using (AndroidJavaClass ajc = new AndroidJavaClass("android.app.PendingIntent"))
			{
				return ajc.CallStaticAJO("getActivity", AGUtils.Activity, 0, intent, 134217728);
			}
		}
	}
}
