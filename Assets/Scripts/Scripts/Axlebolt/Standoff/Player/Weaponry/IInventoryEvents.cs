using Axlebolt.Standoff.Inventory;

namespace Axlebolt.Standoff.Player.Weaponry
{
	public interface IInventoryEvents
	{
		void OnWeaponSet(WeaponController weapon);

		void OnNewWeaponSet(WeaponController weapon);

		void OnDropWeapon(WeaponController weapon);
	}
}
