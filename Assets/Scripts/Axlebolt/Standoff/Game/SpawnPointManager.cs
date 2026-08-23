using Axlebolt.Standoff.Core;
using System;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class SpawnPointManager
	{
		private readonly SpawnZone[] _trZones;

		private readonly SpawnZone[] _ctZones;

		public SpawnPointManager(SpawnZoneType type)
		{
			SpawnZone[] source = UnityEngine.Object.FindObjectsOfType<SpawnZone>();
			_trZones = (from area in source
				where area.Team == Team.Tr && area.Type == type
				select area).ToArray();
			_ctZones = (from area in source
				where area.Team == Team.Ct && area.Type == type
				select area).ToArray();
			if (_trZones.Length == 0 || _ctZones.Length == 0)
			{
				throw new InvalidOperationException($"SpawnZone's with type {type} not found");
			}
		}

		public SpawnPoint GetSpawnPoint(Team team)
		{
			object zones;
			switch (team)
			{
			case Team.None:
				throw new ArgumentException("team");
			case Team.Ct:
				zones = _ctZones;
				break;
			default:
				zones = _trZones;
				break;
			}
			return RandomPoint((SpawnZone[])zones);
		}

		private static SpawnPoint RandomPoint(SpawnZone[] zones)
		{
			int num = UnityEngine.Random.Range(0, zones.Length);
			return zones[num].RandomPoint();
		}
	}
}
