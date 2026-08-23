using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Level;
using JetBrains.Annotations;
using System;

namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingFilter : MatchmakingOptions
	{
		public GameMode GameMode
		{
			get;
		}

		public LevelDefinition[] Levels
		{
			get;
			set;
		} = new LevelDefinition[0];


		public string[] ExpectedPlayers
		{
			get;
			set;
		}

		public bool AllowEmptyRoom
		{
			get;
			set;
		}

		public MatchmakingFilter([NotNull] GameMode gameMode)
		{
			if (gameMode == null)
			{
				throw new ArgumentNullException("gameMode");
			}
			GameMode = gameMode;
		}
	}
}
