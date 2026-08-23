using Axlebolt.Bolt.Player;
using I2.Loc;

namespace Axlebolt.Standoff.Bolt
{
	public static class BoltPlayerExtension
	{
		public static string GetLocalizedStatus(this BoltPlayer player)
		{
			if (player.OnlineStatus == OnlineStatus.StateOffline)
			{
				return ScriptLocalization.PlayerStatus.Offline;
			}
			if (player.OnlineStatus == OnlineStatus.StateAway)
			{
				return ScriptLocalization.PlayerStatus.Away;
			}
			if (player.PlayInGame != null)
			{
				if (player.PlayInGame.PhotonGame != null)
				{
					return GetPlayingStatus(player.PlayInGame.PhotonGame.RoomId);
				}
				return string.IsNullOrEmpty(player.PlayInGame.LobbyId) ? ScriptLocalization.PlayerStatus.InGame : ScriptLocalization.PlayerStatus.InLobby;
			}
			return ScriptLocalization.PlayerStatus.Online;
		}

		private static string GetPlayingStatus(string roomId)
		{
			if (roomId.Contains("DeathMatch"))
			{
				return ScriptLocalization.PlayerStatus.Playing + " \"" + ScriptLocalization.GameMode.DeathMatch + "\"";
			}
			return ScriptLocalization.PlayerStatus.Playing + " \"" + ScriptLocalization.GameMode.Defuse + "\"";
		}
	}
}
