using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Movement.States;
using Axlebolt.Standoff.Player.State;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement
{
	public class MovementSnapshot : ObjectSnapshot
	{
		public MessageBase stateParameters;

		public TranslationStatesMap.Parameters stateMapParameters = new TranslationStatesMap.Parameters();

		public float targetSpeed;

		public float currentSpeedMultiplier;

		public float prevDeltaTime;

		public Vector3 prevPosition;

		public Vector3 currentPosition;

		public Vector3 velocity;

		public Vector3 characterRotation;

		public Vector2 targetRelativeDirection;

		public float targetMecanimDirectionMagnitude;

		public Vector2 currentRelativeDirection;

		public ValueBlenderParams jumpTypeValueBlender = new ValueBlenderParams();

		public ValueBlenderParams standTypeValueBlender = new ValueBlenderParams();

		public float standType;

		public float jumpType;

		public Vector3 eulerAng;

		public MovementSnapshot()
		{
			time = 90f;
		}

		public static MessageBase GetStateData(int stateId)
		{
			switch (stateId)
			{
			case 1:
				return new JumpState.JumpData();
			case 2:
				return new WalkState.WalkStateData();
			case 3:
				return new IdleState.IdleData();
			case 4:
				return new CrouchState.CrouchData();
			default:
				UnityEngine.Debug.LogError("Wrong state Id");
				return null;
			}
		}

		public override void Deserialize(NetworkReader reader)
		{
			uint position = reader.Position;
			time = reader.ReadSingle();
			currentPosition = reader.ReadVector3();
			characterRotation = reader.ReadVector3();
			velocity = reader.ReadVector3();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
			writer.Write(currentPosition);
			writer.Write(characterRotation);
			writer.Write(velocity);
		}

		public MovementSnapshot Clone()
		{
			return (MovementSnapshot)MemberwiseClone();
		}
	}
}
