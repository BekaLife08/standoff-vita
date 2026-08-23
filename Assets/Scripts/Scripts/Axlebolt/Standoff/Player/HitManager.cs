using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Main.Inventory;
using JetBrains.Annotations;
using System;

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
			int num = victim.GetArmor();
			int num2 = 0;
			BulletHitData[] hits = hitData.Hits;
			foreach (BulletHitData bulletHitData in hits)
			{
				num2 += bulletHitData.Damage;
				bool flag = BipedMap.IsHead(bulletHitData.Bone);
				if ((!flag || !victim.HasHelmet()) && flag)
				{
					int num3 = (int)((float)num2 * (100f - bulletHitData.ArmorPenetration) / 100f);
					num2 -= num3;
					num -= num3;
					if (num < 0)
					{
						num2 += -num;
						num = 0;
					}
				}
			}
			health -= num2;
			victim.SetHealth(health);
			victim.SetArmor(num);
			if (victim.IsDead())
			{
				victim.SetDeathTime(hitTime);
			}
			onHit(victim.IsDead());
			HitEventArgs hitEventArgs = GetHitEventArgs(shooter, victim, hitData, num2);
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
			return new HitEventArgs(shooter, victim, damage, parameters, skin, flag, flag2);
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
