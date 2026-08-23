using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Player;

namespace Axlebolt.Standoff.Controls
{
	public class DropButton : InteractableButton, IHudComponentView
	{
		public void SetPlayerController(PlayerController playerController)
		{
			SetActive(active: false);
		}

		private void SetActive(bool active)
		{
			base.IsActive = active;
			base.gameObject.SetActive(active);
		}

		public void UpdateView(PlayerController playerController)
		{
			if (!ScenePhotonBehavior<WeaponDropManager>.IsInitialized())
			{
				return;
			}
			if (base.IsActive)
			{
				if (!playerController.WeaponryController.CurrentWeapon.IsDroppable())
				{
					SetActive(active: false);
				}
			}
			else if (playerController.WeaponryController.CurrentWeapon.IsDroppable())
			{
				SetActive(active: true);
			}
		}
	}
}
