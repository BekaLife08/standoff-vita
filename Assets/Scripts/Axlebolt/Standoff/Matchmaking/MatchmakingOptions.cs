namespace Axlebolt.Standoff.Matchmaking
{
	public abstract class MatchmakingOptions
	{
		public string PlayerId
		{
			get;
			set;
		}

		public string LobbyName
		{
			get;
			set;
		}

		public int TimeInGame
		{
			get;
			set;
		} = -1;

	}
}
