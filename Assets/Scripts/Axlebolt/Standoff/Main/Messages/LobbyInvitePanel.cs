using System;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Messages
{
	public class LobbyInvitePanel : View
	{
		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Button _acceptButton;

		[SerializeField]
		private Button _rejectButton;

		private Texture _defaultAvatar;

		public Action AcceptActionHandler { get; set; }

		public Action RejectActionHandler { get; set; }

		private void Awake()
		{
			_acceptButton.onClick.AddListener(delegate
			{
				Action acceptActionHandler = AcceptActionHandler;
				if (acceptActionHandler != null)
				{
					acceptActionHandler();
				}
			});
			_rejectButton.onClick.AddListener(delegate
			{
				Action rejectActionHandler = RejectActionHandler;
				if (rejectActionHandler != null)
				{
					rejectActionHandler();
				}
			});
		}

		public void SetInvite(BoltLobbyInvite invite)
		{
			_nameText.text = invite.Friend.Name;
		}
	}
}
