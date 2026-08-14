using System;

namespace Axlebolt.Standoff.Core
{
	public static class TeamExtension
	{
		public static Team GetOtherTeam(this Team team)
		{
			int result;
			switch (team)
			{
			case Team.None:
				throw new Exception("Team.None is not supported");
			case Team.Ct:
				result = 1;
				break;
			default:
				result = 2;
				break;
			}
			return (Team)result;
		}
	}
}
