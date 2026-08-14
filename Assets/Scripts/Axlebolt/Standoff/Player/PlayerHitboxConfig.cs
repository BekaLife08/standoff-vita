using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerHitboxConfig : ScriptableObject
	{
		public enum HitboxType
		{
			Box,
			Capsule,
			Mesh
		}

		[Serializable]
		public class HitboxConfig
		{
			public BipedMap.Bip bone;

			public HitboxType hitboxType;

			public Vector3 center;

			public Vector3 size;

			public int direction;

			public float radius;

			public float height;
		}

		public List<HitboxConfig> hitboxes = new List<HitboxConfig>();

		public HitboxConfig trigger = new HitboxConfig();

		public int layer;
	}
}
