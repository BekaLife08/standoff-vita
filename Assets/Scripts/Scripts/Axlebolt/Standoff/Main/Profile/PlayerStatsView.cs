using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Stats;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class PlayerStatsView : View
	{
		[SerializeField]
		private PlayerGeneralStatsView _playerGeneralStatsView;

		[SerializeField]
		private WeaponsStatsView _weaponsStatsView;

		public void SetPlayer(BoltPlayer player, BoltPlayerStats stats)
		{
			_playerGeneralStatsView.SetPlayer(player, stats);
			_weaponsStatsView.SetPlayer(stats);
		}
	}
}
