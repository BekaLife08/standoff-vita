using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGMaps
	{
		public const int MinMapZoomLevel = 1;

		public const int MaxMapZoomLevel = 23;

		public const int DefaultMapZoomLevel = 7;

		private const string MapUriFormat = "geo:{0},{1}?z={2}";

		private const string MapUriFormatLabel = "geo:0,0?q={0},{1}({2})";

		private const string MapUriFormatAddress = "geo:0,0?q={0}";

		public static bool UserHasMapsApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW");
			AndroidJavaObject data = AndroidUri.Parse($"geo:{0},{0}?z={7}");
			return androidIntent.SetData(data).ResolveActivity();
		}

		public static void OpenMapLocation(float latitude, float longitude, int zoom = 7)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (latitude < -90f || latitude > 90f)
				{
					throw new ArgumentOutOfRangeException("latitude", "Latitude must be from -90.0 to 90.0.");
				}
				if (longitude < -180f || longitude > 180f)
				{
					throw new ArgumentOutOfRangeException("longitude", "Longitude must be from -180.0 to 180.0.");
				}
				if (zoom < 1 || zoom > 23)
				{
					throw new ArgumentOutOfRangeException("zoom", "Zoom level must be between 1 and 23");
				}
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW");
				AndroidJavaObject data = AndroidUri.Parse($"geo:{latitude},{longitude}?z={zoom}");
				androidIntent.SetData(data);
				AGUtils.StartActivity(androidIntent.AJO);
			}
		}

		public static void OpenMapLocationWithLabel(float latitude, float longitude, string label)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (latitude < -90f || latitude > 90f)
				{
					throw new ArgumentOutOfRangeException("latitude", "Latitude must be from -90.0 to 90.0.");
				}
				if (longitude < -180f || longitude > 180f)
				{
					throw new ArgumentOutOfRangeException("longitude", "Longitude must be from -180.0 to 180.0.");
				}
				if (string.IsNullOrEmpty(label))
				{
					throw new ArgumentException("Label must not be null or empty");
				}
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW");
				AndroidJavaObject data = AndroidUri.Parse($"geo:0,0?q={latitude},{longitude}({label})");
				androidIntent.SetData(data);
				AGUtils.StartActivity(androidIntent.AJO);
			}
		}

		public static void OpenMapLocation(string address)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (string.IsNullOrEmpty(address))
				{
					throw new ArgumentException("Address must not be null or empty");
				}
				address = WWW.EscapeURL(address);
				AndroidIntent androidIntent = new AndroidIntent("android.intent.action.VIEW");
				AndroidJavaObject data = AndroidUri.Parse($"geo:0,0?q={address}");
				androidIntent.SetData(data);
				AGUtils.StartActivity(androidIntent.AJO);
			}
		}
	}
}
