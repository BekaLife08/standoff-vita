using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Matchmaking.Events;
using Axlebolt.Bolt.Player;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Main.Play;
using Axlebolt.Standoff.Matchmaking;
using Axlebolt.Standoff.Photon;
using Axlebolt.Standoff.UI;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyController : TabController<LobbyController>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CCanClose_003Ec__async0 : IAsyncStateMachine
		{
			internal Action<bool> callback;

			internal LobbyController _0024this;

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
						if (!_0024this._matchmakingService.IsInLobby())
						{
							callback(true);
							return;
						}
						_0024awaiter0 = LobbyUtility.LeaveLobby().GetAwaiter();
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
					callback(!_0024this._matchmakingService.IsInLobby());
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
		private struct _003CSendLobbyMessage_003Ec__async1 : IAsyncStateMachine
		{
			internal string message;

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
						if (!BoltService<BoltMatchmakingService>.Instance.IsInLobby())
						{
							return;
						}
						_0024awaiter0 = BoltService<BoltMatchmakingService>.Instance.SendLobbyChatMsg(message).GetAwaiter();
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSafeAwait_003Ec__async2 : IAsyncStateMachine
		{
			internal Func<Task> task;

			internal LobbyController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

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
							_0024awaiter0 = task().GetAwaiter();
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
						_0024awaiter0.GetResult();
					}
					catch (ConnectionFailedException message)
					{
						Log.Debug(message);
					}
					catch (Exception ex)
					{
						Log.Error(ex);
						_0024this._lobbyChat.AddErrorMessage(ex.Message);
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
		private struct _003CSearchGame_003Ec__async3 : IAsyncStateMachine
		{
			private sealed class _003CSearchGame_003Ec__AnonStoreyC
			{
				internal BoltPhotonGame boltPhotonGame;

				internal _003CSearchGame_003Ec__async3 _003C_003Ef__ref_00243;

				internal Task _003C_003Em__0()
				{
					return _003C_003Ef__ref_00243._0024this._matchmakingService.SetLobbyPhotonGame(boltPhotonGame);
				}
			}

			internal MatchmakingResult _003Cresult_003E__1;

			internal LobbyController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private _003CSearchGame_003Ec__AnonStoreyC _0024locvar0;

			private TaskAwaiter<MatchmakingResult> _0024awaiter0;

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
							_0024locvar0 = new _003CSearchGame_003Ec__AnonStoreyC();
							_0024locvar0._003C_003Ef__ref_00243 = this;
							if (MatchmakingManager.InProgress)
							{
								return;
							}
							_0024awaiter0 = _0024this.SearchGameAsync().GetAwaiter();
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
						_0024locvar0.boltPhotonGame = new BoltPhotonGame(_003Cresult_003E__1.Region, _003Cresult_003E__1.RoomId, Application.version);
						if (Log.DebugEnabled)
						{
							Log.Debug(string.Format("SetLobbyPhotonGame {0}", _0024locvar0.boltPhotonGame.RoomId));
						}
						LobbyController _004this = _0024this;
						BoltPhotonGame boltPhotonGame = _0024locvar0.boltPhotonGame;
						_0024this.SafeAwait(() => _004this._matchmakingService.SetLobbyPhotonGame(boltPhotonGame));
					}
					catch (OperationCanceledException)
					{
						_0024this.StopSearchProgress();
					}
					catch (PhotonConnectionFailedException message)
					{
						Log.Debug(message);
						_0024this._lobbyChat.AddErrorMessage(ScriptLocalization.Common.ServerConnectionFailed);
						_0024this.StopSearchProgress();
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						_0024this._lobbyChat.AddErrorMessage(string.Format("Something went wrong ({0})", ex2.Message));
						_0024this.StopSearchProgress();
					}
					finally
					{
						if (!flag)
						{
							_0024this._matchmakingCancellation = null;
						}
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
		private struct _003COnLobbyPhotonGameChangedEvent_003Ec__async4 : IAsyncStateMachine
		{
			internal LobbyPhotonGameChangedEventArgs args;

			internal MatchmakingJoinOptions _003Coptions_003E__1;

			internal LobbyController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

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
						if (!_0024this._matchmakingService.IsLobbyOwner())
						{
							num = 4294967293u;
							break;
						}
						LobbyUtility.StartGame();
						goto end_IL_0010;
					case 1u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_003Coptions_003E__1 = new MatchmakingJoinOptions(args.PhotonGame.RoomId)
							{
								PlayerId = BoltService<BoltPlayerService>.Instance.Player.Id
							};
							LobbyController _004this = _0024this;
							_0024awaiter0 = MatchmakingManager.Join(args.PhotonGame.Region, _003Coptions_003E__1, delegate(PhotonServer region, int online, int ping)
							{
								_004this._searchGameView.Region = region.GetDisplayName();
								_004this._searchGameView.Online = 2 * online;
								_004this._searchGameView.Ping = ping;
							}).GetAwaiter();
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
						_0024awaiter0.GetResult();
						LobbyUtility.StartGame();
					}
					catch (PhotonConnectionFailedException)
					{
						_0024this._lobbyChat.AddErrorMessage(ScriptLocalization.Common.ServerConnectionFailed);
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						_0024this._lobbyChat.AddErrorMessage(string.Format("Something went wrong ({0})", ex2.Message));
					}
					end_IL_0010:;
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
		private struct _003CInit_003Ec__async6 : IAsyncStateMachine
		{
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
						_0024awaiter0 = LobbyUtility.CreateLobby().GetAwaiter();
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInit_003Ec__async8 : IAsyncStateMachine
		{
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
						_0024awaiter0 = LobbyUtility.LeaveLobby().GetAwaiter();
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

		private static readonly Log Log = Log.Create(typeof(LobbyController));

		[SerializeField]
		private Button _createLobbyButton;

		[SerializeField]
		private Button _startButton;

		[SerializeField]
		private LobbyMembersView _lobbyMembersView;

		[SerializeField]
		private LobbyChat _lobbyChat;

		[SerializeField]
		private LobbyToolbar _lobbyToolbar;

		[SerializeField]
		private Button _leaveButton;

		[SerializeField]
		private GameModeDialog _gameModeDialog;

		[SerializeField]
		private LobbyTypeSelectDialog _lobbyTypeSelectDialog;

		[SerializeField]
		private LevelSelectDialog _levelSelectDialog;

		[SerializeField]
		private MatchmakingHelper _matchmakingHelper;

		[SerializeField]
		private LobbySearchGameView _searchGameView;

		private BoltMatchmakingService _matchmakingService;

		private CancellationTokenSource _matchmakingCancellation;

		public override void Init()
		{
			_matchmakingService = BoltService<BoltMatchmakingService>.Instance;
			_createLobbyButton.onClick.AddListener(async () =>
			{
				await LobbyUtility.CreateLobby();
			});
			_startButton.onClick.AddListener(StartMatchmaking);
			_lobbyChat.SendMessageAction = SendLobbyMessage;
			_lobbyToolbar.GameModeHandler = OnGameModeHandler;
			_lobbyToolbar.LevelHandler = OnLevelSelectHandler;
			_lobbyToolbar.LobbyTypeHandler = OnLobbyTypeSelectHandler;
			_searchGameView.Hide();
			_searchGameView.CancelHandler = StopMatchmaking;
			_leaveButton.onClick.AddListener(async () =>
			{
				await LobbyUtility.LeaveLobby();
			});
			_gameModeDialog.Hide();
			_gameModeDialog.DeathMatchHandler = delegate
			{
				SetGameMode("DeathMatch");
			};
			_gameModeDialog.DefuseHandler = delegate
			{
				SetGameMode("Defuse");
			};
		}

		public override void OnOpen()
		{
			_matchmakingService.LobbyJoinedEvent.AddListener(OnLobbyJoinedEvent);
			_matchmakingService.LobbyLeftEvent.AddListener(OnLobbyLeftEvent);
			_matchmakingService.NewPlayerJoinedEvent.AddListener(OnNewPlayerJoinedEvent);
			_matchmakingService.NewPlayerInvitedEvent.AddListener(OnNewPlayerInvited);
			_matchmakingService.PlayerLeftEvent.AddListener(OnPlayerLeftEvent);
			_matchmakingService.PlayerKickedEvent.AddListener(OnPlayerKickedEvent);
			_matchmakingService.PlayerRefusedEvent.AddListener(OnPlayerRefusedEvent);
			_matchmakingService.PlayerRevokedEvent.AddListener(OnPlayerRevokedEvent);
			_matchmakingService.LobbyOwnerChangedEvent.AddListener(OnLobbyOwnerChangedEvent);
			_matchmakingService.LobbyDataChangedEvent.AddListener(OnLobbyDataChangedEvent);
			_matchmakingService.LobbyTypeChangedEvent.AddListener(OnLobbyTypeChangedEvent);
			_matchmakingService.LobbyChatMessageEvent.AddListener(OnOnLobbyChatMessage);
			_matchmakingService.LobbyJoinableChangedEvent.AddListener(LobbyJoinableChangedEvent);
			_matchmakingService.LobbyPhotonGameChangedEvent.AddListener(OnLobbyPhotonGameChangedEvent);
			if (_matchmakingService.IsInLobby())
			{
				OnLobbyJoinedEvent();
			}
			else
			{
				UpdateLobbyStateView();
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCanClose_003Ec__async0))]
		public override void CanClose(Action<bool> callback)
		{
			_003CCanClose_003Ec__async0 stateMachine = default(_003CCanClose_003Ec__async0);
			stateMachine.callback = callback;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public override void OnClose()
		{
			_matchmakingService.LobbyJoinedEvent.RemoveListener(OnLobbyJoinedEvent);
			_matchmakingService.LobbyLeftEvent.RemoveListener(OnLobbyLeftEvent);
			_matchmakingService.NewPlayerJoinedEvent.RemoveListener(OnNewPlayerJoinedEvent);
			_matchmakingService.NewPlayerInvitedEvent.RemoveListener(OnNewPlayerInvited);
			_matchmakingService.PlayerLeftEvent.RemoveListener(OnPlayerLeftEvent);
			_matchmakingService.PlayerKickedEvent.RemoveListener(OnPlayerKickedEvent);
			_matchmakingService.PlayerRefusedEvent.RemoveListener(OnPlayerRefusedEvent);
			_matchmakingService.PlayerRevokedEvent.RemoveListener(OnPlayerRevokedEvent);
			_matchmakingService.LobbyOwnerChangedEvent.RemoveListener(OnLobbyOwnerChangedEvent);
			_matchmakingService.LobbyDataChangedEvent.RemoveListener(OnLobbyDataChangedEvent);
			_matchmakingService.LobbyTypeChangedEvent.RemoveListener(OnLobbyTypeChangedEvent);
			_matchmakingService.LobbyChatMessageEvent.RemoveListener(OnOnLobbyChatMessage);
			_matchmakingService.LobbyJoinableChangedEvent.RemoveListener(LobbyJoinableChangedEvent);
			_matchmakingService.LobbyPhotonGameChangedEvent.RemoveListener(OnLobbyPhotonGameChangedEvent);
		}

		private void OnLobbyJoinedEvent(LobbyJoinedEventArgs args)
		{
			OnLobbyJoinedEvent();
		}

		private void OnLobbyJoinedEvent()
		{
			_lobbyChat.AddMessage(ScriptLocalization.Lobby.ConnectedToLobby);
			UpdateLobbyStateView();
			InitLobbyOptions();
			SetBadgeId();
		}

		private void OnLobbyLeftEvent(LobbyLeftEventArgs args)
		{
			UpdateLobbyStateView();
		}

		private bool ValidateVersionsCompatibility()
		{
			BoltLobby currentLobby = _matchmakingService.CurrentLobby;
			string gameVersion = currentLobby.LobbyOwner.PlayInGame.GameVersion;
			bool flag = false;
			BoltFriend[] lobbyMembers = currentLobby.LobbyMembers;
			foreach (BoltFriend boltFriend in lobbyMembers)
			{
				string gameVersion2 = boltFriend.PlayInGame.GameVersion;
				if (!VersionUtility.IsCompatibleVersions(gameVersion, gameVersion2))
				{
					string message = string.Format(ScriptLocalization.Lobby.IncompatibleVersion, boltFriend.Name, gameVersion2, gameVersion);
					_lobbyChat.AddErrorMessage(message);
					flag = true;
				}
			}
			return !flag;
		}

		private void OnNewPlayerJoinedEvent(NewPlayerJoinedEventArgs args)
		{
			UpdateLobbyMembersView();
			_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.NewPlayerJoined, args.JoinedPlayer.Name));
			StopMatchmaking();
		}

		private void OnPlayerLeftEvent(PlayerLeftEventArgs args)
		{
			UpdateLobbyMembersView();
			_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.PlayerLeft, args.LeftPlayer.Name));
			StopMatchmaking();
		}

		private void OnNewPlayerInvited(NewPlayerInvitedEventArgs args)
		{
			UpdateLobbyMembersView();
			_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.NewPlayerInvited, args.InviteSender.Name, args.InvitedPlayer.Name));
		}

		private void OnPlayerRevokedEvent(PlayerRevokedEventArgs args)
		{
			UpdateLobbyMembersView();
			_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.PlayerRevoked, args.InviteSender.Name, args.RevokedPlayer.Name));
		}

		private void OnPlayerKickedEvent(PlayerKickedEventArgs args)
		{
			if (args.KickedPlayer.IsLocal())
			{
				UpdateLobbyStateView();
				Dialogs.Message(ScriptLocalization.Common.Attention, ScriptLocalization.Lobby.YouHaveBeenKicked, delegate
				{
				});
			}
			else
			{
				UpdateLobbyMembersView();
				_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.PlayerKicked, args.KickInitiator.Name, args.KickedPlayer.Name));
				StopMatchmaking();
			}
		}

		private void OnPlayerRefusedEvent(PlayerRefusedEventArgs args)
		{
			UpdateLobbyMembersView();
			_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.PlayerRefused, args.RefusedPlayer.Name));
		}

		private void OnLobbyOwnerChangedEvent(LobbyOwnerChangedEventArgs args)
		{
			UpdateLobbyStateView();
			UpdateLobbyMembersView();
			UpdateLobbyDataView();
			_lobbyChat.AddWarnMessage(string.Format(ScriptLocalization.Lobby.OwnerWasChanged, args.Owner.Name));
			if (args.Owner.IsLocal())
			{
				StopSearchProgress();
			}
		}

		[AsyncStateMachine(typeof(_003CSendLobbyMessage_003Ec__async1))]
		[DebuggerStepThrough]
		private void SendLobbyMessage(string message)
		{
			_003CSendLobbyMessage_003Ec__async1 stateMachine = default(_003CSendLobbyMessage_003Ec__async1);
			stateMachine.message = message;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void OnOnLobbyChatMessage(LobbyChatMessageEventArgs args)
		{
			if (_matchmakingService.IsInLobby())
			{
				BoltLobbyMessage lobbyMessage = args.LobbyMessage;
				BoltFriend lobbyMember = _matchmakingService.CurrentLobby.GetLobbyMember(lobbyMessage.SenderId);
				if (lobbyMember != null)
				{
					_lobbyChat.AddPlayerMessage(lobbyMember, lobbyMessage.Message);
				}
			}
		}

		private void OnLobbyDataChangedEvent(LobbyDataChangedEventArgs args)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("OnLobbyDataChangedEvent " + string.Join(",", args.LobbyData.Keys.ToArray()));
			}
			if (args.IsLobbyOptionsChanged())
			{
				UpdateLobbyDataView();
				StopMatchmaking();
			}
			if (args.IsSomeMemberBadgeIdChanged())
			{
				UpdateLobbyMembersView();
			}
		}

		private void OnLobbyTypeChangedEvent(LobbyTypeChangedEventArgs args)
		{
			UpdateLobbyDataView();
			StopMatchmaking();
		}

		private void InitLobbyOptions()
		{
			if (_matchmakingService.IsLobbyOwner() && !_matchmakingService.IsLobbyOptionsExists())
			{
				SetGameMode("Defuse");
			}
		}

		private void SetBadgeId()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("SetBadgeId");
			}
			SafeAwait(() => _matchmakingService.SetBadgeId(Singleton<InventoryManager>.Instance.GetMainBadgeId()));
		}

		private void UpdateLobbyStateView()
		{
			bool flag = _matchmakingService.IsInLobby();
			bool active = flag && _matchmakingService.IsLobbyOwner();
			_createLobbyButton.gameObject.SetActive(!flag);
			_startButton.transform.parent.gameObject.SetActive(active);
			_lobbyMembersView.IsVisible = flag;
			_lobbyChat.IsVisible = flag;
			_lobbyToolbar.IsVisible = flag;
			if (flag)
			{
				UpdateLobbyMembersView();
				UpdateLobbyDataView();
			}
			else
			{
				_searchGameView.Hide();
			}
		}

		private void UpdateLobbyMembersView()
		{
			_lobbyMembersView.SetLobby(BoltService<BoltMatchmakingService>.Instance.CurrentLobby);
		}

		private void UpdateLobbyDataView()
		{
			if (_matchmakingService.IsLobbyOptionsExists())
			{
				BoltLobby.LobbyType type = _matchmakingService.CurrentLobby.Type;
				LobbyOptions lobbyOptions = _matchmakingService.GetLobbyOptions();
				string gameModeDisplayName = GetGameModeDisplayName(lobbyOptions.GameModeName);
				string[] displayLevelNames = GetDisplayLevelNames(lobbyOptions.LevelNames);
				_lobbyToolbar.SetOptions(gameModeDisplayName, displayLevelNames, type, _matchmakingService.IsLobbyOwner());
			}
			else
			{
				_lobbyToolbar.SetEmpty();
			}
		}

		private void OnGameModeHandler()
		{
			if (_matchmakingService.IsLobbyOwner())
			{
				_gameModeDialog.Show();
			}
		}

		private void SetGameMode(string gameMode)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("SetGameMode {0}", gameMode));
			}
			SafeAwait(() => _matchmakingService.SetLobbyOptions(new LobbyOptions
			{
				GameModeName = gameMode,
				LevelNames = new string[0]
			}));
		}

		private void OnLevelSelectHandler()
		{
			if (_matchmakingService.IsLobbyOwner() && _matchmakingService.CurrentLobby.Type != BoltLobby.LobbyType.Public)
			{
				LobbyOptions lobbyOptions = _matchmakingService.GetLobbyOptions();
				LevelDefinition[] levels = LevelUtility.GetLevels(lobbyOptions.GameModeName);
				string[] levelNames = _matchmakingService.GetLobbyOptions().LevelNames;
				_levelSelectDialog.Show(levels, levelNames, SetSelectedLevels);
			}
		}

		private void SetSelectedLevels(string[] selectedLevels)
		{
			LobbyOptions lobbyOptions = _matchmakingService.GetLobbyOptions();
			lobbyOptions.LevelNames = selectedLevels;
			if (Log.DebugEnabled)
			{
				Log.Debug("SetSelectedLevels");
			}
			SafeAwait(() => _matchmakingService.SetLobbyOptions(lobbyOptions));
		}

		private void OnLobbyTypeSelectHandler()
		{
			if (_matchmakingService.IsLobbyOwner())
			{
				_lobbyTypeSelectDialog.Show(SetLobbyType);
			}
		}

		private void SetLobbyType(BoltLobby.LobbyType type)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("SetLobbyType {0}", type));
			}
			SafeAwait(() => _matchmakingService.SetLobbyType(type));
		}

		private string[] GetDisplayLevelNames(IEnumerable<string> levelNames)
		{
			return levelNames.Select((string levelName) => LevelUtility.GetLevel(levelName).DisplayName).ToArray();
		}

		public string GetGameModeDisplayName(string gameMode)
		{
			return GameModeUtility.GetByName(gameMode).LocalizedName;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CSafeAwait_003Ec__async2))]
		private void SafeAwait(Func<Task> task)
		{
			_003CSafeAwait_003Ec__async2 stateMachine = default(_003CSafeAwait_003Ec__async2);
			stateMachine.task = task;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void StartMatchmaking()
		{
			if (!_matchmakingService.IsLobbyOwner())
			{
				Log.Error("Only lobby owner can start matchmaking");
				return;
			}
			if (Log.DebugEnabled)
			{
				Log.Debug("StartMatchmaking");
			}
			SafeAwait(() => _matchmakingService.SetLobbyJoinable(false));
		}

		private void LobbyJoinableChangedEvent(LobbyJoinableChangedEventArgs args)
		{
			bool flag = !args.Joinable;
			_startButton.interactable = !flag;
			_searchGameView.SetIsLobbyOwner(_matchmakingService.IsLobbyOwner());
			if (flag)
			{
				if (ValidateVersionsCompatibility())
				{
					if (_matchmakingService.IsLobbyOwner())
					{
						SearchGame();
					}
					else
					{
						_searchGameView.Show();
					}
				}
				else if (_matchmakingService.IsLobbyOwner())
				{
					StopSearchProgress();
				}
			}
			else
			{
				_lobbyChat.AddWarnMessage(ScriptLocalization.Lobby.MatchmakingCanceled);
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CSearchGame_003Ec__async3))]
		private void SearchGame()
		{
			_003CSearchGame_003Ec__async3 stateMachine = default(_003CSearchGame_003Ec__async3);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void StopSearchProgress()
		{
			if (!_matchmakingService.CurrentLobby.Joinable)
			{
				if (Log.DebugEnabled)
				{
					Log.Debug("SetLobbyJoinable true");
				}
				SafeAwait(() => _matchmakingService.SetLobbyJoinable(true));
			}
		}

		private void StopMatchmaking()
		{
			if (_matchmakingService.IsLobbyOwner() && _matchmakingCancellation != null)
			{
				_matchmakingCancellation.Cancel();
			}
		}

		private Task<MatchmakingResult> SearchGameAsync()
		{
			if (_matchmakingCancellation != null)
			{
				throw new Exception("Matcmaking already in progress");
			}
			_matchmakingCancellation = new CancellationTokenSource();
			LobbyOptions lobbyOptions = _matchmakingService.GetLobbyOptions();
			BoltLobby currentLobby = _matchmakingService.CurrentLobby;
			BoltLobby.LobbyType type = currentLobby.Type;
			GameMode byName = GameModeUtility.GetByName(lobbyOptions.GameModeName);
			LevelDefinition[] levels = lobbyOptions.LevelNames.Select(LevelUtility.GetLevel).ToArray();
			string[] expectedPlayers = currentLobby.LobbyMembers.Select((BoltFriend member) => member.Id).ToArray();
			if (type == BoltLobby.LobbyType.Public)
			{
				return _matchmakingHelper.Find(new MatchmakingFilter(byName)
				{
					AllowEmptyRoom = true,
					ExpectedPlayers = expectedPlayers,
					PlayerId = BoltService<BoltPlayerService>.Instance.Player.Id,
					TimeInGame = BoltService<BoltPlayerService>.Instance.Player.TimeInGame
				}, _matchmakingCancellation);
			}
			return _matchmakingHelper.Create(new MatchmakingCreateOptions(byName)
			{
				ExpectedPlayers = expectedPlayers,
				IsPrivate = (type == BoltLobby.LobbyType.Private),
				IsVisible = false,
				Levels = levels,
				PlayerId = BoltService<BoltPlayerService>.Instance.Player.Id
			}, _matchmakingCancellation);
		}

		[AsyncStateMachine(typeof(_003COnLobbyPhotonGameChangedEvent_003Ec__async4))]
		[DebuggerStepThrough]
		private void OnLobbyPhotonGameChangedEvent(LobbyPhotonGameChangedEventArgs args)
		{
			_003COnLobbyPhotonGameChangedEvent_003Ec__async4 stateMachine = default(_003COnLobbyPhotonGameChangedEvent_003Ec__async4);
			stateMachine.args = args;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}
