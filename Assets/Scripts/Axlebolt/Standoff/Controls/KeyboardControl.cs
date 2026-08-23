using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class KeyboardControl : MonoBehaviour
	{
		private const float SensitivityX = 5f;

		private const float SensitivityY = 5f;

		public PlayerInputs GetPlayerInputs()
		{
			PlayerInputs playerInputs = new PlayerInputs();
			playerInputs.SwitchToWeapon = -1;
			PlayerInputs playerInputs2 = playerInputs;
			playerInputs2.SwitchToWeapon = (UnityEngine.Input.GetKeyDown("1") ? 1 : playerInputs2.SwitchToWeapon);
			playerInputs2.SwitchToWeapon = ((!Input.GetKeyDown("2")) ? playerInputs2.SwitchToWeapon : 2);
			playerInputs2.SwitchToWeapon = ((!Input.GetKeyDown("3")) ? playerInputs2.SwitchToWeapon : 3);
			playerInputs2.SwitchToWeapon = ((!Input.GetKeyDown("4")) ? playerInputs2.SwitchToWeapon : 4);
			playerInputs2.SwitchToWeapon = ((!Input.GetKeyDown("5")) ? playerInputs2.SwitchToWeapon : 5);
			playerInputs2.IsToReload = false;
			playerInputs2.IsToReload = UnityEngine.Input.GetKeyDown("r");
			playerInputs2.IsToFire = Input.GetMouseButton(0);
			playerInputs2.IsToAim = Input.GetMouseButtonDown(1);
			playerInputs2.Vertical = UnityEngine.Input.GetAxis("Vertical");
			playerInputs2.Horizontal = UnityEngine.Input.GetAxis("Horizontal");
			playerInputs2.IsCrouching = UnityEngine.Input.GetKey(KeyCode.LeftControl);
			playerInputs2.IsToJump = UnityEngine.Input.GetKeyDown(KeyCode.Space);
			playerInputs2.IsToDrop = UnityEngine.Input.GetKeyDown(KeyCode.G);
			playerInputs2.IsToAction = UnityEngine.Input.GetKey(KeyCode.E);
			playerInputs2.DeltaAimAngles.y = UnityEngine.Input.GetAxis("Mouse X") * 5f;
			playerInputs2.DeltaAimAngles.x = UnityEngine.Input.GetAxis("Mouse Y") * 5f;
			return playerInputs2;
		}
	}
}
