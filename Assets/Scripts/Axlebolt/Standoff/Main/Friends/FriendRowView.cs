using System;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Bolt;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class FriendRowView : View
	{
		[SerializeField]
		private Text _uidText;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Image _avatarStatusImage;

		[SerializeField]
		private Text _statusText;

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

		[SerializeField]
		private FriendActionButton _primaryAction;

		[SerializeField]
		private FriendActionButton _ignoreAction;

		private Texture _defaultAvatar;

		public Action<PointerEventData> ClickHandler { get; set; }

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

		public virtual void SetFriend(BoltFriend friend)
		{
			_uidText.text = "ID " + friend.Uid;
			_nameText.text = friend.Name;
			_avatarImage.texture = ((friend.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(friend.Avatar));
			_statusText.text = friend.GetLocalizedStatus();
			Color color = ((friend.OnlineStatus != OnlineStatus.StateOnline) ? _offlineColor : _inGameColor);
			_statusText.color = color;
			_avatarStatusImage.color = color;
			if ((object)_primaryAction != null)
			{
				_primaryAction.SetFriend(friend);
			}
			if ((object)_ignoreAction != null)
			{
				_ignoreAction.SetFriend(friend);
			}
		}

		public void SetListener(IFriendActionListener listener)
		{
			if ((object)_primaryAction != null)
			{
				_primaryAction.SetListener(listener);
			}
			if ((object)_ignoreAction != null)
			{
				_ignoreAction.SetListener(listener);
			}
		}
	}
}
