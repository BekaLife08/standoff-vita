using Axlebolt.Standoff.Core;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Main
{
	public class VideoAdsManager : Singleton<VideoAdsManager>
	{
		private static readonly Log Log = Log.Create(typeof(VideoAdsManager));

		private bool _initialized;

		public void Init()
		{
			_initialized = true;
			Log.Debug("VideoAdsManager: ads disabled on PSP2");
		}

		public bool IsInitialized()
		{
			return _initialized;
		}

		public void ShowInMenu()
		{
			Log.Debug("VideoAdsManager.ShowInMenu: no-op on PSP2");
		}
	}
}
