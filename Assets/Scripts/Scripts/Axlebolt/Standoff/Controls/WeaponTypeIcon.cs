using Axlebolt.Standoff.Inventory;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	[Serializable]
	public class WeaponTypeIcon
	{
		[SerializeField]
		private WeaponType _weaponType;

		[SerializeField]
		private Sprite _icon;

		public WeaponType WeaponType
		{
			[CompilerGenerated]
			get
			{
				return _weaponType;
			}
		}

		public Sprite Icon
		{
			[CompilerGenerated]
			get
			{
				return _icon;
			}
		}
	}
}
