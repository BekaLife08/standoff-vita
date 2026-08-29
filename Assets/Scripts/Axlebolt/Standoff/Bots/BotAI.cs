using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Bots
{
	public class BotAI : MonoBehaviour
	{
		private const float SightRange = 40f;
		private const float FireRange = 28f;
		private const float StopRange = 30f;
		private const float MaxTurnSpeed = 200f;
		private const float ThinkInterval = 0.2f;
		private const float ReactionTime = 1.0f;
		private const float SniperReactionTime = 2.0f;
		private const float BaseSpread = 0.18f;
		private const float SpreadPerMeter = 0.014f;
		private const float MaxSpread = 0.6f;
		private const float MinFireInterval = 0.45f;
		private const float DamageMultiplier = 1f;
		private const float SniperDamageMultiplier = 0.6f;
		private const float SniperSpreadMultiplier = 2.2f;
		private const float RepositionMinInterval = 2f;
		private const float RepositionMaxInterval = 5f;
		private const float CloseRange = 15f;
		private const float BombDangerRadius = 15f;
		private const float BombRunawayTime = 10f;
		private const float DefuseDistance = 3f;
		private const float SitePatrolRadius = 8f;
		private const float FollowPlayerMinDist = 10f;
		private const float FollowPlayerMaxDist = 30f;
		private const float ObstacleAvoidDistance = 3f;

		private static readonly RaycastHit[] ShotHits = new RaycastHit[16];
		private static readonly Dictionary<int, int> _botTargetMap = new Dictionary<int, int>();

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
		private PlayerController _chaseTarget;
		private float _targetAcquiredTime;
		private float _nextStatusLogTime;
		private bool _dead;
		private bool _isDefuseMode;
		private float _nextBuyTime;
		private bool _hasBought;
		private Vector3 _bombSiteTarget;
		private bool _hasBombSiteTarget;
		private bool _isFrozen;
		private float _freezeEndTime;

		public void Init(PhotonPlayer bot, PlayerController pc)
		{
			_bot = bot;
			_pc = pc;
			_isDefuseMode = IsDefuseGameMode();
			_dead = false;
			_target = null;
			_targetAcquiredTime = 0f;
			_hasBombSiteTarget = false;

			if (_isDefuseMode)
			{
				_isFrozen = true;
				_freezeEndTime = Time.time + 5f;
				_nextBuyTime = Time.time + 5f;
				_hasBought = false;
				BotBuyWeapon();
			}
			else
			{
				_isFrozen = false;
				_weaponId = Singleton<WeaponManager>.Instance.GetRandomGunId(GunType.Heavy, GunType.Rifels, GunType.Smg);
			}
			SetupWeapon();
			_nextFireTime = Time.time + 0.5f;
		}

		private bool IsDefuseGameMode()
		{
			try
			{
				string gameModeName = PhotonNetwork.room.GetGameModeName();
				return gameModeName == GameModeIds.Defuse;
			}
			catch
			{
				return false;
			}
		}

		private void BotBuyWeapon()
		{
			if (_hasBought) return;
			int money = _bot.GetMoney();
			WeaponId[] affordableWeapons = new WeaponId[]
			{
				WeaponId.AWM,
				WeaponId.M16,
				WeaponId.M4,
				WeaponId.SM1014,
				WeaponId.UMP45,
				WeaponId.Deagle,
				WeaponId.P350,
				WeaponId.G22
			};
			WeaponId bestWeapon = WeaponId.G22;
			for (int i = 0; i < affordableWeapons.Length; i++)
			{
				WeaponParameters parameters = Singleton<WeaponManager>.Instance.GetParameters(affordableWeapons[i]);
				if (parameters != null && money >= parameters.Cost)
				{
					bestWeapon = affordableWeapons[i];
					break;
				}
			}
			_weaponId = bestWeapon;
			_hasBought = true;
		}

		private void SetupWeapon()
		{
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
			WeaponController weaponController = Singleton<WeaponManager>.Instance.Get(_weaponId);
			if (weaponController != null)
			{
				_pc.WeaponryController.SetWeapon(weaponController);
			}
		}

		private float _strafeDir = 1f;
		private float _nextStrafeChangeTime;
		private float _nextRepositionTime;

		private void Update()
		{
			if (_bot == null || _pc == null || !PhotonNetwork.offlineMode)
			{
				return;
			}

			if (_bot.IsDead())
			{
				if (!_dead)
				{
					_dead = true;
					ReleaseTarget();
				}
				return;
			}

			if (_dead)
			{
				_dead = false;
			}

			if (_isDefuseMode)
			{
				if (_isFrozen && Time.time < _freezeEndTime)
				{
					_pc.SetInputs(default(PlayerInputs), Time.deltaTime);
					return;
				}
				if (_isFrozen && Time.time >= _freezeEndTime)
				{
					_isFrozen = false;
				}
			}

			PlayerInputs playerInputs = new PlayerInputs();

			if (_target == null || _target.Player == null || _target.Player.IsDead() || _target.Player.GetTeam() == _bot.GetTeam() || GetPlayerController(_target.Player) == null)
			{
				ReleaseTarget();
				PlayerController previous = _target;
				_target = FindTarget();
				if (_target != null && _target != previous)
				{
					ClaimTarget(_bot.ID, _target.Player.ID);
					_targetAcquiredTime = Time.time;
					_nextFireTime = Mathf.Max(_nextFireTime, Time.time + _reactionTime);
					_nextRepositionTime = Time.time + Random.Range(RepositionMinInterval, RepositionMaxInterval);
				}
			}

			if (_target != null)
			{
				CombatBehavior(ref playerInputs);
			}
			else if (_isDefuseMode)
			{
				DefuseModeBehavior(ref playerInputs);
			}
			else
			{
				if (_chaseTarget != null && !_chaseTarget.Player.IsDead())
				{
					ChaseEnemy(ref playerInputs);
				}
				else
				{
					Patrol(ref playerInputs);
				}
			}

			_pc.SetInputs(playerInputs, Time.deltaTime);
		}

		private void CombatBehavior(ref PlayerInputs inputs)
		{
			Vector3 toTarget = _target.transform.position - base.transform.position;
			toTarget.y = 0f;
			float magnitude = toTarget.magnitude;
			Vector3 moveDir = toTarget.normalized;

			moveDir = AvoidObstacles(moveDir);

			TurnToward(moveDir, ref inputs);

			if (magnitude > StopRange)
			{
				inputs.Vertical = 1f;
			}
			else if (magnitude > FireRange)
			{
				inputs.Vertical = 0.5f;
			}
			else
			{
				if (Time.time >= _nextRepositionTime)
				{
					inputs.Vertical = 0.5f;
					if (Time.time >= _nextStrafeChangeTime)
					{
						_strafeDir = Random.Range(0, 2) == 0 ? -1f : 1f;
						_nextStrafeChangeTime = Time.time + Random.Range(0.8f, 2f);
					}
					inputs.Horizontal = _strafeDir;
					if (Time.time - _nextRepositionTime > 1.5f)
					{
						_nextRepositionTime = Time.time + Random.Range(RepositionMinInterval, RepositionMaxInterval);
					}
				}
			}

			if (magnitude <= FireRange && Time.time >= _nextFireTime && Time.time - _targetAcquiredTime >= _reactionTime && HasLineOfSight(_target))
			{
				_nextFireTime = Time.time + _fireInterval;
				Shoot(_target);
			}
		}

		private void DefuseModeBehavior(ref PlayerInputs inputs)
		{
			if (_bot.GetTeam() == Team.Tr)
			{
				TerroristBehavior(ref inputs);
			}
			else
			{
				CounterTerroristBehavior(ref inputs);
			}
		}

		private void TerroristBehavior(ref PlayerInputs inputs)
		{
			if (ScenePhotonBehavior<BombManager>.Instance.IsBombPlanted())
			{
				Vector3 bombPos = ScenePhotonBehavior<BombManager>.Instance.GetBombPosition();
				float distToBomb = Vector3.Distance(base.transform.position, bombPos);

				if (distToBomb > SitePatrolRadius)
				{
					Vector3 toBomb = (bombPos - base.transform.position).normalized;
					toBomb = AvoidObstacles(toBomb);
					TurnToward(toBomb, ref inputs);
					inputs.Vertical = 0.8f;
				}
				else
				{
					Patrol(ref inputs);
				}
				return;
			}

			PhotonPlayer localPlayer = PhotonNetwork.player;
			if (localPlayer != null && !localPlayer.IsDead() && localPlayer.GetTeam() == Team.Tr)
			{
				PlayerController playerPc = GetPlayerController(localPlayer);
				if (playerPc != null)
				{
					float distToPlayer = Vector3.Distance(base.transform.position, playerPc.transform.position);
					if (distToPlayer > FollowPlayerMaxDist)
					{
						Vector3 toPlayer = (playerPc.transform.position - base.transform.position).normalized;
						toPlayer = AvoidObstacles(toPlayer);
						TurnToward(toPlayer, ref inputs);
						inputs.Vertical = 0.8f;
					}
					else if (distToPlayer < FollowPlayerMinDist)
					{
						Vector3 awayDir = (base.transform.position - playerPc.transform.position).normalized;
						awayDir.y = 0f;
						awayDir = AvoidObstacles(awayDir);
						TurnToward(awayDir, ref inputs);
						inputs.Vertical = 0.5f;
					}
					else
					{
						Vector3 toPlayer = (playerPc.transform.position - base.transform.position).normalized;
						TurnToward(toPlayer, ref inputs);
					}
				}
			}
			else
			{
				Patrol(ref inputs);
			}
		}

		private void CounterTerroristBehavior(ref PlayerInputs inputs)
		{
			if (ScenePhotonBehavior<BombManager>.Instance.IsBombPlanted())
			{
				Vector3 bombPos = ScenePhotonBehavior<BombManager>.Instance.GetBombPosition();
				float distToBomb = Vector3.Distance(base.transform.position, bombPos);

				if (distToBomb <= DefuseDistance)
				{
					bool enemiesNearby = AnyEnemyNearby(BombDangerRadius);
					if (!enemiesNearby)
					{
						TurnToward(bombPos - base.transform.position, ref inputs);
						inputs.Vertical = 0f;
						return;
					}
				}

				Vector3 toBomb = (bombPos - base.transform.position).normalized;
				toBomb = AvoidObstacles(toBomb);
				TurnToward(toBomb, ref inputs);
				inputs.Vertical = 0.9f;
				return;
			}

			if (!_hasBombSiteTarget || Vector3.Distance(base.transform.position, _bombSiteTarget) < 3f)
			{
				PickBombSiteTarget();
			}

			Vector3 toSite = (_bombSiteTarget - base.transform.position).normalized;
			toSite = AvoidObstacles(toSite);
			TurnToward(toSite, ref inputs);
			inputs.Vertical = 0.7f;
		}

		private void PickBombSiteTarget()
		{
			BombSite[] sites = Object.FindObjectsOfType<BombSite>();
			if (sites.Length > 0)
			{
				BombSite site = sites[Random.Range(0, sites.Length)];
				_bombSiteTarget = site.transform.position;
			}
			else
			{
				Vector2 offset = Random.insideUnitCircle * SitePatrolRadius;
				_bombSiteTarget = base.transform.position + new Vector3(offset.x, 0f, offset.y);
			}
			_hasBombSiteTarget = true;
		}

		private bool AnyEnemyNearby(float radius)
		{
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			for (int i = 0; i < playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = playerList[i];
				if (photonPlayer.ID == _bot.ID || photonPlayer.IsDead() || photonPlayer.GetTeam() == _bot.GetTeam())
				{
					continue;
				}
				PlayerController controller = GetPlayerController(photonPlayer);
				if (controller != null && Vector3.Distance(base.transform.position, controller.transform.position) < radius)
				{
					return true;
				}
			}
			return false;
		}

		private Vector3 AvoidObstacles(Vector3 desiredDir)
		{
			RaycastHit hit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.5f, desiredDir, out hit, ObstacleAvoidDistance))
			{
				if (hit.collider.GetComponentInParent<PlayerController>() == null)
				{
					Vector3 slide = Vector3.Cross(hit.normal, Vector3.up).normalized;
					if (slide.sqrMagnitude < 0.01f)
					{
						slide = base.transform.right;
					}
					RaycastHit secondHit;
					Vector3 slideDir = (slide + desiredDir * 0.5f).normalized;
					if (Physics.Raycast(base.transform.position + Vector3.up * 0.5f, slideDir, out secondHit, ObstacleAvoidDistance))
					{
						if (secondHit.collider.GetComponentInParent<PlayerController>() == null)
						{
							Vector3 oppositeSlide = -slide;
							return (oppositeSlide + desiredDir * 0.5f).normalized;
						}
					}
					return slideDir;
				}
			}
			return desiredDir;
		}

		private void TurnToward(Vector3 direction, ref PlayerInputs inputs)
		{
			if (direction.sqrMagnitude < 0.001f) return;
			float targetYaw = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
			float currentYaw = Mathf.Repeat(base.transform.eulerAngles.y, 360f);
			float deltaYaw = Mathf.DeltaAngle(currentYaw, targetYaw);
			inputs.DeltaAimAngles.y = Mathf.Clamp(deltaYaw, -MaxTurnSpeed * Time.deltaTime, MaxTurnSpeed * Time.deltaTime);
		}

		private PlayerController FindTarget()
		{
			if (Time.time < _nextThinkTime)
			{
				return _target;
			}
			_nextThinkTime = Time.time + ThinkInterval;

			PlayerController closestInRange = null;
			float bestDistanceInRange = CloseRange;
			PlayerController closestAny = null;
			float bestDistanceAny = SightRange;

			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			for (int i = 0; i < playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = playerList[i];
				if (photonPlayer.ID == _bot.ID || photonPlayer.IsDead() || photonPlayer.GetTeam() == _bot.GetTeam())
				{
					continue;
				}
				if (IsTargetClaimed(photonPlayer.ID))
				{
					continue;
				}
				PlayerController controller = GetPlayerController(photonPlayer);
				if (controller == null)
				{
					continue;
				}
				float distance = Vector3.Distance(base.transform.position, controller.transform.position);
				if (distance > SightRange)
				{
					continue;
				}
				if (HasLineOfSight(controller))
				{
					if (distance < bestDistanceInRange)
					{
						bestDistanceInRange = distance;
						closestInRange = controller;
					}
				}
				if (distance < bestDistanceAny)
				{
					bestDistanceAny = distance;
					closestAny = controller;
				}
			}

			_chaseTarget = closestAny;
			return closestInRange;
		}

		private PlayerController FindNearestEnemy()
		{
			PlayerController nearest = null;
			float bestDist = float.MaxValue;
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
				if (distance < bestDist)
				{
					bestDist = distance;
					nearest = controller;
				}
			}
			return nearest;
		}

		private static void ClaimTarget(int botId, int targetId)
		{
			_botTargetMap[botId] = targetId;
		}

		private void ReleaseTarget()
		{
			if (_botTargetMap.ContainsKey(_bot.ID))
			{
				_botTargetMap.Remove(_bot.ID);
			}
		}

		private bool IsTargetClaimed(int targetId)
		{
			foreach (var kvp in _botTargetMap)
			{
				if (kvp.Value == targetId && kvp.Key != _bot.ID)
				{
					return true;
				}
			}
			return false;
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
				_nextPatrolTime = Time.time + Random.Range(2f, 5f);
				Vector2 circle = Random.insideUnitCircle * 12f;
				_patrolTarget = base.transform.position + new Vector3(circle.x, 0f, circle.y);
			}

			Vector3 toTarget = _patrolTarget - base.transform.position;
			toTarget.y = 0f;
			if (toTarget.sqrMagnitude < 1f)
			{
				return;
			}

			Vector3 moveDir = AvoidObstacles(toTarget.normalized);
			TurnToward(moveDir, ref inputs);
			inputs.Vertical = 0.6f;
		}

		private void ChaseEnemy(ref PlayerInputs inputs)
		{
			if (_chaseTarget == null || _chaseTarget.Player.IsDead())
			{
				_chaseTarget = FindNearestEnemy();
				if (_chaseTarget == null)
				{
					Patrol(ref inputs);
					return;
				}
			}

			Vector3 toEnemy = (_chaseTarget.transform.position - base.transform.position);
			toEnemy.y = 0f;
			Vector3 moveDir = AvoidObstacles(toEnemy.normalized);
			TurnToward(moveDir, ref inputs);
			inputs.Vertical = 0.8f;
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
			Vector3 shotDirection = (direction + Random.insideUnitSphere * spread).normalized;
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
						else if (!victim.IsLocal && PhotonNetwork.offlineMode && !_isDefuseMode)
						{
							BotManager.Respawn(victim);
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
