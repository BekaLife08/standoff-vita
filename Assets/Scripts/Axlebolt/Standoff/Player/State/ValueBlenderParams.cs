using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.State
{
	public class ValueBlenderParams : MessageBase
	{
		public float progress;

		public float duration;

		public float blendDuration;

		public float blendStartPrevValue;

		public float blendStartCurveValue;

		public float timeBlendStarted_relative;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(progress);
			writer.Write(duration);
			writer.Write(blendDuration);
			writer.Write(blendStartPrevValue);
			writer.Write(blendStartCurveValue);
			writer.Write(timeBlendStarted_relative);
		}

		public override void Deserialize(NetworkReader reader)
		{
			progress = reader.ReadSingle();
			duration = reader.ReadSingle();
			blendDuration = reader.ReadSingle();
			blendStartPrevValue = reader.ReadSingle();
			blendStartCurveValue = reader.ReadSingle();
			timeBlendStarted_relative = reader.ReadSingle();
		}
	}
}
