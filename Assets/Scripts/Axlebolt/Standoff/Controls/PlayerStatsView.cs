using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class PlayerStatsView : HudComponentView
	{
		[SerializeField]
		private Image _hpImg;

		[SerializeField]
		private Image _armorImg;

		[SerializeField]
		private Image _hpFillImg;

		[SerializeField]
		private Image _armorFillImg;

		[SerializeField]
		private Text _hpText;

		[SerializeField]
		private Text _armorText;

		[SerializeField]
		private Color _normalColor;

		[SerializeField]
		private Color _criticalColor;

		public override void SetPlayerController(PlayerController playerController)
		{
			SetElementsEnabled(playerController != null);
		}

		public override void UpdateView(PlayerController playerController)
		{
			int health = playerController.GetHealth();
			int armor = playerController.GetArmor();
			_hpFillImg.fillAmount = (float)health / 100f;
			_armorFillImg.fillAmount = (float)armor / 100f;
			_hpText.text = health.ToString();
			_armorText.text = armor.ToString();
			Color criticalColor;
			if (health <= 20)
			{
				Image hpFillImg = _hpFillImg;
				criticalColor = _criticalColor;
				_hpText.color = criticalColor;
				criticalColor = criticalColor;
				_hpImg.color = criticalColor;
				hpFillImg.color = criticalColor;
			}
			else
			{
				Image hpFillImg2 = _hpFillImg;
				criticalColor = _normalColor;
				_hpText.color = criticalColor;
				criticalColor = criticalColor;
				_hpImg.color = criticalColor;
				hpFillImg2.color = criticalColor;
			}
			Image armorFillImg = _armorFillImg;
			criticalColor = _normalColor;
			_armorText.color = criticalColor;
			criticalColor = criticalColor;
			_armorImg.color = criticalColor;
			armorFillImg.color = criticalColor;
		}

		private void SetElementsEnabled(bool isEnabled)
		{
			Image hpImg = _hpImg;
			bool flag = isEnabled;
			_armorText.enabled = flag;
			flag = flag;
			_hpText.enabled = flag;
			flag = flag;
			_armorFillImg.enabled = flag;
			flag = flag;
			_hpFillImg.enabled = flag;
			flag = flag;
			_armorImg.enabled = flag;
			hpImg.enabled = flag;
		}
	}
}
