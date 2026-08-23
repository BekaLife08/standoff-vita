using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main
{
	public class NotificationView : View
	{
		private const float AnimationDuration = 4f;

		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private TMP_Text _actionText;

		[SerializeField]
		private Button _actionButton;

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private Color _lobbyAndFriendshipColor;

		[SerializeField]
		private Color _newMessageColor;

		private Texture _defaultAvatar;

		public Action ActionHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			_defaultAvatar = _avatarImage.texture;
			_actionButton.onClick.AddListener(delegate
			{
				Hide();
				ActionHandler?.Invoke();
			});
			_closeButton.onClick.AddListener(Hide);
		}

		public void ShowLobbyInvite(BoltLobbyInvite invite)
		{
			Show();
			SetFriend(invite.Friend);
			_actionText.color = _lobbyAndFriendshipColor;
			_actionText.text = ScriptLocalization.Notifications.LobbyInvite;
		}

		public void ShowNewFriendshipRequest(BoltFriend friend)
		{
			Show();
			SetFriend(friend);
			_actionText.color = _lobbyAndFriendshipColor;
			_actionText.text = ScriptLocalization.Notifications.FriendshipRequest;
		}

		public void ShowNewFriendMessage(BoltFriend friend, string message)
		{
			Show();
			SetFriend(friend);
			_actionText.color = _newMessageColor;
			_actionText.text = message;
		}

		public override void Show()
		{
			Hide();
			base.Show();
			base.transform.SetAsLastSibling();
			Invoke("Hide", 4f);
		}

		private void SetFriend(BoltFriend friend)
		{
			_avatarImage.texture = ((friend.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(friend.Avatar));
			_nameText.text = friend.Name;
		}
	}
}
