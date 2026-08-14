namespace Axlebolt.Bolt.Messages
{
	public class BoltChatEventArgs
	{
		public BoltChat BoltChat;

		public BoltChatEventArgs(BoltChat Chat)
		{
			BoltChat = Chat;
		}
	}
}
