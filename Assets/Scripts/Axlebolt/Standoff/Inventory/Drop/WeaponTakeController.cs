using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class WeaponTakeController : MonoBehaviour
	{
		private const float TakeDistance = 1f;

		private IWeaponryController _weaponryController;

		private Transform _transform;

		public Transform Transform
		{
			[CompilerGenerated]
			get
			{
				return _transform ?? (_transform = base.transform);
			}
		}

		public void Init(IWeaponryController weaponryController)
		{
			_weaponryController = weaponryController;
		}

		private void Update()
		{
			if (_weaponryController == null || !_weaponryController.PhotonView.isMine || !ScenePhotonBehavior<WeaponDropManager>.IsInitialized() || ScenePhotonBehavior<WeaponDropManager>.Instance.InProgress)
			{
				return;
			}
			foreach (DroppedWeaponController droppedWeapon in ScenePhotonBehavior<WeaponDropManager>.Instance.GetDroppedWeapons())
			{
				byte slotIndex = droppedWeapon.SlotIndex;
				if (!_weaponryController.IsSlotFree(slotIndex) || Vector3.Distance(Transform.position, droppedWeapon.Transform.position) > 1f)
				{
					continue;
				}
				ScenePhotonBehavior<WeaponDropManager>.Instance.Take(droppedWeapon.DropId, _weaponryController);
				break;
			}
		}
	}
}
