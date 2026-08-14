using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Messages
{
	public class BoltUserMessage
	{
		public BoltFriend Sender { get; internal set; }

		public string Message { get; internal set; }

		public long Timestamp { get; internal set; }

		public bool IsRead { get; internal set; }
	}
}
