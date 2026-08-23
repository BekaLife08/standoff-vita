using Axlebolt.Bolt.Stats;
using Axlebolt.Standoff.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Bolt
{
	public static class BoltPlayerStatsExtension
	{
		public const string Kills = "kills";

		public const string Deaths = "deaths";

		public const string Assists = "assists";

		public const string Shots = "shots";

		public const string Hits = "hits";

		public const string Headshots = "headshots";

		public const string Damage = "damage";

		public const string GamesPlayed = "games_played";

		public const string GunKills = "kills";

		public const string GunShots = "shots";

		public const string GunHits = "hits";

		public const string GunHeadshots = "headshots";

		public const string GunDamage = "damage";

		private static readonly HashSet<string> SupportedGameModes;

		static BoltPlayerStatsExtension()
		{
			SupportedGameModes = new HashSet<string>();
			SupportedGameModes.Add("DeathMatch");
			SupportedGameModes.Add("Defuse");
		}

		private static string WrapMode(string apiName, string gameMode)
		{
			return gameMode.ToLower() + "_" + apiName;
		}

		private static string WrapModeGun(string apiName, string gameMode, WeaponId weaponId)
		{
			return "gun_" + gameMode.ToLower() + "_" + weaponId.ToString().ToLower() + "_" + apiName;
		}

		public static int GetKills(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetKills);
		}

		public static int GetDeaths(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetDeaths);
		}

		public static int GetAssists(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetAssists);
		}

		public static int GetShots(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetShots);
		}

		public static int GetHits(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetHits);
		}

		public static int GetHeadshots(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetHeadshots);
		}

		public static int GetGamesPlayed(this BoltPlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetGamesPlayed);
		}

		public static int GetKills(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("kills", gameMode));
		}

		public static void IncKills(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("kills", gameMode));
		}

		public static int GetDeaths(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("deaths", gameMode));
		}

		public static void IncDeaths(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("deaths", gameMode));
		}

		public static int GetAssists(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("assists", gameMode));
		}

		public static void IncAssists(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("assists", gameMode));
		}

		public static int GetShots(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("shots", gameMode));
		}

		public static void IncShots(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("shots", gameMode));
		}

		public static int GetHits(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("hits", gameMode));
		}

		public static void IncHits(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("hits", gameMode));
		}

		public static int GetHeadshots(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("headshots", gameMode));
		}

		public static void IncHeadshots(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("headshots", gameMode));
		}

		public static void AddDamage(this BoltPlayerStats stats, string gameMode, int damage)
		{
			string apiName = WrapMode("damage", gameMode);
			stats.SetIntStat(apiName, stats.GetIntStat(apiName) + damage);
		}

		public static int GetGamesPlayed(this BoltPlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("games_played", gameMode));
		}

		public static void IncGamesPlayed(this BoltPlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("games_played", gameMode));
		}

		public static float GetAccuracyPercentage(this BoltPlayerStats stats)
		{
			return (stats.GetShots() <= 0) ? 0f : ((float)stats.GetHits() / (float)stats.GetShots() * 100f);
		}

		public static float GetHeadshotsPercentage(this BoltPlayerStats stats)
		{
			return (stats.GetKills() <= 0) ? 0f : ((float)stats.GetHeadshots() / (float)stats.GetKills() * 100f);
		}

		public static float GetKd(this BoltPlayerStats stats)
		{
			return (stats.GetDeaths() <= 0) ? 0f : ((float)stats.GetKills() / (float)stats.GetDeaths());
		}

		public static void IncGunKills(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("kills", gameMode, weaponId));
		}

		public static int GetGunKills(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("kills", gameMode, weaponId));
		}

		public static int GetGunKills(this BoltPlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("kills", gameMode, weaponId)));
		}

		public static void IncGunShots(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("shots", gameMode, weaponId));
		}

		public static int GetGunShots(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("shots", gameMode, weaponId));
		}

		public static int GetGunShots(this BoltPlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("shots", gameMode, weaponId)));
		}

		public static void IncGunHits(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("hits", gameMode, weaponId));
		}

		public static int GetGunHits(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("hits", gameMode, weaponId));
		}

		public static int GetGunHits(this BoltPlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("hits", gameMode, weaponId)));
		}

		public static void IncGunHeadshots(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("headshots", gameMode, weaponId));
		}

		public static int GetGunHeadshots(this BoltPlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("headshots", gameMode, weaponId));
		}

		public static int GetGunHeadshots(this BoltPlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("headshots", gameMode, weaponId)));
		}

		public static float GetGunAccuracyPercentage(this BoltPlayerStats stats, WeaponId weaponId)
		{
			int gunShots = stats.GetGunShots(weaponId);
			return (gunShots <= 0) ? 0f : ((float)stats.GetGunHits(weaponId) / (float)gunShots * 100f);
		}

		public static void AddGunDamage(this BoltPlayerStats stats, string gameMode, WeaponId weaponId, int damage)
		{
			string apiName = WrapModeGun("damage", gameMode, weaponId);
			stats.SetIntStat(apiName, stats.GetIntStat(apiName) + damage);
		}
	}
}
