namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingJoinOptions : MatchmakingOptions
	{
		public string GameId
		{
			get;
		}

		public MatchmakingJoinOptions(string gameId)
		{
			GameId = gameId;
		}
	}
}
