using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Friends
{
	public class BoltFriendsService : BoltService<BoltFriendsService>
	{
		public class FriendsEventListener : IFriendsRemoteEventListener
		{
			private readonly BoltFriendsService _service;

			public FriendsEventListener(BoltFriendsService service)
			{
				_service = service;
			}

			public void OnFriendNameChanged(string friendId, string newName)
			{
				if (_service._friends.ContainsKey(friendId))
				{
					_service._friends[friendId].Name = newName;
					NameChangedEventArgs arg = new NameChangedEventArgs(friendId, newName);
					_service.FriendNameChangedEvent.Invoke(arg);
					_service.FriendUpdatedEvent.Invoke(_service._friends[friendId]);
				}
			}

			public void OnNewFriendshipRequest(PlayerFriend playerFriend)
			{
				BoltFriend boltFriend = FriendMapper.Instance.ToOriginal(playerFriend);
				BoltAvatars.Instance.Fill(boltFriend);
				_service.AddFriendshipRequestId(playerFriend.Player.Id);
				_service.NewFriendshipRequestEvent.Invoke(boltFriend);
			}

			public void OnRevokeFriendshipRequest(string friendId)
			{
				_service.RevokeFriendshipEvent.Invoke(friendId);
				_service.RemoveFriendshipRequestId(friendId);
			}

			public void OnPlayerStatusChanged(string friendId, Axlebolt.Bolt.Protobuf.PlayerStatus newStatus)
			{
				if (_service._friends.ContainsKey(friendId))
				{
					BoltFriend boltFriend = _service._friends[friendId];
					boltFriend.PlayerStatus = PlayerStatusMapper.Instance.ToOriginal(newStatus);
					PlayerStatusEventArgs arg = new PlayerStatusEventArgs
					{
						NewPlayerStatus = PlayerStatusMapper.Instance.ToOriginal(newStatus),
						Friend = boltFriend
					};
					_service.FriendOnlineStatusChangeEvent.Invoke(arg);
					_service.FriendUpdatedEvent.Invoke(boltFriend);
				}
			}

			public void OnFriendAdded(PlayerFriend playerFriend)
			{
				BoltFriend boltFriend = FriendMapper.Instance.ToOriginal(playerFriend);
				BoltAvatars.Instance.Fill(boltFriend);
				_service._friends.TryAdd(boltFriend.Id, boltFriend);
				_service.FriendAddedEvent.Invoke(boltFriend);
			}

			public void OnFriendRemoved(string friendId)
			{
				_service.FriendRemovedEvent.Invoke(_service._friends[friendId]);
				BoltFriend value;
				_service._friends.TryRemove(friendId, out value);
			}

			public void OnFriendAvatarChanged(string friendId, string avatarId)
			{
				if (_service._friends.ContainsKey(friendId))
				{
					BoltFriend boltFriend = _service._friends[friendId];
					boltFriend.AvatarId = avatarId;
					BoltAvatars.Instance.FillCached(boltFriend);
					AvatarEventArgs arg = new AvatarEventArgs(friendId, avatarId);
					_service.FriendAvatarChangeEvent.Invoke(arg);
					_service.FriendUpdatedEvent.Invoke(_service._friends[friendId]);
				}
			}

			public void OnMsgFromFriend(string friendId, string textMessage)
			{
			}
		}

		private readonly FriendsRemoteService _friendsRemoteService;

		private readonly ConcurrentDictionary<string, BoltFriend> _friends;

		public readonly BoltEvent<NameChangedEventArgs> FriendNameChangedEvent = new BoltEvent<NameChangedEventArgs>();

		public readonly BoltEvent<PlayerStatusEventArgs> FriendOnlineStatusChangeEvent = new BoltEvent<PlayerStatusEventArgs>();

		public readonly BoltEvent<AvatarEventArgs> FriendAvatarChangeEvent = new BoltEvent<AvatarEventArgs>();

		public readonly BoltEvent<BoltFriend> FriendUpdatedEvent = new BoltEvent<BoltFriend>();

		public readonly BoltEvent<BoltFriend> FriendAddedEvent = new BoltEvent<BoltFriend>();

		public readonly BoltEvent<BoltFriend> FriendRemovedEvent = new BoltEvent<BoltFriend>();

		public readonly BoltEvent<BoltFriend> NewFriendshipRequestEvent = new BoltEvent<BoltFriend>();

		public readonly BoltEvent<string> RevokeFriendshipEvent = new BoltEvent<string>();

		public readonly BoltEvent<FriendshipRequestCountChangedArgs> FriendshipRequestCountChangedEvent = new BoltEvent<FriendshipRequestCountChangedArgs>();

		private HashSet<string> _friendshipRequestIds;

		private readonly FriendsEventListener _remoteEventListener;

		public BoltFriendsService()
		{
			_friends = new ConcurrentDictionary<string, BoltFriend>();
			_friendsRemoteService = new FriendsRemoteService(BoltApi.Instance.ClientService);
			_remoteEventListener = new FriendsEventListener(this);
		}

		public int GetFriendshipRequestCount()
		{
			return _friendshipRequestIds.Count;
		}

		internal override void BindEvents()
		{
			BoltApi.Instance.AddEventListener(_remoteEventListener);
		}

		internal override void UnbindEvents()
		{
			BoltApi.Instance.RemoveEventListener(_remoteEventListener);
		}

		internal override void Load()
		{
			_friends.Clear();
			BoltAvatars.Instance.FillCached(_friends.Values.ToArray());
		}

		private IEnumerable<PlayerFriend> LoadFriends()
		{
			string[] playerFriendsIds = _friendsRemoteService.GetPlayerFriendsIds(new Axlebolt.Bolt.Protobuf.RelationshipStatus[1] { Axlebolt.Bolt.Protobuf.RelationshipStatus.RequestInitiator });
			_friendshipRequestIds = new HashSet<string>(playerFriendsIds);
			return _friendsRemoteService.GetPlayerFriends(new Axlebolt.Bolt.Protobuf.RelationshipStatus[1] { Axlebolt.Bolt.Protobuf.RelationshipStatus.Friend }, 0, int.MaxValue);
		}

		public Task<BoltFriend[]> GetFriendsByStatus(RelationshipStatus[] relationshipStatuses, int page, int size)
		{
			if (relationshipStatuses == null)
			{
				throw new ArgumentNullException("relationshipStatuses");
			}
			return BoltApi.Instance.Async(delegate
			{
				return new BoltFriend[0];
			});
		}

		public Task<long> GetPlayerFriendsCount(RelationshipStatus[] relationshipStatuses)
		{
			if (relationshipStatuses == null)
			{
				throw new ArgumentNullException("relationshipStatuses");
			}
			return BoltApi.Instance.Async(() => { return 0l;  });
		}

		public BoltFriend[] GetFriends()
		{
			return _friends.Values.ToArray();
		}

		public BoltFriend GetFriend(string id)
		{
			BoltFriend value;
			_friends.TryGetValue(id, out value);
			return value;
		}

		public Task SendFriendRequest(string friendId)
		{
			return BoltApi.Instance.Async(delegate
			{
				
			});
		}

		public Task SendFriendRequest(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task RevokeFriendRequest(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task AcceptFriendRequest(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task IgnoreFriendRequest(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task BlockFriend(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task UnblockFriend(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task RemoveFriend(BoltFriend friend)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task<BoltFriend[]> SearchFriends(string friendIdOrName, int page, int size)
		{
			if (friendIdOrName == null)
			{
				throw new ArgumentNullException("friendIdOrName");
			}
			return BoltApi.Instance.Async(delegate
			{
				return new BoltFriend[0];
			});
		}

		public Task<long> GetSearchFriendsCount(string friendIdOrName)
		{
			if (friendIdOrName == null)
			{
				throw new ArgumentNullException("friendIdOrName");
			}
			return BoltApi.Instance.Async(() => { return 0l; });
		}

		private void RemoveFriendshipRequestId(string friendId)
		{
			if (_friendshipRequestIds.Remove(friendId))
			{
				FriendshipRequestCountChangedArgs arg = new FriendshipRequestCountChangedArgs(_friendshipRequestIds.Count);
				FriendshipRequestCountChangedEvent.Invoke(arg);
			}
		}

		private void AddFriendshipRequestId(string friendId)
		{
			if (_friendshipRequestIds.Add(friendId))
			{
				FriendshipRequestCountChangedArgs arg = new FriendshipRequestCountChangedArgs(_friendshipRequestIds.Count);
				FriendshipRequestCountChangedEvent.Invoke(arg);
			}
		}

		private void InvokeOnFriendAddedEvent(BoltFriend friend)
		{
			if (_friends.TryAdd(friend.Id, friend))
			{
				FriendAddedEvent.Invoke(friend);
			}
		}

		private void InvokeFriendRemovedEvent(BoltFriend friend)
		{
			BoltFriend value;
			if (_friends.TryRemove(friend.Id, out value))
			{
				FriendRemovedEvent.Invoke(friend);
			}
		}
	}
}
