using UnityEngine;

namespace Axlebolt.Standoff.Player.State
{
	public class BlendedValue<T>
	{
		public T actual;

		public T actualPrevFrame;

		public T blended;

		public T previous;

		public float timeFix;

		public float blendDuration = 0.1f;

		public static void BlendValue(BlendedValue<float> stateParams, float time)
		{
			float num = stateParams.actual - stateParams.actualPrevFrame;
			stateParams.previous += num;
			float t = (time - stateParams.timeFix) / stateParams.blendDuration;
			stateParams.blended = Mathf.Lerp(stateParams.previous, stateParams.actual, t);
			stateParams.actualPrevFrame = stateParams.actual;
		}

		public static void FixNewPackage(BlendedValue<T> stateParam, float time)
		{
			stateParam.previous = stateParam.blended;
			stateParam.timeFix = time;
		}
	}
}
