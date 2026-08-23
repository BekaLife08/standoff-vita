using Axlebolt.Standoff.Player;

namespace Axlebolt.Standoff.Inventory
{
	public interface IWeaponSystemEventListener
	{
		void OnWeaponShoot(PlayerController controller, WeaponController weaponController);
	}
}
