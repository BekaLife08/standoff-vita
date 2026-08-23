using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Messages
{
	public class ChatRowView : View
	{
		[SerializeField]
		private TMP_Text _nameText;

		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Image _avatarStatusImage;

		[SerializeField]
		private TMP_Text _lastMessageText;

		[SerializeField]
		private Color _oldMessageColor;

		[SerializeField]
		private Color _inviteMessageColor;

		[SerializeField]
		private Color _newMessageColor;

		[SerializeField]
		private Text _dateTimeText;

		[SerializeField]
		private Text _unreadMessageCount;

		[SerializeField]
		private Image _backgroundImage;

		[SerializeField]
		private Color _selectedColor;

		[SerializeField]
		private Color _unselectedColor;

		[SerializeField]
		private Color _inGameColor;

		[SerializeField]
		private Color _offlineColor;

		private Texture _defaultAvatar;

		public bool Selected
		{
			set
			{
				_backgroundImage.color = ((!value) ? _unselectedColor : _selectedColor);
			}
		}

		protected virtual void Awake()
		{
			Selected = false;
			_defaultAvatar = _avatarImage.texture;
		}

		public virtual void SetChat(ChatModel chat)
		{
			BoltFriend friend = chat.Friend;
			_nameText.text = friend.Name;
			_avatarImage.texture = ((friend.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(friend.Avatar));
			_lastMessageText.text = ((chat.LobbyInvite != null) ? ScriptLocalization.Notifications.LobbyInvite : chat.Message);
			_avatarStatusImage.color = ((friend.OnlineStatus != OnlineStatus.StateOnline) ? _offlineColor : _inGameColor);
			int num = chat.UnreadMsgsCount + (chat.HasLobbyInvite ? 1 : 0);
			_lastMessageText.color = (chat.HasLobbyInvite ? _inviteMessageColor : ((num <= 0) ? _oldMessageColor : _newMessageColor));
			_unreadMessageCount.transform.parent.gameObject.SetActive(num > 0);
			_unreadMessageCount.text = ((num != 0) ? num.ToString() : string.Empty);
			_dateTimeText.text = ChatUtility.LocalizeChatTimestamp(chat.Timestamp);
		}
	}
}
