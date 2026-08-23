using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[Serializable]
	public class WeaponAnimationClip
	{
		public List<string> sourceAnimationStateNameList = new List<string>();

		public List<int> sourceAnimationStateNameHashList = new List<int>();

		public string targetAnimationStateName = string.Empty;

		public int targetAnimationStateNameHash;

		public float clipDuration;

		public float clipSpeed;

		public List<WeaponAnimationEvent> eventList = new List<WeaponAnimationEvent>();

		public List<WeaponAnimationMarker> markerList = new List<WeaponAnimationMarker>();

		public List<WeaponAnimationSegment> segmentsList = new List<WeaponAnimationSegment>();
	}
}
