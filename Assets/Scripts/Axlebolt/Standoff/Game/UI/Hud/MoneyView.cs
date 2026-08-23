using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Hud
{
	public class MoneyView : HudComponentView
	{
		[SerializeField]
		private Text _text;

		private int _money = int.MinValue;

		public override void SetPlayerController(PlayerController playerController)
		{
			base.gameObject.SetActive(playerController != null);
		}

		public override void UpdateView(PlayerController playerController)
		{
			if (_money != playerController.Player.GetMoney())
			{
				_money = playerController.Player.GetMoney();
				_text.text = "$" + _money;
			}
		}
	}
}
