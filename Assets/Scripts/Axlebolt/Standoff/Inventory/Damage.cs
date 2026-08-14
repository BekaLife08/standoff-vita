using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[Serializable]
	public class Damage
	{
		[SerializeField]
		private int _headDamage;

		[SerializeField]
		private int _chestAndArmsDamage;

		[SerializeField]
		private int _stomachDamage;

		[SerializeField]
		private int _legsDamage;

		public int HeadDamage
		{
			[CompilerGenerated]
			get
			{
				return _headDamage;
			}
		}

		public int ChestAndArmsDamage
		{
			[CompilerGenerated]
			get
			{
				return _chestAndArmsDamage;
			}
		}

		public int LegsDamage
		{
			[CompilerGenerated]
			get
			{
				return _legsDamage;
			}
		}

		public int StomachDamage
		{
			[CompilerGenerated]
			get
			{
				return _stomachDamage;
			}
		}

		public static Damage Create(int headDamage, int chestAndArmsDamage, int stomachDamage, int legsDamage)
		{
			Damage damage = new Damage();
			damage._headDamage = headDamage;
			damage._chestAndArmsDamage = chestAndArmsDamage;
			damage._stomachDamage = stomachDamage;
			damage._legsDamage = legsDamage;
			return damage;
		}
	}
}
