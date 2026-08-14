using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGTelephony
	{
		public static string TelephonyDeviceId => CallTelephonyMethod<string>("getDeviceId", Array.Empty<object>());

		public static string TelephonySimSerialNumber => CallTelephonyMethod<string>("getSimSerialNumber", Array.Empty<object>());

		public static string NetworkCountryIso
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return string.Empty;
				}
				return AGSystemService.TelephonyService.CallStr("getNetworkCountryIso");
			}
		}

		private static T CallTelephonyMethod<T>(string methodName, params object[] args)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return default(T);
			}
			try
			{
				return AGSystemService.TelephonyService.Call<T>(methodName, args);
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogWarning("Could not call method : " + methodName + ". Check the device API level if the property is present, reason: " + ex.Message);
				}
				return default(T);
			}
		}
	}
}
