namespace Axlebolt.Bolt.Messages
{
	public class BoltUserMessagesEventArgs
	{
		public BoltUserMessage BoltUserMessage;

		public BoltUserMessagesEventArgs(BoltUserMessage userMessage)
		{
			BoltUserMessage = userMessage;
		}
	}
}
