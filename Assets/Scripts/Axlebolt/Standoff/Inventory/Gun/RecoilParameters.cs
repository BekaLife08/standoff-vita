using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Gun
{
	[Serializable]
	public class RecoilParameters
	{
		[Header("Angular")]
		public float horizontalRange;

		public float verticalRange;

		[Header("Curves")]
		public AnimationCurve xAxisDeviation;

		public AnimationCurve yAxisDeviation;

		public AnimationCurve recoilAccelerationCurve;

		public AnimationCurve fallbackDurationCurve;

		public float maxFallbackDuration;

		public AnimationCurve relativeDispertionCurve;

		public float cameraDeviationCoeff = 0.4f;

		public float maxApproachSpeed = 2f;

		public float recoilAccelDuration;

		public float recoilAccelStep;

		public AnimationCurve progressFallbackCurve;
	}
}
