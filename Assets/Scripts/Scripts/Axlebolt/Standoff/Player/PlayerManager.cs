using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player.Networking;
using Axlebolt.Standoff.Settings.Video;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerManager : Singleton<PlayerManager>
	{
		public delegate void EventHandler(PlayerController playerController);

		private static readonly Log Log = Log.Create(typeof(PlayerManager));

		private PlayerHitboxConfig _playerHitboxConfig;

		private PlayerPool _ctPool;

		private PlayerPool _trPool;

		private PlayerSnapshot _lastSnapshot;

		private int _playerViewId;

		private readonly Dictionary<int, PlayerController> _playerById = new Dictionary<int, PlayerController>();

		internal PlayerController PlayerPrefab
		{
			get;
			private set;
		}

		public PlayerController CurrentPlayer
		{
			get;
			private set;
		}

		public event EventHandler RemoteInstantiateEvent;

		public event EventHandler RemoteDestroyEvent;

		public event EventHandler LocalInstantiateEvent;

		public event EventHandler LocalDestroyEvent;

		public void Init([NotNull] string ctArms, [NotNull] PlayerCharacters ctCharacters, [NotNull] string trArms, [NotNull] PlayerCharacters trCharacters, int poolSize)
		{
			if (ctArms == null)
			{
				throw new ArgumentNullException("ctArms");
			}
			if (ctCharacters == null)
			{
				throw new ArgumentNullException("ctCharacters");
			}
			if (trArms == null)
			{
				throw new ArgumentNullException("trArms");
			}
			if (trCharacters == null)
			{
				throw new ArgumentNullException("trCharacters");
			}
			PlayerPrefab = PlayerUtility.LoadPlayerPrefab();
			_playerHitboxConfig = PlayerUtility.LoadPlayerHitboxConfig();
			_ctPool = new PlayerPool(PlayerPrefab, _playerHitboxConfig, ctArms, ctCharacters, poolSize);
			_trPool = new PlayerPool(PlayerPrefab, _playerHitboxConfig, trArms, trCharacters, poolSize);
			VideoSettingsManager.Instance.ModelDetailChanged += OnModelDetailChanged;
			VideoSettingsManager.Instance.ShaderDetailChanged += OnShaderDetailChanged;
		}

		private void OnModelDetailChanged()
		{
			_ctPool.RefreshLods();
			_trPool.RefreshLods();
		}

		private void OnShaderDetailChanged()
		{
			_ctPool.UpdateMaterials();
			_trPool.UpdateMaterials();
		}

		public void Instantiate(Team team, Vector3 position, Quaternion rotation, int health = 100, int armor = 0, bool hasHelmet = false)
		{
			PhotonNetwork.player.SetHealth(0);
			PhotonNetwork.player.SetArmor(0);
			PhotonNetwork.player.SetHelmet(hasHelmet: false);
			string freeId = GetFreeId(team);
			GameObject gameObject = PhotonNetwork.Instantiate(freeId, position, rotation, 0);
			PlayerController component = gameObject.GetComponent<PlayerController>();
			component.Instantiate(health, armor, hasHelmet);
		}

		internal void OnRemoteInstantiateEvent(PlayerController playerController)
		{
			_playerById[playerController.PhotonView.ownerId] = playerController;
			if (this.RemoteInstantiateEvent != null)
			{
				this.RemoteInstantiateEvent(playerController);
			}
		}

		internal void OnLocalInstantiateEvent(PlayerController playerController)
		{
			if (!playerController.PhotonView.isMine)
			{
				Log.Error("Incorrent local instantiate event, actual remote player");
				return;
			}
			if (CurrentPlayer == playerController)
			{
				Log.Error("OnLocalInstantiateEvent already called.");
				return;
			}
			PlayerController currentPlayer = CurrentPlayer;
			CurrentPlayer = playerController;
			if (currentPlayer != null)
			{
				PhotonNetwork.Destroy(currentPlayer.gameObject);
			}
			if (_playerViewId == CurrentPlayer.PhotonView.viewID && _lastSnapshot != null)
			{
				Log.Debug("Set last snapshot to reconnected player");
				CurrentPlayer.SetSnapsot(FixSnapshot(_lastSnapshot));
				CurrentPlayer.WeaponryController.ReswitchCurrentWeapon();
			}
			_playerViewId = CurrentPlayer.PhotonView.viewID;
			_lastSnapshot = null;
			if (this.LocalInstantiateEvent != null)
			{
				this.LocalInstantiateEvent(CurrentPlayer);
			}
		}

		private PlayerSnapshot FixSnapshot(PlayerSnapshot snapshot)
		{
			return (PlayerSnapshot)CurrentPlayer.Interpolator.Interpolate(snapshot, snapshot, 0f);
		}

		internal void OnLocalDestroyEvent(PlayerController playercontroller)
		{
			if (!(CurrentPlayer != playercontroller))
			{
				CurrentPlayer = null;
				_lastSnapshot = playercontroller.GetSnapshot();
				if (this.LocalDestroyEvent != null)
				{
					this.LocalDestroyEvent(playercontroller);
				}
			}
		}

		internal void OnRemoteDestroyEvent(PlayerController playerController, int id)
		{
			_playerById.Remove(id);
			if (this.RemoteDestroyEvent != null)
			{
				this.RemoteDestroyEvent(playerController);
			}
		}

		public PlayerController GetController(int playerId)
		{
			PlayerController value;
			return (!_playerById.TryGetValue(playerId, out value)) ? null : value;
		}

		public PlayerController[] GetControllers()
		{
			return _playerById.Values.ToArray();
		}

		internal void CacheSnapshot([NotNull] PlayerSnapshot playerSnapshot)
		{
			if (playerSnapshot == null)
			{
				throw new ArgumentNullException("playerSnapshot");
			}
			if (CurrentPlayer == null)
			{
				Log.Error("Current player is null, is's impossible! ");
			}
			else
			{
				_lastSnapshot = playerSnapshot;
			}
		}

		public GameObject GetFromPool(string id, Vector3 position, Quaternion rotation)
		{
			PlayerUtility.ParseId(id, out Team team, out string character);
			PlayerController fromPool = GetPool(team).GetFromPool(character);
			fromPool.name = id;
			fromPool.gameObject.SetActive(value: true);
			fromPool.transform.position = position;
			fromPool.transform.rotation = rotation;
			fromPool.Initialize();
			return fromPool.gameObject;
		}

		public void ReturnToPool(GameObject go)
		{
			PlayerController component = go.GetComponent<PlayerController>();
			if (component == null)
			{
				Log.Error("Can't return to pool {0}, Player not found", go);
				return;
			}
			string name = go.name;
			PlayerUtility.ParseId(name, out Team team, out string _);
			go.SetActive(value: false);
			component.OnReturnToPool();
			GetPool(team).ReturnToPool(component);
		}

		private PlayerPool GetPool(Team team)
		{
			object result;
			switch (team)
			{
			case Team.None:
				throw new InvalidOperationException("Can't get Player for None team");
			case Team.Ct:
				result = _ctPool;
				break;
			default:
				result = _trPool;
				break;
			}
			return (PlayerPool)result;
		}

		public List<string> GetIds()
		{
			List<string> list = (from skin in _trPool.Characters
				select PlayerUtility.CreateId(Team.Tr, skin)).ToList();
			list.AddRange((from skin in _ctPool.Characters
				select PlayerUtility.CreateId(Team.Ct, skin)).ToList());
			return list;
		}

		public string GetFreeId(Team team)
		{
			string freeCharacter = GetPool(team).GetFreeCharacter();
			return PlayerUtility.CreateId(team, freeCharacter);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			VideoSettingsManager.Instance.ModelDetailChanged -= OnModelDetailChanged;
			VideoSettingsManager.Instance.ShaderDetailChanged -= OnShaderDetailChanged;
		}
	}
}
