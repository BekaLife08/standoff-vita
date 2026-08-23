namespace Axlebolt.Standoff.Inventory
{
	public static class WeaponIdExtension
	{
		public static byte GetSlotIndex(this WeaponId weaponId)
		{
			if ((int)weaponId < 20)
			{
				return 2;
			}
			if ((int)weaponId >= 20 && (int)weaponId < 70)
			{
				return 1;
			}
			if ((int)weaponId >= 70 && (int)weaponId < 100)
			{
				return 3;
			}
			return 4;
		}

		public static GunType GetGunType(this WeaponId weaponId)
		{
			if ((int)weaponId < 20)
			{
				return GunType.Pistol;
			}
			if ((int)weaponId >= 20 && (int)weaponId < 40)
			{
				return GunType.Smg;
			}
			if ((int)weaponId >= 40 && (int)weaponId < 60)
			{
				return GunType.Rifels;
			}
			if ((int)weaponId >= 60 && (int)weaponId < 70)
			{
				return GunType.Shotgun;
			}
			return GunType.None;
		}
	}
}
