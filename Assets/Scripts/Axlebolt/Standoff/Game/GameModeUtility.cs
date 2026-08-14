using Axlebolt.Standoff.Common;

namespace Axlebolt.Standoff.Game
{
	public static class GameModeUtility
	{
		public static GameMode GetByName(string name)
		{
			return ResourcesUtility.Load<GameMode>("GameModes/" + name);
		}
	}
}
