using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class GroundMovementState : PlayerTranslationState
	{
		public GroundMovementState(TranslationStatesMap stateMap, MovementController movementController)
			: base(stateMap, movementController)
		{
		}

		protected void GroundTranslationControl()
		{
			Vector2 a = _translationData.targetRelativeDirection - _translationData.currentRelativeDirection.actual;
			float magnitude = a.magnitude;
			a.Normalize();
			Vector2 vector = a * _translationParameters.moveDirectionChangeSpeed * base.DeltaTime;
			if (vector.magnitude > magnitude)
			{
				_translationData.currentRelativeDirection.actual = _translationData.targetRelativeDirection;
			}
			else
			{
				_translationData.currentRelativeDirection.actual += vector;
			}
			_translationData.targetSpeed = _translationParameters.speedDefault * _translationData.currentSpeedMultiplier;
			_translationData.moveDirection = _movementController.MovementDirector.TransformDirection(_translationData.currentRelativeDirection3.actual);
			_translationData.targetDeltaPosition += _translationData.moveDirection * _translationData.targetSpeed * base.DeltaTime;
			float num = Mathf.Min(_translationData.currentSpeedY, 0f);
			num -= 10f * base.DeltaTime;
			_translationData.targetDeltaPosition.y += num * base.DeltaTime;
			_translationData.targetDeltaPosition *= _translationData.WeaponSpeedMult;
			if (!_characterController.isGrounded && !_movementController.CharacterControllerTrigger.collides)
			{
				Vector3 startPos = _movementController.MovementDirector.TransformPoint(_characterController.center) + Vector3.down * _characterController.height * 0.3f;
				if (!Raycaster.Raycast(startPos, Vector3.down, 1.3f, out RaycastHit _, null))
				{
					_statesMap.SetState(TranslationStatesEnum.Jump);
					((JumpState)_statesMap.GetState(TranslationStatesEnum.Jump)).SetSubState(JumpState.JumpSubState.Landing);
					return;
				}
			}
			if (base.cmdParameters.IsToJump)
			{
				_statesMap.SetState(TranslationStatesEnum.Jump);
				((JumpState)_statesMap.GetState(TranslationStatesEnum.Jump)).SetSubState(JumpState.JumpSubState.Starting);
			}
		}

		protected void MecanimMovementControl()
		{
			Vector3 forward = _movementController.characterTransform.forward;
			Vector3 velocity = _translationData.velocity;
			Vector3 b = new Vector3(velocity.x, 0f, velocity.z);
			float num = 0f - VectorAngle.AngleDirected(forward, b, _movementController.transform.up);
			float f = num / 180f * (float)Math.PI;
			Vector2 a = new Vector2(Mathf.Sin(f), Mathf.Cos(f));
			float d = b.magnitude / _translationParameters.speedDefault;
			a *= d;
			Vector2 vector = a - _translationData.currentRelativeDirectionMecanim;
			_translationData.currentRelativeDirectionMecanim = Vector2.Lerp(_translationData.currentRelativeDirectionMecanim, a, base.DeltaTime * 10f);
			vector.Normalize();
		}

		public virtual void RotationAppliying()
		{
		}
	}
}
