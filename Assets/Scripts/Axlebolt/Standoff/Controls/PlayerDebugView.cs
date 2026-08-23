using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class PlayerDebugView : HudComponentView
	{
		[SerializeField]
		private Image _gunRecoilProgresBar;

		public override void SetPlayerController(PlayerController playerController)
		{
		}

		public override void UpdateView(PlayerController playerController)
		{
			WeaponController currentWeapon = playerController.WeaponryController.CurrentWeapon;
			if (currentWeapon != null && currentWeapon is GunController)
			{
				_gunRecoilProgresBar.fillAmount = ((GunController)currentWeapon).RecoilProgress;
			}
		}

		public void ClearDecals()
		{
			Singleton<SurfaceImpactsEmitter>.Instance.ClearDecals();
		}

		protected override void OnVisibleChanged(bool isvisible)
		{
			base.OnVisibleChanged(isvisible);
		}
	}
}
