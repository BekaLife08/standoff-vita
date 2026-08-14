using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Messages
{
	public class BoltMessagesService : BoltService<BoltMessagesService>
	{
		public class FriendsEventListener : IFriendsRemoteEventListener
		{
			private readonly BoltMessagesService _service;

			public FriendsEventListener(BoltMessagesService service)
			{
				_service = service;
			}

			public void OnFriendNameChanged(string friendId, string newName)
			{
				if (_service._chats.ContainsKey(friendId))
				{
					BoltChat boltChat = _service._chats[friendId];
					boltChat.Friend.Name = newName;
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}

			public void OnNewFriendshipRequest(PlayerFriend playerFriend)
			{
				if (_service._chats.ContainsKey(playerFriend.Player.Id))
				{
					BoltChat boltChat = _service._chats[playerFriend.Player.Id];
					Axlebolt.Bolt.Friends.RelationshipStatus relationship = EnumMapper<Axlebolt.Bolt.Protobuf.RelationshipStatus, Axlebolt.Bolt.Friends.RelationshipStatus>.ToOriginal(playerFriend.RelationshipStatus);
					boltChat.Friend.Relationship = relationship;
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}

			public void OnRevokeFriendshipRequest(string friendId)
			{
				if (_service._chats.ContainsKey(friendId))
				{
					BoltChat boltChat = _service._chats[friendId];
					boltChat.Friend.Relationship = Axlebolt.Bolt.Friends.RelationshipStatus.None;
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}

			public void OnPlayerStatusChanged(string friendId, Axlebolt.Bolt.Protobuf.PlayerStatus newStatus)
			{
				if (_service._chats.ContainsKey(friendId))
				{
					BoltChat boltChat = _service._chats[friendId];
					boltChat.Friend.PlayerStatus = PlayerStatusMapper.Instance.ToOriginal(newStatus);
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}

			public void OnFriendAdded(PlayerFriend playerFriend)
			{
				if (_service._chats.ContainsKey(playerFriend.Player.Id))
				{
					BoltChat boltChat = _service._chats[playerFriend.Player.Id];
					Axlebolt.Bolt.Friends.RelationshipStatus relationship = EnumMapper<Axlebolt.Bolt.Protobuf.RelationshipStatus, Axlebolt.Bolt.Friends.RelationshipStatus>.ToOriginal(playerFriend.RelationshipStatus);
					boltChat.Friend.Relationship = relationship;
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}

			public void OnFriendRemoved(string friendId)
			{
				if (_service._chats.ContainsKey(friendId))
				{
					BoltChat boltChat = _service._chats[friendId];
					boltChat.Friend.Relationship = Axlebolt.Bolt.Friends.RelationshipStatus.None;
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}

			public void OnFriendAvatarChanged(string friendId, string avatarId)
			{
				if (_service._chats.ContainsKey(friendId))
				{
					BoltChat boltChat = _service._chats[friendId];
					boltChat.Friend.AvatarId = avatarId;
					BoltAvatars.Instance.FillCached(boltChat.Friend);
					_service.ChatMemberUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
			}
		}

		public class MessagesEventListener : IMessagesRemoteEventListener
		{
			private readonly BoltMessagesService _service;

			public MessagesEventListener(BoltMessagesService service)
			{
				_service = service;
			}

			public void OnMsgFromFriend(UserMessage userMessage)
			{
				if (!_service._chats.ContainsKey(userMessage.SenderId))
				{
					PlayerFriend playerFriend = _service._friendsRemoteService.GetPlayerFriend(userMessage.SenderId);
					BoltFriend boltFriend = FriendMapper.Instance.ToOriginal(playerFriend);
					BoltAvatars.Instance.FillCached(boltFriend);
					BoltChat boltChat = new BoltChat(boltFriend, userMessage.Message, userMessage.Timestamp, 1);
					_service._chats.TryAdd(userMessage.SenderId, boltChat);
					_service.IncrementUnreadChatsCount();
					_service.ChatAddedEvent.Invoke(new BoltChatEventArgs(boltChat));
				}
				else
				{
					BoltChat boltChat2 = _service._chats[userMessage.SenderId];
					if (boltChat2.UnreadMsgsCount == 0)
					{
						_service.IncrementUnreadChatsCount();
					}
					boltChat2.Message = userMessage.Message;
					boltChat2.Timestamp = userMessage.Timestamp;
					boltChat2.UnreadMsgsCount++;
					_service.ChatUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat2));
				}
				BoltChat boltChat3 = _service._chats[userMessage.SenderId];
				BoltUserMessage userMessage2 = BoltUserMessageMapper.Instance.ToOriginal(userMessage, boltChat3.Friend);
				BoltUserMessagesEventArgs arg = new BoltUserMessagesEventArgs(userMessage2);
				_service.MessageFromFriendEvent.Invoke(arg);
			}
		}

		private readonly ChatRemoteService _chatRemoteService;

		private readonly FriendsRemoteService _friendsRemoteService;

		private readonly ConcurrentDictionary<string, BoltChat> _chats;

		private readonly FriendsEventListener _remoteFriendsEventListener;

		private readonly MessagesEventListener _remoteMessagesEventListener;

		public readonly BoltEvent<BoltUserMessagesEventArgs> MessageFromFriendEvent = new BoltEvent<BoltUserMessagesEventArgs>();

		public readonly BoltEvent<BoltChatEventArgs> ChatUpdatedEvent = new BoltEvent<BoltChatEventArgs>();

		public readonly BoltEvent<BoltChatEventArgs> ChatMemberUpdatedEvent = new BoltEvent<BoltChatEventArgs>();

		public readonly BoltEvent<UnreadChatsCountChangedEventArgs> UnreadChatsCountChangedEvent = new BoltEvent<UnreadChatsCountChangedEventArgs>();

		public readonly BoltEvent<BoltChatEventArgs> ChatAddedEvent = new BoltEvent<BoltChatEventArgs>();

		private int _unreadChats;

		public int GetUnreaChatsCount()
		{
			return _unreadChats;
		}

		public BoltChat GetFriendChat(string friendId)
		{
			BoltChat value;
			_chats.TryGetValue(friendId, out value);
			return value;
		}

		public BoltMessagesService()
		{
			_chatRemoteService = new ChatRemoteService(BoltApi.Instance.ClientService);
			_friendsRemoteService = new FriendsRemoteService(BoltApi.Instance.ClientService);
			_chats = new ConcurrentDictionary<string, BoltChat>();
			_remoteFriendsEventListener = new FriendsEventListener(this);
			_remoteMessagesEventListener = new MessagesEventListener(this);
		}

		public List<BoltChat> GetChats()
		{
			return _chats.Values.OrderByDescending((BoltChat chat) => chat.Timestamp).ToList();
		}

		internal override void BindEvents()
		{
			BoltApi.Instance.AddEventListener(_remoteFriendsEventListener);
			BoltApi.Instance.AddEventListener(_remoteMessagesEventListener);
		}

		internal override void UnbindEvents()
		{
			BoltApi.Instance.RemoveEventListener(_remoteFriendsEventListener);
			BoltApi.Instance.RemoveEventListener(_remoteMessagesEventListener);
		}

		public Task DeleteFriendMsgs(string friendId)
		{
			if (friendId == null)
			{
				throw new ArgumentNullException("friendId");
			}
			return BoltApi.Instance.Async(delegate
			{
				_chatRemoteService.DeleteFriendMsgs(friendId);
				BoltChat value;
				_chats.TryRemove(friendId, out value);
			});
		}

		public Task SendGroupMsg(string groupId, string msg)
		{
			if (groupId == null)
			{
				throw new ArgumentNullException("groupId");
			}
			if (msg == null)
			{
				throw new ArgumentNullException("msg");
			}
			return BoltApi.Instance.Async(delegate
			{
				_chatRemoteService.SendGroupMsg(groupId, msg);
			});
		}

		public Task<BoltUserMessage[]> GetGroupMsgs(string groupId, int page, int pageSize)
		{
			return BoltApi.Instance.Async(delegate
			{
				UserMessage[] groupMsgs = _chatRemoteService.GetGroupMsgs(groupId, page, pageSize);
				return BoltUserMessageMapper.Instance.ToOriginalList(groupMsgs).ToArray();
			});
		}

		public Task ReadFriendMsgs(string friendId)
		{
			if (friendId == null)
			{
				throw new ArgumentNullException("friendId");
			}
			return BoltApi.Instance.Async(delegate
			{
				if (_chats.ContainsKey(friendId))
				{
					_chatRemoteService.ReadFriendMsgs(friendId);
					DecrementUnreadChatsCount();
					_chats[friendId].UnreadMsgsCount = 0;
					ChatUpdatedEvent.Invoke(new BoltChatEventArgs(_chats[friendId]));
				}
			});
		}

		public Task DeleteGroupMsgs(string groupId)
		{
			if (groupId == null)
			{
				throw new ArgumentNullException("groupId");
			}
			return BoltApi.Instance.Async(delegate
			{
				_chatRemoteService.DeleteGroupMsgs(groupId);
			});
		}

		public Task ReadGroupMsgs(string groupId)
		{
			if (groupId == null)
			{
				throw new ArgumentNullException("groupId");
			}
			return BoltApi.Instance.Async(delegate
			{
				_chatRemoteService.ReadGroupMsgs(groupId);
			});
		}

		public Task<BoltUserMessage[]> GetFriendMsgs(string friendId, int page, int pageSize)
		{
			if (friendId == null)
			{
				throw new ArgumentNullException("friendId");
			}
			return BoltApi.Instance.Async(delegate
			{
				UserMessage[] friendMsgs = _chatRemoteService.GetFriendMsgs(friendId, page, pageSize);
				return friendMsgs.Select((UserMessage message) => message.SenderId.Equals(BoltService<BoltPlayerService>.Instance.Player.Id) ? BoltUserMessageMapper.Instance.ToOriginal(message) : BoltUserMessageMapper.Instance.ToOriginal(message, _chats[message.SenderId].Friend)).ToArray();
			});
		}

		public Task<BoltUserMessage> SendFriendMsg(string friendId, string msg)
		{
			if (friendId == null)
			{
				throw new ArgumentNullException("friendId");
			}
			if (msg == null)
			{
				throw new ArgumentNullException("msg");
			}
			return BoltApi.Instance.Async(delegate
			{
				_chatRemoteService.SendFriendMsg(friendId, msg);
				if (!_chats.ContainsKey(friendId))
				{
					PlayerFriend playerFriend = _friendsRemoteService.GetPlayerFriend(friendId);
					BoltFriend boltFriend = FriendMapper.Instance.ToOriginal(playerFriend);
					BoltAvatars.Instance.FillCached(boltFriend);
					BoltChat boltChat = new BoltChat(boltFriend, msg, GetCurrentTimeMillis(), 0);
					_chats.TryAdd(friendId, boltChat);
					ChatAddedEvent.Invoke(new BoltChatEventArgs(boltChat));
					return new BoltUserMessage
					{
						Message = msg,
						Timestamp = boltChat.Timestamp,
						IsRead = false
					};
				}
				BoltChat boltChat2 = _chats[friendId];
				boltChat2.Message = msg;
				boltChat2.Timestamp = GetCurrentTimeMillis();
				ChatUpdatedEvent.Invoke(new BoltChatEventArgs(boltChat2));
				return new BoltUserMessage
				{
					Message = msg,
					Timestamp = boltChat2.Timestamp,
					IsRead = false
				};
			});
		}

		internal override void Load()
		{
			_chats.Clear();
		}

		private IEnumerable<ChatUser> LoadChats()
		{
			_unreadChats = _chatRemoteService.GetUnreadChatUsersCount();
			return _chatRemoteService.GetChatUsers();
		}

		private void IncrementUnreadChatsCount()
		{
			_unreadChats++;
			UnreadChatsCountChangedEventArgs arg = new UnreadChatsCountChangedEventArgs(_unreadChats);
			UnreadChatsCountChangedEvent.Invoke(arg);
		}

		private void DecrementUnreadChatsCount()
		{
			_unreadChats--;
			UnreadChatsCountChangedEventArgs arg = new UnreadChatsCountChangedEventArgs(_unreadChats);
			UnreadChatsCountChangedEvent.Invoke(arg);
		}

		private static long GetCurrentTimeMillis()
		{
			return (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}
