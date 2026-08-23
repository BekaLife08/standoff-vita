using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Photon;
using ExitGames.Client.Photon;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingManager
	{
		public delegate void ServerUpdatedHandler(PhotonServer server, int onlinePlayers, int ping);

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CCreate_003Ec__async0 : IAsyncStateMachine
		{
			internal string region;

			internal MatchmakingCreateOptions options;

			internal CancellationToken cancellationToken;

			internal ServerUpdatedHandler handler;

			internal Exception _003Cex_003E__1;

			internal AsyncTaskMethodBuilder<MatchmakingResult> _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				MatchmakingResult result = default(MatchmakingResult);
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						if (region == null)
						{
							throw new ArgumentNullException("region");
						}
						if (options == null)
						{
							throw new ArgumentNullException("options");
						}
						Setup(region, options, cancellationToken);
						num = 4294967293u;
						break;
					case 1u:
					case 2u:
					case 3u:
						break;
					}
					try
					{
						Exception ex2;
						int num2;
						switch (num)
						{
						default:
							try
							{
								switch (num)
								{
								default:
									_serverUpdatedHandler = handler;
									_0024awaiter0 = Connect().GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 1;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									goto case 1u;
								case 1u:
									_0024awaiter0.GetResult();
									UpdateHandlers();
									_0024awaiter0 = CreateRoom(options.GameMode, options.Levels, options.ExpectedPlayers, GetSandboxFilter(options), options.IsVisible, options.IsPrivate).GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 2;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									break;
								case 2u:
									break;
								}
								_0024awaiter0.GetResult();
								UpdateHandlers();
								Log.Debug("Game created successfully!");
								result = new MatchmakingResult(_photonServer.Location, PhotonNetwork.room.Name);
							}
							catch (Exception ex)
							{
								ex2 = ex;
								num2 = 1;
								goto IL_01b1;
							}
							goto end_IL_0071;
						case 3u:
							break;
							IL_01b1:
							switch (num2)
							{
							default:
								goto end_IL_0071;
							case 1:
								_003Cex_003E__1 = ex2;
								Log.Debug(_003Cex_003E__1);
								_0024awaiter0 = Disconnect().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 3;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
							break;
						}
						_0024awaiter0.GetResult();
						throw _003Cex_003E__1;
						end_IL_0071:;
					}
					finally
					{
						if (!flag)
						{
							Clear();
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
		private struct _003CJoin_003Ec__async1 : IAsyncStateMachine
		{
			internal string region;

			internal MatchmakingJoinOptions options;

			internal CancellationToken cancellationToken;

			internal Exception _003Cex_003E__1;

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
						if (region == null)
						{
							throw new ArgumentNullException("region");
						}
						if (options == null)
						{
							throw new ArgumentNullException("options");
						}
						Setup(region, options, cancellationToken);
						num = 4294967293u;
						break;
					case 1u:
					case 2u:
					case 3u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
						{
							Exception ex2 = default(Exception);
							int num2 = default(int);
							try
							{
								switch (num)
								{
								default:
									_0024awaiter0 = Connect().GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 1;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									goto case 1u;
								case 1u:
									_0024awaiter0.GetResult();
									UpdateHandlers();
									_0024awaiter0 = JoinRoom(options.GameId, null).GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 2;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									break;
								case 2u:
									break;
								}
								_0024awaiter0.GetResult();
								UpdateHandlers();
								CheckIsPrivateGame(options);
								Log.Debug("Game joined successfully!");
							}
							catch (Exception ex)
							{
								ex2 = ex;
								num2 = 1;
							}
							switch (num2)
							{
							default:
								goto end_IL_0071;
							case 1:
								_003Cex_003E__1 = ex2;
								Log.Debug(_003Cex_003E__1);
								_0024awaiter0 = Disconnect().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 3;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
							break;
						}
						case 3u:
							break;
						}
						_0024awaiter0.GetResult();
						throw _003Cex_003E__1;
						end_IL_0071:;
					}
					finally
					{
						if (!flag)
						{
							Clear();
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
		private struct _003CFind_003Ec__async2 : IAsyncStateMachine
		{
			internal string region;

			internal MatchmakingFilter filter;

			internal CancellationToken cancellationToken;

			internal ServerUpdatedHandler handler;

			internal SandboxFilter _003CsandboxFilter_003E__1;

			internal bool _003Csuccess_003E__1;

			internal Exception _003Cex_003E__2;

			internal AsyncTaskMethodBuilder<MatchmakingResult> _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			private TaskAwaiter<bool> _0024awaiter1;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				MatchmakingResult result = default(MatchmakingResult);
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						if (region == null)
						{
							throw new ArgumentNullException("region");
						}
						if (filter == null)
						{
							throw new ArgumentNullException("filter");
						}
						Setup(region, filter, cancellationToken);
						num = 4294967293u;
						break;
					case 1u:
					case 2u:
					case 3u:
					case 4u:
					case 5u:
						break;
					}
					try
					{
						Exception ex2;
						int num2;
						switch (num)
						{
						default:
							try
							{
								switch (num)
								{
								default:
									_serverUpdatedHandler = handler;
									goto IL_00a8;
								case 1u:
									_0024awaiter0.GetResult();
									UpdateHandlers();
									_003CsandboxFilter_003E__1 = GetSandboxFilter(filter);
									_0024awaiter0 = JoinOrCreateRoom(filter.GameMode, filter.Levels, filter.ExpectedPlayers, _003CsandboxFilter_003E__1).GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 2;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									goto case 2u;
								case 2u:
									_0024awaiter0.GetResult();
									if (filter.AllowEmptyRoom)
									{
										break;
									}
									_0024awaiter1 = WaitOtherPlayers().GetAwaiter();
									if (!_0024awaiter1.IsCompleted)
									{
										_0024PC = 3;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
										return;
									}
									goto case 3u;
								case 3u:
									_003Csuccess_003E__1 = _0024awaiter1.GetResult();
									if (_003Csuccess_003E__1)
									{
										break;
									}
									_0024awaiter0 = Disconnect().GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 4;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									goto case 4u;
								case 4u:
									{
										_0024awaiter0.GetResult();
										goto IL_00a8;
									}
									IL_00a8:
									_0024awaiter0 = Connect().GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 1;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									goto case 1u;
								}
								Log.Debug("Game found successfully!");
								result = new MatchmakingResult(_photonServer.Location, PhotonNetwork.room.Name);
							}
							catch (Exception ex)
							{
								ex2 = ex;
								num2 = 1;
								goto IL_0278;
							}
							goto end_IL_0079;
						case 5u:
							break;
							IL_0278:
							switch (num2)
							{
							default:
								goto end_IL_0079;
							case 1:
								_003Cex_003E__2 = ex2;
								Log.Debug(_003Cex_003E__2);
								_0024awaiter0 = Disconnect().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 5;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
							break;
						}
						_0024awaiter0.GetResult();
						throw _003Cex_003E__2;
						end_IL_0079:;
					}
					finally
					{
						if (!flag)
						{
							Clear();
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
		private struct _003CWaitOtherPlayers_003Ec__async3 : IAsyncStateMachine
		{
			internal float _003Ctime_003E__0;

			internal float _003Cduration_003E__0;

			internal AsyncTaskMethodBuilder<bool> _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

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
						Log.Debug("Waiting other players");
						_003Ctime_003E__0 = Time.time;
						_003Cduration_003E__0 = UnityEngine.Random.Range(3f, 9f);
						goto IL_00a8;
					case 1u:
						{
							_0024awaiter0.GetResult();
							goto IL_00a8;
						}
						IL_00a8:
						if (PhotonNetwork.playerList.Length > 1 || !(Time.time - _003Ctime_003E__0 < _003Cduration_003E__0))
						{
							break;
						}
						UpdateHandlers();
						_0024awaiter0 = Task.Delay(500).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					}
					result = PhotonNetwork.playerList.Length > 1;
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
		private struct _003CJoinRoom_003Ec__async4 : IAsyncStateMachine
		{
			internal string roomName;

			internal string[] expectedPlayers;

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
						_0024awaiter0 = PhotonAsync.JoinRoom(roomName, expectedPlayers, false).GetAwaiter();
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
		private struct _003CJoinOrCreateRoom_003Ec__async5 : IAsyncStateMachine
		{
			internal GameMode gameMode;

			internal LevelDefinition[] levels;

			internal string[] expectedPlayers;

			internal SandboxFilter sandboxFilter;

			internal bool _003Cfound_003E__0;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<bool> _0024awaiter0;

			private TaskAwaiter<bool> _0024awaiter1;

			private TaskAwaiter _0024awaiter2;

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
						_0024awaiter0 = JoinRandomRoom(gameMode, levels, 19, expectedPlayers, sandboxFilter).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_003Cfound_003E__0 = _0024awaiter0.GetResult();
						UpdateHandlers();
						if (!_003Cfound_003E__0)
						{
							_0024awaiter1 = JoinRandomRoom(gameMode, levels, 199, expectedPlayers, sandboxFilter).GetAwaiter();
							if (!_0024awaiter1.IsCompleted)
							{
								_0024PC = 2;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
								return;
							}
							goto case 2u;
						}
						goto IL_0113;
					case 2u:
						_003Cfound_003E__0 = _0024awaiter1.GetResult();
						UpdateHandlers();
						goto IL_0113;
					case 3u:
						break;
						IL_0113:
						if (!_003Cfound_003E__0)
						{
							_0024awaiter2 = CreateRoom(gameMode, levels, expectedPlayers, sandboxFilter).GetAwaiter();
							if (!_0024awaiter2.IsCompleted)
							{
								_0024PC = 3;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter2, ref this);
								return;
							}
							break;
						}
						goto end_IL_000e;
					}
					_0024awaiter2.GetResult();
					UpdateHandlers();
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
		private struct _003CConnect_003Ec__async6 : IAsyncStateMachine
		{
			internal PhotonServer server;

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
						_0024awaiter0 = PhotonAsync.Connect(server.Ip, 5055, string.Empty, Application.version).GetAwaiter();
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
		private struct _003CDisconnect_003Ec__async7 : IAsyncStateMachine
		{
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
						_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
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
		private struct _003CCreateRoom_003Ec__async8 : IAsyncStateMachine
		{
			internal LevelDefinition[] levels;

			internal GameMode gameMode;

			internal LevelDefinition _003Clevel_003E__0;

			internal SandboxFilter sandboxFilter;

			internal Hashtable _003CroomProperties_003E__0;

			internal bool isVisible;

			internal RoomOptions _003CroomOptions_003E__0;

			internal bool isPrivate;

			internal string _003CroomName_003E__0;

			internal string[] expectedPlayers;

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
						_003Clevel_003E__0 = ((levels.Length <= 0) ? LevelUtility.GetRandomLevel(gameMode.Name) : levels.Random());
						_003CroomProperties_003E__0 = new Hashtable
						{
							{ "C0", gameMode.Name },
							{ "C1", _003Clevel_003E__0.name },
							{
								"C2",
								(byte)0
							},
							{
								"C3",
								(int)sandboxFilter
							}
						};
						_003CroomOptions_003E__0 = new RoomOptions
						{
							PlayerTtl = 25000,
							PublishUserId = true,
							IsVisible = isVisible,
							MaxPlayers = gameMode.MaxPlayers,
							CustomRoomPropertiesForLobby = new string[4] { "C0", "C1", "C2", "C3" },
							CustomRoomProperties = _003CroomProperties_003E__0
						};
						_003CroomName_003E__0 = BuildRoomName(gameMode, isPrivate);
						_0024awaiter0 = PhotonAsync.CreateRoom(_003CroomName_003E__0, _003CroomOptions_003E__0, GetTypedLobby(), expectedPlayers).GetAwaiter();
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
		private struct _003CJoinRandomRoom_003Ec__async9 : IAsyncStateMachine
		{
			internal GameMode gameMode;

			internal int maxGameStateId;

			internal string _003Csql_003E__1;

			internal LevelDefinition[] levels;

			internal SandboxFilter sandboxFilter;

			internal string[] expectedPlayers;

			internal PhotonJoinRoomException _003Cex_003E__2;

			internal AsyncTaskMethodBuilder<bool> _0024builder;

			internal int _0024PC;

			private static Func<LevelDefinition, string> _003C_003Ef__am_0024cache0;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				bool result = default(bool);
				try
				{
					PhotonJoinRoomException ex2;
					int num2;
					switch (num)
					{
					default:
						return;
					case 0u:
						num = 4294967293u;
						goto case 1u;
					case 1u:
					case 2u:
						try
						{
							switch (num)
							{
							default:
								_003Csql_003E__1 = string.Format("{0}=\"{1}\" AND {2}<={3}", "C0", gameMode.name, "C2", maxGameStateId);
								if (levels.Length > 0)
								{
									string arg = string.Join(",", levels.Select((LevelDefinition l) => string.Format("'{0}'", l.name)).ToArray());
									_003Csql_003E__1 += string.Format(" AND {0} IN ({1})", "C1", arg);
								}
								if (sandboxFilter != 0)
								{
									_003Csql_003E__1 += string.Format(" AND {0}={1}", "C3", (int)sandboxFilter);
								}
								_0024awaiter0 = PhotonAsync.JoinRandomRoom(null, gameMode.MaxPlayers, MatchmakingMode.FillRoom, GetTypedLobby(), _003Csql_003E__1, expectedPlayers, false).GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								goto case 1u;
							case 1u:
								_0024awaiter0.GetResult();
								if (IsRoomInvalid())
								{
									_0024awaiter0 = FixInvalidRoom().GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 2;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									break;
								}
								result = true;
								goto end_IL_002e;
							case 2u:
								break;
							}
							_0024awaiter0.GetResult();
							result = false;
							end_IL_002e:;
						}
						catch (PhotonJoinRoomException ex)
						{
							ex2 = ex;
							num2 = 1;
							goto IL_01f4;
						}
						goto end_IL_0010;
					case 3u:
						break;
						IL_01f4:
						switch (num2)
						{
						default:
							goto end_IL_0010;
						case 1:
							_003Cex_003E__2 = ex2;
							Log.Debug(string.Format("State is {0}", PhotonNetwork.connectionState));
							Log.Debug(_003Cex_003E__2);
							_0024awaiter0 = Task.Delay(2000).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 3;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						}
						break;
					}
					_0024awaiter0.GetResult();
					if (_003Cex_003E__2.Code == 32765 || _003Cex_003E__2.Code == 32760 || _003Cex_003E__2.Code == 32758 || _003Cex_003E__2.Code == 32742 || _003Cex_003E__2.Code == 32757)
					{
						result = false;
					}
					else
					{
						Log.Error(_003Cex_003E__2);
						result = false;
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
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}

			private static string _003C_003Em__0(LevelDefinition l)
			{
				return string.Format("'{0}'", l.name);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFixInvalidRoom_003Ec__asyncA : IAsyncStateMachine
		{
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
						Log.Error(string.Format("Room {0} is invalid, set IsVisible=false", PhotonNetwork.room.Name));
						if (!PhotonNetwork.isMessageQueueRunning)
						{
							PhotonNetwork.isMessageQueueRunning = true;
						}
						PhotonNetwork.room.IsVisible = false;
						_0024awaiter0 = Task.Delay(2000).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						_0024awaiter0 = PhotonAsync.LeaveRoom().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 2u:
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

		private static readonly Log Log = Log.Create(typeof(MatchmakingManager));

		private const string PrivateLobbyPrefix = "Closed";

		private const float MinEmptyRoomWaitTime = 3f;

		private const float MaxEmptyRoomWaitTime = 9f;

		public const int PlayerTtl = 25000;

		public const float ReconnectDuration = 20f;

		private const int SandboxTimeInGame = 7200;

		private static string _lobbyName;

		private static CancellationToken _cancellationToken;

		private static PhotonServer _photonServer;

		private static ServerUpdatedHandler _serverUpdatedHandler;

		public static bool InProgress { get; private set; }

		[AsyncStateMachine(typeof(_003CCreate_003Ec__async0))]
		[DebuggerStepThrough]
		public static Task<MatchmakingResult> Create([NotNull] string region, [NotNull] MatchmakingCreateOptions options, ServerUpdatedHandler handler = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003CCreate_003Ec__async0 stateMachine = default(_003CCreate_003Ec__async0);
			stateMachine.region = region;
			stateMachine.options = options;
			stateMachine.cancellationToken = cancellationToken;
			stateMachine.handler = handler;
			stateMachine._0024builder = AsyncTaskMethodBuilder<MatchmakingResult>.Create();
			AsyncTaskMethodBuilder<MatchmakingResult> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CJoin_003Ec__async1))]
		[DebuggerStepThrough]
		public static Task Join([NotNull] string region, MatchmakingJoinOptions options, ServerUpdatedHandler handler = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003CJoin_003Ec__async1 stateMachine = default(_003CJoin_003Ec__async1);
			stateMachine.region = region;
			stateMachine.options = options;
			stateMachine.cancellationToken = cancellationToken;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static void CheckIsPrivateGame(MatchmakingJoinOptions options)
		{
			if (PhotonNetwork.room.Name.StartsWith("Closed") && !PhotonNetwork.room.ExpectedUsers.Contains(options.PlayerId))
			{
				throw new PrivateGameException();
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CFind_003Ec__async2))]
		public static Task<MatchmakingResult> Find([NotNull] string region, [NotNull] MatchmakingFilter filter, ServerUpdatedHandler handler = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003CFind_003Ec__async2 stateMachine = default(_003CFind_003Ec__async2);
			stateMachine.region = region;
			stateMachine.filter = filter;
			stateMachine.cancellationToken = cancellationToken;
			stateMachine.handler = handler;
			stateMachine._0024builder = AsyncTaskMethodBuilder<MatchmakingResult>.Create();
			AsyncTaskMethodBuilder<MatchmakingResult> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static SandboxFilter GetSandboxFilter(MatchmakingOptions filter)
		{
			if (filter.TimeInGame < 0)
			{
				return SandboxFilter.None;
			}
			return (filter.TimeInGame < 7200) ? SandboxFilter.True : SandboxFilter.False;
		}

		private static void Setup(string region, MatchmakingOptions options, CancellationToken cancellationToken)
		{
			if (InProgress)
			{
				throw new InvalidOperationException("FindGame already in progress");
			}
			_cancellationToken = cancellationToken;
			_lobbyName = options.LobbyName;
			if (string.IsNullOrEmpty(_lobbyName))
			{
				_lobbyName = Application.version;
			}
			_photonServer = Regions.Servers.FirstOrDefault((PhotonServer server) => server.Location == region);
			if (_photonServer == null)
			{
				throw new Exception(string.Format("Server with location {0} not found", region));
			}
			Log.Debug(string.Format("Selected region {0}", _photonServer.Location));
			InProgress = true;
			if (!string.IsNullOrEmpty(options.PlayerId))
			{
				PhotonNetwork.AuthValues = new AuthenticationValues(options.PlayerId);
			}
		}

		private static void Clear()
		{
			InProgress = false;
			_cancellationToken = CancellationToken.None;
			_photonServer = null;
			_serverUpdatedHandler = null;
			_lobbyName = null;
		}

		[AsyncStateMachine(typeof(_003CWaitOtherPlayers_003Ec__async3))]
		[DebuggerStepThrough]
		private static Task<bool> WaitOtherPlayers()
		{
			_003CWaitOtherPlayers_003Ec__async3 stateMachine = default(_003CWaitOtherPlayers_003Ec__async3);
			stateMachine._0024builder = AsyncTaskMethodBuilder<bool>.Create();
			AsyncTaskMethodBuilder<bool> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinRoom_003Ec__async4))]
		private static Task JoinRoom(string roomName, string[] expectedPlayers)
		{
			_003CJoinRoom_003Ec__async4 stateMachine = default(_003CJoinRoom_003Ec__async4);
			stateMachine.roomName = roomName;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinOrCreateRoom_003Ec__async5))]
		private static Task JoinOrCreateRoom(GameMode gameMode, LevelDefinition[] levels, string[] expectedPlayers, SandboxFilter sandboxFilter)
		{
			_003CJoinOrCreateRoom_003Ec__async5 stateMachine = default(_003CJoinOrCreateRoom_003Ec__async5);
			stateMachine.gameMode = gameMode;
			stateMachine.levels = levels;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine.sandboxFilter = sandboxFilter;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static void UpdateHandlers()
		{
			_cancellationToken.ThrowIfCancellationRequested();
			if (PhotonNetwork.connected && _serverUpdatedHandler != null)
			{
				_serverUpdatedHandler(_photonServer, PhotonNetwork.countOfPlayers, PhotonNetwork.GetPing());
			}
		}

		private static Task Connect()
		{
			return Connect(_photonServer);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CConnect_003Ec__async6))]
		private static Task Connect(PhotonServer server)
		{
			_003CConnect_003Ec__async6 stateMachine = default(_003CConnect_003Ec__async6);
			stateMachine.server = server;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CDisconnect_003Ec__async7))]
		[DebuggerStepThrough]
		private static Task Disconnect()
		{
			_003CDisconnect_003Ec__async7 stateMachine = default(_003CDisconnect_003Ec__async7);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCreateRoom_003Ec__async8))]
		private static Task CreateRoom(GameMode gameMode, LevelDefinition[] levels, string[] expectedPlayers, SandboxFilter sandboxFilter, bool isVisible = true, bool isPrivate = false)
		{
			_003CCreateRoom_003Ec__async8 stateMachine = default(_003CCreateRoom_003Ec__async8);
			stateMachine.levels = levels;
			stateMachine.gameMode = gameMode;
			stateMachine.sandboxFilter = sandboxFilter;
			stateMachine.isVisible = isVisible;
			stateMachine.isPrivate = isPrivate;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static string BuildRoomName(GameMode gameMode, bool isPrivate)
		{
			string text = gameMode.name + "_" + Guid.NewGuid();
			if (isPrivate)
			{
				text = "Closed_" + text;
			}
			return text;
		}

		private static TypedLobby GetTypedLobby()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Lobby {0}", _lobbyName));
			}
			return new TypedLobby(_lobbyName, LobbyType.SqlLobby);
		}

		[AsyncStateMachine(typeof(_003CJoinRandomRoom_003Ec__async9))]
		[DebuggerStepThrough]
		private static Task<bool> JoinRandomRoom(GameMode gameMode, LevelDefinition[] levels, int maxGameStateId, string[] expectedPlayers, SandboxFilter sandboxFilter)
		{
			_003CJoinRandomRoom_003Ec__async9 stateMachine = default(_003CJoinRandomRoom_003Ec__async9);
			stateMachine.gameMode = gameMode;
			stateMachine.maxGameStateId = maxGameStateId;
			stateMachine.levels = levels;
			stateMachine.sandboxFilter = sandboxFilter;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine._0024builder = AsyncTaskMethodBuilder<bool>.Create();
			AsyncTaskMethodBuilder<bool> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CFixInvalidRoom_003Ec__asyncA))]
		[DebuggerStepThrough]
		private static Task FixInvalidRoom()
		{
			_003CFixInvalidRoom_003Ec__asyncA stateMachine = default(_003CFixInvalidRoom_003Ec__asyncA);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static bool IsRoomInvalid()
		{
			return PhotonNetwork.room.CustomProperties.Count == 0;
		}
	}
}
