using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyMembersView : View
	{
		private const int RowCount = 5;

		[SerializeField]
		private LobbyMemberView _lobbyMemberTemplate;

		private ViewPool<LobbyMemberView> _pool;

		private void Awake()
		{
			_pool = new ViewPool<LobbyMemberView>(_lobbyMemberTemplate, 10);
		}

		public void SetLobby(BoltLobby lobby)
		{
			LobbyMemberView[] items = GetItems(lobby);
			int num = 0;
			items[num++].SetMember(lobby.LobbyOwner, lobby.GetMemberBadgeId(lobby.LobbyOwner.Id), true, true);
			BoltFriend[] lobbyMembers = lobby.LobbyMembers;
			foreach (BoltFriend boltFriend in lobbyMembers)
			{
				if (!object.Equals(boltFriend, lobby.LobbyOwner))
				{
					if (num >= items.Length)
					{
						break;
					}
					items[num++].SetMember(boltFriend, lobby.GetMemberBadgeId(boltFriend.Id), false, true);
				}
			}
			BoltFriend[] lobbyInvites = lobby.LobbyInvites;
			foreach (BoltFriend boltFriend2 in lobbyInvites)
			{
				if (num >= items.Length)
				{
					break;
				}
				items[num++].SetMember(boltFriend2, lobby.GetMemberBadgeId(boltFriend2.Id), false, false);
			}
			while (num < items.Length)
			{
				items[num++].SetEmpty();
			}
		}

		public override void Hide()
		{
			base.Hide();
			Clear();
		}

		public void Clear()
		{
			LobbyMemberView[] items = _pool.Items;
			foreach (LobbyMemberView lobbyMemberView in items)
			{
				lobbyMemberView.SetEmpty();
			}
			_pool.Clear();
		}

		private LobbyMemberView[] GetItems(BoltLobby lobby)
		{
			return (lobby.MaxMembers > 5) ? _pool.GetItems(10) : _pool.GetItems(5);
		}
	}
}
