using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class ProgressView : HudComponentView
	{
		[SerializeField]
		private Image _actionImage;

		[SerializeField]
		private Text _actionText;

		[SerializeField]
		private Text _timeText;

		[SerializeField]
		private Image _progressIndicator;

		public override void SetPlayerController(PlayerController playerController)
		{
			base.gameObject.SetActive(value: false);
		}

		public override void UpdateView(PlayerController playerController)
		{
			WeaponController weaponController = playerController.WeaponryController.CurrentWeapon;
			IProgressWeaponController progressWeaponController = weaponController as IProgressWeaponController;
			if (progressWeaponController == null)
			{
				weaponController = playerController.WeaponryController.CurrentKit;
				progressWeaponController = (weaponController as IProgressWeaponController);
			}
			if (progressWeaponController != null)
			{
				bool flag = progressWeaponController.HasProgress();
				base.gameObject.SetActive(flag);
				if (flag)
				{
					_actionImage.sprite = weaponController.WeaponParameters.Sprites.Icon;
					_actionText.text = progressWeaponController.GetProgressDisplayName();
					_progressIndicator.fillAmount = progressWeaponController.GetProgress();
					_timeText.text = StringUtils.FormatTwoDigit(progressWeaponController.GetProgressTime());
				}
			}
			else if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}
}
