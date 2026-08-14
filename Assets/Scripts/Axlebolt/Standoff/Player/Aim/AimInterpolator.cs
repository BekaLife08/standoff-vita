using Axlebolt.Networking;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Aim
{
	public class AimInterpolator : Interpolator
	{
		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			AimSnapshot aimSnapshot = (AimSnapshot)a;
			AimSnapshot aimSnapshot2 = (AimSnapshot)b;
			AimSnapshot aimSnapshot3 = new AimSnapshot();
			AimingData aimingData = aimSnapshot3.aimingData;
			AimingData aimingData2 = aimSnapshot.aimingData;
			AimingData aimingData3 = aimSnapshot2.aimingData;
			aimingData.curAimAngle = VectorAngle.LerpEulerAngle(aimingData2.curAimAngle, aimingData3.curAimAngle, progress);
			aimingData.curEulerAngles = VectorAngle.LerpEulerAngle(aimingData2.curEulerAngles, aimingData3.curEulerAngles, progress);
			aimingData.curMTCoeff = Mathf.Lerp(aimingData2.curMTCoeff, aimingData3.curMTCoeff, progress);
			aimingData.currentSpineZAxisOffset = Mathf.Lerp(aimingData2.currentSpineZAxisOffset, aimingData3.currentSpineZAxisOffset, progress);
			aimingData.curWTCoeff = Mathf.Lerp(aimingData2.curWTCoeff, aimingData3.curWTCoeff, progress);
			aimingData.standTypeCoeff = Mathf.Lerp(aimingData2.standTypeCoeff, aimingData3.standTypeCoeff, progress);
			aimingData.gunSubstitude = VectorAngle.LerpTR(aimingData2.gunSubstitude, aimingData3.gunSubstitude, progress);
			return aimSnapshot3;
		}
	}
}
