using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public interface IWeaponryController
	{
		Transform Transform
		{
			get;
		}

		Vector3 WeaponDropPosition
		{
			get;
		}

		Vector3 WeaponDropDirection
		{
			get;
		}

		PhotonView PhotonView
		{
			get;
		}

		void SetWeapon(WeaponController weaponController);

		void ClearSlot(WeaponId weaponId);

		bool IsSlotFree(byte slotIndex);

		bool HasWeapon(WeaponId weaponId);

		void DropAllWeapons();
	}
}
