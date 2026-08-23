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
			_nameText.text = player.Name;
			_avatarImage.texture = ((player.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(player.Avatar));
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
