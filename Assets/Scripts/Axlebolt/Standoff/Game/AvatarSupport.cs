using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.Event;
using Axlebolt.Standoff.Utils;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class AvatarSupport : Singleton<AvatarSupport>
	{
		private static readonly Log Log = Log.Create(typeof(AvatarSupport));

		public const int AvatarSize = 96;

		public const int Width = 2048;

		public const int PlayerLimit = 18;

		private int _maxPlayer;

		private Texture2D _texture2D;

		private Sprite _emptySprite;

		private readonly Queue<Sprite> _sprites = new Queue<Sprite>();

		private readonly Dictionary<int, Sprite> _playerAvatars = new Dictionary<int, Sprite>();

		private AvatarConfig _config;

		public void Init(int maxPlayers)
		{
			if (_config != null)
			{
				throw new InvalidOperationException(string.Format("{0} already initialized", "AvatarSupport"));
			}
			if (maxPlayers > 18)
			{
				throw new ArgumentException(string.Format("{0} more than {1}", "maxPlayers", 18));
			}
			_config = ResourcesUtility.Load<AvatarConfig>("SceneObjects/AvatarsConfig");
			_maxPlayer = maxPlayers;
			BindEvents();
			InitAvatars(maxPlayers);
		}

		private void BindEvents()
		{
			GameController.Instance.PlayerConnectedEvent.AddListener(PlayerConnected);
			GameController.Instance.PlayerDisconnectedEvent.AddListener(PlayerDisconnected);
			GameController.Instance.PlayerPropertiesChangedEvent.AddListener(PlayerPropertiesChanged);
		}

		public void InitAvatars(int maxPlayers)
		{
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			_texture2D = new Texture2D(2048, 128, TextureFormat.RGB24, false);
			_emptySprite = CreateSprite(0);
			for (int i = 1; i < maxPlayers + 2; i++)
			{
				Sprite item = CreateSprite(i);
				_sprites.Enqueue(item);
			}
			CopyToAtlas(_config.EmptyAvatar, _emptySprite);
			PhotonPlayer[] array = playerList;
			foreach (PhotonPlayer player in array)
			{
				CreateAvatar(player);
			}
		}

		private Sprite CreateSprite(int index)
		{
			return Sprite.Create(_texture2D, new Rect(index * 96, 0f, 96f, 96f), new Vector2(0.5f, 0.5f));
		}

		public void PlayerConnected(PhotonPlayer player)
		{
			CreateAvatar(player);
		}

		public void PlayerPropertiesChanged(PlayerPropertiesEventArg args)
		{
			if (!_playerAvatars.ContainsKey(args.Player.ID) && args.Contains("avatar"))
			{
				CreateAvatar(args.Player);
			}
		}

		private void CreateAvatar(PhotonPlayer player)
		{
			byte[] avatar = player.GetAvatar();
			if (avatar != null && avatar.Length != 0 && !_playerAvatars.ContainsKey(player.ID))
			{
				if (_sprites.Count == 0)
				{
					Log.Error($"Avatar atlas limit exceeded, maxPlayerCount {_maxPlayer}");
					return;
				}
				Texture2D avatarTexture = TextureUtility.FromBytes(player.GetAvatar());
				Sprite sprite = _sprites.Dequeue();
				_playerAvatars[player.ID] = sprite;
				CopyToAtlas(avatarTexture, sprite);
			}
		}

		private void CopyToAtlas(Texture2D avatarTexture, Sprite sprite)
		{
			Color[] pixels = avatarTexture.GetPixels();
			_texture2D.SetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height, pixels);
			_texture2D.Apply();
		}

		public void PlayerDisconnected(PhotonPlayer player)
		{
			int iD = player.ID;
			if (_playerAvatars.ContainsKey(iD))
			{
				_sprites.Enqueue(_playerAvatars[iD]);
				_playerAvatars.Remove(iD);
			}
		}

		public static void SetPlayerAvatar(byte[] avatar)
		{
			if (avatar != null && avatar.Length != 0)
			{
				try
				{
					Texture2D tex = TextureUtility.FromBytes(avatar);
					TextureUtility.Bilinear(tex, 96, 96);
					PhotonNetwork.player.SetAvatar(tex.EncodeToJPG());
				}
				catch (Exception message)
				{
					Log.Error(message);
				}
			}
		}

		[NotNull]
		public static Sprite GetAvatar([NotNull] PhotonPlayer player)
		{
			return Singleton<AvatarSupport>.Instance.GetPlayerAvatar(player);
		}

		[NotNull]
		public Sprite GetPlayerAvatar([NotNull] PhotonPlayer player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			return (!_playerAvatars.ContainsKey(player.ID)) ? _emptySprite : _playerAvatars[player.ID];
		}
	}
}
