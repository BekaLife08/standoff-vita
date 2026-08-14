using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Game
{
	public class WinTeam
	{
		public Team Team
		{
			get;
		}

		public PhotonPlayer MvpPlayer
		{
			get;
		}

		public byte MvpCode
		{
			get;
		}

		public WinTeam(Team winTeam, PhotonPlayer mvpPlayer, byte mvpCode)
		{
			Team = winTeam;
			MvpPlayer = mvpPlayer;
			MvpCode = mvpCode;
		}
	}
}
