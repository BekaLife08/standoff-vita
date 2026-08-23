using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	[CreateAssetMenu(fileName = "Bomb", menuName = "Standoff/Create BombParameters")]
	public class BombParameters : WeaponParameters
	{
		[SerializeField]
		private float _plantDuration;

		[SerializeField]
		private float _detonationDuration;

		[SerializeField]
		private float _damageRadius;

		[SerializeField]
		private float _damage;

		[SerializeField]
		private float _takeDuration;

		[SerializeField]
		private AnimationCurve _damageCurve;

		[SerializeField]
		private AnimationCurve _armorPenetrationCurve;

		[SerializeField]
		private float _impulse;

		[SerializeField]
		private TransformTR _spine2Offset;

		public float DamageRadius
		{
			[CompilerGenerated]
			get
			{
				return _damageRadius;
			}
		}

		public float Damage
		{
			[CompilerGenerated]
			get
			{
				return _damage;
			}
		}

		public float TakeDuration
		{
			[CompilerGenerated]
			get
			{
				return _takeDuration;
			}
		}

		public AnimationCurve DamageCurve
		{
			[CompilerGenerated]
			get
			{
				return _damageCurve;
			}
		}

		public AnimationCurve ArmorPenetrationCurve
		{
			[CompilerGenerated]
			get
			{
				return _armorPenetrationCurve;
			}
		}

		public float PlantDuration
		{
			[CompilerGenerated]
			get
			{
				return _plantDuration;
			}
		}

		public float DetonationDuration
		{
			[CompilerGenerated]
			get
			{
				return _detonationDuration;
			}
		}

		public float Impulse
		{
			[CompilerGenerated]
			get
			{
				return _impulse;
			}
		}

		public TransformTR Spine2Offset
		{
			[CompilerGenerated]
			get
			{
				return _spine2Offset;
			}
		}
	}
}
