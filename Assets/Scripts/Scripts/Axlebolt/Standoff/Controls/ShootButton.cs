using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class ShootButton : InteractableButton, IHudComponentView
	{
		[SerializeField]
		private WeaponTypeIcon[] _icons;

		[SerializeField]
		private Image _iconImage;

		private WeaponType _currentType;

		private Dictionary<WeaponType, Sprite> _sprites;

		protected override void Awake()
		{
			base.Awake();
			_sprites = _icons.ToDictionary((WeaponTypeIcon icon) => icon.WeaponType, (WeaponTypeIcon icon) => icon.Icon);
		}

		public void SetPlayerController(PlayerController playerController)
		{
		}

		public void UpdateView(PlayerController playerController)
		{
			WeaponType weaponType = playerController.WeaponryController.CurrentWeapon.WeaponType;
			if (weaponType != _currentType)
			{
				_currentType = weaponType;
				_sprites.TryGetValue(_currentType, out Sprite value);
				_iconImage.sprite = value;
			}
		}
	}
}
