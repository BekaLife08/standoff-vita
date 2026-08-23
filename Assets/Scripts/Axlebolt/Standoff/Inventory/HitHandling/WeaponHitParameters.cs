using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	[Serializable]
	public class WeaponHitParameters
	{
		[SerializeField]
		protected WeaponId _weaponId;

		[SerializeField]
		protected InventoryItemId _skinId;

		[SerializeField]
		protected float _penetrationPower;

		[SerializeField]
		protected float _armorPenetration;

		[SerializeField]
		protected float _impulse;

		[SerializeField]
		protected Damage _damage;

		public WeaponId WeaponId
		{
			[CompilerGenerated]
			get
			{
				return _weaponId;
			}
		}

		public InventoryItemId SkinId
		{
			[CompilerGenerated]
			get
			{
				return _skinId;
			}
		}

		public float PenetrationPower
		{
			[CompilerGenerated]
			get
			{
				return _penetrationPower;
			}
		}

		public float ArmorPenetration
		{
			[CompilerGenerated]
			get
			{
				return _armorPenetration;
			}
		}

		public float Impulse
		{
			[CompilerGenerated]
			get
			{
				return _impulse;
			}
		}

		public Damage Damage
		{
			[CompilerGenerated]
			get
			{
				return _damage;
			}
		}

		public static WeaponHitParameters Create(WeaponId weaponId, InventoryItemId skinId, float penetrationPower, float armorPenetration, float impulse, [NotNull] Damage damage)
		{
			if (damage == null)
			{
				throw new ArgumentNullException("damage");
			}
			WeaponHitParameters weaponHitParameters = new WeaponHitParameters();
			weaponHitParameters._weaponId = weaponId;
			weaponHitParameters._skinId = skinId;
			weaponHitParameters._penetrationPower = penetrationPower;
			weaponHitParameters._armorPenetration = armorPenetration;
			weaponHitParameters._impulse = impulse;
			weaponHitParameters._damage = damage;
			return weaponHitParameters;
		}

		public virtual int GetDamage(PlayerHitbox hitbox, Vector3 hitDirection)
		{
			BipedMap.Bip bone = hitbox.Bone;
			if (BipedMap.IsHead(bone))
			{
				return _damage.HeadDamage;
			}
			if (BipedMap.IsChestAndArmsDamage(bone))
			{
				return _damage.ChestAndArmsDamage;
			}
			if (BipedMap.IsLegs(bone))
			{
				return _damage.LegsDamage;
			}
			if (BipedMap.IsStomach(bone))
			{
				return _damage.StomachDamage;
			}
			UnityEngine.Debug.LogError($"Unsupported bone {bone}");
			return 0;
		}
	}
}
