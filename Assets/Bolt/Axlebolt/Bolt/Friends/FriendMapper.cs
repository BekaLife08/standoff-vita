using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Friends
{
	public class FriendMapper : MessageMapper<PlayerFriend, BoltFriend>
	{
		public static readonly FriendMapper Instance = new FriendMapper();

		public override BoltFriend ToOriginal(PlayerFriend proto)
		{
			BoltFriend boltFriend = PlayerMapper.Instance.ToOriginal(proto.Player, new BoltFriend());
			boltFriend.Relationship = EnumMapper<Axlebolt.Bolt.Protobuf.RelationshipStatus, RelationshipStatus>.ToOriginal(proto.RelationshipStatus);
			return boltFriend;
		}
	}
}
