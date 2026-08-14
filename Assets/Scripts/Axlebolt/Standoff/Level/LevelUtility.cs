using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Level
{
	public class LevelUtility
	{
		private static readonly Log Log = Log.Create(typeof(LevelUtility));

		[CompilerGenerated]
		private static Func<LevelDefinition, bool> _003C_003Ef__mg_0024cache0;

		public static LevelDefinition[] GetLevels(string gameMode)
		{
			return (from level in GetLevels()
				where level.GameModes != null && level.GameModes.Contains(gameMode)
				select level).ToArray();
		}

		public static LevelDefinition GetRandomLevel(string gameMode)
		{
			LevelDefinition[] levels = GetLevels(gameMode);
			return levels[UnityEngine.Random.Range(0, levels.Length)];
		}

		public static LevelDefinition[] GetLevels(string[] levelNames)
		{
			return (from level in GetLevels()
				where levelNames.Contains(level.name)
				select level).ToArray();
		}

		public static LevelDefinition GetLevel(string levelName)
		{
			LevelDefinition levelDefinition = ResourcesUtility.Load<LevelDefinition>("Levels/" + levelName);
			if (IsValidLevel(levelDefinition))
			{
				return levelDefinition;
			}
			throw new ArgumentException($"{levelName} is not valid");
		}

		public static LevelDefinition[] GetLevels()
		{
			return Resources.LoadAll<LevelDefinition>("Levels").Where(IsValidLevel).ToArray();
		}

		public static bool IsValidLevel(LevelDefinition levelDefinition)
		{
			if (string.IsNullOrEmpty(levelDefinition.SceneName))
			{
				Log.Error($"Invalid {levelDefinition.Name}, SceneName is empty");
				return false;
			}
			if (levelDefinition.CtCharacters == null || levelDefinition.CtCharacters.Skins.Length == 0)
			{
				Log.Error($"Invalid {levelDefinition.Name}, CtCharacters is empty");
				return false;
			}
			if (levelDefinition.TrCharacters == null || levelDefinition.TrCharacters.Skins.Length == 0)
			{
				Log.Error($"Invalid {levelDefinition.Name}, TrCharacters is empty");
				return false;
			}
			return true;
		}
	}
}
