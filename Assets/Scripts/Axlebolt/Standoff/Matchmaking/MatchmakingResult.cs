namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingResult
	{
		public string Region
		{
			get;
		}

		public string RoomId
		{
			get;
		}

		public MatchmakingResult(string region, string roomId)
		{
			Region = region;
			RoomId = roomId;
		}
	}
}
