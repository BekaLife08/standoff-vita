namespace Axlebolt.Standoff.Inventory.Drop
{
	public interface IWeaponDropListener
	{
		void OnWeaponDrop(WeaponParameters weapon);

		void OnWeaponTake(WeaponParameters weapon);
	}
}
