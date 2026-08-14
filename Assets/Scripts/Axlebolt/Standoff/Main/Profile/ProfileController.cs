using Axlebolt.Bolt;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Stats;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class ProfileController : TabController<ProfileController>
	{
		[SerializeField]
		private ShortProfileController _shortProfileController;

		[SerializeField]
		private PlayerStatsView _playerStatsView;

		public override void Init()
		{
			base.Init();
			WeaponUtility.LoadBaseWeapons();
		}

		public override void OnOpen()
		{
			_shortProfileController.gameObject.SetActive(true);
			_playerStatsView.Show();
			_shortProfileController.SetPlayer(BoltService<BoltPlayerService>.Instance.Player);
			_playerStatsView.SetPlayer(BoltService<BoltPlayerService>.Instance.Player, BoltService<BoltPlayerStatsService>.Instance.Stats);
		}

		public override void OnClose()
		{
			_shortProfileController.gameObject.SetActive(false);
			_playerStatsView.Hide();
		}
	}
}
