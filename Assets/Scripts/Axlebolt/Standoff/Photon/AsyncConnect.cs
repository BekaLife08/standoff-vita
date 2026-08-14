using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncConnect : AsyncOperation<AsyncConnect>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CConnectUsingSettings_003Ec__async0 : IAsyncStateMachine
		{
			internal string gameVersion;

			internal AsyncConnect _0024this;

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
							if (PhotonNetwork.ConnectUsingSettings(gameVersion))
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CConnect_003Ec__async1 : IAsyncStateMachine
		{
			internal string masterServerAddress;

			internal int port;

			internal string appId;

			internal string gameVersion;

			internal AsyncConnect _0024this;

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
							AsyncOperation<AsyncConnect>.Log.Debug(string.Format("Connect to {0}", masterServerAddress));
							if (PhotonNetwork.ConnectToMaster(masterServerAddress, port, appId, gameVersion))
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

		[AsyncStateMachine(typeof(_003CConnectUsingSettings_003Ec__async0))]
		[DebuggerStepThrough]
		public Task ConnectUsingSettings(string gameVersion)
		{
			_003CConnectUsingSettings_003Ec__async0 stateMachine = default(_003CConnectUsingSettings_003Ec__async0);
			stateMachine.gameVersion = gameVersion;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CConnect_003Ec__async1))]
		public Task Connect(string masterServerAddress, int port, string appId, string gameVersion)
		{
			_003CConnect_003Ec__async1 stateMachine = default(_003CConnect_003Ec__async1);
			stateMachine.masterServerAddress = masterServerAddress;
			stateMachine.port = port;
			stateMachine.appId = appId;
			stateMachine.gameVersion = gameVersion;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		public override void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			if (AsyncOperation<AsyncConnect>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncConnect>.Log.Debug(string.Format("OnFailedToConnectToPhoton({0})", cause));
			}
			Result = new AsyncResult(new PhotonConnectionFailedException(cause));
		}

		public override void OnConnectedToMaster()
		{
			if (AsyncOperation<AsyncConnect>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncConnect>.Log.Debug("OnConnectedToMaster");
			}
			Result = new AsyncResult();
		}
	}
}
