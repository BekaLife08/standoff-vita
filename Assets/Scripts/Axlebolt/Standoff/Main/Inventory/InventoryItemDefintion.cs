using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Inventory
{
	public abstract class InventoryItemDefintion : ScriptableObject
	{
		[SerializeField]
		private InventoryItemId _id;

		[SerializeField]
		private string _displayName;

		public InventoryItemId Id
		{
			[CompilerGenerated]
			get
			{
				return _id;
			}
		}

		public string DisplayName
		{
			[CompilerGenerated]
			get
			{
				return _displayName;
			}
		}
	}
}
