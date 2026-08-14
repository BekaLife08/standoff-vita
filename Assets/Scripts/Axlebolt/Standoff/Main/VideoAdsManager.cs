using Axlebolt.Standoff.Core;
using Heyzap;
using System;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Main
{
	public class VideoAdsManager : Singleton<VideoAdsManager>
	{
		private static readonly Log Log = Log.Create(typeof(VideoAdsManager));

		private const int MinTimeInSeconds = 120;

		private DateTime _lastShowTime = DateTime.MinValue;

		private bool _initialized;

		public void Init()
		{
			if (_initialized)
			{
				throw new Exception(string.Format("{0} already initialized", "VideoAdsManager"));
			}
			if (Application.isMobilePlatform)
			{
				HeyzapAds.Start("f6fbdb0cbfd9ae5177cd4a7b84246685", 0);
				HZIncentivizedAd.AdDisplayListener displayListener = delegate(string adState, string adTag)
				{
					Log.Debug($"AdDisplayListener {adState}");
					if (adState.Equals("incentivized_result_complete"))
					{
						_lastShowTime = DateTime.Now;
					}
					if (adState.Equals("incentivized_result_incomplete"))
					{
					}
					if (!HZIncentivizedAd.IsAvailable())
					{
						HZIncentivizedAd.Fetch();
					}
				};
				HZIncentivizedAd.SetDisplayListener(displayListener);
				HZIncentivizedAd.Fetch();
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				_initialized = true;
				Log.Debug(string.Format("{0} initialized successfully", "VideoAdsManager"));
			}
		}

		public bool IsInitialized()
		{
			return _initialized;
		}

		public void ShowInMenu()
		{
			if (!_initialized)
			{
				throw new Exception(string.Format("{0} is not initialized", "VideoAdsManager"));
			}
			if (Application.isMobilePlatform)
			{
				Log.Debug("Trying show video ads");
				double totalSeconds = (DateTime.Now - _lastShowTime).TotalSeconds;
				if (totalSeconds < 120.0)
				{
					Log.Debug($"VideoAds time has not come ({totalSeconds} < {120})");
				}
				else if (!HZIncentivizedAd.IsAvailable())
				{
					Log.Debug("VideoAds is not available, trying fetch");
					HZIncentivizedAd.Fetch();
				}
				else
				{
					StartCoroutine(ShowNextFrame());
				}
			}
		}

		private IEnumerator ShowNextFrame()
		{
			yield return new WaitForSeconds(1f);
			Log.Debug("VideoAds showing");
			HZIncentivizedAd.Show();
		}
	}
}
