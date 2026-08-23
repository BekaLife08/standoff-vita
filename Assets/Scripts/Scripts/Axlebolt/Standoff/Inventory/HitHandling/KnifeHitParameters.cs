using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	public class KnifeHitParameters : WeaponHitParameters
	{
		private int _backDamage;

		public static KnifeHitParameters Create(WeaponId weaponId, InventoryItemId skinId, float penetrationPower, float armorPenetration, float impulse, [NotNull] Damage damage, int backDamage)
		{
			if (damage == null)
			{
				throw new ArgumentNullException("damage");
			}
			KnifeHitParameters knifeHitParameters = new KnifeHitParameters();
			knifeHitParameters._weaponId = weaponId;
			knifeHitParameters._skinId = skinId;
			knifeHitParameters._penetrationPower = penetrationPower;
			knifeHitParameters._armorPenetration = armorPenetration;
			knifeHitParameters._impulse = impulse;
			knifeHitParameters._damage = damage;
			knifeHitParameters._backDamage = backDamage;
			return knifeHitParameters;
		}

		private bool IsBacksideHit(Transform character, Vector3 hitDirection)
		{
			float num = Vector3.Angle(character.forward, -hitDirection);
			return num > 90f;
		}

		public override int GetDamage(PlayerHitbox hitbox, Vector3 hitDirection)
		{
			BipedMap.Bip bone = hitbox.Bone;
			if (BipedMap.IsTorso(bone) && IsBacksideHit(hitbox.PlayerHitController.transform, hitDirection))
			{
				return _backDamage;
			}
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
