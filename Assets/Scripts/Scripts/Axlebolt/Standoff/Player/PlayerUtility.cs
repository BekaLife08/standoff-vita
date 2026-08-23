using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Player
{
	public class PlayerUtility
	{
		private const char Split = '_';

		public static string CreateId(Team team, string character)
		{
			return team.ToString() + '_' + character;
		}

		public static void ParseId(string id, out Team team, out string character)
		{
			string[] array = id.Split('_');
			if (array.Length != 2)
			{
				throw new InvalidOperationException("Invalid ID (" + id + ")");
			}
			team = (Team)Enum.Parse(typeof(Team), array[0]);
			character = array[1];
		}

		public static BipedMap LoadArms(string arms)
		{
			string path = "Arms/" + arms + "/" + arms;
			return ResourcesUtility.Load<BipedMap>(path);
		}

		public static Dictionary<string, BipedMap> LoadCharacters(PlayerCharacters characters)
		{
			Dictionary<string, BipedMap> dictionary = new Dictionary<string, BipedMap>();
			string[] skins = characters.Skins;
			foreach (string text in skins)
			{
				string path = "Characters/" + characters.Character + "/" + text;
				dictionary[text] = ResourcesUtility.Load<BipedMap>(path);
			}
			return dictionary;
		}

		public static ArmsMaterial LoadArmsPbrMaterial(string arms)
		{
			return ResourcesUtility.Load<ArmsMaterial>("Arms/" + arms + "/" + arms + "_PBR");
		}

		public static ArmsMaterial LoadArmsBumpedSpecular(string arms)
		{
			return ResourcesUtility.Load<ArmsMaterial>("Arms/" + arms + "/" + arms + "_BumpedSpecular");
		}

		public static ArmsMaterial LoadArmsBumpedDiffuse(string arms)
		{
			return ResourcesUtility.Load<ArmsMaterial>("Arms/" + arms + "/" + arms + "_BumpedDiffuse");
		}

		public static ArmsMaterial LoadArmsDiffuseMaterial(string arms)
		{
			return ResourcesUtility.Load<ArmsMaterial>("Arms/" + arms + "/" + arms + "_Diffuse");
		}

		public static PlayerController LoadPlayerPrefab()
		{
			return ResourcesUtility.Load<PlayerController>("Player/Player");
		}

		public static PlayerHitboxConfig LoadPlayerHitboxConfig()
		{
			return ResourcesUtility.Load<PlayerHitboxConfig>("Player/PlayerHitboxConfig");
		}
	}
}
