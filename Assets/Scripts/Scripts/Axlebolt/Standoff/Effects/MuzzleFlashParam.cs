using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[Serializable]
	public class MuzzleFlashParam
	{
		[SerializeField]
		private MuzzleFlashType _type;

		[SerializeField]
		private ParticleSystem _particles;

		public MuzzleFlashType Type
		{
			[CompilerGenerated]
			get
			{
				return _type;
			}
		}

		public ParticleSystem Particles
		{
			[CompilerGenerated]
			get
			{
				return _particles;
			}
		}
	}
}
