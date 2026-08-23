using Axlebolt.Standoff.Cam;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Game.UI.Hud;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class DefuseController : GameController, IBombListener
	{
		private static readonly Log Log = Log.Create("DefuseController");

		[SerializeField]
		private int _warmupTime;

		[SerializeField]
		private int _startingTime;

		[SerializeField]
		private int _roundStartingTime;

		[SerializeField]
		private int _roundRunningTime;

		[SerializeField]
		private int _roundEndingTime;

		[SerializeField]
		private int _statisticsTime;

		[SerializeField]
		private float _weaponBuyTime;

		[SerializeField]
		private int _roundsCount;

		[SerializeField]
		private int _startMoney;

		[SerializeField]
		[Header("Economics")]
		private int _maxMoney;

		[SerializeField]
		private int _winTeamReward;

		[SerializeField]
		private int _lossReward;

		[SerializeField]
		private int _consecutiveLossReward;

		[SerializeField]
		private int _maxLossReward;

		[SerializeField]
		private int _bombPlantReward;

		[SerializeField]
		private int _bombDefusedReward;

		[SerializeField]
		private float _bombAlertInteval;

		[SerializeField]
		[Header("Weapon rewards")]
		private DefuseGunTypeReward[] _gunTypeRewards;

		[SerializeField]
		private WeaponIdReward[] _weaponIdRewards;

		[SerializeField]
		private WeaponId _defaultPistolId;

		[SerializeField]
		private DefuseAudioClips _defuseAudioClips;

		private readonly Dictionary<GunType, int> _gunTypeRewardsMap = new Dictionary<GunType, int>();

		private readonly Dictionary<WeaponId, int> _weaponIdRewardsMap = new Dictionary<WeaponId, int>();

		private WeaponBuyZone[] _ctWeaponBuyZones;

		private WeaponBuyZone[] _trWeaponBuyZones;

		private List<WeaponId> _savedWeaponIds = new List<WeaponId>();

		private BombExplosionEffect _bombExplosionEffect;

		private float _bombAlertTime;

		public BombParameters BombParameters
		{
			get;
			private set;
		}

		public DefuseKitParameters DefuseKitParameters
		{
			get;
			private set;
		}

		protected override GameStateRouter CreateStateRouter()
		{
			return GameStateRouter.Create(10, () => 11).Route(11, () => 21).Route(21, () => 22)
				.Route(22, () => 31)
				.Route(31, RoundRunningRoute)
				.Route(40, () => 101)
				.Route(101, RoundEndingRoute)
				.Route(201, () => byte.MaxValue);
		}

		private byte RoundRunningRoute()
		{
			return (byte)((!IsBombPlantedOnTime()) ? 101 : 40);
		}

		private byte RoundEndingRoute()
		{
			int num = _roundsCount / 2;
			if (PhotonNetwork.room.GetScore(Team.Ct) > num || PhotonNetwork.room.GetScore(Team.Tr) > num)
			{
				return 201;
			}
			return 22;
		}

		protected override void RegisterStates(GameStateMachine stateMachine)
		{
			stateMachine.Register(new WaitingPlayersState());
			stateMachine.Register(new WarmUpState(_warmupTime));
			stateMachine.Register(new StartingState(_startingTime));
			stateMachine.Register(new DefuseRoundStartingState(_roundStartingTime));
			stateMachine.Register(new DefuseRoundRunningState(_roundRunningTime));
			stateMachine.Register(new DefuseBombPlantedState(_defuseAudioClips.BombHasBeenDefused.length));
			stateMachine.Register(new DefuseRoundEndingGameState(_roundEndingTime));
			stateMachine.Register(new DefuseFinalStatisticsState(_statisticsTime));
		}

		protected override SpawnPointManager CreateSpawnPointManager()
		{
			return new SpawnPointManager(SpawnZoneType.Fixed);
		}

		protected override void InitWeapons()
		{
			base.InitWeapons();
			BombParameters = (BombParameters)WeaponUtility.LoadWeapon(WeaponId.Bomb);
			DefuseKitParameters = (DefuseKitParameters)WeaponUtility.LoadWeapon(WeaponId.DefuseKit);
			DefuseGunTypeReward[] gunTypeRewards = _gunTypeRewards;
			foreach (DefuseGunTypeReward defuseGunTypeReward in gunTypeRewards)
			{
				_gunTypeRewardsMap[defuseGunTypeReward.GunType] = defuseGunTypeReward.RewardMoney;
			}
			WeaponIdReward[] weaponIdRewards = _weaponIdRewards;
			foreach (WeaponIdReward weaponIdReward in weaponIdRewards)
			{
				_weaponIdRewardsMap[weaponIdReward.WeaponId] = weaponIdReward.RewardMoney;
			}
		}

		protected override WeaponId[] GetWeaponIds()
		{
			List<WeaponId> list = new List<WeaponId>();
			list.AddRange(WeaponUtility.GetBaseWeaponIds());
			list.Add(WeaponId.Bomb);
			list.Add(WeaponId.DefuseKit);
			return list.ToArray();
		}

		protected override void InitZones()
		{
			base.InitZones();
			WeaponBuyZone[] source = UnityEngine.Object.FindObjectsOfType<WeaponBuyZone>();
			_ctWeaponBuyZones = (from zone in source
				where zone.Team == Team.Ct
				select zone).ToArray();
			_trWeaponBuyZones = (from zone in source
				where zone.Team == Team.Tr
				select zone).ToArray();
			if (_ctWeaponBuyZones.Length == 0)
			{
				throw new Exception("CT weapon buy zone not found");
			}
			if (_trWeaponBuyZones.Length == 0)
			{
				throw new Exception("TR weapon buy zone not found");
			}
		}

		private int GetGunKillReward(WeaponParameters weaponParameters)
		{
			if (_weaponIdRewardsMap.ContainsKey(weaponParameters.Id))
			{
				return _weaponIdRewardsMap[weaponParameters.Id];
			}
			GunParameters gunParameters = weaponParameters as GunParameters;
			if (gunParameters != null && _gunTypeRewardsMap.ContainsKey(gunParameters.GunType))
			{
				return _gunTypeRewardsMap[gunParameters.GunType];
			}
			Log.Error($"{weaponParameters.Id} kill reward not set!");
			return 0;
		}

		protected override void InitUi()
		{
			base.InitUi();
			TeamGameStatsView.Show();
			StatisticsView.ScoreEnabled = true;
			StatisticsView.MoneyEnabled = true;
			StatisticsView.StateProvider = GetPlayerState;
			WeaponBuyView.MoneyEnabled = true;
		}

		protected override void InitCamera()
		{
			base.InitCamera();
			_bombExplosionEffect = MainCamera.gameObject.AddComponent<BombExplosionEffect>();
		}

		protected override HudComponentView[] GetHudViews()
		{
			MoneyView view = base.GameView.GetView<MoneyView>();
			BombOwnerIndicator view2 = base.GameView.GetView<BombOwnerIndicator>();
			return new HudComponentView[2]
			{
				view,
				view2
			};
		}

		private Sprite GetPlayerState(PhotonPlayer player)
		{
			if (player.GetTeam() == Team.Tr)
			{
				return (!ScenePhotonBehavior<BombManager>.Instance.IsBomber(player)) ? null : BombParameters.Sprites.Icon;
			}
			return DefuseKitParameters.Sprites.Icon;
		}

		protected override void InitPrefabPool(GamePrefabPool pool)
		{
			base.InitPrefabPool(pool);
			pool.Register<BombManager>();
		}

		protected override IEnumerator InitSceneManagers()
		{
			yield return base.InitSceneManagers();
			yield return ScenePhotonBehavior<BombManager>.Init(null);
			ScenePhotonBehavior<BombManager>.Instance.SetBombListener(this);
			ScenePhotonBehavior<WeaponDropManager>.Instance.TakeRule = CanTakeWeapon;
		}

		private bool CanTakeWeapon(WeaponId weaponId)
		{
			return weaponId != WeaponId.Bomb || base.Player.GetTeam() != Team.Ct;
		}

		protected override void OnPlayerSuicide()
		{
			base.OnPlayerSuicide();
			SpawnPlayer();
		}

		protected override IEnumerator OnPlayerSuicideCoroutine()
		{
			yield break;
		}

		protected override void OnPlayerKilled(HitEventArgs hitEventArgs)
		{
			if (hitEventArgs.Weapon.Id != WeaponId.Bomb)
			{
				base.OnPlayerKilled(hitEventArgs);
				PhotonNetwork.player.AddRoundKills();
				int gunKillReward = GetGunKillReward(hitEventArgs.Weapon);
				AddMoney(gunKillReward);
			}
		}

		protected override void OnPlayerAssist(HitEventArgs hitEventArgs)
		{
			base.OnPlayerAssist(hitEventArgs);
			PhotonNetwork.player.AddRoundAssist();
		}

		public override void OnGameInit()
		{
			base.OnGameInit();
			TryToSpectatorMode();
		}

		protected override IEnumerator OnPlayerWasKilledCoroutine(HitEventArgs hitEventArgs, HitLogs takenHits)
		{
			if (hitEventArgs.Weapon.Id == WeaponId.Bomb)
			{
				yield return PlayDieEffect(ScenePhotonBehavior<BombManager>.Instance.GetBombTransform());
				yield break;
			}
			yield return PlayDieWithFocusEffect(hitEventArgs.Shooter);
			if (base.CurrentGameState is WaitingPlayersState || base.CurrentGameState is WarmUpState)
			{
				SpawnPlayer();
				yield break;
			}
			ScreenshotView.Hide();
			TryToSpectatorMode();
		}

		protected void TryToSpectatorMode()
		{
			if (base.CurrentGameState is DefuseRoundRunningState || base.CurrentGameState is DefuseBombPlantedState)
			{
				UnityEngine.Debug.Log("SpectatorController");
				SpectatorController.ToSpectatorMode();
			}
		}

		public override void SpawnPlayer()
		{
			SpectatorController.CancelSpectateMode();
			base.SpawnPlayer();
		}

		protected override void OnPlayerSpawned(PlayerController playerController)
		{
			base.OnPlayerSpawned(playerController);
			SetSavedWeapons();
			InitBomb();
			InitDefuseKit();
			StartWeaponBuyCoroutine();
			if (base.CurrentGameState is DefuseRoundStartingState)
			{
				PlayerSpawnEffect.PlayEffect();
			}
		}

		protected override void BindPlayerToManagers(PlayerController playerController)
		{
			base.BindPlayerToManagers(playerController);
			ScenePhotonBehavior<BombManager>.Instance.SetPlayer(playerController?.gameObject);
		}

		private void SetSavedWeapons()
		{
			if (_savedWeaponIds.Count == 0 || _savedWeaponIds.All((WeaponId weaponId) => weaponId.GetGunType() != GunType.Pistol))
			{
				_savedWeaponIds.Add(_defaultPistolId);
			}
			foreach (WeaponId savedWeaponId in _savedWeaponIds)
			{
				SetPlayerWeapon(savedWeaponId);
			}
			base.PlayerController.WeaponryController.SwitchWeapon(2);
			_savedWeaponIds.Clear();
		}

		public void RoundStarting()
		{
			if (PhotonNetwork.room.GetRound() == 1)
			{
				PhotonNetwork.player.SetMoney(_startMoney);
			}
			if (base.PlayerController == null || PhotonNetwork.room.GetRound() == 1)
			{
				SpawnPlayer();
				return;
			}
			_savedWeaponIds = (from controller in base.PlayerController.WeaponryController.GetWeapons()
				select controller.WeaponId into weaponId
				where weaponId != WeaponId.Bomb
				select weaponId).ToList();
			SpawnPlayer();
		}

		protected override void InstantiatePlayer(Team team, SpawnPoint spawnPoint)
		{
			Singleton<PlayerManager>.Instance.Instantiate(team, spawnPoint.Position, spawnPoint.Rotation, 100, 100, hasHelmet: true);
		}

		private void InitBomb()
		{
			if (GetTeam() == Team.Tr && !ScenePhotonBehavior<BombManager>.Instance.IsBomberInitialized() && IsStartingState() && PhotonNetwork.room.GetInitBomberId() == base.Player.ID)
			{
				ScenePhotonBehavior<BombManager>.Instance.SetInitBomberIsMe();
			}
		}

		private void InitDefuseKit()
		{
			if (GetTeam() == Team.Ct)
			{
				SetPlayerWeapon(WeaponId.DefuseKit);
			}
		}

		private bool IsStartingState()
		{
			return base.CurrentGameState is DefuseRoundStartingState;
		}

		public override void OnGameStateChanged(IGameState state)
		{
			base.OnGameStateChanged(state);
			if (state is WaitingPlayersState)
			{
				PhotonNetwork.player.SetMoney(_maxMoney);
			}
			else if (state is WarmUpState)
			{
				PhotonNetwork.player.SetMoney(_maxMoney);
			}
		}

		private void StartWeaponBuyCoroutine()
		{
			if (base.CurrentGameState is WaitingPlayersState || base.CurrentGameState is WarmUpState)
			{
				StartWeaponBuyCoroutine(PhotonNetwork.time, _weaponBuyTime);
			}
			else
			{
				StartWeaponBuyCoroutine(PhotonNetwork.room.GetRoundStartTime(), _weaponBuyTime);
			}
		}

		public override bool IsWeaponBuySupport()
		{
			return true;
		}

		public override bool CanBuyWeapons()
		{
			return IsInWeaponBuyZone();
		}

		private bool IsInWeaponBuyZone()
		{
			if (PhotonNetwork.player.GetTeam() == Team.Ct)
			{
				return _ctWeaponBuyZones.Any((WeaponBuyZone zone) => zone.IsInZone(base.PlayerController.Transform.position));
			}
			if (PhotonNetwork.player.GetTeam() == Team.Tr)
			{
				return _trWeaponBuyZones.Any((WeaponBuyZone zone) => zone.IsInZone(base.PlayerController.Transform.position));
			}
			throw new Exception("Invalid team state!");
		}

		public override bool CanBuyWeapon(WeaponParameters weapon)
		{
			return PhotonNetwork.player.GetMoney() >= weapon.Cost;
		}

		public override void BuyWeapon(WeaponParameters weapon)
		{
			PhotonNetwork.player.AddMoney(-weapon.Cost);
			base.BuyWeapon(weapon);
		}

		public override bool IsWeaponDropSupport()
		{
			return true;
		}

		private bool CanShowBombAlert()
		{
			return Time.time - _bombAlertTime > _bombAlertInteval;
		}

		public void OnBombPlantError()
		{
			if (CanShowBombAlert())
			{
				AlertView.ShowAttention(ScriptLocalization.Alert.BombArea);
				_bombAlertTime = Time.time;
			}
		}

		public void OnBombPlanting()
		{
			if (CanShowBombAlert())
			{
				PlayAudioClip(_defuseAudioClips.PlantingTheBomb);
				_bombAlertTime = Time.time;
			}
		}

		public void OnBombPlanted(PhotonPlayer bomber, float detonationTime)
		{
			if (IsBombPlantedOnTime())
			{
				AlertView.ShowAttention(string.Format(ScriptLocalization.Alert.BombHasBeenPlanted, detonationTime));
				PlayAudioClip(_defuseAudioClips.BombHasBeenPlanted);
			}
			if (bomber != null && bomber.IsLocal)
			{
				AddMoney(_bombPlantReward);
			}
		}

		public void OnBombBeepSignal()
		{
			TeamGameStatsView.SetBombTimerVisible(visible: false);
			TeamGameStatsView.SetBombTimerVisible(visible: true);
		}

		public bool IsBombPlantedOnTime()
		{
			if (!ScenePhotonBehavior<BombManager>.Instance.IsBombPlanted())
			{
				return false;
			}
			return ScenePhotonBehavior<BombManager>.Instance.PlantTime - PhotonNetwork.room.GetRoundStartTime() < (double)(_roundRunningTime + _roundStartingTime);
		}

		public void OnDetonated(PhotonPlayer bomber)
		{
			if (PlayerControls.Instance.PlayerController != null)
			{
				_bombExplosionEffect.PlayEffect();
			}
		}

		public void OnDefusingBomb()
		{
			if (CanShowBombAlert())
			{
				PlayAudioClip(_defuseAudioClips.DefusingTheBomb);
				_bombAlertTime = Time.time;
			}
		}

		public void OnDefused(PhotonPlayer sapper, float time)
		{
			if (sapper != null && sapper.IsLocal)
			{
				AddMoney(_bombDefusedReward);
			}
			PlayAudioClip(_defuseAudioClips.BombHasBeenDefused);
		}

		public void RoundFinished(Team team, PhotonPlayer mvpPlayer)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"RoundFinished, win team {team}");
			}
			if (team == GetTeam())
			{
				AddMoney(_winTeamReward);
			}
			else
			{
				int num = PhotonNetwork.room.GetConsecutiveLosses(GetTeam()) - 1;
				int num2 = _lossReward + _consecutiveLossReward * num;
				if (num2 > _maxLossReward)
				{
					num2 = _maxLossReward;
				}
				AddMoney(num2);
			}
			PlayAudioClip((team != Team.Tr) ? _defuseAudioClips.CtWin : _defuseAudioClips.TrWin);
		}

		private void AddMoney(int money)
		{
			money = base.Player.GetMoney() + money;
			if (money > _maxMoney)
			{
				money = _maxMoney;
			}
			base.Player.SetMoney(money);
		}
	}
}
