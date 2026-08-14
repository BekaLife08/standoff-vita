using System;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyMemberView : View, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Image _lobbyLeaderGlowImage;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Color _waitAvatarColor;

		[SerializeField]
		private Color _waitNameColor;

		[SerializeField]
		private Image _badgeImage;

		[SerializeField]
		private FriendActionPopupMenu _lobbyMemberPopup;

		private Texture _defaultAvatar;

		private Color _defaultAvatarColor;

		private Color _defaultNameColor;

		public BoltFriend Friend { get; private set; }

		private void Awake()
		{
			_defaultAvatar = _avatarImage.mainTexture;
			_defaultAvatarColor = _avatarImage.color;
			_defaultNameColor = _nameText.color;
		}

		public void SetMember([NotNull] BoltFriend member, InventoryItemId id, bool isOwner, bool isInLobby)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			Friend = member;
			_avatarImage.gameObject.SetActive(true);
			_avatarImage.texture = ((member.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(member.Avatar));
			_lobbyLeaderGlowImage.gameObject.SetActive(isOwner);
			_nameText.text = member.Name;
			_avatarImage.color = (isInLobby ? _defaultAvatarColor : _waitAvatarColor);
			_nameText.color = (isInLobby ? _defaultNameColor : _waitNameColor);
			_badgeImage.enabled = id != InventoryItemId.None;
			if (_badgeImage.enabled)
			{
				_badgeImage.sprite = Singleton<InventoryManager>.Instance.GetBadgeItemDefinition(id).Sprite;
			}
		}

		public void SetEmpty()
		{
			Friend = null;
			_avatarImage.gameObject.SetActive(false);
			_lobbyLeaderGlowImage.gameObject.SetActive(false);
			_nameText.text = string.Empty;
			_badgeImage.enabled = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (Friend != null)
			{
				_lobbyMemberPopup.Show(Friend, eventData.position);
			}
		}
	}
}
