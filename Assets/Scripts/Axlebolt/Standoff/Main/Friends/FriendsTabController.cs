using System;
using System.Linq;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.UI;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class FriendsTabController : TabController<FriendsTabController>
	{
		[SerializeField]
		private Text _inGameText;

		[SerializeField]
		private FriendsScrollView _friendsScrollView;

		[SerializeField]
		private InputField _searchField;

		[SerializeField]
		private Button _searchButton;

		[SerializeField]
		private LobbyController _lobbyController;

		private string _inGameCountPattern;

		private BoltFriendsService _friendsService;

		public override void Init()
		{
			base.Init();
			_friendsService = BoltService<BoltFriendsService>.Instance;
			_lobbyController.Init();
			_inGameCountPattern = _inGameText.text;
			_searchButton.onClick.AddListener(Reload);
		}

		public override void OnOpen()
		{
			_lobbyController.OnOpen();
			_searchField.text = string.Empty;
			ReloadFriendCount();
			Reload();
			_friendsService.FriendAddedEvent.AddListener(OnFriendUpdated);
			_friendsService.FriendRemovedEvent.AddListener(OnFriendUpdated);
			_friendsService.FriendUpdatedEvent.AddListener(OnFriendUpdated);
		}

		public override void CanClose(Action<bool> callback)
		{
			_lobbyController.CanClose(callback);
		}

		public override void OnClose()
		{
			_lobbyController.OnClose();
			_friendsService.FriendAddedEvent.RemoveListener(OnFriendUpdated);
			_friendsService.FriendRemovedEvent.RemoveListener(OnFriendUpdated);
			_friendsService.FriendUpdatedEvent.RemoveListener(OnFriendUpdated);
		}

		private void Reload()
		{
			_friendsScrollView.SetContent(GetFriends());
		}

		private void OnFriendUpdated(BoltFriend friend)
		{
			ReloadFriendCount();
			_friendsScrollView.ReplaceContent(GetFriends());
		}

		private void ReloadFriendCount()
		{
			int inGameFriendsCount = GetInGameFriendsCount();
			int totalFriendsCount = GetTotalFriendsCount();
			string online = ScriptLocalization.PlayerStatus.Online;
			_inGameText.text = string.Format(_inGameCountPattern, inGameFriendsCount, totalFriendsCount, online);
		}

		public BoltFriend[] GetFriends()
		{
			string searchFilter = _searchField.text;
			BoltFriend[] source = _friendsService.GetFriends();
			if (!string.IsNullOrEmpty(searchFilter))
			{
				source = source.Where((BoltFriend friend) => friend.Uid.Equals(searchFilter) || friend.Name.ToLower().Contains(searchFilter.ToLower())).ToArray();
			}
			return source.OrderByDescending((BoltFriend friend) => friend, new FriendComparer()).ToArray();
		}

		public int GetInGameFriendsCount()
		{
			BoltFriend[] friends = _friendsService.GetFriends();
			return friends.Count((BoltFriend friend) => friend.OnlineStatus == OnlineStatus.StateOnline);
		}

		public int GetTotalFriendsCount()
		{
			return _friendsService.GetFriends().Length;
		}
	}
}
