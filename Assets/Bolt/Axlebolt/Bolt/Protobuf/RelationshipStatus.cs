using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum RelationshipStatus
	{
		[OriginalName("None")]
		None = 0,
		[OriginalName("Blocked")]
		Blocked = 1,
		[OriginalName("RequestRecipient")]
		RequestRecipient = 2,
		[OriginalName("Friend")]
		Friend = 3,
		[OriginalName("RequestInitiator")]
		RequestInitiator = 4,
		[OriginalName("Ignored")]
		Ignored = 5
	}
}
