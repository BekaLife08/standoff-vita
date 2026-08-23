using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[Serializable]
	public class WeaponRecoilAnimation
	{
		public AnimationCurve AnimationProgressCurve;

		public float Duration;

		public float BlendOffset;

		public float BlendDuration;
	}
}
