using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Game
{
	public class FinalWinTeam
	{
		public bool IsDraw
		{
			get;
		}

		public bool IsGiveUp
		{
			get;
		}

		public Team Team
		{
			get;
		}

		public FinalWinTeam(bool isDraw, bool isGiveUp, Team team)
		{
			IsDraw = isDraw;
			IsGiveUp = isGiveUp;
			Team = team;
		}

		public static FinalWinTeam Draw()
		{
			return new FinalWinTeam(true, false, Team.None);
		}

		public static FinalWinTeam GiveUp(Team winTeam)
		{
			return new FinalWinTeam(false, true, winTeam);
		}

		public static FinalWinTeam Winner(Team winTeam)
		{
			return new FinalWinTeam(false, false, winTeam);
		}
	}
}
