using System.Collections.Generic;

namespace Axlebolt.Standoff.Player.State
{
	public class StateMap<TEnumState>
	{
		internal List<State<TEnumState>> _stateList = new List<State<TEnumState>>();

		public State<TEnumState> CurrentState
		{
			get;
			set;
		}

		public State<TEnumState> PreviousState
		{
			get;
			set;
		}

		public virtual void SetState(TEnumState state)
		{
		}

		public virtual void UpdateState()
		{
			if (CurrentState != null)
			{
				CurrentState.UpdateState();
			}
		}

		public virtual void LateUpdateState()
		{
			if (CurrentState != null)
			{
				CurrentState.LateUpdateState();
			}
		}

		public virtual State<TEnumState> GetState(TEnumState state)
		{
			return null;
		}

		public virtual List<State<TEnumState>> GetAllStates()
		{
			return null;
		}
	}
}
