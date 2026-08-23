using Axlebolt.Common.States;
using Axlebolt.Standoff.Player.State;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class CrouchState : GroundMovementState, ISynchronizableState
	{
		public class CrouchData : MessageBase
		{
			public bool isZeroMagnitudeFixed;

			public float timeZeroMagnitudeFixed;

			public int crouchSubStateNO = -1;

			public int crouchSubStatePrevNO = -1;

			public float crouchSubStateEnterTime;

			public bool isCrouchSubStateJustSwitched;

			public override void Serialize(NetworkWriter writer)
			{
				writer.Write(isZeroMagnitudeFixed);
				writer.Write(timeZeroMagnitudeFixed);
				writer.WritePackedUInt32((uint)crouchSubStateNO);
				writer.WritePackedUInt32((uint)crouchSubStatePrevNO);
				writer.Write(crouchSubStateEnterTime);
				writer.Write(isCrouchSubStateJustSwitched);
			}

			public override void Deserialize(NetworkReader reader)
			{
				isZeroMagnitudeFixed = reader.ReadBoolean();
				timeZeroMagnitudeFixed = reader.ReadSingle();
				crouchSubStateNO = (int)reader.ReadPackedUInt32();
				crouchSubStatePrevNO = (int)reader.ReadPackedUInt32();
				crouchSubStateEnterTime = reader.ReadSingle();
				isCrouchSubStateJustSwitched = reader.ReadBoolean();
			}
		}

		public enum CrouchSubState
		{
			Moving,
			Idle,
			NotStated
		}

		public CrouchData crouchData = new CrouchData();

		private StateSimple<CrouchSubState> _crouchSubStateInfo = new StateSimple<CrouchSubState>(CrouchSubState.NotStated);

		public CrouchState(TranslationStatesMap stateMap, MovementController movementController)
			: base(stateMap, movementController)
		{
		}

		public void SetParameters(MessageBase parameters)
		{
			crouchData = (CrouchData)parameters;
			_characterController.height = _translationParameters.characterColliderParameters.heightOnCrouch;
			_characterController.center = _translationParameters.characterColliderParameters.centerOnCrouch;
			_crouchSubStateInfo.curState = (CrouchSubState)crouchData.crouchSubStateNO;
			_crouchSubStateInfo.timeSwitched = crouchData.crouchSubStateEnterTime;
			_crouchSubStateInfo.prevState = (CrouchSubState)crouchData.crouchSubStatePrevNO;
			_crouchSubStateInfo.isJustSwitched = crouchData.isCrouchSubStateJustSwitched;
		}

		public MessageBase GetParameters()
		{
			crouchData.crouchSubStateNO = (int)_crouchSubStateInfo.curState;
			crouchData.crouchSubStatePrevNO = (int)_crouchSubStateInfo.prevState;
			crouchData.crouchSubStateEnterTime = _crouchSubStateInfo.timeSwitched;
			crouchData.isCrouchSubStateJustSwitched = _crouchSubStateInfo.isJustSwitched;
			return crouchData;
		}

		public override void OnStateEntered()
		{
			crouchData.isZeroMagnitudeFixed = false;
			StateSimple<CrouchSubState>.SetState(CrouchSubState.Moving, _crouchSubStateInfo);
			_characterController.height = _translationParameters.characterColliderParameters.heightOnCrouch;
			_characterController.center = _translationParameters.characterColliderParameters.centerOnCrouch;
			OnCrouchEvent();
			_statesMap.blendedValues.standType.BlendValue(0.1f, _translationParameters.crouchParameters.standToCrouchCurve, _translationData.standTypeCoeff.actual, base.LocalTime);
			_translationData.currentSpeedMultiplier = _translationParameters.crouchParameters.crouchSpeedMultiplier;
		}

		public override void OnStateUpdate()
		{
			_translationData.standTypeCoeff.actual = ((!_statesMap.blendedValues.standType.IsFinished(base.LocalTime)) ? _statesMap.blendedValues.standType.GetValue(base.LocalTime) : _statesMap.blendedValues.standType.GetFinalValue(base.LocalTime));
			_translationData.targetDeltaPosition = Vector3.zero;
			if (!base.cmdParameters.IsCrouching)
			{
				_statesMap.SetState(TranslationStatesEnum.Walk);
			}
			_translationData.jumpTypeCoeff.actual = _statesMap.blendedValues.jumpType.GetValue(base.LocalTime);
			if (_crouchSubStateInfo.curState == CrouchSubState.Moving)
			{
				if (_crouchSubStateInfo.isJustSwitched)
				{
					_crouchSubStateInfo.isJustSwitched = false;
				}
				if (_translationData.currentSpeedXZ < 0.01f && !crouchData.isZeroMagnitudeFixed)
				{
					crouchData.isZeroMagnitudeFixed = true;
					crouchData.timeZeroMagnitudeFixed = base.LocalTime;
				}
				if (_translationData.currentSpeedXZ > 0.1f)
				{
					crouchData.isZeroMagnitudeFixed = false;
				}
				if (_translationData.currentSpeedXZ < 0.2f)
				{
					_crouchSubStateInfo.SetState(CrouchSubState.Idle, base.LocalTime);
				}
			}
			if (_crouchSubStateInfo.curState == CrouchSubState.Idle)
			{
				if (_crouchSubStateInfo.isJustSwitched)
				{
					_crouchSubStateInfo.isJustSwitched = false;
					crouchData.isZeroMagnitudeFixed = false;
					_movementController.MecanimController.SetCrouchIdle(state: true);
				}
				if (_translationData.currentSpeedXZ > 0.1f)
				{
					_crouchSubStateInfo.SetState(CrouchSubState.Moving, base.LocalTime);
				}
			}
			GroundTranslationControl();
			MecanimMovementControl();
		}

		public override void OnStateLateUpdate()
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
