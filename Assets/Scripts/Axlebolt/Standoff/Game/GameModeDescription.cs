using System.Collections.Generic;

namespace Axlebolt.Standoff.Game
{
	public class GameModeDescription
	{
		public readonly string name;

		public readonly string description;

		public readonly List<string> settings;

		public GameModeDescription(string name, string description, string[] settings)
		{
			this.name = name;
			this.description = description;
			this.settings = new List<string>(settings);
		}
	}
}
