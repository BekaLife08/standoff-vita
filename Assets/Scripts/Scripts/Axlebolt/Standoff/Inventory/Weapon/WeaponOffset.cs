using Axlebolt.Standoff.Common;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class WeaponOffset : MonoBehaviour
	{
		[Serializable]
		public class OffsetInfo
		{
			public Vector3 leftArmAnglesStand;

			public Vector3 rightArmAnglesStand;

			public Vector3 leftArmAnglesCrouch;

			public Vector3 rightArmAnglesCrouch;

			public TransformTRS wDirOnStand;

			public TransformTRS wDirOnCrouch;

			public TransformTRS rightHandOffset;

			public TransformTRS leftHandOffset;

			public TransformTRS spineOffset;

			public TransformTRS hipOffset;

			public TransformTRS spineFPSOffset;

			public TransformTRS cameraFPSOffset;
		}

		[Serializable]
		public class HandleOffsetInfo
		{
			public TransformTRS rightHandTransform;

			public TransformTRS leftHandTransform;
		}

		public OffsetInfo offsetInfo;

		public HandleOffsetInfo handleOffsetInfo;
	}
}
