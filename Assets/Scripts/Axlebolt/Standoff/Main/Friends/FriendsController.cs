using System;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends
{
	public class FriendsController : TabController<FriendsController>
	{
		[SerializeField]
		private FriendsTabBar _tabBar;

		[SerializeField]
		private FriendsTabController _friendsController;

		[SerializeField]
		private FindFriendsTabController _findFriendsTabController;

		[SerializeField]
		private RequestsTabController _requestsTabController;

		[SerializeField]
		private BlockedTabController _blockedController;

		private BoltFriendsService _friendsService;

		public override void Init()
		{
			base.Init();
			_friendsService = BoltService<BoltFriendsService>.Instance;
			_friendsService.FriendshipRequestCountChangedEvent.AddListener(OnRequestCountUpdated);
			_tabBar.SetFriendsController(_friendsController);
			_tabBar.SetFindFriendsController(_findFriendsTabController);
			_tabBar.SetRequestController(_requestsTabController);
			_tabBar.SetBlockedController(_blockedController);
		}

		private void OnRequestCountUpdated(FriendshipRequestCountChangedArgs args)
		{
			OnNewNotificationEvent();
		}

		public override void OnOpen()
		{
			OpenFriendsTab();
		}

		public override void CanClose(Action<bool> callback)
		{
			ITabController currentController = _tabBar.CurrentController;
			if (currentController != null)
			{
				currentController.CanClose(callback);
			}
			else
			{
				callback(true);
			}
		}

		public override void OnClose()
		{
			_tabBar.Unselect();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (_friendsService != null)
			{
				_friendsService.FriendshipRequestCountChangedEvent.RemoveListener(OnRequestCountUpdated);
			}
		}

		public override int GetNotificationsCount()
		{
			return _friendsService.GetFriendshipRequestCount();
		}

		public void OpenFriendsTab()
		{
			_tabBar.Select(_friendsController);
		}

		public void OpenRequestsTab()
		{
			_tabBar.Select(_requestsTabController);
		}
	}
}
