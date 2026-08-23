using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	[Serializable]
	public class BombBeepStep
	{
		[SerializeField]
		private float _time;

		[SerializeField]
		private float _interval;

		public float Time
		{
			[CompilerGenerated]
			get
			{
				return _time;
			}
		}

		public float Interval
		{
			[CompilerGenerated]
			get
			{
				return _interval;
			}
		}
	}
}
