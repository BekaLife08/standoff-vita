using System;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyTypeSelectDialog : View
	{
		private Action<BoltLobby.LobbyType> _callback;

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private Button _publicLobbyType;

		[SerializeField]
		private Button _friendsLobbyType;

		[SerializeField]
		private Button _privateLobbyType;

		private void Awake()
		{
			_closeButton.onClick.AddListener(Hide);
			_publicLobbyType.onClick.AddListener(delegate
			{
				HideWithResult(BoltLobby.LobbyType.Public);
			});
			_friendsLobbyType.onClick.AddListener(delegate
			{
				HideWithResult(BoltLobby.LobbyType.FriendsOnly);
			});
			_privateLobbyType.onClick.AddListener(delegate
			{
				HideWithResult(BoltLobby.LobbyType.Private);
			});
		}

		public void Show([NotNull] Action<BoltLobby.LobbyType> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			_callback = callback;
			base.Show();
		}

		private void HideWithResult(BoltLobby.LobbyType lobbyType)
		{
			_callback(lobbyType);
			Hide();
		}
	}
}
