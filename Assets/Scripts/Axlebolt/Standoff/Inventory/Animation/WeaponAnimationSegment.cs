using System;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[Serializable]
	public class WeaponAnimationSegment
	{
		public enum SegmentActionType
		{
			Enable,
			Disable
		}

		public int startEventIndex;

		public int endEventIndex;

		public float startEventNT;

		public float endEventNT;

		public WeaponMap.WeaponPart weaponPart;

		public SegmentActionType actionType;
	}
}
