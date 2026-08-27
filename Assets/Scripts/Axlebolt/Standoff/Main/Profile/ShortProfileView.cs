using System;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class ShortProfileView : View
	{
		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Text _idText;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Image _badgeImage;

		private Texture _defaultAvatar;

		public Action NameChangeHandler { get; set; }

		public Action AvatarChangeEvent { get; set; }

		private bool Editable { get; set; }

		private void Awake()
		{
			Editable = false;
			_defaultAvatar = _avatarImage.texture;
			_avatarImage.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				if (Editable)
				{
					Action avatarChangeEvent = AvatarChangeEvent;
					if (avatarChangeEvent != null)
					{
						avatarChangeEvent();
					}
				}
			});
			_nameText.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				if (Editable)
				{
					Action nameChangeHandler = NameChangeHandler;
					if (nameChangeHandler != null)
					{
						nameChangeHandler();
					}
				}
			});
		}

		public void SetPlayer(BoltPlayer player, bool editable)
		{
			Editable = editable;
			_idText.text = player.Uid;
			string displayName = player.Name;
			byte[] displayAvatar = player.Avatar;
			string savedName = PlayerPrefs.GetString("PlayerName", "");
			if (editable && !string.IsNullOrEmpty(savedName))
			{
				displayName = savedName;
			}
			string savedAvatarB64 = PlayerPrefs.GetString("PlayerAvatar", "");
			if (editable && !string.IsNullOrEmpty(savedAvatarB64))
			{
				try { displayAvatar = System.Convert.FromBase64String(savedAvatarB64); } catch {}
			}
			_nameText.text = displayName;
			_avatarImage.texture = ((displayAvatar == null) ? _defaultAvatar : TextureUtility.FromBytes(displayAvatar));
			if (Editable)
			{
				_badgeImage.enabled = Singleton<InventoryManager>.Instance.GetMainBadgeId() != InventoryItemId.None;
				if (_badgeImage.enabled)
				{
					_badgeImage.sprite = Singleton<InventoryManager>.Instance.GetMainBadgeDefinition().Sprite;
				}
			}
		}
	}
}
