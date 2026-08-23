using Axlebolt.Bolt;
using Axlebolt.Bolt.Analytics;
using Axlebolt.Standoff.Photon;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Analytics
{
	public static class AnalyticsManager
	{
		private const string Photon = "photon";

		private const string BoltAuth = "bolt_auth";

		private const string Main = "main";

		public static void MainInitError(string exceptionType)
		{
			BoltService<BoltAnalytics>.Instance.SendEvent("main", exceptionType);
		}

		public static void BoltAuthError(string exceptionType)
		{
			BoltService<BoltAnalytics>.Instance.SendEvent("bolt_auth", exceptionType);
		}

		public static void PhotonMatchmakingError(Exception exception)
		{
			try
			{
				if (!(exception is OperationCanceledException))
				{
					if (exception is PhotonCreateRoomException)
					{
						PhotonCreateRoomException ex = (PhotonCreateRoomException)exception;
						PhotonCreateRoomError(Parse(ex.Code));
					}
					else if (exception is PhotonJoinRoomException)
					{
						PhotonJoinRoomException ex2 = (PhotonJoinRoomException)exception;
						PhotonCreateRoomError(Parse(ex2.Code));
					}
					else
					{
						PhotonUnknownError(exception);
					}
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		public static void PhotonCreateRoomError(string errorCode)
		{
			BoltService<BoltAnalytics>.Instance.SendEvent("photon", $"PhotonCreateRoomException({errorCode})");
		}

		public static void PhotonJoinRoomError(string errorCode)
		{
			BoltService<BoltAnalytics>.Instance.SendEvent("photon", $"PhotonJoinRoomException({errorCode})");
		}

		public static void PhotonUnknownError(Exception ex)
		{
			BoltService<BoltAnalytics>.Instance.SendEvent("photon", ex.GetType().Name);
		}

		private static string Parse(short errorCode)
		{
			try
			{
				PhotonErrorCode photonErrorCode = (PhotonErrorCode)errorCode;
				return photonErrorCode.ToString();
			}
			catch (Exception)
			{
				return "Unknown" + errorCode;
			}
		}
	}
}
