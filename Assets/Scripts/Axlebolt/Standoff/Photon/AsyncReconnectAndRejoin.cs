using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncReconnectAndRejoin : AsyncOperation<AsyncReconnectAndRejoin>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CReconnectAndRejoin_003Ec__async0 : IAsyncStateMachine
		{
			internal bool isMessageQueueRunning;

			internal AsyncReconnectAndRejoin _0024this;

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
						if (PhotonNetwork.ReconnectAndRejoin())
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
						_0024this.Result = new AsyncResult(new ReconnectException());
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

		[AsyncStateMachine(typeof(_003CReconnectAndRejoin_003Ec__async0))]
		[DebuggerStepThrough]
		public Task ReconnectAndRejoin(bool isMessageQueueRunning = true)
		{
			_003CReconnectAndRejoin_003Ec__async0 stateMachine = default(_003CReconnectAndRejoin_003Ec__async0);
			stateMachine.isMessageQueueRunning = isMessageQueueRunning;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		public override void OnConnectionFail(DisconnectCause cause)
		{
			AsyncOperation<AsyncReconnectAndRejoin>.Log.Debug(string.Format("OnConnectionFail {0}", cause));
			Result = new AsyncResult(new PhotonConnectionFailedException(cause));
		}

		public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
			AsyncOperation<AsyncReconnectAndRejoin>.Log.Debug(string.Format("OnPhotonJoinRoomFailed {0}", codeAndMsg));
			Result = new AsyncResult(new PhotonJoinRoomException((short)codeAndMsg[0], (string)codeAndMsg[1]));
		}

		public override void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			AsyncOperation<AsyncReconnectAndRejoin>.Log.Debug(string.Format("OnFailedToConnectToPhoton {0}", cause));
			Result = new AsyncResult(new PhotonConnectionFailedException(cause));
		}

		public override void OnJoinedRoom()
		{
			AsyncOperation<AsyncReconnectAndRejoin>.Log.Debug("OnJoinedRoom");
			PhotonNetwork.isMessageQueueRunning = _isMessageQueueRunning;
			Result = new AsyncResult();
		}
	}
}
