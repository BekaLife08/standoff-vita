using Axlebolt.Standoff.Core;
using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Player
{
	public static class PhotonPlayerExtensions
	{
		private static readonly Log Log = Log.Create(typeof(PhotonPlayerExtensions));

		public const string HealthProp = "health";

		public const string ArmorProp = "armor";

		public const string IsDeadProp = "isDeath";

		public const string DeathTime = "DeathTime";

		public const string HelmetProp = "helmet";

		public static T GetProperty<T>(this PhotonPlayer player, string property, T defaultValue)
		{
			if (player.CustomProperties.TryGetNonNullValue(property, out object result))
			{
				return (T)result;
			}
			return defaultValue;
		}

		public static void SetHealth(this PhotonPlayer player, int health)
		{
			if (health > 100)
			{
				Log.Error("Health can't be more than 100");
				health = 100;
			}
			if (health < 0)
			{
				health = 0;
			}
			player.CustomProperties["health"] = health;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, player, new Hashtable
			{
				{
					"isDeath",
					player.IsDead()
				}
			});
		}

		public static int GetHealth(this PhotonPlayer player)
		{
			return player.GetProperty("health", 0);
		}

		public static void SetArmor(this PhotonPlayer player, int armor)
		{
			if (armor > 100)
			{
				Log.Error("Armor can't be more than 100");
				armor = 100;
			}
			if (armor < 0)
			{
				armor = 0;
			}
			player.CustomProperties["armor"] = armor;
		}

		public static int GetArmor(this PhotonPlayer player)
		{
			return player.GetProperty("armor", 0);
		}

		public static void SetDeathTime(this PhotonPlayer player, double deathTime)
		{
			player.CustomProperties["DeathTime"] = deathTime;
		}

		public static double GetDeathTime(this PhotonPlayer player)
		{
			return (double)player.CustomProperties["DeathTime"];
		}

		public static bool IsDead(this PhotonPlayer player)
		{
			return player.GetHealth() <= 0;
		}

		public static void SetHelmet(this PhotonPlayer player, bool hasHelmet)
		{
			player.CustomProperties["helmet"] = hasHelmet;
		}

		public static bool HasHelmet(this PhotonPlayer player)
		{
			return player.GetProperty("helmet", defaultValue: false);
		}

		public static void Clear(this PhotonPlayer player)
		{
			Hashtable hashtable = new Hashtable();
			foreach (object key in player.CustomProperties.Keys)
			{
				hashtable[key] = null;
			}
			player.SetCustomProperties(hashtable);
		}
	}
}
