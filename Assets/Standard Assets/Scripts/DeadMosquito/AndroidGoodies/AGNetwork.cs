using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGNetwork
	{
		public class WifiInfo
		{
			public string BSSID
			{
				get;
				set;
			}

			public string SSID
			{
				get;
				set;
			}

			public string MacAddress
			{
				get;
				set;
			}

			public int LinkSpeed
			{
				get;
				set;
			}

			public int IpAddress
			{
				get;
				set;
			}

			public int NetworkId
			{
				get;
				set;
			}

			public int Rssi
			{
				get;
				set;
			}

			public override string ToString()
			{
				return $"[WifiInfo: BSSID={BSSID}, SSID={SSID}, MacAddress={MacAddress}, LinkSpeed={LinkSpeed} Mbps, IpAddress={IpAddress}, NetworkId={NetworkId}, Rssi={Rssi}]";
			}
		}

		private const int TYPE_MOBILE = 0;

		private const int TYPE_WIFI = 1;

		public static bool IsInternetAvailable()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			AndroidJavaObject androidJavaObject;
			try
			{
				androidJavaObject = AGSystemService.ConnectivityService.CallAJO("getActiveNetworkInfo");
			}
			catch (Exception)
			{
				return false;
			}
			return androidJavaObject.Call<bool>("isConnected", Array.Empty<object>());
		}

		public static bool IsWifiEnabled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			try
			{
				return AGSystemService.WifiService.Call<bool>("isWifiEnabled", Array.Empty<object>());
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("Failed to check if wi-fi is enabled. Error: " + ex.Message);
				}
				return false;
			}
		}

		public static bool IsWifiConnected()
		{
			return IsNetworkConnected(1);
		}

		public static bool IsMobileConnected()
		{
			return IsNetworkConnected(0);
		}

		private static bool IsNetworkConnected(int networkType)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			AndroidJavaObject androidJavaObject;
			try
			{
				androidJavaObject = AGSystemService.ConnectivityService.CallAJO("getNetworkInfo", networkType);
			}
			catch (Exception)
			{
				return false;
			}
			return androidJavaObject.Call<bool>("isConnected", Array.Empty<object>());
		}

		public static WifiInfo GetWifiConnectionInfo()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			using (AndroidJavaObject androidJavaObject = GetWifiInfoAJO())
			{
				if (androidJavaObject == null)
				{
					return null;
				}
				WifiInfo wifiInfo = new WifiInfo();
				wifiInfo.BSSID = androidJavaObject.CallStr("getBSSID");
				wifiInfo.SSID = androidJavaObject.CallStr("getSSID");
				wifiInfo.MacAddress = androidJavaObject.CallStr("getMacAddress");
				wifiInfo.LinkSpeed = androidJavaObject.CallInt("getLinkSpeed");
				wifiInfo.NetworkId = androidJavaObject.CallInt("getNetworkId");
				wifiInfo.IpAddress = androidJavaObject.CallInt("getIpAddress");
				wifiInfo.Rssi = androidJavaObject.CallInt("getRssi");
				return wifiInfo;
			}
		}

		public static int GetWifiSignalLevel()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}
			WifiInfo wifiConnectionInfo = GetWifiConnectionInfo();
			return (wifiConnectionInfo != null) ? AGSystemService.WifiService.CallStatic<int>("calculateSignalLevel", new object[2]
			{
				wifiConnectionInfo.Rssi,
				100
			}) : 0;
		}

		private static AndroidJavaObject GetWifiInfoAJO()
		{
			try
			{
				return AGSystemService.WifiService.CallAJO("getConnectionInfo");
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("Failed to get wifi info. Error: " + ex.Message);
				}
				return null;
			}
		}
	}
}
