using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Messages;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Bolt;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Messages
{
	public class ChatMessagesToolbar : View, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Text _nameAndStateTemplate;

		[SerializeField]
		private FriendActionPopupMenu _friendActionPopupMenu;

		private Texture _defaultAvatar;

		private string _template;

		private BoltFriend _friend;

		public FriendActionPopupMenu FriendActionPopupMenu
		{
			[CompilerGenerated]
			get
			{
				return _friendActionPopupMenu;
			}
		}

		private void Awake()
		{
			_defaultAvatar = _avatarImage.texture;
			_template = _nameAndStateTemplate.text;
		}

		public void Show(BoltChat chat)
		{
			base.Show();
			_friend = chat.Friend;
			_avatarImage.texture = ((_friend.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(_friend.Avatar));
			string text = ((_friend.OnlineStatus != OnlineStatus.StateOnline) ? string.Empty : _friend.GetLocalizedStatus());
			string arg = ((!(text == string.Empty)) ? string.Empty : _friend.GetLocalizedStatus());
			_nameAndStateTemplate.text = string.Format(_template, _friend.Name, text, arg);
		}

		public override void Hide()
		{
			base.Hide();
			_friend = null;
			_friendActionPopupMenu.Hide();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_friendActionPopupMenu.Show(_friend, eventData.position);
		}
	}
}
