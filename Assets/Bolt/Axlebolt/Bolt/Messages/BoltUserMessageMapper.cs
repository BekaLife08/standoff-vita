using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Messages
{
	public class BoltUserMessageMapper : MessageMapper<UserMessage, BoltUserMessage>
	{
		public static readonly BoltUserMessageMapper Instance = new BoltUserMessageMapper();

		public override BoltUserMessage ToOriginal(UserMessage proto)
		{
			return new BoltUserMessage
			{
				Message = proto.Message,
				Timestamp = proto.Timestamp,
				IsRead = proto.IsRead
			};
		}

		public BoltUserMessage ToOriginal(UserMessage proto, BoltFriend boltFriend)
		{
			return new BoltUserMessage
			{
				Sender = boltFriend,
				Message = proto.Message,
				Timestamp = proto.Timestamp,
				IsRead = proto.IsRead
			};
		}
	}
}
