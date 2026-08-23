using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[CreateAssetMenu(fileName = "NewMaterial", menuName = "Standoff/Create Weapon Material", order = 5)]
	public class WeaponMaterial : ScriptableObject
	{
		[SerializeField]
		[NotNull]
		private Material _mainMaterial;

		[NotNull]
		public Material MainMaterial
		{
			[CompilerGenerated]
			get
			{
				return _mainMaterial;
			}
		}
	}
}
