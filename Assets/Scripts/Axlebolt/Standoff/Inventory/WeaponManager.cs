using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Settings.Video;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponManager : Singleton<WeaponManager>
	{
		public readonly Event<WeaponController> ShootEvent = new Event<WeaponController>();

		private const WeaponId PrimaryWeapon = WeaponId.Knife;

		public const int PoolSize = 1;

		private bool _infinityAmmo;

		private WeaponPool _pool;

		public WeaponParameters[] Weapons
		{
			[CompilerGenerated]
			get
			{
				return _pool.Weapons;
			}
		}

		public WeaponMaterialManager WeaponMaterialManager
		{
			get;
			private set;
		}

		public void Init()
		{
			Init(WeaponUtility.LoadWeapons());
		}

		public void Init(WeaponParameters[] weapons)
		{
			if (_pool != null)
			{
				throw new InvalidOperationException("WeaponManager already initialized");
			}
			_pool = new WeaponPool(weapons, 1, base.transform, UpdateMaterial);
			WeaponMaterialManager = new WeaponMaterialManager(weapons, Singleton<InventoryManager>.Instance.GetSkinDefinitions());
			VideoSettingsManager.Instance.ModelDetailChanged += OnModelDetailChanged;
			VideoSettingsManager.Instance.ShaderDetailChanged += OnShaderDetailChanged;
		}

		private void CheckIsInitialized()
		{
			if (_pool == null)
			{
				throw new InvalidOperationException("WeaponManager does not initialized");
			}
		}

		public WeaponParameters GetParameters(WeaponId weaponId)
		{
			CheckIsInitialized();
			return _pool.GetParameters(weaponId);
		}

		public WeaponController GetLocal(WeaponId weaponId)
		{
			InventoryItemId selectedSkinId = Singleton<InventoryManager>.Instance.GetSelectedSkinId(weaponId);
			return Get(weaponId, selectedSkinId);
		}

		public WeaponController GetPrimaryWeapon()
		{
			return Get(WeaponId.Knife);
		}

		public WeaponController Get(WeaponId weaponId, InventoryItemId skinId)
		{
			CheckIsInitialized();
			WeaponController fromPool = _pool.GetFromPool(weaponId);
			fromPool.Initialize(skinId);
			UpdateMaterial(fromPool);
			GunController gunController = fromPool as GunController;
			if (gunController != null)
			{
				gunController.InfinityAmmo = _infinityAmmo;
			}
			return fromPool;
		}

		public WeaponController Get(WeaponId weaponId)
		{
			return Get(weaponId, InventoryItemId.None);
		}

		private void UpdateMaterial(WeaponController controller)
		{
			WeaponMaterialManager.SetMaterial(controller);
		}

		public void SetInfinityAmmo(bool infinityAmmo)
		{
			_infinityAmmo = infinityAmmo;
		}

		public void Return(WeaponController weapon)
		{
			CheckIsInitialized();
			_pool.ReturnToPool(weapon);
		}

		private void OnModelDetailChanged()
		{
			_pool.RefreshLods();
		}

		private void OnShaderDetailChanged()
		{
			WeaponMaterialManager.UpdateMaterials();
			WeaponController[] allInstances = _pool.GetAllInstances();
			foreach (WeaponController material in allInstances)
			{
				WeaponMaterialManager.SetMaterial(material);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			VideoSettingsManager.Instance.ModelDetailChanged -= OnModelDetailChanged;
			VideoSettingsManager.Instance.ShaderDetailChanged -= OnShaderDetailChanged;
		}

		public WeaponId GetRandomGunId(params GunType[] gunTypes)
		{
			GunParameters[] array = (from GunParameters gun in from weapon in _pool.Weapons
					where weapon is GunParameters
					select weapon
				where gunTypes.Contains(gun.GunType)
				select gun).ToArray();
			if (array.Length == 0)
			{
				throw new InvalidOperationException($"Invalid gunTypes {gunTypes}, not found");
			}
			GunParameters gunParameters = array[UnityEngine.Random.Range(0, array.Length)];
			return gunParameters.Id;
		}

		internal void OnShootEvent(WeaponController weaponController)
		{
			ShootEvent.Invoke(weaponController);
		}
	}
}
