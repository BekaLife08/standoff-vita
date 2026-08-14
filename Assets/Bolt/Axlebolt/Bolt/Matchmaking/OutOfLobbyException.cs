namespace Axlebolt.Bolt.Matchmaking
{
	public class OutOfLobbyException : BoltApiException
	{
		public OutOfLobbyException()
			: base("Unsupported operation. Create/join lobby first!")
		{
		}
	}
}
