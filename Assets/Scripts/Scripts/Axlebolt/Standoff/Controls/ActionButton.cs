using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class ActionButton : InteractableButton, IHudComponentView
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
			SetActive(active: false);
		}

		public void UpdateView(PlayerController playerController)
		{
			if (playerController.PhotonView.isMine)
			{
				KitController currentKit = playerController.WeaponryController.CurrentKit;
				if (!base.IsActive && currentKit != null && currentKit.CanPerformAction())
				{
					SetActive(active: true);
				}
				else if (base.IsActive && (currentKit == null || !currentKit.CanPerformAction()))
				{
					SetActive(active: false);
				}
				if (currentKit != null && currentKit.CanPerformAction() && currentKit.WeaponType != _currentType)
				{
					_currentType = currentKit.WeaponType;
					_sprites.TryGetValue(_currentType, out Sprite value);
					_iconImage.sprite = value;
				}
			}
		}

		private void SetActive(bool active)
		{
			base.IsActive = active;
			base.gameObject.SetActive(active);
		}
	}
}
