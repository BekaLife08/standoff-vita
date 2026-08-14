using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncLeaveRoom : AsyncOperation<AsyncLeaveRoom>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLeaveRoom_003Ec__async0 : IAsyncStateMachine
		{
			internal AsyncLeaveRoom _0024this;

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
						AsyncOperation<AsyncLeaveRoom>.Log.Debug("Leave room");
						if (PhotonNetwork.LeaveRoom())
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
						_0024this.Result = new AsyncResult();
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

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CLeaveRoom_003Ec__async0))]
		public Task LeaveRoom()
		{
			_003CLeaveRoom_003Ec__async0 stateMachine = default(_003CLeaveRoom_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		public override void OnLeftRoom()
		{
			AsyncOperation<AsyncLeaveRoom>.Log.Debug("OnLeftRoom");
		}

		public override void OnConnectedToMaster()
		{
			AsyncOperation<AsyncLeaveRoom>.Log.Debug("OnConnectedToMaster");
			Result = new AsyncResult();
		}

		public override void OnConnectionFail(DisconnectCause cause)
		{
			AsyncOperation<AsyncLeaveRoom>.Log.Warning(string.Format("OnConnectionFail {0}", cause));
			Result = new AsyncResult(new PhotonConnectionFailedException(cause));
		}
	}
}
