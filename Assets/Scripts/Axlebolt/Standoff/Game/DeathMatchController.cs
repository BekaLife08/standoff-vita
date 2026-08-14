using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using I2.Loc;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class DeathMatchController : GameController
	{
		[SerializeField]
		private float _startingDuration;

		[SerializeField]
		private float _deathMatchDuration;

		[SerializeField]
		private float _winnersDuration;

		[SerializeField]
		private float _statisticsDuration;

		[SerializeField]
		private float _respawnTime;

		[SerializeField]
		private float _nextLevelVotingDuration;

		[SerializeField]
		private float _nextLevelVotingResultDuration;

		[SerializeField]
		private float _weaponBuyDuration;

		[SerializeField]
		private WeaponId _defaultPistolId = WeaponId.G22;

		private Coroutine _buyWeaponsCoroutine;

		private WeaponId? _lastWeaponId;

		protected override void InitUi()
		{
			base.InitUi();
			PersonalGameStatsView.Show();
			PersonalGameStatsView.ScoreVisible = false;
			PersonalGameStatsView.TimeVisible = false;
			WeaponBuyView.TimeLeftTitle = ScriptLocalization.TeamDeathMatch.WeaponBuyTitle;
		}

		protected override GameStateRouter CreateStateRouter()
		{
			return GameStateRouter.Create(10, () => 21).Route(21, () => 30).Route(30, () => 200)
				.Route(200, () => 201)
				.Route(201, () => byte.MaxValue);
		}

		protected override void RegisterStates(GameStateMachine stateMachine)
		{
			stateMachine.Register(new WaitingPlayersState());
			stateMachine.Register(new StartingState(_startingDuration));
			stateMachine.Register(new RunningState(_deathMatchDuration));
			stateMachine.Register(new WinnersState(_winnersDuration));
			stateMachine.Register(new FinalStatisticsState(_statisticsDuration));
		}

		protected override SpawnPointManager CreateSpawnPointManager()
		{
			return new SpawnPointManager(SpawnZoneType.Random);
		}

		protected override void InitManagers()
		{
			base.InitManagers();
			Singleton<WeaponManager>.Instance.SetInfinityAmmo(infinityAmmo: true);
		}

		protected override void InstantiatePlayer(Team team, SpawnPoint spawnPoint)
		{
			Singleton<PlayerManager>.Instance.Instantiate(team, spawnPoint.Position, spawnPoint.Rotation, 100, 100, hasHelmet: true);
		}

		protected override void OnPlayerSpawned(PlayerController playerController)
		{
			base.OnPlayerSpawned(playerController);
			SetPlayerWeapon(_defaultPistolId);
			SetPlayerWeapon(GetSpawnWeaponId());
			_buyWeaponsCoroutine = StartCoroutine(BuyWeapons());
		}

		protected override void OnDestroyed(PlayerController playerControlller)
		{
			base.OnDestroyed(playerControlller);
			StopCoroutine(_buyWeaponsCoroutine);
		}

		protected override void OnPlayerKilled(HitEventArgs hitEventArgs)
		{
			base.OnPlayerKilled(hitEventArgs);
			base.Player.AddScore(12);
		}

		protected override void OnPlayerAssist(HitEventArgs hitEventArgs)
		{
			base.OnPlayerAssist(hitEventArgs);
			base.Player.AddScore(4);
		}

		protected override IEnumerator OnPlayerSuicideCoroutine()
		{
			yield return new WaitForSeconds(_respawnTime);
			SpawnPlayer();
		}

		protected override IEnumerator OnPlayerWasKilledCoroutine(HitEventArgs hitEventArgs, HitLogs takenHits)
		{
			HitLogs givenHits = GetGamePlayer(hitEventArgs.Shooter).GetHits(base.Player);
			yield return PlayDieWithFocusEffect(hitEventArgs.Shooter);
			string weaponName = WeaponUtility.GetWeaponName(hitEventArgs.Weapon, hitEventArgs.Skin);
			Sprite weaponPreview = WeaponUtility.GetWeaponPreview(hitEventArgs.Weapon, hitEventArgs.Skin);
			WhoKillView.Show(weaponName, weaponPreview, hitEventArgs.Shooter, takenHits, givenHits);
			yield return new WaitForSeconds(_respawnTime);
			SpawnPlayer();
		}

		private IEnumerator BuyWeapons()
		{
			WeaponBuyButton.Show();
			while (CanBuyWeapons())
			{
				float timeLeft = _weaponBuyDuration - (Time.time - base.PlayerSpawnedTime);
				WeaponBuyView.TimeLeft = timeLeft;
				yield return new WaitForSeconds(0.1f);
			}
			WeaponBuyButton.Hide();
			HideWeaponBuyView();
		}

		private WeaponId GetSpawnWeaponId()
		{
			WeaponId? lastWeaponId = _lastWeaponId;
			if (!lastWeaponId.HasValue)
			{
				_lastWeaponId = Singleton<WeaponManager>.Instance.GetRandomGunId(GunType.Heavy, GunType.Rifels, GunType.Smg);
			}
			WeaponId? lastWeaponId2 = _lastWeaponId;
			return lastWeaponId2.Value;
		}

		public override void BuyWeapon(WeaponParameters weapon)
		{
			base.BuyWeapon(weapon);
			_lastWeaponId = weapon.Id;
		}

		public override bool IsWeaponBuySupport()
		{
			return true;
		}

		public override bool CanBuyWeapons()
		{
			return Time.time - base.PlayerSpawnedTime < _weaponBuyDuration;
		}

		public override bool CanBuyWeapon(WeaponParameters weapon)
		{
			return true;
		}

		public override bool IsWeaponDropSupport()
		{
			return false;
		}
	}
}
