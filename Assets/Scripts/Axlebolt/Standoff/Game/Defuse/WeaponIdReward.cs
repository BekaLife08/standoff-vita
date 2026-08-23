using Axlebolt.Standoff.Inventory;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Defuse
{
	[Serializable]
	public class WeaponIdReward
	{
		[SerializeField]
		private WeaponId _weaponId;

		[SerializeField]
		private int _rewardMoney;

		public WeaponId WeaponId
		{
			[CompilerGenerated]
			get
			{
				return _weaponId;
			}
		}

		public int RewardMoney
		{
			[CompilerGenerated]
			get
			{
				return _rewardMoney;
			}
		}
	}
}
