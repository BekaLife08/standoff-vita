using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum AuthType
	{
		[OriginalName("TEST")]
		Test = 0,
		[OriginalName("GUEST")]
		Guest = 1,
		[OriginalName("GOOGLE_PLAY")]
		GooglePlay = 2,
		[OriginalName("GAME_CENTER")]
		GameCenter = 3,
		[OriginalName("FACEBOOK")]
		Facebook = 4
	}
}
