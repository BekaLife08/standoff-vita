using System.Collections.Generic;
using System.Linq;
using Axlebolt.Bolt.Stats;
using Axlebolt.Standoff.Bolt;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class WeaponsStatsView : View
	{
		[SerializeField]
		private List<WeaponStatsView> _weaponStatsViews;

		public void SetPlayer(BoltPlayerStats stats)
		{
			foreach (WeaponStatsView weaponStatsView in _weaponStatsViews)
			{
				weaponStatsView.Hide();
			}
			WeaponStatsView.WeaponStats[] bestWeapons = GetBestWeapons(stats);
			for (int i = 0; i < bestWeapons.Length && i < _weaponStatsViews.Count; i++)
			{
				_weaponStatsViews[i].Show(i, bestWeapons[i]);
			}
		}

		public WeaponStatsView.WeaponStats[] GetBestWeapons(BoltPlayerStats stats)
		{
			List<WeaponStatsView.WeaponStats> list = new List<WeaponStatsView.WeaponStats>();
			WeaponParameters[] array = WeaponUtility.LoadBaseWeapons();
			foreach (WeaponParameters weaponParameters in array)
			{
				if (!(weaponParameters is KnifeParameters))
				{
					int gunKills = stats.GetGunKills(weaponParameters.Id);
					float gunAccuracyPercentage = stats.GetGunAccuracyPercentage(weaponParameters.Id);
					list.Add(new WeaponStatsView.WeaponStats(weaponParameters, gunKills, gunAccuracyPercentage));
				}
			}
			return list.OrderByDescending((WeaponStatsView.WeaponStats stat) => stat.Kills).ToArray();
		}
	}
}
