using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGGPS
	{
		public sealed class Location
		{
			private readonly double _latitude;

			private readonly double _longitude;

			private readonly bool _hasAccuracy;

			private readonly float _accuracy;

			private readonly long _timestamp;

			public double Latitude => _latitude;

			public double Longitude => _longitude;

			public bool HasAccuracy => _hasAccuracy;

			public float Accuracy => _accuracy;

			public bool HasSpeed
			{
				get;
				set;
			}

			public float Speed
			{
				get;
				set;
			}

			public bool HasBearing
			{
				get;
				set;
			}

			public float Bearing
			{
				get;
				set;
			}

			public bool IsFromMockProvider
			{
				get;
				set;
			}

			public long Timestamp => _timestamp;

			public Location(double latitude, double longitude, bool hasAccuracy, float accuracy, long timestamp)
			{
				_latitude = latitude;
				_longitude = longitude;
				_hasAccuracy = hasAccuracy;
				_accuracy = accuracy;
				_timestamp = timestamp;
			}

			public float DistanceTo(Location destination)
			{
				float[] array = new float[1];
				DistanceBetween(_latitude, _longitude, destination.Latitude, destination.Longitude, array);
				return array[0];
			}

			public override string ToString()
			{
				return $"[Location: Latitude={Latitude}, Longitude={Longitude}, HasAccuracy={HasAccuracy}, Accuracy={Accuracy}, Timestamp={Timestamp}, HasSpeed={HasSpeed}, Speed={Speed}, HasBearing={HasBearing}, Bearing={Bearing}, IsFromMockProvider={IsFromMockProvider}]";
			}
		}

		private class LocationListenerProxy : AndroidJavaProxy
		{
			private readonly Action<Location> _onLocationChanged;

			private static bool thatWasMe;

			public LocationListenerProxy(Action<Location> onLocationChanged)
				: base("android.location.LocationListener")
			{
				_onLocationChanged = onLocationChanged;
			}

			private void onLocationChanged(AndroidJavaObject locationAJO)
			{
				Location location = LocationFromAJO(locationAJO);
				GoodiesSceneHelper.Queue(delegate
				{
					_onLocationChanged(location);
				});
			}

			private void onProviderDisabled(string provider)
			{
			}

			private void onProviderEnabled(string provider)
			{
			}

			private void onStatusChanged(string provider, int status, AndroidJavaObject extras)
			{
			}

			private new int hashCode()
			{
				thatWasMe = true;
				return GetHashCode();
			}

			private new bool equals(AndroidJavaObject o)
			{
				thatWasMe = false;
				o.Call<int>("hashCode", Array.Empty<object>());
				return thatWasMe;
			}
		}

		private const string GPS_PROVIDER = "gps";

		private static LocationListenerProxy _currentListener;

		public static void DistanceBetween(double startLatitude, double startLongitude, double endLatitude, double endLongitude, float[] results)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (results == null || results.Length < 1)
				{
					throw new ArgumentException("results is null or has length < 1");
				}
				LocationUtils.ComputeDistanceAndBearing(startLatitude, startLongitude, endLatitude, endLongitude, results);
			}
		}

		public static bool DeviceHasGPS()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.SystemFeatures.HasSystemFeature("android.hardware.location.gps");
		}

		public static bool IsGPSEnabled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGSystemService.LocationService.Call<bool>("isProviderEnabled", new object[1]
			{
				"gps"
			});
		}

		public static Location GetLastKnownGPSLocation()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			try
			{
				AndroidJavaObject locationAJO = AGSystemService.LocationService.Call<AndroidJavaObject>("getLastKnownLocation", new object[1]
				{
					"gps"
				});
				return LocationFromAJO(locationAJO);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void RequestLocationUpdates(long minTime, float minDistance, Action<Location> onLocationChangedCallback)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (minTime <= 0)
				{
					throw new ArgumentOutOfRangeException("minTime", "Time cannot be less then zero");
				}
				if (minDistance <= 0f)
				{
					throw new ArgumentOutOfRangeException("minDistance", "minDistance cannot be less then zero");
				}
				if (onLocationChangedCallback == null)
				{
					throw new ArgumentNullException("onLocationChangedCallback", "Location changed callback cannot be null");
				}
				_currentListener = new LocationListenerProxy(onLocationChangedCallback);
				try
				{
					AGUtils.RunOnUiThread(delegate
					{
						AGSystemService.LocationService.Call("requestLocationUpdates", "gps", minTime, minDistance, _currentListener);
					});
				}
				catch (Exception ex)
				{
					if (Debug.isDebugBuild)
					{
						UnityEngine.Debug.LogWarning("Failed to register for location updates. Current device probably does not have GPS. Please check if device has GPS before invoking this method. " + ex.Message);
					}
				}
			}
		}

		public static void RemoveUpdates()
		{
			if (!AGUtils.IsNotAndroidCheck() && _currentListener != null)
			{
				AGUtils.RunOnUiThread(delegate
				{
					AGSystemService.LocationService.Call("removeUpdates", _currentListener);
				});
			}
		}

		private static Location LocationFromAJO(AndroidJavaObject locationAJO)
		{
			using (locationAJO)
			{
				double latitude = locationAJO.Call<double>("getLatitude", Array.Empty<object>());
				double longitude = locationAJO.Call<double>("getLongitude", Array.Empty<object>());
				bool hasAccuracy = locationAJO.Call<bool>("hasAccuracy", Array.Empty<object>());
				float accuracy = locationAJO.Call<float>("getAccuracy", Array.Empty<object>());
				long timestamp = locationAJO.Call<long>("getTime", Array.Empty<object>());
				bool flag = locationAJO.CallBool("hasSpeed");
				float speed = locationAJO.Call<float>("getSpeed", Array.Empty<object>());
				bool flag2 = locationAJO.CallBool("hasBearing");
				float bearing = locationAJO.Call<float>("getBearing", Array.Empty<object>());
				Location location = new Location(latitude, longitude, hasAccuracy, accuracy, timestamp);
				if (flag)
				{
					location.HasSpeed = true;
					location.Speed = speed;
				}
				if (flag2)
				{
					location.HasBearing = true;
					location.Bearing = bearing;
				}
				bool isFromMockProvider = false;
				try
				{
					isFromMockProvider = locationAJO.CallBool("isFromMockProvider");
				}
				catch (Exception)
				{
				}
				location.IsFromMockProvider = isFromMockProvider;
				return location;
			}
		}
	}
}
