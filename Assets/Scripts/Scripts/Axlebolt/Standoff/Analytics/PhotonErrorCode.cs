using System;

namespace Axlebolt.Standoff.Analytics
{
	public enum PhotonErrorCode
	{
		Ok = 0,
		OperationNotAllowedInCurrentState = -3,
		[Obsolete("Use InvalidOperation.")]
		InvalidOperationCode = -2,
		InvalidOperation = -2,
		InternalServerError = -1,
		InvalidAuthentication = 0x7FFF,
		GameIdAlreadyExists = 32766,
		GameFull = 32765,
		GameClosed = 32764,
		[Obsolete("No longer used, cause random matchmaking is no longer a process.")]
		AlreadyMatched = 32763,
		ServerFull = 32762,
		UserBlocked = 32761,
		NoRandomMatchFound = 32760,
		GameDoesNotExist = 32758,
		MaxCcuReached = 32757,
		InvalidRegion = 32756,
		CustomAuthenticationFailed = 32755,
		AuthenticationTicketExpired = 32753,
		PluginReportedError = 32752,
		PluginMismatch = 32751,
		JoinFailedPeerAlreadyJoined = 32750,
		JoinFailedFoundInactiveJoiner = 32749,
		JoinFailedWithRejoinerNotFound = 32748,
		JoinFailedFoundExcludedUserId = 32747,
		JoinFailedFoundActiveJoiner = 32746,
		HttpLimitReached = 32745,
		ExternalHttpCallFailed = 32744,
		SlotError = 32742,
		InvalidEncryptionParameters = 32741
	}
}
