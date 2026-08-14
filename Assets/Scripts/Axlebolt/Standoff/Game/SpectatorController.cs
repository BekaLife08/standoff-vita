using Axlebolt.Standoff.Cam;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class SpectatorController : PunBehaviour
	{
		private class PlayerComparer : IComparer<PhotonPlayer>
		{
			private readonly Team _team;

			public PlayerComparer(Team team)
			{
				_team = team;
			}

			public int Compare(PhotonPlayer x, PhotonPlayer y)
			{
				if (x.GetTeam() == y.GetTeam())
				{
					return x.ID.CompareTo(y.ID);
				}
				return (_team == x.GetTeam()) ? 1 : (-1);
			}
		}

		private static readonly Log Log = Log.Create("SpectatorController");

		private Camera _fpsCamera;

		private Camera _mainCamera;

		private bool _spectateMode;

		private PlayerController _spectateController;

		private int _index;

		private PhotonPlayer[] _players;

		private CameraCharacterFollower _cameraCharacterFollower;

		public Action<bool> OnSpectateMode
		{
			get;
			set;
		}

		public Action<PhotonPlayer> OnSectateTo
		{
			get;
			set;
		}

		public void Init(Camera fpsCamera, Camera mainCamera)
		{
			_fpsCamera = fpsCamera;
			_mainCamera = mainCamera;
			Singleton<PlayerManager>.Instance.RemoteDestroyEvent += OnPlayerDestroyed;
			PhotonNetworkExtension.MessageListeners.Add(this);
			_cameraCharacterFollower = mainCamera.gameObject.AddComponent<CameraCharacterFollower>();
		}

		private void CheckIsInitialized()
		{
			if (_fpsCamera == null || _mainCamera == null)
			{
				throw new Exception(string.Format("{0} is not initialized", "SpectatorController"));
			}
		}

		public void ToSpectatorMode()
		{
			CheckIsInitialized();
			CancelSpectateMode();
			UpdatePlayers();
			_spectateMode = true;
			OnSpectateMode?.Invoke(obj: true);
			ToSpectateNext();
		}

		public void ToSpectate(PhotonPlayer player)
		{
			CheckIsInitialized();
			if (player.IsDead())
			{
				throw new Exception("Can't spectate dead player");
			}
			PlayerController controller = Singleton<PlayerManager>.Instance.GetController(player.ID);
			if (controller == null)
			{
				Log.Error("Player is alive, but controller is null");
				return;
			}
			PlayerControls.Instance.PlayerController = controller;
			_cameraCharacterFollower.Follow(controller.Transform);
			_spectateController = controller;
			OnSectateTo?.Invoke(player);
		}

		public void ToSpectateNext()
		{
			CheckIsInitialized();
			_index++;
			_players = (from player in _players
				where !player.IsDead()
				select player).ToArray();
			if (_players.Length == 0)
			{
				CancelSpectateMode();
				return;
			}
			if (_index < 0 || _index >= _players.Length)
			{
				_index = 0;
			}
			ToSpectate(_players[_index]);
		}

		public void ToSpectatePrevious()
		{
			CheckIsInitialized();
			_index--;
			_players = (from player in _players
				where !player.IsDead()
				select player).ToArray();
			if (_players.Length == 0)
			{
				CancelSpectateMode();
				return;
			}
			if (_index < 0 || _index >= _players.Length)
			{
				_index = _players.Length - 1;
			}
			ToSpectate(_players[_index]);
		}

		private void UpdatePlayers()
		{
			_players = (from player in PhotonNetwork.playerList
				where !player.IsLocal
				select player).OrderBy((PhotonPlayer player) => player, new PlayerComparer(PhotonNetwork.player.GetTeam())).ToArray();
		}

		public void CancelSpectateMode()
		{
			if (_spectateMode)
			{
				_spectateMode = false;
				if (PlayerControls.Instance.PlayerController == _spectateController)
				{
					PlayerControls.Instance.PlayerController = null;
				}
				_spectateController = null;
				_players = null;
				_index = -1;
				_cameraCharacterFollower.Stop();
				OnSpectateMode?.Invoke(obj: false);
			}
		}

		private void OnPlayerDestroyed(PlayerController playerController)
		{
			if (_spectateMode && _spectateController == playerController)
			{
				ToSpectateNext();
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			UpdatePlayers();
		}

		private void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
		}
	}
}
