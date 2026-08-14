using Axlebolt.Standoff.Player.State;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class PlayerTranslationState : State<TranslationStatesEnum>
	{
		internal PlayerTranslationData _translationData;

		internal PlayerTranslationParameters _translationParameters;

		internal List<ValueBlender> _transitionsList = new List<ValueBlender>();

		internal MovementController _movementController;

		internal CharacterController _characterController;

		internal TranslationStatesMap _statesMap;

		internal Transform characterTransform;

		protected float _transitionDuration;

		protected Transform CharacterTransform
		{
			get
			{
				if (characterTransform != _movementController.characterTransform)
				{
					characterTransform = _movementController.characterTransform;
				}
				return characterTransform;
			}
			set
			{
				characterTransform = value;
			}
		}

		public float transitionDuration
		{
			get
			{
				return _transitionDuration;
			}
			set
			{
				_transitionDuration = value;
			}
		}

		protected float DeltaTime
		{
			get
			{
				return _movementController.DeltaTime;
			}
			private set
			{
			}
		}

		protected MovementControllerCmd cmdParameters
		{
			get
			{
				return _movementController.CmdParameters;
			}
			private set
			{
			}
		}

		public PlayerTranslationState(TranslationStatesMap statesMap, MovementController movementController)
		{
			_movementController = movementController;
			_characterController = movementController.CharacterController;
			_translationData = movementController.translationData;
			_translationParameters = movementController.translationParameters;
			_statesMap = statesMap;
			CharacterTransform = movementController.characterTransform;
			cmdParameters = movementController.CmdParameters;
		}

		public override float GetLocalTime()
		{
			return _movementController.LocalTime;
		}

		protected void OnLandEvent(float landDuration)
		{
			IMovementEvents[] movementEventsImpls = _movementController.MovementEventsImpls;
			foreach (IMovementEvents movementEvents in movementEventsImpls)
			{
				movementEvents.OnLand(_movementController, landDuration);
			}
		}

		protected void OnCrouchEvent()
		{
			IMovementEvents[] movementEventsImpls = _movementController.MovementEventsImpls;
			foreach (IMovementEvents movementEvents in movementEventsImpls)
			{
				movementEvents.OnCrouch(_movementController);
			}
		}

		protected void OnJumpEvent()
		{
			IMovementEvents[] movementEventsImpls = _movementController.MovementEventsImpls;
			foreach (IMovementEvents movementEvents in movementEventsImpls)
			{
				movementEvents.OnJump(_movementController);
			}
		}

		protected void OnStandEvent()
		{
			IMovementEvents[] movementEventsImpls = _movementController.MovementEventsImpls;
			foreach (IMovementEvents movementEvents in movementEventsImpls)
			{
				movementEvents.OnStand(_movementController);
			}
		}

		public override bool IsInTransition()
		{
			foreach (ValueBlender transitions in _transitionsList)
			{
				if (!transitions.IsFinished(base.LocalTime))
				{
					return true;
				}
			}
			return false;
		}

		public virtual List<object> GetStateSubData()
		{
			return new List<object>();
		}

		public virtual void SetStateSubData(List<object> dataList)
		{
		}

		public virtual void OnPostProcessParameters()
		{
		}
	}
}
