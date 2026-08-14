using UnityEngine;

namespace Axlebolt.Common.States
{
	public class StateSimple<T>
	{
		public T curState;

		public T prevState;

		public float timeSwitched;

		public bool isJustSwitched;

		public int stateNo;

		public StateSimple()
		{
		}

		public StateSimple(T state)
		{
			curState = state;
		}

		public static void SetState(T state, StateSimple<T> stateInfo)
		{
			SetState(state, stateInfo, false, -1);
		}

		public static void SetState(T state, StateSimple<T> stateInfo, bool isForced)
		{
			SetState(state, stateInfo, isForced, -1);
		}

		public static void SetState(T state, StateSimple<T> stateInfo, int stateNo)
		{
			SetState(state, stateInfo, false, stateNo);
		}

		public static void SetState(T state, StateSimple<T> stateInfo, bool isForced, int stateNo)
		{
			stateInfo.prevState = stateInfo.curState;
			stateInfo.curState = state;
			stateInfo.timeSwitched = Time.time;
			stateInfo.isJustSwitched = true;
			if (stateNo != -1)
			{
				stateInfo.stateNo = stateNo;
			}
			else
			{
				stateInfo.stateNo++;
			}
		}

		public void SetState(T targetState, float time)
		{
			prevState = curState;
			curState = targetState;
			timeSwitched = time;
			isJustSwitched = true;
		}
	}
}
