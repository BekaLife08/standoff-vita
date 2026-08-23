using Axlebolt.Standoff.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponOffsetData : ScriptableObject
	{
		public TransformTRS wDirOnStand;

		public TransformTRS wDirOnCrouch;

		public TransformTRS rightHandOffset;

		public TransformTRS leftHandOffset;

		public TransformTRS spineOffset;

		public TransformTRS hipOffset;

		public TransformTRS spineFPSOffset;

		public TransformTRS cameraFPSOffset;

		public List<TransformTRS> additveOffsetData = new List<TransformTRS>();
	}
}
