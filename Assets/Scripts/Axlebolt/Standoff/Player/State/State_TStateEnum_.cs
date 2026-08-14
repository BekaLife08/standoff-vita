using UnityEngine;

namespace Axlebolt.Standoff.Player.State
{
	public class State<TStateEnum>
	{
		public enum EnterType
		{
			ImmediateEnter,
			Normal
		}

		internal readonly StateTime StateTime = new StateTime();

		public int StateNo
		{
			get;
			set;
		}

		public TStateEnum StateEnum
		{
			get;
			set;
		}

		protected float LocalTime => GetLocalTime();

		public State()
		{
			StateNo = -1;
		}

		public virtual float GetLocalTime()
		{
			return -1f;
		}

		public virtual void EnterState()
		{
			StateTime.TimeStateEntered = Time.time;
			OnStateEntered();
		}

		public virtual void ExitState()
		{
			OnStateExit();
		}

		public virtual void UpdateState()
		{
			OnStateUpdate();
			PostProcessParameters();
		}

		public virtual void LateUpdateState()
		{
			OnStateLateUpdate();
		}

		public virtual void PostProcessParameters()
		{
		}

		public virtual void OnStateEntered()
		{
		}

		public virtual void OnStateExit()
		{
		}

		public virtual void OnStateUpdate()
		{
		}

		public virtual void OnStateLateUpdate()
		{
		}

		public virtual bool IsInTransition()
		{
			return false;
		}
	}
}
