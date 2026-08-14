using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[Serializable]
	public class KnifeImpactParam
	{
		[SerializeField]
		private Sprite[] _decals;

		[SerializeField]
		private Vector2 _size;

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
