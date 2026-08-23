using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends
{
	public class FriendsTabBar : TabBar
	{
		[SerializeField]
		[NotNull]
		private TabButton _friendsButton;

		[NotNull]
		[SerializeField]
		private TabButton _findFriendButton;

		[NotNull]
		[SerializeField]
		private TabButton _requestsButton;

		[SerializeField]
		[NotNull]
		private TabButton _blockedButton;

		public void SetFriendsController(FriendsTabController controller)
		{
			Add(_friendsButton, controller);
		}

		public void SetFindFriendsController(FindFriendsTabController controller)
		{
			Add(_findFriendButton, controller);
		}

		public void SetRequestController(RequestsTabController controller)
		{
			Add(_requestsButton, controller);
		}

		public void SetBlockedController(BlockedTabController controller)
		{
			Add(_blockedButton, controller);
		}
	}
}
