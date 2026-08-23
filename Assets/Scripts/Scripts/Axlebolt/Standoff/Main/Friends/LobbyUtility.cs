using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Api.Exception;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Player;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Matchmaking;
using Axlebolt.Standoff.Photon;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyUtility
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CCreateLobby_003Ec__async0 : IAsyncStateMachine
		{
			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<bool> _0024awaiter0;

			private TaskAwaiter _0024awaiter1;

			private static Action _003C_003Ef__am_0024cache0;

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
						_0024awaiter0 = ExitGame().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							flag = true;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						if (!_0024awaiter0.GetResult())
						{
							num = 4294967293u;
							break;
						}
						goto end_IL_0010;
					case 2u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_0024awaiter1 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Lobby.CreatingLobby, ScriptLocalization.Common.PleaseWait), BoltService<BoltMatchmakingService>.Instance.CreateLobby("NewLobby", BoltLobby.LobbyType.Public, 10)).GetAwaiter();
							if (!_0024awaiter1.IsCompleted)
							{
								_0024PC = 2;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
								return;
							}
							break;
						case 2u:
							break;
						}
						_0024awaiter1.GetResult();
					}
					catch (ConnectionFailedException message)
					{
						Log.Debug(message);
					}
					catch (Exception ex)
					{
						Dialogs.Message(ScriptLocalization.Common.Error, ex.Message, delegate
						{
						});
						Log.Error(ex);
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

			private static void _003C_003Em__0()
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLeaveLobby_003Ec__async1 : IAsyncStateMachine
		{
			internal Dialog _003Cconfirm_003E__0;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

			private TaskAwaiter _0024awaiter1;

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
						_003Cconfirm_003E__0 = Dialogs.Confirm(ScriptLocalization.Lobby.LeaveLobby, ScriptLocalization.Lobby.ConfirmLeaveLobby);
						_0024awaiter0 = _003Cconfirm_003E__0.ShowAndWait().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						if (_003Cconfirm_003E__0.ActionButton == Dialogs.Ok)
						{
							_0024awaiter1 = AsyncUtility.AsyncComplete(BoltService<BoltMatchmakingService>.Instance.LeaveLobby()).GetAwaiter();
							if (!_0024awaiter1.IsCompleted)
							{
								_0024PC = 2;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
								return;
							}
							break;
						}
						goto end_IL_000e;
					case 2u:
						break;
					}
					_0024awaiter1.GetResult();
					end_IL_000e:;
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
		private struct _003CJoinLobby_003Ec__async2 : IAsyncStateMachine
		{
			internal Dialog _003Cconfirm_003E__1;

			internal string lobbyId;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<bool> _0024awaiter0;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter1;

			private TaskAwaiter _0024awaiter2;

			private static Action _003C_003Ef__am_0024cache0;

			private static Action _003C_003Ef__am_0024cache1;

			private static Action _003C_003Ef__am_0024cache2;

			private static Action _003C_003Ef__am_0024cache3;

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
						_0024awaiter0 = ExitGame().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							flag = true;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						if (!_0024awaiter0.GetResult())
						{
							if (BoltService<BoltMatchmakingService>.Instance.IsInLobby())
							{
								_003Cconfirm_003E__1 = Dialogs.Confirm(ScriptLocalization.Lobby.JoinAnotherLobby, ScriptLocalization.Lobby.ConfirmJoinAnotherLobby);
								_0024awaiter1 = _003Cconfirm_003E__1.ShowAndWait().GetAwaiter();
								if (!_0024awaiter1.IsCompleted)
								{
									_0024PC = 2;
									flag = true;
									_0024builder.AwaitOnCompleted(ref _0024awaiter1, ref this);
									return;
								}
								goto case 2u;
							}
							goto IL_0163;
						}
						goto end_IL_0010;
					case 2u:
						_0024awaiter1.GetResult();
						if (_003Cconfirm_003E__1.ActionButton == Dialogs.Ok)
						{
							_0024awaiter2 = AsyncUtility.AsyncComplete(BoltService<BoltMatchmakingService>.Instance.LeaveLobby()).GetAwaiter();
							if (!_0024awaiter2.IsCompleted)
							{
								_0024PC = 3;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter2, ref this);
								return;
							}
							goto case 3u;
						}
						goto IL_0163;
					case 3u:
						_0024awaiter2.GetResult();
						goto IL_0163;
					case 4u:
						break;
						IL_0163:
						num = 4294967293u;
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_0024awaiter2 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Lobby.JoiningLobby, ScriptLocalization.Common.PleaseWait), BoltService<BoltMatchmakingService>.Instance.JoinLobby(lobbyId)).GetAwaiter();
							if (!_0024awaiter2.IsCompleted)
							{
								_0024PC = 4;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter2, ref this);
								return;
							}
							break;
						case 4u:
							break;
						}
						_0024awaiter2.GetResult();
					}
					catch (ConnectionFailedException message)
					{
						Log.Debug(message);
					}
					catch (LobbyIsFullRpcException message2)
					{
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ScriptLocalization.Lobby.LobbyIsFull, delegate
						{
						});
						Log.Debug(message2);
					}
					catch (LobbyJoinRestrictedRpcException message3)
					{
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ScriptLocalization.Lobby.LobbyJoinRestricted, delegate
						{
						});
						Log.Debug(message3);
					}
					catch (LobbyNotFoundRpcException message4)
					{
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ScriptLocalization.Lobby.LobbyNotFound, delegate
						{
						});
						Log.Debug(message4);
					}
					catch (Exception ex)
					{
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ex.Message, delegate
						{
						});
						Log.Error(ex);
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

			private static void _003C_003Em__0()
			{
			}

			private static void _003C_003Em__1()
			{
			}

			private static void _003C_003Em__2()
			{
			}

			private static void _003C_003Em__3()
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExitGame_003Ec__async3 : IAsyncStateMachine
		{
			internal bool _003Cexit_003E__1;

			internal AsyncTaskMethodBuilder<bool> _0024builder;

			internal int _0024PC;

			private TaskAwaiter<bool> _0024awaiter0;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter1;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool result;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						if (GameManager.InGame)
						{
							_0024awaiter0 = GameManager.Instance.ExitGameWithConfirmAsync().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
						result = false;
						break;
					case 1u:
						_003Cexit_003E__1 = _0024awaiter0.GetResult();
						if (!_003Cexit_003E__1)
						{
							result = true;
							break;
						}
						_0024awaiter1 = AsyncUtility.WaitFrame().GetAwaiter();
						if (!_0024awaiter1.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitOnCompleted(ref _0024awaiter1, ref this);
							return;
						}
						goto case 2u;
					case 2u:
						_0024awaiter1.GetResult();
						result = false;
						break;
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CRejectInvite_003Ec__async4 : IAsyncStateMachine
		{
			internal string lobbyId;

			internal AsyncTaskMethodBuilder _0024builder;

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
						_0024awaiter0 = AsyncUtility.AsyncComplete(BoltService<BoltMatchmakingService>.Instance.RefuseInvitationToLobby(lobbyId)).GetAwaiter();
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
		private struct _003CJoinGame_003Ec__async5 : IAsyncStateMachine
		{
			internal string gameVersion;

			internal BoltPhotonGame photonGame;

			internal MatchmakingJoinOptions _003Coptions_003E__1;

			internal Dialog _003Cdialog_003E__1;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private static Action _003C_003Ef__am_0024cache0;

			private TaskAwaiter<bool> _0024awaiter0;

			private TaskAwaiter _0024awaiter1;

			private static Action _003C_003Ef__am_0024cache1;

			private static Action _003C_003Ef__am_0024cache2;

			private static Action _003C_003Ef__am_0024cache3;

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
					case 2u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
						{
							if (VersionUtility.IsCompatibleVersions(Application.version, gameVersion))
							{
								_0024awaiter0 = ExitGame().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								goto case 1u;
							}
							string message = string.Format(ScriptLocalization.Common.IncompatibleVersion, gameVersion);
							Dialogs.Message(ScriptLocalization.Common.Error, message, delegate
							{
							});
							goto end_IL_002a;
						}
						case 1u:
							if (!_0024awaiter0.GetResult())
							{
								_003Coptions_003E__1 = new MatchmakingJoinOptions(photonGame.RoomId)
								{
									PlayerId = BoltService<BoltPlayerService>.Instance.Player.Id
								};
								_003Cdialog_003E__1 = Dialogs.Create(ScriptLocalization.Lobby.JoiningGame, ScriptLocalization.Common.PleaseWait);
								_003Cdialog_003E__1.Show();
								num = 4294967293u;
								break;
							}
							goto end_IL_002a;
						case 2u:
							break;
						}
						try
						{
							switch (num)
							{
							default:
								_0024awaiter1 = MatchmakingManager.Join(photonGame.Region, _003Coptions_003E__1).GetAwaiter();
								if (!_0024awaiter1.IsCompleted)
								{
									_0024PC = 2;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
									return;
								}
								break;
							case 2u:
								break;
							}
							_0024awaiter1.GetResult();
							StartGame();
						}
						catch (Exception)
						{
							_003Cdialog_003E__1.Hide();
							throw;
						}
						end_IL_002a:;
					}
					catch (PhotonConnectionFailedException message2)
					{
						Log.Debug(message2);
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ScriptLocalization.Common.ServerConnectionFailed, delegate
						{
						});
					}
					catch (PrivateGameException message3)
					{
						Log.Debug(message3);
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ScriptLocalization.Lobby.GameIsClosed, delegate
						{
						});
					}
					catch (PhotonJoinRoomException ex2)
					{
						Log.Debug(ex2);
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, (ex2.Code != 32765) ? ex2.Message : ScriptLocalization.Lobby.GameIsFull, delegate
						{
						});
					}
					catch (Exception ex3)
					{
						Log.Error(ex3);
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ex3.Message);
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

			private static void _003C_003Em__0()
			{
			}

			private static void _003C_003Em__1()
			{
			}

			private static void _003C_003Em__2()
			{
			}

			private static void _003C_003Em__3()
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInviteToLobby_003Ec__async6 : IAsyncStateMachine
		{
			internal string friendId;

			internal AsyncTaskMethodBuilder _0024builder;

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
							_0024awaiter0 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Dialogs.Processing, ScriptLocalization.Common.PleaseWait), BoltService<BoltMatchmakingService>.Instance.InviteToLobby(friendId)).GetAwaiter();
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
					catch (BlockedPlayerRpcException)
					{
						Dialogs.Message(ScriptLocalization.Common.Error, ScriptLocalization.Friends.BlockedException);
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						Dialogs.Message(ScriptLocalization.Lobby.JoinFailed, ex2.Message);
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
		private struct _003COnExit_003Ec__async7 : IAsyncStateMachine
		{
			internal bool gameFinished;

			internal string errorMsg;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private Exception _0024stack0;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

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
						try
						{
							try
							{
								if (gameFinished)
								{
									if (BoltApi.Instance.IsConnectedAndReady && BoltService<BoltMatchmakingService>.Instance.IsLobbyOwner())
									{
										BoltService<BoltMatchmakingService>.Instance.SetLobbyJoinable(true).Wait();
									}
								}
								else if (BoltApi.Instance.IsConnectedAndReady && BoltService<BoltMatchmakingService>.Instance.IsInLobby())
								{
									BoltService<BoltMatchmakingService>.Instance.LeaveLobby().Wait();
								}
							}
							catch (ConnectionFailedException)
							{
							}
							catch (Exception message)
							{
								Log.Error(message);
							}
						}
						catch (Exception obj)
						{
							_0024stack0 = obj;
						}
						SceneManager.LoadScene("Main", LoadSceneMode.Single);
						if (errorMsg == null)
						{
							break;
						}
						_0024awaiter0 = AsyncUtility.WaitFrame().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						Dialogs.Message(ScriptLocalization.Common.Attention, errorMsg);
						break;
					}
					if (_0024stack0 != null)
					{
						throw _0024stack0;
					}
					_0024stack0 = null;
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

		private static readonly Log Log = Log.Create(typeof(LobbyUtility));

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCreateLobby_003Ec__async0))]
		public static Task CreateLobby()
		{
			_003CCreateLobby_003Ec__async0 stateMachine = default(_003CCreateLobby_003Ec__async0);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CLeaveLobby_003Ec__async1))]
		[DebuggerStepThrough]
		public static Task LeaveLobby()
		{
			_003CLeaveLobby_003Ec__async1 stateMachine = default(_003CLeaveLobby_003Ec__async1);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CJoinLobby_003Ec__async2))]
		[DebuggerStepThrough]
		public static Task JoinLobby(string lobbyId)
		{
			_003CJoinLobby_003Ec__async2 stateMachine = default(_003CJoinLobby_003Ec__async2);
			stateMachine.lobbyId = lobbyId;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExitGame_003Ec__async3))]
		private static Task<bool> ExitGame()
		{
			_003CExitGame_003Ec__async3 stateMachine = default(_003CExitGame_003Ec__async3);
			stateMachine._0024builder = AsyncTaskMethodBuilder<bool>.Create();
			AsyncTaskMethodBuilder<bool> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CRejectInvite_003Ec__async4))]
		public static Task RejectInvite(string lobbyId)
		{
			_003CRejectInvite_003Ec__async4 stateMachine = default(_003CRejectInvite_003Ec__async4);
			stateMachine.lobbyId = lobbyId;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinGame_003Ec__async5))]
		public static Task JoinGame(BoltPhotonGame photonGame, string gameVersion)
		{
			_003CJoinGame_003Ec__async5 stateMachine = default(_003CJoinGame_003Ec__async5);
			stateMachine.gameVersion = gameVersion;
			stateMachine.photonGame = photonGame;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CInviteToLobby_003Ec__async6))]
		public static Task InviteToLobby(string friendId)
		{
			_003CInviteToLobby_003Ec__async6 stateMachine = default(_003CInviteToLobby_003Ec__async6);
			stateMachine.friendId = friendId;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public static void StartGame()
		{
			BoltPlayer player = BoltService<BoltPlayerService>.Instance.Player;
			PlayerAttr playerAttr = new PlayerAttr(player.Uid, player.Name, player.Avatar, Singleton<InventoryManager>.Instance.GetMainBadgeId());
			GameManager.InitGame(playerAttr, OnExit);
		}

		[AsyncStateMachine(typeof(_003COnExit_003Ec__async7))]
		[DebuggerStepThrough]
		private static void OnExit(bool gameFinished, string errorMsg)
		{
			_003COnExit_003Ec__async7 stateMachine = default(_003COnExit_003Ec__async7);
			stateMachine.gameFinished = gameFinished;
			stateMachine.errorMsg = errorMsg;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}
