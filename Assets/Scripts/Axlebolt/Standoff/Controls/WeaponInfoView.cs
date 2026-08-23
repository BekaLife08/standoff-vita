using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class WeaponInfoView : HudComponentView
	{
		[SerializeField]
		private Image _weaponImage;

		[SerializeField]
		private AmmoView _ammoView;

		private WeaponController _weapon;

		public Image WeaponImage
		{
			[CompilerGenerated]
			get
			{
				return _weaponImage;
			}
		}

		public AmmoView AmmoView
		{
			[CompilerGenerated]
			get
			{
				return _ammoView;
			}
		}

		public override void SetPlayerController(PlayerController playerController)
		{
			if (playerController == null)
			{
				_weaponImage.enabled = false;
				_ammoView.SetTextViewVisible(isEnabled: false);
			}
			_weapon = null;
		}

		public override void UpdateView(PlayerController playerController)
		{
			if (playerController.WeaponryController.CurrentWeapon == null)
			{
				_weaponImage.enabled = false;
				_ammoView.SetTextViewVisible(isEnabled: false);
				return;
			}
			if (_weapon != playerController.WeaponryController.CurrentWeapon)
			{
				_weapon = playerController.WeaponryController.CurrentWeapon;
				_weaponImage.sprite = _weapon.WeaponParameters.Sprites.Icon;
				_weaponImage.enabled = true;
			}
			if (_weapon is GunController)
			{
				GunController gunController = (GunController)_weapon;
				_ammoView.SetTextViewVisible(isEnabled: true);
				_ammoView.SetCurMagazineAmmo(gunController.MagazineCapacity);
				_ammoView.SetRestAmmoAmount(gunController.Capacity);
				_ammoView.SetCriticalMagazineAmmo(IsCriticalMagazineAmmo(gunController));
			}
			else
			{
				_ammoView.SetTextViewVisible(isEnabled: false);
			}
		}

		private static bool IsCriticalMagazineAmmo(GunController gunController)
		{
			return (short)((float)gunController.GunParameters.Ammunition.MagazineCapacity * 0.25f) >= gunController.MagazineCapacity;
		}
	}
}
