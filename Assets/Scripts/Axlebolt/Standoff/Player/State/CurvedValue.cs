using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player.State
{
	[Serializable]
	public class CurvedValue
	{
		public AnimationCurve curve;

		public float multiplier;

		public float duration;

		public int id = -1;

		public static float GetCurveValue(AnimationCurve curve, float duration, float timeStarted, float localTime)
		{
			float num = localTime - timeStarted;
			return curve.Evaluate(num / duration);
		}
	}
}
