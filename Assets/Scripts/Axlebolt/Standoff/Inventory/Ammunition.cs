using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[Serializable]
	public class Ammunition
	{
		[SerializeField]
		private short _magazineCapacity;

		[SerializeField]
		private short _capacity;

		public short MagazineCapacity
		{
			[CompilerGenerated]
			get
			{
				return _magazineCapacity;
			}
		}

		public short Capacity
		{
			[CompilerGenerated]
			get
			{
				return _capacity;
			}
		}
	}
}
