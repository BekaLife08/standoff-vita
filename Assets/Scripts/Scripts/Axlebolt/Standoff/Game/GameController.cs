using Axlebolt.Standoff.Cam;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Game.Event;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Game.UI.GameStats;
using Axlebolt.Standoff.Game.UI.Pause;
using Axlebolt.Standoff.Game.UI.Spectator;
using Axlebolt.Standoff.Game.UI.Statistics;
using Axlebolt.Standoff.Game.UI.TeamSelect;
using Axlebolt.Standoff.Game.UI.Weaponary;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.UI;
using ExitGames.Client.Photon;
using I2.Loc;
using JetBrains.Annotations;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public abstract class GameController : PunBehaviour, IGameStateMachineListener, IWeaponDropListener
	{
		private static readonly Log Log = Log.Create(typeof(GameController));

		public const int AssistMinDamage = 41;

		public readonly Event<GameInitEventArgs> GameInitEvent = new Event<GameInitEventArgs>();

		public readonly Event<GameExitEventArgs> GameExitEvent = new Event<GameExitEventArgs>();

		public readonly Event<PhotonPlayer> PlayerConnectedEvent = new Event<PhotonPlayer>();

		public readonly Event<PhotonPlayer> PlayerDisconnectedEvent = new Event<PhotonPlayer>();

		public readonly Event<PlayerPropertiesEventArg> PlayerPropertiesChangedEvent = new Event<PlayerPropertiesEventArg>();

		public readonly Event<HitEventArgs> PlayerWasKilledEvent = new Event<HitEventArgs>();

		public readonly Event<HitEventArgs> PlayerKilledEvent = new Event<HitEventArgs>();

		public readonly Event<HitEventArgs> PlayerAssistEvent = new Event<HitEventArgs>();

		[SerializeField]
		private bool _teamCollisionOn;

		[SerializeField]
		private bool _friendlyFireOn;

		[SerializeField]
		private byte _maxPlayers;

		private GameStateMachine _stateMachine;

		private Coroutine _stateMachineCoroutine;

		private SpawnPointManager _spawnPointManager;

		private LevelDefinition _level;

		protected Camera FpsCamera;

		protected Camera MainCamera;

		private KillCamera _killCamera;

		protected PlayerSpawnEffect PlayerSpawnEffect;

		protected SpectatorController SpectatorController;

		private AudioSource _gameAudioSource;

		private Dictionary<int, GamePlayer> _gamePlayers;

		private WeaponParameters[] _allWeapons;

		private SettingsController _settingsController;

		private Coroutine _weaponBuyCoroutine;

		protected TeamSelectView TeamSelectView;

		protected WeaponBuyView WeaponBuyView;

		protected WeaponBuyButton WeaponBuyButton;

		protected StatisticsView StatisticsView;

		protected StatisticsButton StatisticsButton;

		protected AlertView AlertView;

		protected TeamGameStatsView TeamGameStatsView;

		protected PersonalGameStatsView PersonalGameStatsView;

		protected KillLogView KillLogView;

		protected PauseButton PauseButton;

		protected PauseView PauseView;

		protected ScreenshotView ScreenshotView;

		protected WhoKillView WhoKillView;

		protected SpectatorView SpectatorView;

		public static GameController Instance
		{
			get;
			private set;
		}

		public bool TeamCollisionOn
		{
			[CompilerGenerated]
			get
			{
				return _teamCollisionOn;
			}
		}

		public bool FriendlyFireOn
		{
			[CompilerGenerated]
			get
			{
				return _friendlyFireOn;
			}
		}

		public byte MaxPlayers
		{
			[CompilerGenerated]
			get
			{
				return _maxPlayers;
			}
		}

		public GameView GameView
		{
			get;
			private set;
		}

		public PlayerControls PlayerControls
		{
			get;
			private set;
		}

		protected IGameState CurrentGameState
		{
			[CompilerGenerated]
			get
			{
				return _stateMachine.GameState;
			}
		}

		protected byte CurrentGameStateId
		{
			[CompilerGenerated]
			get
			{
				return _stateMachine.GameState.Id;
			}
		}

		public virtual GameType GameType
		{
			[CompilerGenerated]
			get
			{
				return GameType.Personal;
			}
		}

		public float PlayerSpawnedTime
		{
			get;
			private set;
		}

		public PlayerController PlayerController
		{
			get;
			private set;
		}

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return PhotonNetwork.player;
			}
		}

		public IEnumerator Init([NotNull] LevelDefinition level)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level");
			}
			if (Instance != null)
			{
				throw new InvalidOperationException("GameController already initialized");
			}
			Instance = this;
			_level = level;
			InitGamePlayers();
			InitWeapons();
			InitManagers();
			InitSceneObjects();
			InitSpawnPointProvider();
			InitZones();
			PhotonNetworkExtension.MessageListeners.Add(this);
			StartCoroutine(PhotonGameExtension.UpdatePing());
			yield return CreateUi();
			InitUi();
			string gameMode = PhotonNetwork.room.GetGameModeName();
			GameInitEvent.Invoke(new GameInitEventArgs(gameMode, _maxPlayers));
		}

		public IEnumerator InitNetwork()
		{
			yield return InitSceneManagers();
		}

		public void StartGame()
		{
			InitCamera();
			BindHitManager();
			InitSpectatorController();
			InitStateMachine();
			StartStateMachine();
		}

		protected virtual void InitCamera()
		{
			GameObject original = ResourcesUtility.Load<GameObject>("Camera/FpsCamera");
			GameObject original2 = ResourcesUtility.Load<GameObject>("Camera/MainCamera");
			FpsCamera = UnityEngine.Object.Instantiate(original).GetRequireComponent<Camera>();
			MainCamera = UnityEngine.Object.Instantiate(original2).GetRequireComponent<Camera>();
			_gameAudioSource = MainCamera.gameObject.AddComponent<AudioSource>();
			_gameAudioSource.loop = false;
			_gameAudioSource.spatialBlend = 0f;
			_gameAudioSource.playOnAwake = false;
			_killCamera = MainCamera.gameObject.AddComponent<KillCamera>();
			PlayerSpawnEffect = MainCamera.gameObject.AddComponent<PlayerSpawnEffect>();
		}

		protected void PlayAudioClip(AudioClip clip)
		{
			_gameAudioSource.clip = clip;
			_gameAudioSource.Play();
		}

		private IEnumerator CreateUi()
		{
			Canvas canvas = GameObject.Find("Canvas").GetRequireComponent<Canvas>();
			yield return Singleton<ScenePrefab>.Instance.LoadPrefab<GameView>("GameView");
			yield return Singleton<ScenePrefab>.Instance.LoadPrefab<SettingsController>("Settings");
			_settingsController = Singleton<ScenePrefab>.Instance.Singleton<SettingsController>(canvas.transform);
			_settingsController.transform.SetAsFirstSibling();
			GameView = Singleton<ScenePrefab>.Instance.Singleton<GameView>(canvas.transform);
			GameView.transform.SetAsFirstSibling();
			yield return PlayerControls.CreateInstance(canvas.transform);
			PlayerControls = PlayerControls.Instance;
			PlayerControls.transform.SetAsFirstSibling();
			PlayerControls.PlayerController = null;
			yield return GameView.Init();
		}

		protected virtual void InitUi()
		{
			TeamSelectView = GameView.GetView<TeamSelectView>();
			TeamGameStatsView = GameView.GetView<TeamGameStatsView>();
			PersonalGameStatsView = GameView.GetView<PersonalGameStatsView>();
			AlertView = GameView.GetView<AlertView>();
			StatisticsButton = GameView.GetView<StatisticsButton>();
			StatisticsView = GameView.GetView<StatisticsView>();
			WeaponBuyButton = GameView.GetView<WeaponBuyButton>();
			WeaponBuyView = GameView.GetView<WeaponBuyView>();
			KillLogView = GameView.GetView<KillLogView>();
			PauseButton = GameView.GetView<PauseButton>();
			PauseView = GameView.GetView<PauseView>();
			ScreenshotView = GameView.GetView<ScreenshotView>();
			WhoKillView = GameView.GetView<WhoKillView>();
			SpectatorView = GameView.GetView<SpectatorView>();
			InitWeaponBuyView();
			InitStatisticsView();
			InitTeamSelectView();
			InitPauseView();
			InitGameHud();
		}

		protected virtual void InitWeaponBuyView()
		{
			if (IsWeaponBuySupport())
			{
				WeaponBuyView.Init(this);
				WeaponBuyButton.Clicked = ShowWeaponBuyView;
				WeaponBuyView.Closed = HideWeaponBuyView;
				WeaponBuyView.VisibleChanged += DisableControlsHandler(WeaponBuyView);
			}
		}

		protected virtual void InitStatisticsView()
		{
			StatisticsView.Init(string.Empty, string.Empty);
			StatisticsButton.Show();
			StatisticsButton.Down = delegate
			{
				StatisticsView.Show();
			};
			StatisticsButton.Up = delegate
			{
				StatisticsView.Hide();
			};
		}

		protected virtual void InitTeamSelectView()
		{
			TeamSelectView.Init(this);
			TeamSelectView.LevelImage = _level.FullScreenImage;
			TeamSelectView.VisibleChanged += DisableControlsHandler(TeamSelectView);
		}

		protected virtual void InitPauseView()
		{
			PauseButton.Show();
			PauseButton.OnClick = delegate
			{
				PauseView.Show();
			};
			PauseView.VisibleChanged += DisableControlsHandler(PauseView);
			PauseView.VisibleChanged += delegate(bool isVisible)
			{
				PauseButton.IsVisible = !isVisible;
			};
			PauseView.Main.AddItem(ScriptLocalization.PauseMenu.BackToGame, delegate
			{
				PauseView.Hide();
			}, KeyCode.Escape);
			if (IsTeamSelectEnabled())
			{
				PauseView.Main.AddItem(ScriptLocalization.PauseMenu.ChangeTeam, delegate
				{
					PauseView.Hide();
					TeamSelectView.Show();
				});
			}
			_settingsController.Init();
			_settingsController.Close();
			_settingsController.OpenStateChangedEvent += delegate(bool open)
			{
				PauseView.gameObject.SetActive(!open);
			};
			PauseView.Main.AddItem(ScriptLocalization.PauseMenu.Settings, delegate
			{
				_settingsController.Open();
			});
			PauseView.Main.AddItem(ScriptLocalization.PauseMenu.ExitGame, delegate
			{
				GameManager.Instance.ExitGameWithConfirm();
				PauseView.Hide();
			});
		}

		protected virtual void InitGameHud()
		{
			HudComponentView[] hudViews = GetHudViews();
			HudComponentView[] array = hudViews;
			foreach (HudComponentView hudComponentView in array)
			{
				PlayerControls.HudView.RegisterHud(hudComponentView);
			}
		}

		protected virtual HudComponentView[] GetHudViews()
		{
			return new HudComponentView[0];
		}

		public Action<bool> DisableControlsHandler(UnityEngine.Object view)
		{
			return delegate(bool visible)
			{
				if (visible)
				{
					PlayerControls.RequestDisable(view);
				}
				else
				{
					PlayerControls.RequestEnable(view);
				}
			};
		}

		protected virtual void InitSpawnPointProvider()
		{
			_spawnPointManager = CreateSpawnPointManager();
		}

		protected abstract SpawnPointManager CreateSpawnPointManager();

		protected virtual void InitWeapons()
		{
			_allWeapons = WeaponUtility.LoadWeapons(GetWeaponIds());
		}

		protected virtual void InitZones()
		{
		}

		protected virtual WeaponId[] GetWeaponIds()
		{
			return WeaponUtility.GetBaseWeaponIds();
		}

		protected virtual void InitManagers()
		{
			EffectsModule.Init();
			Singleton<InventoryManager>.Instance.Init();
			Singleton<WeaponManager>.Instance.Init(GetAllWeapons());
			Singleton<RagdollManager>.Instance.Init(_level.CtCharacters, _level.TrCharacters);
			Singleton<PlayerManager>.Instance.Init(_level.CtArms, _level.CtCharacters, _level.TrArms, _level.TrCharacters, 5);
			Singleton<PlayerManager>.Instance.LocalInstantiateEvent += OnPlayerSpawned;
			Singleton<PlayerManager>.Instance.RemoteInstantiateEvent += OnRemotePlayerSpawned;
			Singleton<PlayerManager>.Instance.LocalDestroyEvent += OnDestroyed;
			Singleton<AvatarSupport>.Instance.Init(_maxPlayers);
			GamePrefabPool gamePrefabPool = new GamePrefabPool();
			InitPrefabPool(gamePrefabPool);
			PhotonNetwork.PrefabPool = gamePrefabPool;
		}

		protected virtual void InitPrefabPool(GamePrefabPool pool)
		{
			pool.Register<GameStateHelper>();
			if (IsWeaponBuySupport())
			{
				pool.Register<WeaponDropManager>();
			}
		}

		protected virtual IEnumerator InitSceneManagers()
		{
			yield return ScenePhotonBehavior<GameStateHelper>.Init(null);
			if (IsWeaponDropSupport())
			{
				yield return WeaponDropManager.Init(GetAllWeapons());
				ScenePhotonBehavior<WeaponDropManager>.Instance.DropListener = this;
			}
		}

		private void InitSceneObjects()
		{
			GameObject[] array = ResourcesUtility.LoadAll<GameObject>("SceneObjects");
			GameObject[] array2 = array;
			foreach (GameObject original in array2)
			{
				UnityEngine.Object.Instantiate(original, base.transform);
			}
		}

		private void InitGamePlayers()
		{
			_gamePlayers = new Dictionary<int, GamePlayer>();
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in playerList)
			{
				_gamePlayers.Add(photonPlayer.ID, new GamePlayer(photonPlayer));
			}
		}

		public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable changedProperties)
		{
			if (changedProperties.ContainsKey("C2") && !PhotonNetwork.isMasterClient)
			{
				if (Log.DebugEnabled)
				{
					Log.Debug("State was changed, update state machine");
				}
				if (IsStateMachineRunning())
				{
					_stateMachine.Update();
				}
			}
		}

		private void InitStateMachine()
		{
			_stateMachine = new GameStateMachine(CreateStateRouter(), this);
			RegisterStates(_stateMachine);
			_stateMachine.Setup(this);
		}

		public void StartStateMachine()
		{
			if (_stateMachineCoroutine != null)
			{
				Log.Error("StateMachine already started");
				return;
			}
			Log.Debug("StartStateMachine");
			_stateMachineCoroutine = StartCoroutine(_stateMachine.GameLoop());
		}

		public void StopStateMachine()
		{
			if (_stateMachineCoroutine == null)
			{
				Log.Error("StateMachine is not started");
				return;
			}
			Log.Debug("StopStateMachine");
			StopCoroutine(_stateMachineCoroutine);
			_stateMachineCoroutine = null;
		}

		public bool IsStateMachineRunning()
		{
			return _stateMachineCoroutine != null;
		}

		protected abstract GameStateRouter CreateStateRouter();

		protected abstract void RegisterStates(GameStateMachine stateMachine);

		public virtual void OnGameInit()
		{
			if (IsTeamSelectEnabled())
			{
				TeamSelectView.Show();
			}
		}

		public virtual void OnGameFinishedState()
		{
			GameManager.Instance.GameFinished();
		}

		public virtual void OnGameStateChanged(IGameState state)
		{
		}

		protected virtual void InitSpectatorController()
		{
			SpectatorController = base.gameObject.AddComponent<SpectatorController>();
			SpectatorController.Init(FpsCamera, MainCamera);
			SpectatorController.OnSpectateMode = delegate(bool spectateMode)
			{
				if (!spectateMode)
				{
					SpectatorView.Hide();
				}
			};
			SpectatorController.OnSectateTo = delegate(PhotonPlayer player)
			{
				SpectatorView.Show(player);
			};
			SpectatorView.NextHandler = delegate
			{
				SpectatorController.ToSpectateNext();
			};
			SpectatorView.PreviousHandler = delegate
			{
				SpectatorController.ToSpectatePrevious();
			};
		}

		protected virtual void BindHitManager()
		{
			Singleton<HitManager>.Instance.Init(_friendlyFireOn);
			Singleton<HitManager>.Instance.HitEvent.AddListener(OnHit);
			Singleton<HitManager>.Instance.SuicideEvent.AddListener(OnSuicide);
		}

		protected virtual bool IsTeamSelectEnabled()
		{
			return true;
		}

		public bool CanChangeTeamTo(Team team)
		{
			if (team == Team.None)
			{
				Log.Error("Invalid argument, can't change team to None");
				return false;
			}
			int num = PhotonNetwork.playerList.GetByTeam(Team.Tr, excludeCurrentPlayer: true).Length;
			int num2 = PhotonNetwork.playerList.GetByTeam(Team.Ct, excludeCurrentPlayer: true).Length;
			return (team != Team.Tr) ? (num2 <= num) : (num <= num2);
		}

		public void ChangeTeamTo(Team team)
		{
			if (PhotonNetwork.player.GetTeam() == team)
			{
				TeamSelectView.Hide();
				return;
			}
			PhotonNetwork.player.SetTeam(team);
			if (PlayerController != null)
			{
				KillPlayer();
			}
			else
			{
				SpawnPlayer();
			}
		}

		protected virtual void OnHit(HitEventArgs hitEventArgs)
		{
			GamePlayer gamePlayer = GetGamePlayer(hitEventArgs.Victim);
			gamePlayer.AddHit(hitEventArgs.Shooter, hitEventArgs.Damage);
			if (hitEventArgs.Victim.IsDead())
			{
				HitLogs hits = gamePlayer.GetHits(hitEventArgs.Shooter);
				HitLogs assistHits = gamePlayer.GetAssistHits(hitEventArgs.Shooter, 41);
				KillLogView.LogKill(hitEventArgs.Shooter, assistHits?.Player, hitEventArgs.Victim, hitEventArgs.Weapon, hitEventArgs.Headshot, hitEventArgs.Penetrated);
				if (hitEventArgs.Shooter.IsLocal)
				{
					OnPlayerKilled(hitEventArgs);
					PlayerKilledEvent.Invoke(hitEventArgs);
				}
				if (assistHits?.Player != null && assistHits.Player.IsLocal)
				{
					OnPlayerAssist(hitEventArgs);
					PlayerAssistEvent.Invoke(hitEventArgs);
				}
				if (hitEventArgs.Victim.IsLocal)
				{
					OnPlayerWasKilled(hitEventArgs, hits);
					PlayerWasKilledEvent.Invoke(hitEventArgs);
				}
			}
		}

		protected virtual void OnSuicide(PhotonPlayer photonPlayer)
		{
			if (photonPlayer.IsLocal)
			{
				OnPlayerSuicide();
			}
		}

		protected virtual void OnRemotePlayerSpawned(PlayerController playerController)
		{
			GetGamePlayer(playerController.Player).ClearHits();
		}

		protected virtual void OnPlayerSpawned(PlayerController playerController)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("Player OnPlayerSpawned");
			}
			GetGamePlayer(playerController.Player).ClearHits();
			TeamSelectView.Hide();
			ScreenshotView.Hide();
			WhoKillView.Hide();
			PlayerController = playerController;
			PlayerController.SetMainCamera(MainCamera);
			PlayerController.SetFpsCamera(FpsCamera);
			PlayerController.SetFPSView();
			PlayerControls.PlayerController = PlayerController;
			BindPlayerToManagers(PlayerController);
			SetPlayerWeapon(WeaponId.Knife);
			PlayerSpawnedTime = Time.time;
		}

		protected virtual void BindPlayerToManagers(PlayerController playerController)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("BindPlayerToManagers");
			}
			if (ScenePhotonBehavior<WeaponDropManager>.IsInitialized())
			{
				ScenePhotonBehavior<WeaponDropManager>.Instance.SetPlayer(playerController?.gameObject);
			}
		}

		protected virtual void OnPlayerKilled(HitEventArgs hitEventArgs)
		{
			Player.AddKill();
		}

		protected virtual void OnPlayerAssist(HitEventArgs hitEventArgs)
		{
			Player.AddAssist();
		}

		protected virtual void OnPlayerWasKilled(HitEventArgs hitEventArgs, HitLogs takenHits)
		{
			Player.AddDeath();
			StartCoroutine(OnPlayerWasKilledCoroutine(hitEventArgs, takenHits));
		}

		protected abstract IEnumerator OnPlayerWasKilledCoroutine(HitEventArgs hitEventArgs, HitLogs takenHits);

		protected virtual void OnPlayerSuicide()
		{
			Player.AddDeath();
			StartCoroutine(OnPlayerSuicideCoroutine());
		}

		protected abstract IEnumerator OnPlayerSuicideCoroutine();

		protected virtual void OnDestroyed(PlayerController playerController)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("Player OnDestroyed");
			}
			AlertView.Hide();
			WeaponBuyButton.Hide();
			HideWeaponBuyView();
			PlayerControls.PlayerController = null;
			PlayerController = null;
			BindPlayerToManagers(null);
		}

		protected virtual bool CanSpawnPlayer()
		{
			return _stateMachine.GameState.CanSpawnPlayer();
		}

		public virtual void SpawnPlayer()
		{
			Log.Debug("SpawnPlayer");
			if (!CanSpawnPlayer())
			{
				Log.Debug("Can't spawn player, state rejected");
				return;
			}
			Team team = GetTeam();
			if (team == Team.None)
			{
				Log.Debug("Team None, skip spawn");
				return;
			}
			if (PlayerController != null)
			{
				DestroyPlayer();
			}
			SpawnPoint spawnPoint = GetSpawnPoint();
			MainCamera.transform.position = spawnPoint.Position + Vector3.up * 0.9f;
			MainCamera.transform.rotation = spawnPoint.Rotation;
			InstantiatePlayer(team, spawnPoint);
		}

		protected Team GetTeam()
		{
			return PhotonNetwork.player.GetTeam();
		}

		protected virtual SpawnPoint GetSpawnPoint()
		{
			return _spawnPointManager.GetSpawnPoint(Player.GetTeam());
		}

		protected virtual void InstantiatePlayer(Team team, SpawnPoint spawnPoint)
		{
			Log.Debug("InstantiatePlayer");
			Singleton<PlayerManager>.Instance.Instantiate(team, spawnPoint.Position, spawnPoint.Rotation);
		}

		protected void KillPlayer()
		{
			if (PlayerController != null)
			{
				PlayerController.Die();
			}
		}

		public void DestroyPlayer()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("DestroyPlayer");
			}
			if (PlayerController != null)
			{
				PlayerController.DestroyPlayer();
			}
		}

		public bool IsPlayerAlive()
		{
			return PlayerController != null;
		}

		public void ToSpectateMode()
		{
			SpectatorController.ToSpectatorMode();
		}

		protected IEnumerator PlayDieWithFocusEffect(PhotonPlayer killer)
		{
			ScreenshotView.Hide();
			RagdollController ragdoll = Singleton<RagdollManager>.Instance.GetActiveRagdollByPlayerId(Player.ID);
			PlayerController playerController = Singleton<PlayerManager>.Instance.GetController(killer.ID);
			if (ragdoll == null)
			{
				Log.Error("Can't show effect, ragdoll not found");
				yield break;
			}
			if (playerController == null)
			{
				Log.Error("Can't show effect, killer player not found");
				yield break;
			}
			yield return _killCamera.PlayerDieEffect.PlayEffect(ragdoll.Character, playerController.transform);
			yield return _killCamera.PlayerFocusEffect.PlayEffect(playerController.transform, ScreenshotView.RawImage);
		}

		protected IEnumerator PlayDieEffect(Transform killerTransform)
		{
			ScreenshotView.Hide();
			RagdollController ragdoll = Singleton<RagdollManager>.Instance.GetActiveRagdollByPlayerId(Player.ID);
			if (ragdoll == null)
			{
				Log.Error("Can't show effect, ragdoll not found");
			}
			else
			{
				yield return _killCamera.PlayerDieEffect.PlayEffect(ragdoll.Character, killerTransform);
			}
		}

		public void ShowWeaponBuyView()
		{
			if (IsWeaponBuySupport() && CanBuyWeapons())
			{
				WeaponBuyView.Show();
			}
		}

		public void HideWeaponBuyView()
		{
			WeaponBuyView.Hide();
		}

		protected void StartWeaponBuyCoroutine(double startTime, float duration)
		{
			if (_weaponBuyCoroutine != null)
			{
				StopCoroutine(_weaponBuyCoroutine);
			}
			_weaponBuyCoroutine = StartCoroutine(WeaponBuyCoroutine(startTime, duration));
		}

		protected IEnumerator WeaponBuyCoroutine(double startTime, float duration)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("WeaponBuyCoroutine");
			}
			while (PhotonNetwork.time - startTime <= (double)duration && PlayerController != null)
			{
				if (CanBuyWeapons())
				{
					WeaponBuyButton.Show();
					WeaponBuyView.TimeLeft = duration - (float)(PhotonNetwork.time - startTime);
				}
				else
				{
					WeaponBuyButton.Hide();
					HideWeaponBuyView();
				}
				yield return new WaitForSeconds(0.1f);
			}
			WeaponBuyButton.Hide();
			HideWeaponBuyView();
			_weaponBuyCoroutine = null;
		}

		public abstract bool IsWeaponBuySupport();

		public abstract bool CanBuyWeapons();

		public abstract bool CanBuyWeapon(WeaponParameters weapon);

		public abstract bool IsWeaponDropSupport();

		public virtual void OnWeaponDrop(WeaponParameters weapon)
		{
			AlertView.ShowAttention(string.Format(ScriptLocalization.Alert.YouDropped, weapon.DisplayName));
		}

		public virtual void OnWeaponTake(WeaponParameters weapon)
		{
			AlertView.ShowAttention(string.Format(ScriptLocalization.Alert.YouTake, weapon.DisplayName));
		}

		public virtual void BuyWeapon(WeaponParameters weapon)
		{
			SetPlayerWeapon(weapon.Id);
		}

		protected void SetPlayerWeapon(WeaponId weaponId)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"SetPlayerWeapon {weaponId}");
			}
			if (PlayerController != null)
			{
				WeaponController local = Singleton<WeaponManager>.Instance.GetLocal(weaponId);
				PlayerController.WeaponryController.SetWeapon(local);
			}
			else
			{
				Log.Warning("Can't set weapon, player not spawned");
			}
		}

		public WeaponParameters[] GetPlayerWeapons()
		{
			if (PlayerController != null)
			{
				return (from controller in PlayerController.WeaponryController.GetWeapons()
					select controller.WeaponParameters).ToArray();
			}
			Log.Warning("Can't get weapon's, player not spawned");
			return new WeaponParameters[0];
		}

		public virtual WeaponParameters[] GetAllWeapons()
		{
			return _allWeapons;
		}

		private void Update()
		{
			if (IsStateMachineRunning())
			{
				if (!PhotonNetwork.inRoom)
				{
					StopStateMachine();
					GameManager.Instance.Reconnect();
				}
				else if (_level.name != PhotonNetwork.room.GetLevelName())
				{
					GameManager.Instance.ReInitGame();
				}
			}
		}

		public virtual Dialog CreateConfirmExitDialog()
		{
			return Dialogs.Confirm(ScriptLocalization.Common.Confirmation, ScriptLocalization.Dialogs.ExitGameConfirmation);
		}

		public virtual void OnExitGame()
		{
			GameExitEvent.Invoke(new GameExitEventArgs());
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
		{
			GameView?.Refresh();
			_gamePlayers[photonPlayer.ID] = new GamePlayer(photonPlayer);
			PlayerConnectedEvent.Invoke(photonPlayer);
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
		{
			GameView?.Refresh();
			_gamePlayers.Remove(photonPlayer.ID);
			PlayerDisconnectedEvent.Invoke(photonPlayer);
		}

		public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
			ExitGames.Client.Photon.Hashtable hashtable = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;
			PhotonPlayer photonPlayer = playerAndUpdatedProps[0] as PhotonPlayer;
			if (hashtable != null && photonPlayer != null)
			{
				GameView?.Refresh(hashtable.Keys);
				PlayerPropertiesChangedEvent.Invoke(new PlayerPropertiesEventArg(hashtable.Keys, photonPlayer));
			}
		}

		protected GamePlayer GetGamePlayer([NotNull] PhotonPlayer player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			if (_gamePlayers.ContainsKey(player.ID))
			{
				return _gamePlayers[player.ID];
			}
			Log.Error($"GamePlayer for {player} not found");
			_gamePlayers[player.ID] = new GamePlayer(player);
			return _gamePlayers[player.ID];
		}

		protected virtual void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
		}

		void IGameStateMachineListener.OnGameInit()
		{
			OnGameInit();
		}
	}
}
