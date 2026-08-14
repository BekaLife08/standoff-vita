using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[CreateAssetMenu(fileName = "NewKnife", menuName = "Standoff/Create Knife", order = 3)]
	public class KnifeParameters : WeaponParameters
	{
		[SerializeField]
		private float _hitInterval;

		[SerializeField]
		private float _takeDuration;

		[SerializeField]
		private float _hitImpulse;

		[SerializeField]
		[Range(0f, 100f)]
		private float _armorPenetration;

		[SerializeField]
		private Damage _damage;

		[SerializeField]
		private int _backDamage;

		public float HitInterval
		{
			[CompilerGenerated]
			get
			{
				return _hitInterval;
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

		public float HitImpulse
		{
			[CompilerGenerated]
			get
			{
				return _hitImpulse;
			}
		}

		public float ArmorPenetration
		{
			[CompilerGenerated]
			get
			{
				return _armorPenetration;
			}
		}

		public Damage Damage
		{
			[CompilerGenerated]
			get
			{
				return _damage;
			}
		}

		public int BackDamage
		{
			[CompilerGenerated]
			get
			{
				return _backDamage;
			}
		}
	}
}
