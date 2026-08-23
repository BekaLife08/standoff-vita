using Axlebolt.Standoff.Inventory;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Inventory
{
	[CreateAssetMenu(fileName = "NewSkinDefinition", menuName = "Standoff/Create SkinDefinition", order = 3)]
	public class SkinDefinition : InventoryItemDefintion
	{
		[SerializeField]
		private WeaponId _weaponId;

		[SerializeField]
		private Sprite _previewImage;

		public WeaponId WeaponId
		{
			[CompilerGenerated]
			get
			{
				return _weaponId;
			}
		}

		public Sprite PreviewImage
		{
			[CompilerGenerated]
			get
			{
				return _previewImage;
			}
		}
	}
}
