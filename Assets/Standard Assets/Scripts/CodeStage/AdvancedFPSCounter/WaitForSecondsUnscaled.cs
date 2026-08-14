using UnityEngine;

namespace CodeStage.AdvancedFPSCounter
{
	public class WaitForSecondsUnscaled : CustomYieldInstruction
	{
		private readonly float waitTime;

		private float runUntil;

		public override bool keepWaiting => Time.unscaledTime < runUntil;

		public WaitForSecondsUnscaled(float time)
		{
			waitTime = time;
		}

		public new void Reset()
		{
			runUntil = Time.unscaledTime + waitTime;
		}
	}
}
