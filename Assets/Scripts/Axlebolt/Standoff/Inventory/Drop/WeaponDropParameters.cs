using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Drop
{
	[CreateAssetMenu(fileName = "WeaponDropParameters", menuName = "Standoff/Create WeaponDropParameters")]
	public class WeaponDropParameters : ScriptableObject
	{
		[SerializeField]
		private int _force;

		[SerializeField]
		private PhysicMaterial _material;

		public int Force
		{
			[CompilerGenerated]
			get
			{
				return _force;
			}
		}

		public PhysicMaterial Material
		{
			[CompilerGenerated]
			get
			{
				return _material;
			}
		}
	}
}
