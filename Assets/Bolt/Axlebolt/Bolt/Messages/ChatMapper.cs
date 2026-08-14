using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Messages
{
	public class ChatMapper : MessageMapper<ChatUser, BoltChat>
	{
		public static readonly ChatMapper Instance = new ChatMapper();

		public override BoltChat ToOriginal(ChatUser proto)
		{
			BoltFriend friend = FriendMapper.Instance.ToOriginal(proto.Player);
			return new BoltChat(friend, proto.Message, proto.Timestamp, proto.UnreadMsgsCount);
		}

		public BoltChat ToOriginal(ChatUser proto, bool isGroup)
		{
			BoltFriend friend = FriendMapper.Instance.ToOriginal(proto.Player);
			if (!isGroup)
			{
				return new BoltChat(friend, proto.Message, proto.Timestamp, proto.UnreadMsgsCount);
			}
			return new BoltChat(friend, proto.Message, proto.Timestamp, proto.UnreadMsgsCount);
		}
	}
}
