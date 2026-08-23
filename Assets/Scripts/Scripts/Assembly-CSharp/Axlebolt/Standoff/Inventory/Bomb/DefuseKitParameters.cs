using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	[CreateAssetMenu(fileName = "DefuseKit", menuName = "Standoff/Create DefuseKitParameters")]
	public class DefuseKitParameters : WeaponParameters
	{
		[SerializeField]
		private float _defuseDuration;

		[SerializeField]
		private float _defuseDistance;

		public float DefuseDuration
		{
			[CompilerGenerated]
			get
			{
				return _defuseDuration;
			}
		}

		public float DefuseDistance
		{
			[CompilerGenerated]
			get
			{
				return _defuseDistance;
			}
		}
	}
}
