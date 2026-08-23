using UnityEngine;

namespace Axlebolt.Inventory.Animation
{
	public class WeaponAnimationAvatar
	{
		public enum AvatarDefinition
		{
			Weapon,
			Magazine1,
			Magazine2,
			Trigger,
			GunLock1,
			GunLock2,
			RightHand,
			LeftHand
		}

		public class AvatarElement
		{
			public AvatarDefinition element;

			public Transform elementTransform;
		}
	}
}
