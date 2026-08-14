using System;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	[Serializable]
	public class LodLayer
	{
		[SerializeField]
		private Lod[] _lods = new Lod[0];

		public Lod[] Lods => _lods;
	}
}
