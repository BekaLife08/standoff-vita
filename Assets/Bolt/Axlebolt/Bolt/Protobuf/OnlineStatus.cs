using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum OnlineStatus
	{
		[OriginalName("StateOffline")]
		StateOffline = 0,
		[OriginalName("StateOnline")]
		StateOnline = 1,
		[OriginalName("StateBusy")]
		StateBusy = 2,
		[OriginalName("StateAway")]
		StateAway = 3,
		[OriginalName("StateSnooze")]
		StateSnooze = 4,
		[OriginalName("StateLookingToTrade")]
		StateLookingToTrade = 5,
		[OriginalName("StateLookingToPlay")]
		StateLookingToPlay = 6
	}
}
