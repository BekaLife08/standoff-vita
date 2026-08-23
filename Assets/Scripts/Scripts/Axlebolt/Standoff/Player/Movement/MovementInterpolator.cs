using Axlebolt.Networking;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement
{
	public class MovementInterpolator : Interpolator
	{
		private MovementSnapshot resultSnapshot = new MovementSnapshot();

		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			MovementSnapshot movementSnapshot = (MovementSnapshot)a;
			MovementSnapshot movementSnapshot2 = (MovementSnapshot)b;
			resultSnapshot = new MovementSnapshot();
			resultSnapshot.currentPosition = Vector3.Lerp(movementSnapshot.currentPosition, movementSnapshot2.currentPosition, progress);
			resultSnapshot.characterRotation = VectorAngle.LerpEulerAngle(movementSnapshot.characterRotation, movementSnapshot2.characterRotation, progress);
			resultSnapshot.velocity = Vector3.Lerp(movementSnapshot.velocity, movementSnapshot2.velocity, progress);
			return resultSnapshot;
		}
	}
}
