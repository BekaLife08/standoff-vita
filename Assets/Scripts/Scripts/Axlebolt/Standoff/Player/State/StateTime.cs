using UnityEngine;

namespace Axlebolt.Standoff.Player.State
{
	public class StateTime
	{
		public float TimeStateEntered
		{
			get;
			set;
		}

		public float TimePast
		{
			get
			{
				return Time.time - TimeStateEntered;
			}
			set
			{
				TimeStateEntered = Time.time - value;
			}
		}
	}
}
