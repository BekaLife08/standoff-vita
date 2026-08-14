using Axlebolt.Standoff.Inventory;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Defuse
{
	[Serializable]
	public class DefuseGunTypeReward
	{
		[SerializeField]
		private GunType _gunType;

		[SerializeField]
		private int _rewardMoney;

		public GunType GunType
		{
			[CompilerGenerated]
			get
			{
				return _gunType;
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
