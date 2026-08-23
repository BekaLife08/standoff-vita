using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using Axlebolt.Standoff.Player.Weaponry;
using Photon;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Test
{
	public class LevelTesting : PunBehaviour
	{
		[SerializeField]
		private Canvas _canvas;

		[SerializeField]
		private Camera _mainCamera;

		[SerializeField]
		private Camera _fpsCamera;

		[Space]
		[Header("Counter Terrorist")]
		[PlayerArms]
		[SerializeField]
		private string _ctArms;

		[SerializeField]
		private PlayerCharacters _ctCharacters;

		[Header("Terrorist")]
		[Space]
		[PlayerArms]
		[SerializeField]
		private string _trArms;

		[SerializeField]
		private PlayerCharacters _trCharacters;

		[SerializeField]
		private string _version;

		[SerializeField]
		[Space]
		private WeaponId _primaryWeapon;

		[SerializeField]
		private WeaponId _secondaryWeapon;

		[SerializeField]
		private Team _currentTeam;

		[SerializeField]
		private ViewMode _viewMode = ViewMode.FPS;

		private SpawnPointManager _spawnPointManager;

		private PlayerController _playerController;

		private void Start()
		{
			PhotonNetworkExtension.MessageListeners.Add(this);
			PhotonNetwork.sendRate = 15;
			PhotonNetwork.sendRateOnSerialize = 15;
			PhotonNetwork.ConnectUsingSettings(_version);
		}

		public override void OnConnectedToMaster()
		{
			PhotonNetwork.JoinLobby(TypedLobby.Default);
		}

		public override void OnJoinedLobby()
		{
			PhotonNetwork.JoinOrCreateRoom("LevelTesting2", new RoomOptions
			{
				MaxPlayers = 12,
				IsOpen = true
			}, TypedLobby.Default);
		}

		public override void OnJoinedRoom()
		{
			UnityEngine.MonoBehaviour.print("OnJoinedRoom");
			StartCoroutine(Init());
		}

		private IEnumerator Init()
		{
			PhotonNetwork.isMessageQueueRunning = false;
			EffectsModule.Init();
			Singleton<InventoryManager>.Instance.Init();
			Singleton<WeaponManager>.Instance.Init();
			Singleton<RagdollManager>.Instance.Init(_ctCharacters, _trCharacters);
			Singleton<PlayerManager>.Instance.Init(_ctArms, _ctCharacters, _trArms, _trCharacters, 2);
			Singleton<HitManager>.Instance.Init(friendFireOn: true);
			yield return PlayerControls.CreateInstance(_canvas.transform);
			GamePrefabPool pool = new GamePrefabPool();
			RegisterManagers(pool);
			PhotonNetwork.PrefabPool = pool;
			Singleton<PlayerManager>.Instance.LocalInstantiateEvent += OnSpawned;
			Singleton<PlayerManager>.Instance.LocalDestroyEvent += OnDestroyed;
			PhotonNetwork.isMessageQueueRunning = true;
			yield return InitManagers();
			SpawnPlayer();
		}

		protected virtual void RegisterManagers(GamePrefabPool pool)
		{
			pool.Register<WeaponDropManager>();
		}

		protected virtual IEnumerator InitManagers()
		{
			yield return WeaponDropManager.Init(WeaponUtility.LoadWeapons());
		}

		protected void SpawnPlayer()
		{
			_spawnPointManager = new SpawnPointManager(SpawnZoneType.Random);
			Team team = GetTeam();
			SpawnPoint spawnPoint = _spawnPointManager.GetSpawnPoint(team);
			Singleton<PlayerManager>.Instance.Instantiate(team, spawnPoint.Position, spawnPoint.Rotation, 100, 100, hasHelmet: true);
		}

		protected void DestroyPlayer()
		{
			if ((object)_playerController != null)
			{
				_playerController.DestroyPlayer();
			}
		}

		private Team GetTeam()
		{
			if (_currentTeam == Team.None)
			{
				return (Random.Range(0, 2) != 0) ? Team.Tr : Team.Ct;
			}
			return _currentTeam;
		}

		protected virtual void OnSpawned(PlayerController playerController)
		{
			_playerController = playerController;
			ScenePhotonBehavior<WeaponDropManager>.Instance.SetPlayer(_playerController.gameObject);
			PlayerControls.Instance.PlayerController = playerController;
			ScenePhotonBehavior<WeaponDropManager>.Instance.SetPlayer(_playerController.gameObject);
			playerController.SetMainCamera(_mainCamera);
			playerController.SetFpsCamera(_fpsCamera);
			WeaponryController weaponryController = playerController.WeaponryController;
			weaponryController.SetWeapon(Singleton<WeaponManager>.Instance.GetLocal(_primaryWeapon));
			weaponryController.SetWeapon(Singleton<WeaponManager>.Instance.GetLocal(_secondaryWeapon));
			if (_viewMode == ViewMode.FPS)
			{
				playerController.SetFPSView();
			}
		}

		private void OnDestroyed(PlayerController playerController)
		{
			PlayerControls.Instance.PlayerController = null;
			SpawnPlayer();
		}

		public void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.C) && (object)_playerController != null)
			{
				_playerController.WeaponryController.SetWeapon(Singleton<WeaponManager>.Instance.GetLocal(WeaponId.M4));
			}
			if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.V) && (object)_playerController != null)
			{
				_playerController.WeaponryController.SetWeapon(Singleton<WeaponManager>.Instance.GetLocal(WeaponId.M16));
			}
			if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.B) && (object)_playerController != null)
			{
				_playerController.WeaponryController.SetWeapon(Singleton<WeaponManager>.Instance.GetLocal(WeaponId.AWM));
			}
		}
	}
}
