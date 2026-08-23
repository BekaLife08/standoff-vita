using Axlebolt.Standoff.Player.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class TranslationStatesMap : StateMap<TranslationStatesEnum>
	{
		public class Parameters : MessageBase
		{
			public int currentStateNO;

			public int prevStateNO;

			public override void Serialize(NetworkWriter writer)
			{
				writer.WritePackedUInt32((uint)currentStateNO);
				writer.WritePackedUInt32((uint)prevStateNO);
			}

			public override void Deserialize(NetworkReader reader)
			{
				currentStateNO = (int)reader.ReadPackedUInt32();
				prevStateNO = (int)reader.ReadPackedUInt32();
			}
		}

		public class BlendedValues
		{
			public ValueBlender jumpType = new ValueBlender();

			public ValueBlender standType = new ValueBlender();

			public List<ValueBlender> BlendedValuesList = new List<ValueBlender>();
		}

		public BlendedValues blendedValues = new BlendedValues();

		private int _stateId = -1;

		private readonly MovementController _movementController;

		protected float LocalTime
		{
			get
			{
				return _movementController.LocalTime;
			}
			set
			{
			}
		}

		public int StateId => _stateId;

		public TranslationStatesMap(MovementController movementController)
		{
			_movementController = movementController;
			PlayerTranslationState item = new WalkState(this, movementController)
			{
				StateEnum = TranslationStatesEnum.Walk
			};
			_stateList.Add(item);
			item = new IdleState(this, movementController)
			{
				StateEnum = TranslationStatesEnum.Idle
			};
			_stateList.Add(item);
			item = new CrouchState(this, movementController)
			{
				StateEnum = TranslationStatesEnum.Crouch
			};
			_stateList.Add(item);
			item = new JumpState(this, movementController)
			{
				StateEnum = TranslationStatesEnum.Jump
			};
			_stateList.Add(item);
			Initialize();
		}

		public void Initialize()
		{
			blendedValues = new BlendedValues();
			base.CurrentState = GetStateInstance(TranslationStatesEnum.Walk);
			blendedValues.jumpType.InitializeBlending(_movementController.translationParameters.generalCurveTypes.curveLinear11.id, 0.1f, LocalTime);
		}

		private State<TranslationStatesEnum> GetStateInstance(TranslationStatesEnum stateEnum)
		{
			foreach (State<TranslationStatesEnum> state in _stateList)
			{
				if (state.StateEnum == stateEnum)
				{
					return state;
				}
			}
			UnityEngine.Debug.LogError("State Instance Not Found");
			return null;
		}

		public override void SetState(TranslationStatesEnum state)
		{
			base.PreviousState = base.CurrentState;
			base.CurrentState = GetStateInstance(state);
			base.PreviousState.ExitState();
			base.CurrentState.EnterState();
		}

		private void SetState(int stateID)
		{
			_stateId = stateID;
		}

		public override State<TranslationStatesEnum> GetState(TranslationStatesEnum state)
		{
			foreach (State<TranslationStatesEnum> state2 in _stateList)
			{
				if (state2.StateEnum == state)
				{
					return state2;
				}
			}
			return null;
		}

		public override List<State<TranslationStatesEnum>> GetAllStates()
		{
			return null;
		}

		public Parameters GetParameters()
		{
			Parameters parameters = new Parameters();
			parameters.currentStateNO = (int)base.CurrentState.StateEnum;
			parameters.prevStateNO = (int)base.PreviousState.StateEnum;
			return parameters;
		}

		public void SetParameters(Parameters parameters)
		{
			base.CurrentState = GetState((TranslationStatesEnum)parameters.currentStateNO);
			base.PreviousState = GetState((TranslationStatesEnum)parameters.prevStateNO);
		}
	}
}
