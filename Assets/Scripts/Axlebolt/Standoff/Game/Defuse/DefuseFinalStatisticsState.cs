using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class DefuseFinalStatisticsState : FinalStatisticsState
	{
		public DefuseFinalStatisticsState(double duration)
			: base(duration)
		{
		}

		public override Hashtable Init()
		{
			Hashtable hashtable = base.Init();
			hashtable.SetFinalPlayers(PhotonNetwork.playerList);
			hashtable.SetFinalWinTeam(GetFinalWinTeam());
			return hashtable;
		}

		private FinalWinTeam GetFinalWinTeam()
		{
			int score = PhotonNetwork.room.GetScore(Team.Ct);
			int score2 = PhotonNetwork.room.GetScore(Team.Tr);
			if (score > score2)
			{
				return FinalWinTeam.Winner(Team.Ct);
			}
			return (score >= score2) ? FinalWinTeam.Draw() : FinalWinTeam.Winner(Team.Tr);
		}
	}
}
