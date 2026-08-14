using UnityEngine;

namespace Heyzap
{
	public class HZOfferWallAdAndroid : MonoBehaviour
	{
		public static void ShowWithOptions(HZOfferWallShowOptions showOptions)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showOfferWall", showOptions.Tag);
				}
			}
		}

		public static void Fetch(string tag)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("fetchOfferWall", tag);
				}
			}
		}

		public static bool IsAvailable(string tag)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<bool>("isOfferWallAvailable", new object[1]
				{
					tag
				});
			}
		}

		public static void RequestDeltaOfCurrency(string currencyId)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("virtualCurrencyRequest", currencyId);
				}
			}
		}
	}
}
