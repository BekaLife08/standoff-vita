using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Photon
{
	public class PhotonAsync
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CConnect_003Ec__async0 : IAsyncStateMachine
		{
			internal string masterServerAddress;

			internal int port;

			internal string appId;

			internal string gameVersion;

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
						_0024awaiter0 = AsyncOperation<AsyncConnect>.Create().Connect(masterServerAddress, port, appId, gameVersion).GetAwaiter();
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
		private struct _003CConnectUsingSettings_003Ec__async1 : IAsyncStateMachine
		{
			internal string gameVersion;

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
						_0024awaiter0 = AsyncOperation<AsyncConnect>.Create().ConnectUsingSettings(gameVersion).GetAwaiter();
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
		private struct _003CJoinRandomRoom_003Ec__async2 : IAsyncStateMachine
		{
			internal Hashtable expectedCustomRoomProperties;

			internal byte expectedMaxPlayers;

			internal MatchmakingMode matchingType;

			internal TypedLobby typedLobby;

			internal string sqlLobbyFilter;

			internal string[] expectedUsers;

			internal bool isMessageQueueRunning;

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
						_0024awaiter0 = AsyncOperation<AsyncJoinRandomRoom>.Create().JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, matchingType, typedLobby, sqlLobbyFilter, expectedUsers, isMessageQueueRunning).GetAwaiter();
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
		private struct _003CCreateRoom_003Ec__async3 : IAsyncStateMachine
		{
			internal string roomName;

			internal RoomOptions roomOptions;

			internal TypedLobby typedLobby;

			internal string[] expectedUsers;

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
						_0024awaiter0 = AsyncOperation<AsyncCreateRoom>.Create().CreateRoom(roomName, roomOptions, typedLobby, expectedUsers).GetAwaiter();
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
		private struct _003CJoinRoom_003Ec__async4 : IAsyncStateMachine
		{
			internal string roomName;

			internal string[] expectedUsers;

			internal bool isMessageQueueRunning;

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
						_0024awaiter0 = AsyncOperation<AsyncJoinRoom>.Create().JoinRoom(roomName, expectedUsers, isMessageQueueRunning).GetAwaiter();
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
		private struct _003CReconnectAndRejoin_003Ec__async5 : IAsyncStateMachine
		{
			internal bool isMessageQueueRunning;

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
						_0024awaiter0 = AsyncOperation<AsyncReconnectAndRejoin>.Create().ReconnectAndRejoin(isMessageQueueRunning).GetAwaiter();
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
		private struct _003CDisconnect_003Ec__async6 : IAsyncStateMachine
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
						_0024awaiter0 = AsyncOperation<AsyncDisconnect>.Create().Disconnect().GetAwaiter();
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
		private struct _003CLeaveRoom_003Ec__async7 : IAsyncStateMachine
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
						_0024awaiter0 = AsyncOperation<AsyncLeaveRoom>.Create().LeaveRoom().GetAwaiter();
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

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CConnect_003Ec__async0))]
		public static Task Connect(string masterServerAddress, int port, string appId, string gameVersion)
		{
			_003CConnect_003Ec__async0 stateMachine = default(_003CConnect_003Ec__async0);
			stateMachine.masterServerAddress = masterServerAddress;
			stateMachine.port = port;
			stateMachine.appId = appId;
			stateMachine.gameVersion = gameVersion;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CConnectUsingSettings_003Ec__async1))]
		[DebuggerStepThrough]
		public static Task ConnectUsingSettings(string gameVersion)
		{
			_003CConnectUsingSettings_003Ec__async1 stateMachine = default(_003CConnectUsingSettings_003Ec__async1);
			stateMachine.gameVersion = gameVersion;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinRandomRoom_003Ec__async2))]
		public static Task JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers, bool isMessageQueueRunning)
		{
			_003CJoinRandomRoom_003Ec__async2 stateMachine = default(_003CJoinRandomRoom_003Ec__async2);
			stateMachine.expectedCustomRoomProperties = expectedCustomRoomProperties;
			stateMachine.expectedMaxPlayers = expectedMaxPlayers;
			stateMachine.matchingType = matchingType;
			stateMachine.typedLobby = typedLobby;
			stateMachine.sqlLobbyFilter = sqlLobbyFilter;
			stateMachine.expectedUsers = expectedUsers;
			stateMachine.isMessageQueueRunning = isMessageQueueRunning;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCreateRoom_003Ec__async3))]
		public static Task CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
		{
			_003CCreateRoom_003Ec__async3 stateMachine = default(_003CCreateRoom_003Ec__async3);
			stateMachine.roomName = roomName;
			stateMachine.roomOptions = roomOptions;
			stateMachine.typedLobby = typedLobby;
			stateMachine.expectedUsers = expectedUsers;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinRoom_003Ec__async4))]
		public static Task JoinRoom(string roomName, string[] expectedUsers, bool isMessageQueueRunning)
		{
			_003CJoinRoom_003Ec__async4 stateMachine = default(_003CJoinRoom_003Ec__async4);
			stateMachine.roomName = roomName;
			stateMachine.expectedUsers = expectedUsers;
			stateMachine.isMessageQueueRunning = isMessageQueueRunning;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CReconnectAndRejoin_003Ec__async5))]
		[DebuggerStepThrough]
		public static Task ReconnectAndRejoin(bool isMessageQueueRunning = true)
		{
			_003CReconnectAndRejoin_003Ec__async5 stateMachine = default(_003CReconnectAndRejoin_003Ec__async5);
			stateMachine.isMessageQueueRunning = isMessageQueueRunning;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CDisconnect_003Ec__async6))]
		public static Task Disconnect()
		{
			_003CDisconnect_003Ec__async6 stateMachine = default(_003CDisconnect_003Ec__async6);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CLeaveRoom_003Ec__async7))]
		[DebuggerStepThrough]
		public static Task LeaveRoom()
		{
			_003CLeaveRoom_003Ec__async7 stateMachine = default(_003CLeaveRoom_003Ec__async7);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}
	}
}
