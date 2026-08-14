using Axlebolt.Standoff.Inventory;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerMainCamera : MonoBehaviour
	{
		private CameraScopeZoomer _cameraScopeZoomer;

		public PlayerController PlayerController
		{
			get;
			private set;
		}

		private void Awake()
		{
			_cameraScopeZoomer = base.gameObject.AddComponent<CameraScopeZoomer>();
		}

		public void AttachToPlayer(PlayerController playerController, Transform cameraPlaceholder)
		{
			if (PlayerController != null)
			{
				DetachFromPlayer();
			}
			PlayerController = playerController;
			_cameraScopeZoomer.PlayerController = playerController;
			base.transform.SetParent(cameraPlaceholder);
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}

		public void DetachFromPlayer()
		{
			_cameraScopeZoomer.PlayerController = null;
			base.transform.SetParent(null);
			PlayerController.ResetMainCamera();
			PlayerController = null;
		}
	}
}
