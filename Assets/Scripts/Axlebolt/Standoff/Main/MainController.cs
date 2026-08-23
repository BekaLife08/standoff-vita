using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Matchmaking.Events;
using Axlebolt.Bolt.Messages;
using Axlebolt.Bolt.Unity;
using Axlebolt.Standoff.Analytics;
using Axlebolt.Standoff.Bolt;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Clan;
using Axlebolt.Standoff.Main.Friends;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Main.Messages;
using Axlebolt.Standoff.Main.Play;
using Axlebolt.Standoff.Main.Profile;
using Axlebolt.Standoff.Main.Sidebar;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.UI;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Main
{
	public class MainController : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInitNotifications_003Ec__async1 : IAsyncStateMachine
		{
			internal BoltNotification _003Cnotification_003E__1;

			internal MainController _0024this;

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
						_0024this._notificationView.Hide();
						if (!BoltUnityApi.IsLaunchedViaNotification())
						{
							break;
						}
						_003Cnotification_003E__1 = BoltUnityApi.GetNotification();
						if (_003Cnotification_003E__1.IsLobbyInvite())
						{
							_0024awaiter0 = LobbyUtility.JoinLobby(_003Cnotification_003E__1.LobbyId).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
						if (_003Cnotification_003E__1.IsFriendshipRequest())
						{
							_0024this.NavigateFriendshipRequestsTab();
						}
						else if (_003Cnotification_003E__1.IsFriendMessage())
						{
							TabController<MessagesController>.Instance.OpenWith(_003Cnotification_003E__1.FriendId);
						}
						break;
					case 1u:
						_0024awaiter0.GetResult();
						break;
					}
					_0024this.ProcessLobbyInvites();
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

		[CompilerGenerated]
		private sealed class _003COnReceivedInviteEvent_003Ec__AnonStorey3
		{
			[StructLayout(LayoutKind.Auto)]
			public struct _003COnReceivedInviteEvent_003Ec__async2 : IAsyncStateMachine
			{
				internal _003COnReceivedInviteEvent_003Ec__AnonStorey3 _003C_003Ef__ref_00243;

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
							_0024awaiter0 = LobbyUtility.JoinLobby(_003C_003Ef__ref_00243.args.LobbyInvite.LobbyId).GetAwaiter();
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

			internal ReceivedInviteEventArgs args;

			[AsyncStateMachine(typeof(_003COnReceivedInviteEvent_003Ec__async2))]
			internal void _003C_003Em__0()
			{
				_003COnReceivedInviteEvent_003Ec__async2 stateMachine = default(_003COnReceivedInviteEvent_003Ec__async2);
				stateMachine._003C_003Ef__ref_00243 = this;
				stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
				stateMachine._0024builder.Start(ref stateMachine);
			}
		}

		[SerializeField]
		private BoltController _boltController;

		[SerializeField]
		private SplashScreen _splashScreen;

		[SerializeField]
		private Transform _contentTransform;

		[SerializeField]
		private Axlebolt.Standoff.Main.Sidebar.Sidebar _sidebar;

		[SerializeField]
		private LobbyInviteDialog _lobbyInviteDialog;

		[SerializeField]
		private NotificationView _notificationView;

		private PlayController _playController;

		private ProfileController _profileController;

		private InventoryController _inventoryController;

		private ClanController _clanController;

		private FriendsController _friendsController;

		private SettingsController _settingsController;

		private MessagesController _messagesController;

		private BoltMatchmakingService _matchmakingService;

		private BoltFriendsService _friendsService;

		private BoltMessagesService _messagesService;

		private bool _initialized;

		private void Awake()
		{
			_splashScreen.Show();
		}

		private void Start()
		{
			StartCoroutine(StartInit());
		}

		private IEnumerator StartInit()
		{
			ScenePrefab prefabFactory = Singleton<ScenePrefab>.Instance;
			yield return prefabFactory.LoadPrefab<PlayController>("PlayController");
			_playController = prefabFactory.Singleton<PlayController>(_contentTransform);
			yield return prefabFactory.LoadPrefab<ProfileController>("ProfileController");
			_profileController = prefabFactory.Singleton<ProfileController>(_contentTransform);
			yield return prefabFactory.LoadPrefab<InventoryController>("InventoryController");
			_inventoryController = prefabFactory.Singleton<InventoryController>(_contentTransform);
			yield return prefabFactory.LoadPrefab<ClanController>("ClanController");
			_clanController = prefabFactory.Singleton<ClanController>(_contentTransform);
			yield return prefabFactory.LoadPrefab<FriendsController>("FriendsController");
			_friendsController = prefabFactory.Singleton<FriendsController>(_contentTransform);
			yield return prefabFactory.LoadPrefab<SettingsController>("Settings");
			_settingsController = prefabFactory.Singleton<SettingsController>(base.transform);
			yield return prefabFactory.LoadPrefab<MessagesController>("MessagesController");
			_messagesController = prefabFactory.Singleton<MessagesController>(base.transform);
			_boltController.ConnectedEvent += OnConnectedToBolt;
			_boltController.ReconnectedEvent += OnConnectedToBolt;
			_boltController.ConnectionFailedEvent += OnConnectionFailedHandler;
			_boltController.ConnectToBolt();
		}

		private void OnConnectedToBolt()
		{
			try
			{
				if (!_initialized)
				{
					Init();
				}
				else
				{
					_sidebar.CurrentController.Close();
					_sidebar.CurrentController.Open();
					if (TabController<MessagesController>.Instance.IsOpen)
					{
						TabController<MessagesController>.Instance.Close();
					}
				}
				_splashScreen.Hide();
			}
			catch (Exception ex)
			{
				Dialogs.Message(ScriptLocalization.Common.Error, "Something went wrong!");
				AnalyticsManager.MainInitError(ex.GetType().Name);
				throw;
			}
		}

		private void Init()
		{
			SettingsManager.Init(new BoltFileStorage());
			InitSidebar();
			_matchmakingService = BoltService<BoltMatchmakingService>.Instance;
			_matchmakingService.LobbyJoinedEvent.AddListener(OnLobbyJoinedEvent);
			_matchmakingService.ReceivedInviteEvent.AddListener(OnReceivedInviteEvent);
			_friendsService = BoltService<BoltFriendsService>.Instance;
			_friendsService.NewFriendshipRequestEvent.AddListener(OnNewFriendshiptRequestEvent);
			_messagesService = BoltService<BoltMessagesService>.Instance;
			_messagesService.MessageFromFriendEvent.AddListener(OnMessageFromFriendEvent);
			InitNotifications();
			InitVideoAds();
			_initialized = true;
		}

		private void InitSidebar()
		{
			_sidebar.SetPlayController(_playController);
			_sidebar.SetProfileController(_profileController);
			_sidebar.SetInventoryController(_inventoryController);
			_friendsController.gameObject.SetActive(false);
			_sidebar.SetClanController(_clanController);
			_sidebar.SetSettingsController(_settingsController);
			_sidebar.SetMessagesController(_messagesController);
			if (BoltService<BoltMatchmakingService>.Instance.IsInLobby())
			{
				_sidebar.Select(_friendsController);
			}
			else
			{
				_sidebar.Select(_playController);
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CInitNotifications_003Ec__async1))]
		private void InitNotifications()
		{
			_003CInitNotifications_003Ec__async1 stateMachine = default(_003CInitNotifications_003Ec__async1);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void InitVideoAds()
		{
			if (Singleton<InventoryManager>.Instance.GetMainBadgeId() == InventoryItemId.None)
			{
				if (!Singleton<VideoAdsManager>.Instance.IsInitialized())
				{
					Singleton<VideoAdsManager>.Instance.Init();
				}
				else
				{
					Singleton<VideoAdsManager>.Instance.ShowInMenu();
				}
			}
		}

		private void ProcessLobbyInvites()
		{
			if (!_matchmakingService.IsInLobby())
			{
				List<BoltLobbyInvite> lobbyInvitesIncoming = _matchmakingService.LobbyInvitesIncoming;
				if (lobbyInvitesIncoming.Count > 0)
				{
					_lobbyInviteDialog.Show(lobbyInvitesIncoming[0], ProcessLobbyInvites);
				}
			}
		}

		private void NavigateFriendshipRequestsTab()
		{
			_sidebar.Select(_friendsController);
			_friendsController.OpenRequestsTab();
		}

		private void OnReceivedInviteEvent(ReceivedInviteEventArgs args)
		{
			_003COnReceivedInviteEvent_003Ec__AnonStorey3 CS_0024_003C_003E8__locals0 = new _003COnReceivedInviteEvent_003Ec__AnonStorey3();
			CS_0024_003C_003E8__locals0.args = args;
			if (!TabController<MessagesController>.Instance.IsOpen || !TabController<MessagesController>.Instance.IsFriendChatOpened(CS_0024_003C_003E8__locals0.args.LobbyInvite.Friend.Id))
			{
				_notificationView.ShowLobbyInvite(CS_0024_003C_003E8__locals0.args.LobbyInvite);
				_notificationView.ActionHandler = async () =>
				{
					await LobbyUtility.JoinLobby(args.LobbyInvite.LobbyId);
				};
			}
		}

		private void OnNewFriendshiptRequestEvent(BoltFriend boltFriend)
		{
			_notificationView.ShowNewFriendshipRequest(boltFriend);
			_notificationView.ActionHandler = NavigateFriendshipRequestsTab;
		}

		private void OnMessageFromFriendEvent(BoltUserMessagesEventArgs obj)
		{
			if (!TabController<MessagesController>.Instance.IsOpen)
			{
				BoltUserMessage message = obj.BoltUserMessage;
				_notificationView.ShowNewFriendMessage(message.Sender, message.Message);
				_notificationView.ActionHandler = delegate
				{
					TabController<MessagesController>.Instance.OpenWith(message.Sender);
				};
			}
		}

		private void OnLobbyJoinedEvent(LobbyJoinedEventArgs args)
		{
			_sidebar.Select(_friendsController);
			_friendsController.OpenFriendsTab();
		}

		private void OnConnectionFailedHandler()
		{
			_splashScreen.Show();
		}

		private void OnDestroy()
		{
			if (_matchmakingService != null)
			{
				_matchmakingService.LobbyJoinedEvent.RemoveListener(OnLobbyJoinedEvent);
			}
			if (_matchmakingService != null)
			{
				_matchmakingService.ReceivedInviteEvent.RemoveListener(OnReceivedInviteEvent);
			}
			if (_friendsService != null)
			{
				_friendsService.NewFriendshipRequestEvent.RemoveListener(OnNewFriendshiptRequestEvent);
			}
			if (_messagesService != null)
			{
				_messagesService.MessageFromFriendEvent.RemoveListener(OnMessageFromFriendEvent);
			}
		}
	}
}
