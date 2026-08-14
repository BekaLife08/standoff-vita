using JetBrains.Annotations;
using System;

namespace Axlebolt.Standoff.Game.Event
{
	public class GameInitEventArgs
	{
		public string GameMode
		{
			get;
		}

		public int MaxPlayers
		{
			get;
		}

		public GameInitEventArgs([NotNull] string gameMode, int maxPlayers)
		{
			if (gameMode == null)
			{
				throw new ArgumentNullException("gameMode");
			}
			if (maxPlayers <= 0)
			{
				throw new ArgumentOutOfRangeException("maxPlayers");
			}
			GameMode = gameMode;
			MaxPlayers = maxPlayers;
		}
	}
}
