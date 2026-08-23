using Axlebolt.Standoff.Inventory.HitHandling;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[Serializable]
	public class GunImpactParam
	{
		[SerializeField]
		private SurfaceType[] _surfaceTypes;

		[SerializeField]
		private ParticleSystem _particles;

		[SerializeField]
		private Sprite[] _decals;

		[SerializeField]
		private Vector2 _size;

		public SurfaceType[] SurfaceTypes
		{
			[CompilerGenerated]
			get
			{
				return _surfaceTypes;
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

		public Sprite[] Decals
		{
			[CompilerGenerated]
			get
			{
				return _decals;
			}
		}

		public Vector2 Size
		{
			[CompilerGenerated]
			get
			{
				return _size;
			}
		}
	}
}
