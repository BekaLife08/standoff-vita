using Axlebolt.Standoff.Player.State;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement
{
	public class PlayerTranslationData
	{
		public float targetSpeed;

		public float currentSpeedMultiplier;

		public Vector3 targetDeltaPosition;

		public Vector3 prevDeltaPosition;

		public float prevDeltaTime;

		public Vector3 prevPosition;

		public Vector3 currentPosition;

		public Vector2 targetRelativeDirectionMecanim;

		public Vector2 currentRelativeDirectionMecanim;

		public Vector2 fixedRelativeDirectionMecanim;

		public Vector3 moveDirection;

		public float WeaponSpeedMult = 1f;

		public bool IsVelocitySynchronized;

		private Vector3 _velocity = Vector3.zero;

		public BlendedValue<float> standTypeCoeff = new BlendedValue<float>();

		public BlendedValue<float> jumpTypeCoeff = new BlendedValue<float>();

		public Vector2 targetRelativeDirection;

		public float targetMecanimDirectionMagnitude;

		public BlendedValue<Vector2> currentRelativeDirection = new BlendedValue<Vector2>();

		public float currentSpeedXZ
		{
			get
			{
				if (IsVelocitySynchronized)
				{
					return new Vector3(_velocity.x, 0f, _velocity.y).magnitude;
				}
				Vector3 vector = currentPosition - prevPosition;
				vector.y = 0f;
				return vector.magnitude / prevDeltaTime;
			}
		}

		public float currentSpeedY
		{
			get
			{
				float num = currentPosition.y - prevPosition.y;
				return num / prevDeltaTime;
			}
		}

		public float currentSpeed => (currentPosition - prevPosition).magnitude / prevDeltaTime;

		public Vector3 deltaPosition => currentPosition - prevPosition;

		public Vector3 velocity
		{
			get
			{
				if (IsVelocitySynchronized)
				{
					return _velocity;
				}
				if (Mathf.Abs(prevDeltaTime) < 1E-05f)
				{
					return Vector3.zero;
				}
				return deltaPosition / prevDeltaTime;
			}
			set
			{
				_velocity = value;
			}
		}

		public BlendedValue<Vector3> currentRelativeDirection3
		{
			get
			{
				BlendedValue<Vector3> blendedValue = new BlendedValue<Vector3>();
				blendedValue.actual.x = currentRelativeDirection.actual.x;
				blendedValue.actual.y = 0f;
				blendedValue.actual.z = currentRelativeDirection.actual.y;
				blendedValue.blended.x = currentRelativeDirection.blended.x;
				blendedValue.blended.y = 0f;
				blendedValue.blended.z = currentRelativeDirection.blended.y;
				return blendedValue;
			}
		}

		public void Reset()
		{
			targetSpeed = 0f;
			currentSpeedMultiplier = 0f;
			IsVelocitySynchronized = false;
			targetDeltaPosition = Vector3.zero;
			prevDeltaPosition = Vector3.zero;
			prevDeltaTime = 0f;
			prevPosition = Vector3.zero;
			currentPosition = Vector3.zero;
			targetRelativeDirectionMecanim = Vector2.zero;
			currentRelativeDirectionMecanim = Vector2.zero;
			fixedRelativeDirectionMecanim = Vector2.zero;
			moveDirection = Vector2.zero;
			standTypeCoeff.actual = (standTypeCoeff.blended = 1f);
			jumpTypeCoeff.actual = (jumpTypeCoeff.blended = 0f);
		}
	}
}
