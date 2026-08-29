using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Movement;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class HitManager : Singleton<HitManager>
	{
		private static readonly Log Log = Log.Create("HitManager");

		public readonly Event<HitEventArgs> HitEvent = new Event<HitEventArgs>();

		public readonly Event<PhotonPlayer> SuicideEvent = new Event<PhotonPlayer>();

		private bool _initialized;

		public bool FriendlyFireOn
		{
			get;
			private set;
		}

		public void Init(bool friendFireOn)
		{
			if (_initialized)
			{
				throw new InvalidOperationException("HitManager already initialized");
			}
			FriendlyFireOn = friendFireOn;
			_initialized = true;
		}

		public void Hit([NotNull] PhotonPlayer shooter, [NotNull] PhotonPlayer victim, double hitTime, [NotNull] HitData hitData, [NotNull] Action<bool> onHit)
		{
			if (shooter == null)
			{
				throw new ArgumentNullException("shooter");
			}
			if (victim == null)
			{
				throw new ArgumentNullException("victim");
			}
			if (hitData == null)
			{
				throw new ArgumentNullException("hitData");
			}
			if (onHit == null)
			{
				throw new ArgumentNullException("onHit");
			}
			if (victim.IsDead() || shooter.IsDead() || (victim.GetTeam() == shooter.GetTeam() && !object.Equals(victim, shooter) && !FriendlyFireOn))
			{
				return;
			}
			int health = victim.GetHealth();
			int armor = victim.GetArmor();
			int totalDamage = 0;
			int totalArmorBypass = 0;
			BulletHitData[] hits = hitData.Hits;
			foreach (BulletHitData bulletHitData in hits)
			{
				if (bulletHitData.ArmorPenetration >= 100f)
				{
					totalArmorBypass += bulletHitData.Damage;
				}
				else
				{
					totalDamage += bulletHitData.Damage;
				}
			}
			health -= totalArmorBypass;
			if (armor > 0 && totalDamage > 0)
			{
				int armorAbsorb = Mathf.CeilToInt(totalDamage * 0.65f);
				int healthDamage = totalDamage - armorAbsorb;
				if (armorAbsorb > armor)
				{
					healthDamage += armorAbsorb - armor;
					armor = 0;
				}
				else
				{
					armor -= armorAbsorb;
				}
				health -= healthDamage;
			}
			else
			{
				health -= totalDamage;
			}
			victim.SetHealth(health);
			victim.SetArmor(armor);
			if (victim.IsDead())
			{
				victim.SetDeathTime(hitTime);
			}
			onHit(victim.IsDead());
			HitEventArgs hitEventArgs = GetHitEventArgs(shooter, victim, hitData, totalDamage);
			OnHitEvent(hitEventArgs);
		}

		private static HitEventArgs GetHitEventArgs(PhotonPlayer shooter, PhotonPlayer victim, HitData hitData, int damage)
		{
			WeaponParameters parameters = Singleton<WeaponManager>.Instance.GetParameters(hitData.WeaponId);
			SkinDefinition skin = (hitData.SkinId == InventoryItemId.None) ? null : Singleton<InventoryManager>.Instance.GetSkinDefinition(hitData.SkinId);
			bool flag = false;
			bool flag2 = false;
			BulletHitData[] hits = hitData.Hits;
			foreach (BulletHitData bulletHitData in hits)
			{
				flag = (flag || BipedMap.IsHead(bulletHitData.Bone));
				flag2 = (flag2 || bulletHitData.Penetrated);
			}
			bool airborne = false;
			bool noScope = false;
			if (PhotonNetwork.offlineMode && !shooter.IsDead())
			{
				PlayerController shooterController = Singleton<PlayerManager>.Instance.GetController(shooter.ID);
				if (shooterController == null && shooter.IsLocal)
				{
					shooterController = Singleton<PlayerManager>.Instance.CurrentPlayer;
				}
				if (shooterController != null)
				{
					MovementController movement = shooterController.MovementController;
					if (movement != null && movement.CharacterController != null)
					{
						airborne = !movement.CharacterController.isGrounded;
					}
					if (hitData.WeaponId == WeaponId.AWM)
					{
						bool isAiming = shooterController.AimController != null && shooterController.AimController.ViewMode == ViewMode.FPS;
						noScope = !isAiming;
					}
				}
			}
			return new HitEventArgs(shooter, victim, damage, parameters, skin, flag, flag2, airborne, noScope);
		}

		public void Suicide(PhotonPlayer photonPlayer)
		{
			photonPlayer.SetHealth(0);
			photonPlayer.SetArmor(0);
			OnSuicideEvent(photonPlayer);
		}

		private void OnHitEvent(HitEventArgs eventArgs)
		{
			HitEvent.Invoke(eventArgs);
		}

		protected virtual void OnSuicideEvent(PhotonPlayer player)
		{
			SuicideEvent.Invoke(player);
		}
	}
}
