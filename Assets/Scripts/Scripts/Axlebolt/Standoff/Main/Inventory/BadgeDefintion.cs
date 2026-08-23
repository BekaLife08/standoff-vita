using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Inventory
{
	[CreateAssetMenu(fileName = "NewBadgeItemDefintion", menuName = "Standoff/Create BadgeItemDefintion", order = 2)]
	public class BadgeDefintion : InventoryItemDefintion
	{
		[SerializeField]
		private Sprite _sprite;

		public Sprite Sprite
		{
			[CompilerGenerated]
			get
			{
				return _sprite;
			}
		}
	}
}
