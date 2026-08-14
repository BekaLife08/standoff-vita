using Axlebolt.Standoff.Player.State;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class WalkState : GroundMovementState, ISynchronizableState
	{
		public class WalkStateData : MessageBase
		{
		}

		private WalkStateData stateData = new WalkStateData();

		public WalkState(TranslationStatesMap statesMap, MovementController movementController)
			: base(statesMap, movementController)
		{
		}

		public void SetParameters(MessageBase parameters)
		{
		}

		public MessageBase GetParameters()
		{
			return stateData;
		}

		public override void OnStateEntered()
		{
			if (_statesMap.PreviousState.StateEnum == TranslationStatesEnum.Crouch)
			{
				OnStandEvent();
			}
			_characterController.height = _translationParameters.characterColliderParameters.heightOnStand;
			_characterController.center = _translationParameters.characterColliderParameters.centerOnStand;
			_translationData.currentSpeedMultiplier = _translationParameters.walkParameters.walkSpeedMultiplier;
		}

		public override void OnStateUpdate()
		{
			_translationData.targetDeltaPosition = Vector3.zero;
			GroundTranslationControl();
			MecanimMovementControl();
			if (base.cmdParameters.IsCrouching)
			{
				_statesMap.SetState(TranslationStatesEnum.Crouch);
				return;
			}
			if (_translationData.currentSpeedXZ < 0.1f)
			{
				_statesMap.SetState(TranslationStatesEnum.Idle);
				return;
			}
			_translationData.jumpTypeCoeff.actual = _statesMap.blendedValues.jumpType.GetValue(base.LocalTime);
			_translationData.standTypeCoeff.actual = Mathf.Lerp(_translationData.standTypeCoeff.actual, 1f, base.DeltaTime * 5f);
		}

		public override void LateUpdateState()
		{
			RotationAppliying();
		}

		public override void RotationAppliying()
		{
			Vector3 localEulerAngles = base.CharacterTransform.localEulerAngles;
			if (Mathf.Abs(VectorAngle.DeltaAngle(localEulerAngles.y, 0f)) > 1f)
			{
				localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, 0f, base.DeltaTime * 5f);
				base.CharacterTransform.localEulerAngles = localEulerAngles;
			}
		}

		public override void OnStateExit()
		{
		}
	}
}
