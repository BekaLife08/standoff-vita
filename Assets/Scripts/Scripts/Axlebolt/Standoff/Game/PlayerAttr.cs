using Axlebolt.Standoff.Main.Inventory;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class PlayerAttr
	{
		private static readonly string[] RandomNames = new string[4]
		{
			"ChuckNorris",
			"BruceWillis",
			"JackieChan",
			"Gaben"
		};

		public string Uid
		{
			get;
		}

		public string Name
		{
			get;
		}

		public byte[] Avatar
		{
			get;
		}

		public InventoryItemId BadgeId
		{
			get;
		}

		public PlayerAttr(string uid, string name, byte[] avatar, InventoryItemId badgeId)
		{
			Uid = uid;
			Name = name;
			Avatar = avatar;
			BadgeId = badgeId;
		}

		public static PlayerAttr Random()
		{
			string name = RandomNames[UnityEngine.Random.Range(0, RandomNames.Length)] + UnityEngine.Random.Range(0, 100);
			return new PlayerAttr("Unknown", name, null, InventoryItemId.None);
		}
	}
}
