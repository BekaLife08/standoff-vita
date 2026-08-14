namespace Axlebolt.Bolt.Player
{
	public class PlayerStatus
	{
		public PlayInGame PlayInGame { get; internal set; }

		public OnlineStatus OnlineStatus { get; internal set; }

		public PlayerStatus()
		{
		}

		public PlayerStatus(OnlineStatus onlineStatus)
		{
			OnlineStatus = onlineStatus;
		}

		public PlayerStatus(PlayInGame playInGame, OnlineStatus onlineStatus)
		{
			PlayInGame = playInGame;
			OnlineStatus = onlineStatus;
		}
	}
}
