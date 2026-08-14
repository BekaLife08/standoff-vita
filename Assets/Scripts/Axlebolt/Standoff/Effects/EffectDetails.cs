using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[Serializable]
	public class EffectDetails
	{
		[SerializeField]
		private int _highDetailCount;

		[SerializeField]
		private int _mediumDetailCount;

		[SerializeField]
		private int _lowDetailCount;

		[SerializeField]
		private int _veryLowDetailCount;

		public int HighDetailCount
		{
			[CompilerGenerated]
			get
			{
				return _highDetailCount;
			}
		}

		public int MediumDetailCount
		{
			[CompilerGenerated]
			get
			{
				return _mediumDetailCount;
			}
		}

		public int LowDetailCount
		{
			[CompilerGenerated]
			get
			{
				return _lowDetailCount;
			}
		}

		public int VeryLowDetailCount
		{
			[CompilerGenerated]
			get
			{
				return _veryLowDetailCount;
			}
		}
	}
}
