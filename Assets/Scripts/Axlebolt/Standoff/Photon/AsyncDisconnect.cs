using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncDisconnect : AsyncOperation<AsyncDisconnect>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CDisconnect_003Ec__async0 : IAsyncStateMachine
		{
			internal AsyncDisconnect _0024this;

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
						AsyncOperation<AsyncDisconnect>.Log.Debug("Disconnect");
						if (PhotonNetwork.connectionState != 0)
						{
							PhotonNetwork.isMessageQueueRunning = true;
							PhotonNetwork.Disconnect();
							if (_0024this.Result == null)
							{
								_0024awaiter0 = _0024this.WaitForResult().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
							_0024this.Done();
							goto end_IL_000e;
						}
						_0024this.Result = new AsyncResult();
						_0024this.Done();
						goto end_IL_000e;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
					_0024this.Done();
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

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CDisconnect_003Ec__async0))]
		public Task Disconnect()
		{
			_003CDisconnect_003Ec__async0 stateMachine = default(_003CDisconnect_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public override void OnDisconnectedFromPhoton()
		{
			AsyncOperation<AsyncDisconnect>.Log.Debug("OnDisconnectedFromPhoton");
			Result = new AsyncResult();
		}
	}
}
