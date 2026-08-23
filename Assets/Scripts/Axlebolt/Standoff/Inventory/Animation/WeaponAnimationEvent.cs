using System;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[Serializable]
	public class WeaponAnimationEvent
	{
		public int id = -1;

		public float normalizedTime;

		public WeaponAnimationEventType eventType;
	}
}
