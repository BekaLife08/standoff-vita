using System.Collections;
using UnityEngine;

namespace Heyzap
{
	public class HZOfferWallAd : MonoBehaviour
	{
		public delegate void AdDisplayListener(string state, string tag);

		public delegate void VirtualCurrencyResponseListener(VirtualCurrencyResponse response);

		public delegate void VirtualCurrencyErrorListener(string errorMsg);

		private static AdDisplayListener adDisplayListener;

		private static VirtualCurrencyResponseListener virtualCurrencyResponseListener;

		private static VirtualCurrencyErrorListener virtualCurrencyErrorListener;

		private static HZOfferWallAd _instance;

		public static void Show()
		{
			ShowWithOptions(null);
		}

		public static void ShowWithOptions(HZOfferWallShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZOfferWallShowOptions();
			}
			HZOfferWallAdAndroid.ShowWithOptions(showOptions);
		}

		public static void Fetch()
		{
			Fetch(null);
		}

		public static void Fetch(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			HZOfferWallAdAndroid.Fetch(tag);
		}

		public static bool IsAvailable()
		{
			return IsAvailable(null);
		}

		public static bool IsAvailable(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			return HZOfferWallAdAndroid.IsAvailable(tag);
		}

		public static void RequestDeltaOfCurrency(string currencyId)
		{
			HZOfferWallAdAndroid.RequestDeltaOfCurrency(currencyId);
		}

		public static void SetDisplayListener(AdDisplayListener listener)
		{
			adDisplayListener = listener;
		}

		public static void SetVirtualCurrencyResponseListener(VirtualCurrencyResponseListener listener)
		{
			virtualCurrencyResponseListener = listener;
		}

		public static void SetVirtualCurrencyErrorListener(VirtualCurrencyErrorListener listener)
		{
			virtualCurrencyErrorListener = listener;
		}

		public static void InitReceiver()
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("HZOfferWallAd");
				Object.DontDestroyOnLoad(gameObject);
				_instance = gameObject.AddComponent<HZOfferWallAd>();
			}
		}

		public void SetCallback(string message)
		{
			string[] array = message.Split(',');
			SetCallbackStateAndTag(array[0], array[1]);
		}

		protected static void SetCallbackStateAndTag(string state, string tag)
		{
			if (adDisplayListener != null)
			{
				adDisplayListener(state, tag);
			}
		}

		protected static IEnumerator InvokeCallbackNextFrame(string state, string tag)
		{
			yield return null;
			SetCallbackStateAndTag(state, tag);
		}

		public void VCSResponse(string jsonString)
		{
			SendVCSResponse(jsonString);
		}

		protected static void SendVCSResponse(string jsonString)
		{
			if (virtualCurrencyResponseListener != null)
			{
				VirtualCurrencyResponse response = JsonUtility.FromJson<VirtualCurrencyResponse>(jsonString);
				virtualCurrencyResponseListener(response);
			}
		}

		public void VCSError(string errorMsg)
		{
			SendVCSError(errorMsg);
		}

		protected static void SendVCSError(string errorMsg)
		{
			if (virtualCurrencyErrorListener != null)
			{
				virtualCurrencyErrorListener(errorMsg);
			}
		}

		protected static IEnumerator InvokeVCSErrorNextFrame(string errorMsg)
		{
			yield return null;
			SendVCSError(errorMsg);
		}
	}
}
