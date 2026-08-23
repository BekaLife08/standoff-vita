using System;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyToolbar : View
	{
		[SerializeField]
		private Text _gameModeText;

		[SerializeField]
		private Text _levelNamesText;

		[SerializeField]
		private Text _lobbyTypeText;

		[SerializeField]
		private Image _lobbyTypeImage;

		private Button _gameModeButton;

		private Button _levelNamesButton;

		private Button _lobbyTypeButton;

		[SerializeField]
		private Sprite _publicTypeSprite;

		[SerializeField]
		private Sprite _friendsTypeSprite;

		[SerializeField]
		private Sprite _privateTypeSprite;

		public Action GameModeHandler { get; set; }

		public Action LevelHandler { get; set; }

		public Action LobbyTypeHandler { get; set; }

		private void Awake()
		{
			_gameModeButton = _gameModeText.GetRequireComponent<Button>();
			_gameModeButton.onClick.AddListener(SelectGameMode);
			_levelNamesButton = _levelNamesText.GetRequireComponent<Button>();
			_levelNamesButton.onClick.AddListener(SelectLevel);
			_lobbyTypeButton = _lobbyTypeText.GetRequireComponent<Button>();
			_lobbyTypeButton.onClick.AddListener(SelectLobbyType);
		}

		private void SelectGameMode()
		{
			Action gameModeHandler = GameModeHandler;
			if (gameModeHandler != null)
			{
				gameModeHandler();
			}
		}

		private void SelectLevel()
		{
			Action levelHandler = LevelHandler;
			if (levelHandler != null)
			{
				levelHandler();
			}
		}

		private void SelectLobbyType()
		{
			Action lobbyTypeHandler = LobbyTypeHandler;
			if (lobbyTypeHandler != null)
			{
				lobbyTypeHandler();
			}
		}

		public void SetOptions([NotNull] string gameMode, [NotNull] string[] levelNames, BoltLobby.LobbyType lobbyType, bool isLobbyOwner)
		{
			if (gameMode == null)
			{
				throw new ArgumentNullException("gameMode");
			}
			if (levelNames == null)
			{
				throw new ArgumentNullException("levelNames");
			}
			_gameModeText.text = gameMode;
			_levelNamesText.text = ((levelNames.Length != 0 && lobbyType != BoltLobby.LobbyType.Public) ? string.Join(", ", levelNames) : ScriptLocalization.Lobby.AllMaps);
			switch (lobbyType)
			{
			case BoltLobby.LobbyType.Public:
				_lobbyTypeText.text = ScriptLocalization.Lobby.PublicLobbyType;
				_lobbyTypeImage.sprite = _publicTypeSprite;
				break;
			case BoltLobby.LobbyType.FriendsOnly:
				_lobbyTypeText.text = ScriptLocalization.Lobby.FriendsOnlyLobbyType;
				_lobbyTypeImage.sprite = _friendsTypeSprite;
				break;
			case BoltLobby.LobbyType.Private:
				_lobbyTypeText.text = ScriptLocalization.Lobby.PrivateLobbyType;
				_lobbyTypeImage.sprite = _privateTypeSprite;
				break;
			default:
				throw new ArgumentOutOfRangeException("lobbyType", lobbyType, null);
			}
			_levelNamesButton.interactable = isLobbyOwner && lobbyType != BoltLobby.LobbyType.Public;
			_lobbyTypeButton.interactable = isLobbyOwner;
		}

		public void SetEmpty()
		{
			_levelNamesText.text = ScriptLocalization.Common.Loading;
			_lobbyTypeText.text = ScriptLocalization.Common.Loading;
		}
	}
}
