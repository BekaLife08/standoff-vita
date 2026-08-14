using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum LobbyType
	{
		[OriginalName("PRIVATE")]
		Private = 0,
		[OriginalName("FRIENDS_ONLY")]
		FriendsOnly = 1,
		[OriginalName("PUBLIC")]
		Public = 2,
		[OriginalName("INVISIBLE")]
		Invisible = 3
	}
}
