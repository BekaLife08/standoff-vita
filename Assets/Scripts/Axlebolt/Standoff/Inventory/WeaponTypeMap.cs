using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Inventory.Weapon;
using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Inventory
{
	public static class WeaponTypeMap
	{
		public static Dictionary<WeaponType, Func<WeaponSnapshot>> SnapshotFactory;

		public static Dictionary<Type, WeaponType> SnapshotType;

		public static Dictionary<Type, Type> ControllerType;

		static WeaponTypeMap()
		{
			SnapshotFactory = new Dictionary<WeaponType, Func<WeaponSnapshot>>();
			SnapshotType = new Dictionary<Type, WeaponType>();
			ControllerType = new Dictionary<Type, Type>();
			SnapshotFactory[WeaponType.Gun] = (() => new GunSnapshot());
			ControllerType[typeof(GunParameters)] = typeof(GunController);
			SnapshotFactory[WeaponType.Knife] = (() => new KnifeSnapshot());
			ControllerType[typeof(KnifeParameters)] = typeof(KnifeController);
			SnapshotFactory[WeaponType.Bomb] = (() => new BombSnapshot());
			ControllerType[typeof(BombParameters)] = typeof(BombController);
			SnapshotFactory[WeaponType.DefuseKit] = (() => new DefuseKitSnapshot());
			ControllerType[typeof(DefuseKitParameters)] = typeof(DefuseKitController);
		}

		public static WeaponSnapshot NewSnapshotInstance(WeaponType weaponType)
		{
			if (SnapshotFactory.ContainsKey(weaponType))
			{
				return SnapshotFactory[weaponType]();
			}
			throw new Exception($"{weaponType} snapshot not found");
		}

		public static Type GetControllerType(WeaponParameters parameters)
		{
			return ControllerType[parameters.GetType()];
		}
	}
}
