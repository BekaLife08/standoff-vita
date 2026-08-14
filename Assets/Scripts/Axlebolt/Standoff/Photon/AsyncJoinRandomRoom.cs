using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Photon
{
	internal class AsyncJoinRandomRoom : AsyncOperation<AsyncJoinRandomRoom>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CJoinRandomRoom_003Ec__async0 : IAsyncStateMachine
		{
			internal bool isMessageQueueRunning;

			internal string sqlLobbyFilter;

			internal Hashtable expectedCustomRoomProperties;

			internal byte expectedMaxPlayers;

			internal MatchmakingMode matchingType;

			internal TypedLobby typedLobby;

			internal string[] expectedUsers;

			internal AsyncJoinRandomRoom _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

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
						_0024this._isMessageQueueRunning = isMessageQueueRunning;
						AsyncOperation<AsyncJoinRandomRoom>.Log.Debug(string.Format("JoinRandomRoom ({0})", sqlLobbyFilter));
						if (PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, matchingType, typedLobby, sqlLobbyFilter, expectedUsers))
						{
							_0024awaiter0 = _0024this.WaitForResult().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
						_0024this.Result = new AsyncResult(new PhotonConnectionFailedException());
						_0024this.Done();
						break;
					case 1u:
						_0024awaiter0.GetResult();
						_0024this.Done();
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
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		private bool _isMessageQueueRunning;

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinRandomRoom_003Ec__async0))]
		public Task JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers, bool isMessageQueueRunning)
		{
			_003CJoinRandomRoom_003Ec__async0 stateMachine = default(_003CJoinRandomRoom_003Ec__async0);
			stateMachine.isMessageQueueRunning = isMessageQueueRunning;
			stateMachine.sqlLobbyFilter = sqlLobbyFilter;
			stateMachine.expectedCustomRoomProperties = expectedCustomRoomProperties;
			stateMachine.expectedMaxPlayers = expectedMaxPlayers;
			stateMachine.matchingType = matchingType;
			stateMachine.typedLobby = typedLobby;
			stateMachine.expectedUsers = expectedUsers;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
			if (AsyncOperation<AsyncJoinRandomRoom>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncJoinRandomRoom>.Log.Debug("OnPhotonRandomJoinFailed");
			}
			Result = new AsyncResult(new PhotonJoinRoomException((short)codeAndMsg[0], (string)codeAndMsg[1]));
		}

		public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
			if (AsyncOperation<AsyncJoinRandomRoom>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncJoinRandomRoom>.Log.Debug("OnPhotonJoinRoomFailed");
			}
			Result = new AsyncResult(new PhotonJoinRoomException((short)codeAndMsg[0], (string)codeAndMsg[1]));
		}

		public override void OnJoinedRoom()
		{
			if (AsyncOperation<AsyncJoinRandomRoom>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncJoinRandomRoom>.Log.Debug("OnJoinedRoom");
			}
			PhotonNetwork.isMessageQueueRunning = _isMessageQueueRunning;
			Result = new AsyncResult();
		}

		public override void OnConnectionFail(DisconnectCause cause)
		{
			AsyncOperation<AsyncJoinRandomRoom>.Log.Warning(string.Format("OnConnectionFail {0}", cause));
			Result = new AsyncResult(new PhotonConnectionFailedException(cause));
		}
	}
}
