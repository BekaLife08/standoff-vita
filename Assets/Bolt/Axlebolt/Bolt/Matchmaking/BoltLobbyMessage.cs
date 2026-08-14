namespace Axlebolt.Bolt.Matchmaking
{
	public class BoltLobbyMessage
	{
		public string SenderId { get; internal set; }

		public string Message { get; internal set; }

		public long Date { get; internal set; }

		public BoltLobbyMessage(string senderId, string message, long date)
		{
			SenderId = senderId;
			Message = message;
			Date = date;
		}
	}
}
