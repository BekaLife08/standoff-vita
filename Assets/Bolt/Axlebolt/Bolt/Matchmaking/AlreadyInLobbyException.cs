namespace Axlebolt.Bolt.Matchmaking
{
	public class AlreadyInLobbyException : BoltApiException
	{
		public AlreadyInLobbyException()
			: base("Unsupported operation. Leave lobby first!")
		{
		}
	}
}
