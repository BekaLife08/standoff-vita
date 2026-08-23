using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public abstract class WeaponParameters : ScriptableObject
	{
		[SerializeField]
		private WeaponId _id;

		[SerializeField]
		private string _displayName;

		[SerializeField]
		private int _cost;

		[SerializeField]
		private int _movementRate;

		[SerializeField]
		private InventorySprites _sprites;

		public WeaponId Id
		{
			[CompilerGenerated]
			get
			{
				return _id;
			}
		}

		public byte NumId
		{
			[CompilerGenerated]
			get
			{
				return (byte)_id;
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

		public int Cost
		{
			[CompilerGenerated]
			get
			{
				return _cost;
			}
		}

		public InventorySprites Sprites
		{
			[CompilerGenerated]
			get
			{
				return _sprites;
			}
		}

		public int MovementRate
		{
			[CompilerGenerated]
			get
			{
				return _movementRate;
			}
		}
	}
}
