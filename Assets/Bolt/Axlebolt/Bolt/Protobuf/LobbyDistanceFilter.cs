using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum LobbyDistanceFilter
	{
		[OriginalName("CLOSE")]
		Close = 0,
		[OriginalName("DEFAULT")]
		Default = 1,
		[OriginalName("FAR")]
		Far = 2,
		[OriginalName("WORLDWIDE")]
		Worldwide = 3
	}
}
