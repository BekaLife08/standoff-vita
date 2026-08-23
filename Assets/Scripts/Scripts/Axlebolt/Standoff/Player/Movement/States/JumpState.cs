using Axlebolt.Common.States;
using Axlebolt.Standoff.Player.State;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class JumpState : PlayerTranslationState, ISynchronizableState
	{
		public class JumpData : MessageBase
		{
			public float currentUpwardSpeed;

			public float timeFallStartFixedTime;

			public float timeFallStartDuration;

			public int jumpSubStateNO = -1;

			public int jumpSubStatePrevNO = -1;

			public float jumpSubStateEnterTime;

			public bool isJumpSubStateJustSwitched;

			public override void Serialize(NetworkWriter writer)
			{
				writer.Write(currentUpwardSpeed);
				writer.Write(timeFallStartFixedTime);
				writer.Write(timeFallStartDuration);
				writer.WritePackedUInt32((uint)jumpSubStateNO);
				writer.WritePackedUInt32((uint)jumpSubStatePrevNO);
				writer.Write(jumpSubStateEnterTime);
				writer.Write(isJumpSubStateJustSwitched);
			}

			public override void Deserialize(NetworkReader reader)
			{
				currentUpwardSpeed = reader.ReadSingle();
				timeFallStartFixedTime = reader.ReadSingle();
				timeFallStartDuration = reader.ReadSingle();
				jumpSubStateNO = (int)reader.ReadPackedUInt32();
				jumpSubStatePrevNO = (int)reader.ReadPackedUInt32();
				jumpSubStateEnterTime = reader.ReadSingle();
				isJumpSubStateJustSwitched = reader.ReadBoolean();
			}
		}

		public enum JumpSubState
		{
			Starting = 1,
			Landing,
			NotStated
		}

		public const float G = 12f;

		public JumpData jumpData = new JumpData();

		private StateSimple<JumpSubState> _jumpSubStateInfo = new StateSimple<JumpSubState>(JumpSubState.NotStated);

		public JumpState(TranslationStatesMap statesMap, MovementController movementController)
			: base(statesMap, movementController)
		{
		}

		public void SetParameters(MessageBase parameters)
		{
			jumpData = (JumpData)parameters;
			_jumpSubStateInfo.timeSwitched = jumpData.jumpSubStateEnterTime;
			_jumpSubStateInfo.curState = (JumpSubState)jumpData.jumpSubStateNO;
			_jumpSubStateInfo.prevState = (JumpSubState)jumpData.jumpSubStatePrevNO;
			_jumpSubStateInfo.isJustSwitched = jumpData.isJumpSubStateJustSwitched;
		}

		public MessageBase GetParameters()
		{
			jumpData.jumpSubStateEnterTime = _jumpSubStateInfo.timeSwitched;
			jumpData.jumpSubStateNO = (int)_jumpSubStateInfo.curState;
			jumpData.jumpSubStatePrevNO = (int)_jumpSubStateInfo.prevState;
			jumpData.isJumpSubStateJustSwitched = _jumpSubStateInfo.isJustSwitched;
			return jumpData;
		}

		public void SetSubState(JumpSubState state)
		{
			_jumpSubStateInfo.SetState(state, base.LocalTime);
		}

		public override void OnStateEntered()
		{
			_translationData.moveDirection = _movementController.MovementDirector.TransformDirection(_translationData.currentRelativeDirection3.actual).normalized;
			_translationData.fixedRelativeDirectionMecanim = _translationData.currentRelativeDirectionMecanim;
			_statesMap.blendedValues.jumpType.BlendValue(0.2f, _translationParameters.jumpParameters.jumpCurve, _translationData.jumpTypeCoeff.actual, base.LocalTime);
		}

		public override void OnStateUpdate()
		{
			_translationData.targetDeltaPosition = Vector3.zero;
			float timePast = StateTime.TimePast;
			if (timePast > 0.3f && _characterController.isGrounded)
			{
				jumpData.currentUpwardSpeed = 0f;
				_statesMap.SetState(TranslationStatesEnum.Walk);
				OnLandEvent(base.LocalTime - jumpData.timeFallStartFixedTime);
				return;
			}
			if (_jumpSubStateInfo.curState == JumpSubState.Landing)
			{
				if (_jumpSubStateInfo.isJustSwitched)
				{
					_jumpSubStateInfo.isJustSwitched = false;
					jumpData.timeFallStartFixedTime = base.LocalTime;
					jumpData.timeFallStartDuration = 0f;
				}
				jumpData.timeFallStartDuration += base.DeltaTime;
				_translationData.jumpTypeCoeff.actual = Mathf.Lerp(_translationData.jumpTypeCoeff.actual, 0f, base.DeltaTime * 5f);
			}
			if (_jumpSubStateInfo.curState == JumpSubState.Starting)
			{
				if (_jumpSubStateInfo.isJustSwitched)
				{
					jumpData.currentUpwardSpeed = _translationParameters.jumpParameters.upwardSpeedDefualt;
					_jumpSubStateInfo.isJustSwitched = false;
					OnJumpEvent();
				}
				if (timePast > 0.1f)
				{
					Vector3 velocity = _characterController.velocity;
					if (velocity.y < 0f)
					{
						_jumpSubStateInfo.SetState(JumpSubState.Landing, base.LocalTime);
					}
				}
				_translationData.jumpTypeCoeff.actual = _statesMap.blendedValues.jumpType.GetValue(base.LocalTime);
				_translationData.standTypeCoeff.actual = Mathf.Lerp(_translationData.standTypeCoeff.actual, 1f, base.DeltaTime * 4f);
			}
			jumpData.currentUpwardSpeed -= 12f * base.DeltaTime;
			_translationData.targetDeltaPosition += _movementController.MovementDirector.up * jumpData.currentUpwardSpeed * base.DeltaTime;
			float currentSpeedXZ = _translationData.currentSpeedXZ;
			currentSpeedXZ = Mathf.Max(currentSpeedXZ, ValueBlender.GetCurveValue(_translationParameters.jumpParameters.minSpeedCurve.curve, _translationParameters.jumpParameters.minSpeedCurve.duration, StateTime.TimeStateEntered, base.LocalTime) * _translationParameters.jumpParameters.minSpeedCurve.multiplier);
			_translationData.currentRelativeDirection.actual = _translationData.currentRelativeDirection.actual.normalized * (currentSpeedXZ / _translationData.targetSpeed);
			_translationData.currentRelativeDirectionMecanim = Vector2.Lerp(_translationData.currentRelativeDirectionMecanim, _translationData.fixedRelativeDirectionMecanim * 0.3f, base.DeltaTime * 2f);
			_translationData.targetDeltaPosition += _translationData.moveDirection * currentSpeedXZ * base.DeltaTime;
		}

		public override void OnStateLateUpdate()
		{
		}

		public override void OnStateExit()
		{
			_statesMap.blendedValues.jumpType.BlendValue(0.2f, _translationParameters.jumpParameters.landCurve, _translationData.jumpTypeCoeff.actual, base.LocalTime);
			_jumpSubStateInfo.SetState(JumpSubState.NotStated, base.LocalTime);
		}
	}
}
