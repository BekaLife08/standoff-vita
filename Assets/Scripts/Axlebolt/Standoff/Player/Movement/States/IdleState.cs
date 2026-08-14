using Axlebolt.Common.States;
using Axlebolt.Standoff.Player.State;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class IdleState : GroundMovementState, ISynchronizableState
	{
		public class IdleData : MessageBase
		{
			public Vector3 eulerAnglesFixed = Vector3.zero;

			public float currentCharacterEulerAngleY;

			public float currentRotationCoeff;

			public float currentRotationDuration;

			public float toRotateAngle;

			public float toRotateFixedAngle;

			public int idleSubStateNO = -1;

			public int idleSubStatePrevNO = -1;

			public float idleSubStateEnterTime;

			public bool isIdleSubStateJustSwitched;

			public override void Serialize(NetworkWriter writer)
			{
				writer.Write(eulerAnglesFixed);
				writer.Write(currentCharacterEulerAngleY);
				writer.Write(currentRotationCoeff);
				writer.Write(currentRotationDuration);
				writer.Write(toRotateAngle);
				writer.Write(toRotateFixedAngle);
				writer.WritePackedUInt32((uint)idleSubStateNO);
				writer.WritePackedUInt32((uint)idleSubStatePrevNO);
				writer.Write(idleSubStateEnterTime);
				writer.Write(isIdleSubStateJustSwitched);
			}

			public override void Deserialize(NetworkReader reader)
			{
				eulerAnglesFixed = reader.ReadVector3();
				currentCharacterEulerAngleY = reader.ReadSingle();
				currentRotationCoeff = reader.ReadSingle();
				currentRotationDuration = reader.ReadSingle();
				toRotateAngle = reader.ReadSingle();
				toRotateFixedAngle = reader.ReadSingle();
				idleSubStateNO = (int)reader.ReadPackedUInt32();
				idleSubStatePrevNO = (int)reader.ReadPackedUInt32();
				idleSubStateEnterTime = reader.ReadSingle();
				isIdleSubStateJustSwitched = reader.ReadBoolean();
			}
		}

		public enum IdleSubState
		{
			Rotating,
			Standing,
			NotStated
		}

		private IdleData idleData = new IdleData();

		private StateSimple<IdleSubState> _idleSubStateInfo = new StateSimple<IdleSubState>(IdleSubState.NotStated);

		public IdleState(TranslationStatesMap statesMap, MovementController movementController)
			: base(statesMap, movementController)
		{
		}

		public void SetParameters(MessageBase parameters)
		{
			idleData = (IdleData)parameters;
			_idleSubStateInfo.timeSwitched = idleData.idleSubStateEnterTime;
			_idleSubStateInfo.curState = (IdleSubState)idleData.idleSubStateNO;
			_idleSubStateInfo.prevState = (IdleSubState)idleData.idleSubStatePrevNO;
			_idleSubStateInfo.isJustSwitched = idleData.isIdleSubStateJustSwitched;
		}

		public MessageBase GetParameters()
		{
			idleData.idleSubStateEnterTime = _idleSubStateInfo.timeSwitched;
			idleData.idleSubStateNO = (int)_idleSubStateInfo.curState;
			idleData.idleSubStatePrevNO = (int)_idleSubStateInfo.prevState;
			idleData.isIdleSubStateJustSwitched = _idleSubStateInfo.isJustSwitched;
			return idleData;
		}

		public override void OnStateEntered()
		{
			idleData.eulerAnglesFixed = _movementController.MovementDirector.eulerAngles;
			IdleData obj = idleData;
			Vector3 eulerAngles = _movementController.characterTransform.eulerAngles;
			obj.currentCharacterEulerAngleY = eulerAngles.y;
			_idleSubStateInfo.SetState(IdleSubState.Standing, base.LocalTime);
			_characterController.height = _translationParameters.characterColliderParameters.heightOnStand;
			_characterController.center = _translationParameters.characterColliderParameters.centerOnStand;
			if (_statesMap.PreviousState.StateEnum == TranslationStatesEnum.Crouch)
			{
				OnStandEvent();
			}
		}

		public override void OnStateUpdate()
		{
			_translationData.targetDeltaPosition = Vector3.zero;
			if (_translationData.currentSpeedXZ > 0.3f)
			{
				_statesMap.SetState(TranslationStatesEnum.Walk);
			}
			if (base.cmdParameters.IsCrouching)
			{
				_statesMap.SetState(TranslationStatesEnum.Crouch);
			}
			_translationData.standTypeCoeff.actual = Mathf.Lerp(_translationData.standTypeCoeff.actual, 1f, base.DeltaTime * 5f);
			_translationData.jumpTypeCoeff.actual = _statesMap.blendedValues.jumpType.GetValue(base.LocalTime);
			if (_idleSubStateInfo.curState == IdleSubState.Rotating)
			{
				if (_idleSubStateInfo.isJustSwitched)
				{
					_idleSubStateInfo.isJustSwitched = false;
					idleData.currentRotationCoeff = (Mathf.Abs(idleData.toRotateAngle) - 60f) / 120f;
					idleData.currentRotationDuration = _translationParameters.idleParameters.rotationDuration1 + (_translationParameters.idleParameters.rotationDuration2 - _translationParameters.idleParameters.rotationDuration1) * idleData.currentRotationCoeff;
					_movementController.MecanimController.SetRotating(idleData.toRotateAngle);
				}
				float num = CurvedValue.GetCurveValue(_translationParameters.idleParameters.rotationCurve1, idleData.currentRotationDuration, _idleSubStateInfo.timeSwitched, base.LocalTime) * (1f - idleData.currentRotationCoeff) + CurvedValue.GetCurveValue(_translationParameters.idleParameters.rotationCurve2, idleData.currentRotationDuration, _idleSubStateInfo.timeSwitched, base.LocalTime) * idleData.currentRotationCoeff;
				idleData.currentCharacterEulerAngleY = idleData.toRotateFixedAngle + idleData.toRotateAngle * num;
				float num2 = base.LocalTime - _idleSubStateInfo.timeSwitched;
				if (num2 > idleData.currentRotationDuration)
				{
					_idleSubStateInfo.SetState(IdleSubState.Standing, base.LocalTime);
				}
			}
			if (_idleSubStateInfo.curState == IdleSubState.Standing)
			{
				if (_idleSubStateInfo.isJustSwitched)
				{
					_idleSubStateInfo.isJustSwitched = false;
				}
				Vector3 eulerAngles = _movementController.characterTransform.eulerAngles;
				float y = eulerAngles.y;
				Vector3 eulerAngles2 = _movementController.transform.eulerAngles;
				float num3 = 0f - VectorAngle.DeltaAngle(y, eulerAngles2.y);
				if (Mathf.Abs(num3) > 60f)
				{
					idleData.toRotateAngle = num3;
					IdleData obj = idleData;
					Vector3 eulerAngles3 = _movementController.characterTransform.eulerAngles;
					obj.toRotateFixedAngle = eulerAngles3.y;
					_idleSubStateInfo.SetState(IdleSubState.Rotating, base.LocalTime);
				}
			}
			GroundTranslationControl();
			MecanimMovementControl();
		}

		public override void LateUpdateState()
		{
			RotationAppliying();
		}

		public override void RotationAppliying()
		{
			Vector3 eulerAngles = base.CharacterTransform.eulerAngles;
			eulerAngles.y = idleData.currentCharacterEulerAngleY;
			base.CharacterTransform.eulerAngles = eulerAngles;
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
		}
	}
}
