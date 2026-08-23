using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[Serializable]
	public class InventorySprites
	{
		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private Sprite _line;

		[SerializeField]
		private Sprite _glow;

		[SerializeField]
		private Sprite _preview;

		public Sprite Glow => _glow;

		public Sprite Icon => _icon;

		public Sprite Line => _line;

		public Sprite Preview => _preview;
	}
}
