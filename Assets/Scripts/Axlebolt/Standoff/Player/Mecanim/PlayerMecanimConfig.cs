using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class PlayerMecanimConfig : ScriptableObject
	{
		[Serializable]
		public class StatePair
		{
			public string WeaponName;

			public bool Transitional;

			public string SourceStateName;

			public string TargetStateName;

			public int SourceStateHash;

			public int TargetStateHash;
		}

		[Serializable]
		public class BufferedFloatParameters
		{
			public int LayerId;

			public string Name;

			public int Hash;
		}

		public List<MecanimTransitionInfo> transitionList;

		public float maxTransitionNormalizedTimeDeviation;

		public float maxStateNormalizedTimeDeviation;

		public List<string> nonSyncParatetres = new List<string>();

		public List<StatePair> StateRemaper = new List<StatePair>();

		public List<BufferedFloatParameters> BufferedFloatParams = new List<BufferedFloatParameters>();

		public RuntimeAnimatorController animatorController;

		public Avatar avatar;
	}
}
