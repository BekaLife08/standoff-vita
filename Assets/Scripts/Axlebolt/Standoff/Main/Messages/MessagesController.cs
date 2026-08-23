using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Api.Exception;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Messages;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Friends;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Messages
{
	public class MessagesController : TabController<MessagesController>, IAsyncDataProvider<BoltUserMessage>, IFriendActionListener
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSendMessageToChat_003Ec__async0 : IAsyncStateMachine
		{
			internal string msg;

			internal BoltUserMessage _003Cmessage_003E__1;

			internal MessagesController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<BoltUserMessage> _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						num = 4294967293u;
						break;
					case 1u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_0024awaiter0 = _0024this._messagesService.SendFriendMsg(_0024this._selectedChat.Friend.Id, msg).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						case 1u:
							break;
						}
						_003Cmessage_003E__1 = _0024awaiter0.GetResult();
						if (_0024this._lastMessageChatId != _0024this._selectedChat.Id)
						{
							_0024this.Reload();
						}
						_0024this._lastMessageChatId = _0024this._selectedChat.Id;
						_0024this._messagesView.AddNewMessage(_003Cmessage_003E__1);
					}
					catch (BlockedPlayerRpcException)
					{
						_0024this._messagesView.AddErrorMessage(ScriptLocalization.Friends.BlockedException);
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						_0024this._messagesView.AddErrorMessage(ScriptLocalization.Common.Error + " " + ex2.Message);
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLoadData_003Ec__async1 : IAsyncStateMachine
		{
			internal int page;

			internal int size;

			internal BoltUserMessage[] _003Cresult_003E__1;

			internal Action<BoltUserMessage[]> success;

			internal Action<Exception> failed;

			internal MessagesController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<BoltUserMessage[]> _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						num = 4294967293u;
						break;
					case 1u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_0024awaiter0 = _0024this._messagesService.GetFriendMsgs(_0024this._selectedChat.Friend.Id, page, size).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						case 1u:
							break;
						}
						_003Cresult_003E__1 = _0024awaiter0.GetResult();
						success(_003Cresult_003E__1);
					}
					catch (Exception ex)
					{
						Log.Error(ex);
						failed(ex);
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInit_003Ec__async2 : IAsyncStateMachine
		{
			internal MessagesController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						_0024awaiter0 = LobbyUtility.JoinLobby(_0024this._selectedChat.LobbyInvite.LobbyId).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
					if (_0024this._matchmakingService.IsInLobby())
					{
						_0024this.Close();
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInit_003Ec__async3 : IAsyncStateMachine
		{
			internal MessagesController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						_0024awaiter0 = LobbyUtility.RejectInvite(_0024this._selectedChat.LobbyInvite.LobbyId).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		private static readonly Log Log = Log.Create(typeof(MessagesController));

		[SerializeField]
		private ChatScrollView _chatScrollView;

		[SerializeField]
		private ChatMessagesView _messagesView;

		[SerializeField]
		private View _emptyMessagesView;

		[SerializeField]
		private Button _closeButton;

		private BoltFriendsService _friendService;

		private BoltMessagesService _messagesService;

		private BoltMatchmakingService _matchmakingService;

		private BoltFriend _openWithFriend;

		private ChatModel _selectedChat;

		private string _lastMessageChatId;

		public override bool IsWindowLayout()
		{
			return true;
		}

		public override void Init()
		{
			base.Init();
			_friendService = BoltService<BoltFriendsService>.Instance;
			_messagesService = BoltService<BoltMessagesService>.Instance;
			_matchmakingService = BoltService<BoltMatchmakingService>.Instance;
			_chatScrollView.ItemSelectedHandler = OnChatSelected;
			_messagesView.Init(this, 20);
			_closeButton.onClick.AddListener(Close);
			_messagesView.SetActionListener(this);
			_messagesView.LobbyInvitePanel.AcceptActionHandler = async () =>
			{
				await LobbyUtility.JoinLobby(_selectedChat.LobbyInvite.LobbyId);
				if (_matchmakingService.IsInLobby())
				{
					Close();
				}
			};
			_messagesView.LobbyInvitePanel.RejectActionHandler = async () =>
			{
				await LobbyUtility.RejectInvite(_selectedChat.LobbyInvite.LobbyId);
			};
			_matchmakingService.IncomingInvitesChangedEvent.AddListener(OnNewNotificationEvent);
			_messagesService.UnreadChatsCountChangedEvent.AddListener(OnUnreadChatsCountChanged);
		}

		public override int GetNotificationsCount()
		{
			return _messagesService.GetUnreaChatsCount() + _matchmakingService.LobbyInvitesIncoming.Count;
		}

		public override void OnOpen()
		{
			_matchmakingService.IncomingInvitesChangedEvent.AddListener(LobbyInvitesChangedEvent);
			_messagesService.MessageFromFriendEvent.AddListener(OnMessageFromFriend);
			_messagesService.ChatUpdatedEvent.AddListener(OnChatUpdatedEvent);
			_messagesService.ChatAddedEvent.AddListener(OnChatAddedEvent);
			_messagesService.ChatMemberUpdatedEvent.AddListener(OnChatMemberUpdateEvent);
			_emptyMessagesView.Show();
			_messagesView.Hide();
			Reload();
		}

		public override void OnClose()
		{
			_chatScrollView.SetContent(new ChatModel[0]);
			_messagesView.Hide();
			_openWithFriend = null;
			_selectedChat = null;
			_lastMessageChatId = null;
			_matchmakingService.IncomingInvitesChangedEvent.RemoveListener(LobbyInvitesChangedEvent);
			_messagesService.MessageFromFriendEvent.RemoveListener(OnMessageFromFriend);
			_messagesService.ChatUpdatedEvent.RemoveListener(OnChatUpdatedEvent);
			_messagesService.ChatAddedEvent.RemoveListener(OnChatAddedEvent);
			_messagesService.ChatMemberUpdatedEvent.RemoveListener(OnChatMemberUpdateEvent);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (_matchmakingService != null)
			{
				_matchmakingService.IncomingInvitesChangedEvent.RemoveListener(OnNewNotificationEvent);
			}
			if (_messagesService != null)
			{
				_messagesService.UnreadChatsCountChangedEvent.RemoveListener(OnUnreadChatsCountChanged);
			}
		}

		private void OnUnreadChatsCountChanged(UnreadChatsCountChangedEventArgs obj)
		{
			OnNewNotificationEvent();
		}

		private void LobbyInvitesChangedEvent()
		{
			Reload();
			UpdateSelectedChat();
		}

		private void OnChatAddedEvent(BoltChatEventArgs args)
		{
			Log.Debug("OnChatAddedEvent event");
			Reload();
		}

		private void OnChatUpdatedEvent(BoltChatEventArgs args)
		{
			Log.Debug("OnChatUpdated event");
			ChatModel chatModel = ToChatModel(args.BoltChat);
			if (object.Equals(chatModel, _selectedChat))
			{
				_selectedChat = chatModel;
			}
			_chatScrollView.UpdateView(chatModel);
		}

		private void OnChatMemberUpdateEvent(BoltChatEventArgs args)
		{
			Log.Debug("OnChatMemberUpdateEvent event");
			ChatModel chatModel = ToChatModel(args.BoltChat);
			if (object.Equals(chatModel, _selectedChat))
			{
				_selectedChat = chatModel;
				UpdateSelectedChat();
			}
			_chatScrollView.UpdateView(chatModel);
		}

		private void Reload()
		{
			Log.Debug("Reload ChatScrollView");
			List<ChatModel> chats = GetChats();
			_chatScrollView.SetContent(chats.ToArray());
			if (_selectedChat != null)
			{
				_selectedChat = chats.Find((ChatModel chat) => chat.Id == _selectedChat.Id);
			}
			int selectedIndex = GetSelectedIndex(chats);
			if (selectedIndex >= 0)
			{
				_chatScrollView.SetSelectedRowIndex(selectedIndex);
			}
		}

		private int GetSelectedIndex(List<ChatModel> chats)
		{
			if (_selectedChat != null)
			{
				return chats.IndexOf(_selectedChat);
			}
			if (_openWithFriend != null)
			{
				return 0;
			}
			return -1;
		}

		public void OpenWith(string friendId)
		{
			BoltFriend friend = _friendService.GetFriend(friendId);
			if (friend != null)
			{
				OpenWith(friend);
			}
			else
			{
				Open();
			}
		}

		public void OpenWith(BoltFriend friend)
		{
			_openWithFriend = friend;
			Open();
		}

		private void OnChatSelected(ChatModel chatModel)
		{
			if (!object.Equals(_selectedChat, chatModel))
			{
				_selectedChat = chatModel;
				UpdateSelectedChat();
			}
		}

		private void UpdateSelectedChat()
		{
			if (_selectedChat != null)
			{
				_messagesView.SendMessageHandler = SendMessageToChat;
				_emptyMessagesView.Hide();
				_messagesView.Show(_selectedChat);
				if (_selectedChat.UnreadMsgsCount > 0)
				{
					_messagesService.ReadFriendMsgs(_selectedChat.Friend.Id);
				}
			}
		}

		private void OnMessageFromFriend(BoltUserMessagesEventArgs args)
		{
			BoltUserMessage boltUserMessage = args.BoltUserMessage;
			if (_lastMessageChatId != boltUserMessage.Sender.Id)
			{
				Reload();
			}
			_lastMessageChatId = boltUserMessage.Sender.Id;
			if (_selectedChat != null && !(_selectedChat.Friend.Id != boltUserMessage.Sender.Id))
			{
				_messagesView.AddNewMessage(boltUserMessage);
				_messagesService.ReadFriendMsgs(_selectedChat.Friend.Id);
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CSendMessageToChat_003Ec__async0))]
		private void SendMessageToChat(string msg)
		{
			_003CSendMessageToChat_003Ec__async0 stateMachine = default(_003CSendMessageToChat_003Ec__async0);
			stateMachine.msg = msg;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private List<ChatModel> GetChats()
		{
			List<ChatModel> list = new List<ChatModel>();
			if (_openWithFriend != null)
			{
				list.Add(ToChatModel(GetFriendChat(_openWithFriend)));
			}
			list.AddRange(from chat in GetLobbyInvites()
				where !list.Contains(chat)
				select chat);
			IEnumerable<ChatModel> collection = from chat in _messagesService.GetChats().Select(ToChatModel)
				where !list.Contains(chat)
				select chat;
			list.AddRange(collection);
			return list;
		}

		private IEnumerable<ChatModel> GetLobbyInvites()
		{
			return _matchmakingService.LobbyInvitesIncoming.Select((BoltLobbyInvite invite) => new ChatModel(GetFriendChat(invite.Friend), invite));
		}

		private BoltChat GetFriendChat(BoltFriend friend)
		{
			return _messagesService.GetFriendChat(friend.Id) ?? new BoltChat(friend, string.Empty, ChatUtility.GetCurrentTimeMillis(), 0);
		}

		private ChatModel ToChatModel(BoltChat chat)
		{
			BoltLobbyInvite lobbyInvite = _matchmakingService.GetLobbyInvite(chat.Friend.Id);
			return new ChatModel(chat, lobbyInvite);
		}

		public void LoadCount(Action<int> success, Action<Exception> failed)
		{
		}

		[AsyncStateMachine(typeof(_003CLoadData_003Ec__async1))]
		[DebuggerStepThrough]
		public void LoadData(int page, int size, Action<BoltUserMessage[]> success, Action<Exception> failed)
		{
			_003CLoadData_003Ec__async1 stateMachine = default(_003CLoadData_003Ec__async1);
			stateMachine.page = page;
			stateMachine.size = size;
			stateMachine.success = success;
			stateMachine.failed = failed;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public void OnFriendActionExecuted(FriendActionId friendAction, BoltFriend friend)
		{
			switch (friendAction)
			{
			case FriendActionId.InviteFriendToLobby:
				if (!_matchmakingService.IsInLobby())
				{
					break;
				}
				goto case FriendActionId.JoinGameOrLobby;
			case FriendActionId.JoinGameOrLobby:
				Close();
				break;
			}
		}

		public bool IsFriendChatOpened(string friendId)
		{
			int result;
			if (_selectedChat != null)
			{
				BoltFriend friend = _selectedChat.Friend;
				result = ((((friend != null) ? friend.Id : null) == friendId) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Close();
			}
		}
	}
}
