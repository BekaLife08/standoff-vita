using Axlebolt.Standoff.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Aim
{
	public class AimingData : MessageBase
	{
		public float curWTCoeff = 1f;

		public float curMTCoeff = 1f;

		public Vector3 curAimAngle = Vector3.zero;

		public Vector3 curEulerAngles = Vector3.zero;

		public TransformTR prevGPHOffsetOnStand = new TransformTR();

		public TransformTR prevGPHOffsetOnCrouch = new TransformTR();

		public TransformTR gunSubstitude = new TransformTR();

		public float currentSpineZAxisOffset;

		public float standTypeCoeff;

		public AimingData Clone()
		{
			return (AimingData)MemberwiseClone();
		}

		public override void Deserialize(NetworkReader reader)
		{
			curWTCoeff = reader.ReadSingle();
			curMTCoeff = reader.ReadSingle();
			curAimAngle = reader.ReadVector3();
			curEulerAngles = reader.ReadVector3();
			gunSubstitude.Deserialize(reader);
			currentSpineZAxisOffset = reader.ReadSingle();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(curWTCoeff);
			writer.Write(curMTCoeff);
			writer.Write(curAimAngle);
			writer.Write(curEulerAngles);
			gunSubstitude.Serialize(writer);
			writer.Write(currentSpineZAxisOffset);
		}
	}
}
