using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerInputs
	{
		public float Horizontal;

		public float Vertical;

		public bool IsCrouching;

		public bool IsToReload;

		public int SwitchToWeapon = -1;

		public bool IsToAim;

		public bool IsToFire;

		public bool IsToJump;

		public bool IsToDrop;

		public bool IsToAction;

		public Vector3 AimEulerAngles = Vector3.zero;

		public Vector2 DeltaAimAngles = Vector2.zero;

		public void ResetValues()
		{
			Horizontal = 0f;
			Vertical = 0f;
			IsCrouching = false;
			IsToReload = false;
			SwitchToWeapon = -1;
			IsToAim = false;
			IsToFire = false;
			IsToJump = false;
			IsToDrop = false;
		}
	}
}
