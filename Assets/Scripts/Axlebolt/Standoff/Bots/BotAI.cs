using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using UnityEngine;

namespace Axlebolt.Standoff.Bots
{
	public class BotAI : MonoBehaviour
	{
		private const float SightRange = 40f;
		private const float FireRange = 28f;
		private const float StopRange = 30f;
		private const float MaxTurnSpeed = 180f;
		private const float RespawnDelay = 4f;
		private const float ThinkInterval = 0.2f;
		private const float ReactionTime = 1.4f;
		private const float SniperReactionTime = 2.8f;
		private const float BaseSpread = 0.18f;
		private const float SpreadPerMeter = 0.014f;
		private const float MaxSpread = 0.6f;
		private const float MinFireInterval = 0.45f;
		private const float DamageMultiplier = 0.25f;
		private const float SniperDamageMultiplier = 0.08f;
		private const float SniperSpreadMultiplier = 2.2f;

		private static readonly RaycastHit[] ShotHits = new RaycastHit[16];

		private PhotonPlayer _bot;
		private PlayerController _pc;
		private WeaponId _weaponId;
		private GunParameters _gunParameters;
		private float _fireInterval;
		private float _nextFireTime;
		private float _nextThinkTime;
		private float _nextPatrolTime;
		private float _reactionTime = ReactionTime;
		private float _damageMultiplier = DamageMultiplier;
		private float _spreadMultiplier = 1f;
		private Vector3 _patrolTarget;
		private PlayerController _target;
		private float _targetAcquiredTime;
		private float _respawnTime;
		private float _nextStatusLogTime;
		private bool _dead;

		public void Init(PhotonPlayer bot, PlayerController pc)
		{
			_bot = bot;
			_pc = pc;
			_weaponId = Singleton<WeaponManager>.Instance.GetRandomGunId(GunType.Heavy, GunType.Rifels, GunType.Smg);
			_gunParameters = Singleton<WeaponManager>.Instance.GetParameters(_weaponId) as GunParameters;
			if (_weaponId == WeaponId.AWM)
			{
				_reactionTime = SniperReactionTime;
				_damageMultiplier = SniperDamageMultiplier;
				_spreadMultiplier = SniperSpreadMultiplier;
			}
			else
			{
				_reactionTime = ReactionTime;
				_damageMultiplier = DamageMultiplier;
				_spreadMultiplier = 1f;
			}
			_fireInterval = (_gunParameters != null && _gunParameters.FireRate > 0) ? (60f / _gunParameters.FireRate) : 0.15f;
			_fireInterval = Mathf.Max(_fireInterval, MinFireInterval);
			if (_weaponId == WeaponId.AWM)
			{
				_fireInterval = Mathf.Max(_fireInterval, 2f);
			}
			_nextFireTime = Time.time + 0.5f;
			_dead = false;
			_target = null;
			_targetAcquiredTime = 0f;
			_pickWeapon();
		}

		private void _pickWeapon()
		{
			WeaponController weaponController = Singleton<WeaponManager>.Instance.Get(_weaponId);
			if (weaponController != null)
			{
				_pc.WeaponryController.SetWeapon(weaponController);
			}
		}

		private void Update()
		{
			if (_bot == null || _pc == null || !PhotonNetwork.offlineMode)
			{
				return;
			}
			if (Time.time >= _nextStatusLogTime)
			{
				_nextStatusLogTime = Time.time + 3f;
				UnityEngine.Debug.Log("BotAI status: bot " + _bot.ID + " dead=" + _bot.IsDead() + " health=" + _bot.GetHealth() + " pc=" + (_pc != null) + " target=" + (_target != null ? (_target.Player != null ? _target.Player.ID.ToString() : "nullPlayer") : "none"));
			}
			if (_bot.IsDead())
			{
				if (!_dead)
				{
					_dead = true;
					_respawnTime = Time.time + RespawnDelay;
					UnityEngine.Debug.Log("BotAI: bot " + _bot.ID + " died, respawn at " + _respawnTime);
				}
				else if (Time.time >= _respawnTime)
				{
					_dead = false;
					UnityEngine.Debug.Log("BotAI: respawning bot " + _bot.ID);
					BotManager.Respawn(_bot);
				}
				return;
			}

			PlayerInputs playerInputs = new PlayerInputs();
			if (_target == null || _target.Player == null || _target.Player.IsDead() || _target.Player.GetTeam() == _bot.GetTeam() || GetPlayerController(_target.Player) == null)
			{
				PlayerController previous = _target;
				_target = FindTarget();
				if (_target != null && _target != previous)
				{
					_targetAcquiredTime = Time.time;
					_nextFireTime = Mathf.Max(_nextFireTime, Time.time + _reactionTime);
				}
			}
			if (_target != null)
			{
				Vector3 vector = _target.transform.position - base.transform.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				Vector3 normalized = vector.normalized;
				float targetYaw = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
				float currentYaw = Mathf.Repeat(base.transform.eulerAngles.y, 360f);
				float deltaYaw = Mathf.DeltaAngle(currentYaw, targetYaw);
				playerInputs.DeltaAimAngles.y = Mathf.Clamp(deltaYaw, -MaxTurnSpeed * Time.deltaTime, MaxTurnSpeed * Time.deltaTime);
				if (magnitude > StopRange)
				{
					playerInputs.Vertical = 1f;
				}
				else if (magnitude > FireRange)
				{
					playerInputs.Vertical = 0.5f;
				}
				else
				{
					playerInputs.Vertical = 0f;
				}
				if (magnitude <= FireRange && Time.time >= _nextFireTime && Time.time - _targetAcquiredTime >= _reactionTime && HasLineOfSight(_target))
				{
					_nextFireTime = Time.time + _fireInterval;
					Shoot(_target);
				}
			}
			else
			{
				Patrol(ref playerInputs);
			}
			_pc.SetInputs(playerInputs, Time.deltaTime);
		}

		private PlayerController FindTarget()
		{
			if (Time.time < _nextThinkTime)
			{
				return _target;
			}
			_nextThinkTime = Time.time + ThinkInterval;
			PlayerController result = null;
			float bestDistance = SightRange;
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			for (int i = 0; i < playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = playerList[i];
				if (photonPlayer.ID == _bot.ID || photonPlayer.IsDead() || photonPlayer.GetTeam() == _bot.GetTeam())
				{
					continue;
				}
				PlayerController controller = GetPlayerController(photonPlayer);
				if (controller == null)
				{
					continue;
				}
				float distance = Vector3.Distance(base.transform.position, controller.transform.position);
				if (distance > bestDistance || !HasLineOfSight(controller))
				{
					continue;
				}
				bestDistance = distance;
				result = controller;
			}
			return result;
		}

		private PlayerController GetPlayerController(PhotonPlayer player)
		{
			if (player == null)
			{
				return null;
			}
			if (player.IsLocal)
			{
				return Singleton<PlayerManager>.Instance.CurrentPlayer;
			}
			return Singleton<PlayerManager>.Instance.GetController(player.ID);
		}

		private bool HasLineOfSight(PlayerController target)
		{
			Vector3 origin = base.transform.position + Vector3.up * 1.5f;
			Vector3 end = target.transform.position + Vector3.up * 1.2f;
			Vector3 direction = end - origin;
			RaycastHit[] array = Physics.RaycastAll(origin, direction, direction.magnitude);
			for (int i = 0; i < array.Length; i++)
			{
				PlayerController playerController = array[i].collider.GetComponentInParent<PlayerController>();
				if (playerController == null)
				{
					return false;
				}
				if (playerController != _pc)
				{
					return playerController == target;
				}
			}
			return true;
		}

		private void Patrol(ref PlayerInputs inputs)
		{
			if (Time.time >= _nextPatrolTime || Vector3.Distance(base.transform.position, _patrolTarget) < 2f)
			{
				_nextPatrolTime = Time.time + UnityEngine.Random.Range(2f, 5f);
				Vector2 vector = UnityEngine.Random.insideUnitCircle * 12f;
				_patrolTarget = base.transform.position + new Vector3(vector.x, 0f, vector.y);
			}
			Vector3 vector2 = _patrolTarget - base.transform.position;
			vector2.y = 0f;
			if (vector2.sqrMagnitude < 1f)
			{
				return;
			}
			Vector3 normalized = vector2.normalized;
			float targetYaw = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
			float currentYaw = Mathf.Repeat(base.transform.eulerAngles.y, 360f);
			float deltaYaw = Mathf.DeltaAngle(currentYaw, targetYaw);
			inputs.DeltaAimAngles.y = Mathf.Clamp(deltaYaw, -MaxTurnSpeed * Time.deltaTime, MaxTurnSpeed * Time.deltaTime);
			inputs.Vertical = 0.6f;
		}

		private void PlayBotShotSound()
		{
			try
			{
				WeaponController currentWeapon = _pc.WeaponryController.CurrentWeapon;
				if (currentWeapon != null && currentWeapon.AnimationParameters != null && currentWeapon.AnimationParameters.shotSound != null)
				{
					AudioSource.PlayClipAtPoint(currentWeapon.AnimationParameters.shotSound, base.transform.position, currentWeapon.AnimationParameters.shotSoundVolume);
				}
			}
			catch { }
		}

		private void Shoot(PlayerController target)
		{
			Vector3 origin = base.transform.position + Vector3.up * 1.5f;
			Vector3 end = target.transform.position + Vector3.up * 1.2f;
			Vector3 toTarget = end - origin;
			float distance = toTarget.magnitude;
			Vector3 direction = toTarget / distance;
			float spread = Mathf.Min(BaseSpread * _spreadMultiplier + distance * SpreadPerMeter, MaxSpread);
			Vector3 shotDirection = (direction + UnityEngine.Random.insideUnitSphere * spread).normalized;
			PlayBotShotSound();
			RaycastHit hit;
			PlayerController hitPlayer;
			if (!TryGetShotHit(origin, shotDirection, distance, out hit, out hitPlayer) || hitPlayer != target)
			{
				return;
			}
			HitData hitData = new HitData();
			hitData.Direction = shotDirection;
			hitData.WeaponId = _weaponId;
			hitData.SkinId = InventoryItemId.None;
			BulletHitData bulletHitData = new BulletHitData();
			bulletHitData.Point = hit.point;
			bulletHitData.Impulse = (_gunParameters != null) ? _gunParameters.Impulse : 0f;
			bulletHitData.Damage = (int)(((_gunParameters != null) ? _gunParameters.Damage.ChestAndArmsDamage : 25) * _damageMultiplier);
			bulletHitData.ArmorPenetration = (_gunParameters != null) ? _gunParameters.ArmorPenetration : 0f;
			bulletHitData.Bone = BipedMap.Bip.Spine1;
			bulletHitData.Penetrated = false;
			hitData.Hits = new BulletHitData[]
			{
				bulletHitData
			};
			PhotonPlayer victim = target.Player;
			HitData hitDataCopy = hitData;
			Singleton<HitManager>.Instance.Hit(_bot, victim, PhotonNetwork.time, hitDataCopy, delegate(bool isDeath)
			{
				if (isDeath)
				{
					PlayerController controller = GetPlayerController(victim);
					if (controller != null)
					{
						controller.PlayerOcclusionController.SetVisible(isVisible: true);
						Singleton<RagdollManager>.Instance.Simulate(controller.CharacterRagdollController, hitDataCopy);
						if (victim.IsLocal)
						{
							controller.KillPlayer();
						}
					}
				}
			});
		}

		private bool TryGetShotHit(Vector3 origin, Vector3 direction, float maxDistance, out RaycastHit hit, out PlayerController hitPlayer)
		{
			int count = Physics.RaycastNonAlloc(origin, direction, ShotHits, maxDistance);
			float nearest = float.MaxValue;
			hit = default(RaycastHit);
			hitPlayer = null;
			for (int i = 0; i < count; i++)
			{
				if (ShotHits[i].distance > nearest)
				{
					continue;
				}
				PlayerController playerController = ShotHits[i].collider.GetComponentInParent<PlayerController>();
				if (playerController == _pc)
				{
					continue;
				}
				nearest = ShotHits[i].distance;
				hit = ShotHits[i];
				hitPlayer = playerController;
			}
			return hitPlayer != null;
		}
	}
}
