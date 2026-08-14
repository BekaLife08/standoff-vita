using Axlebolt.Standoff.Main.Clan;
using Axlebolt.Standoff.Main.Friends;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Main.Messages;
using Axlebolt.Standoff.Main.Play;
using Axlebolt.Standoff.Main.Profile;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Sidebar
{
	public class Sidebar : TabBar
	{
		[SerializeField]
		private TabButton _playButton;

		[SerializeField]
		private TabButton _profileButton;

		[SerializeField]
		private TabButton _inventoryButton;

		[SerializeField]
		private TabButton _clanButton;

		[SerializeField]
		private TabButton _friendsButton;

		[SerializeField]
		private TabButton _settingsButton;

		[SerializeField]
		private TabButton _messagesButton;

		public void SetPlayController(PlayController playController)
		{
			Add(_playButton, playController);
		}

		public void SetProfileController(ProfileController profileController)
		{
			Add(_profileButton, profileController);
		}

		public void SetInventoryController(InventoryController inventoryController)
		{
			Add(_inventoryButton, inventoryController);
		}

		public void SetClanController(ClanController clanController)
		{
			Add(_clanButton, clanController);
		}

		public void SetFriendController(FriendsController friendsController)
		{
			Add(_friendsButton, friendsController);
		}

		public void SetMessagesController(MessagesController messagesController)
		{
			Add(_messagesButton, messagesController);
		}

		public void SetSettingsController(SettingsController settingsController)
		{
			Add(_settingsButton, settingsController);
		}
	}
}
