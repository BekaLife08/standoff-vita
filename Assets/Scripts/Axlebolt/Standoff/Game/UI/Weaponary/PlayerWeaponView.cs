using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class PlayerWeaponView : View
	{
		[SerializeField]
		[NotNull]
		private Image _lineImage;

		[SerializeField]
		[NotNull]
		private Image _glowImage;

		[SerializeField]
		[NotNull]
		private Text _nameText;

		public WeaponParameters Weapon
		{
			set
			{
				_nameText.text = value.DisplayName;
				_lineImage.sprite = value.Sprites.Line;
				_glowImage.sprite = value.Sprites.Glow;
			}
		}
	}
}
