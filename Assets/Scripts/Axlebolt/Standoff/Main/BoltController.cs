using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Unity;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Auth;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Main
{
	public class BoltController : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CReconnect_003Ec__async0 : IAsyncStateMachine
		{
			internal BoltController _0024this;

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
								_0024awaiter0 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ScriptLocalization.Dialogs.TryingToReconnect), BoltApi.Instance.Reconnect()).GetAwaiter();
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
						_0024this.OnReconnectedEvent();
					}
					catch (ConnectionFailedException)
					{
						DialogButton dialogButton = new DialogButton(ScriptLocalization.Common.Ok);
						Dialog dialog = Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ScriptLocalization.Dialogs.CheckInternetConnection, dialogButton);
						BoltController _024this = _0024this;
						dialog.Show(delegate
						{
							_024this.Reconnect();
						});
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						DialogButton dialogButton2 = new DialogButton(ScriptLocalization.Common.Ok);
						Dialog dialog2 = Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ex2.Message, dialogButton2);
						BoltController _024this = _0024this;
						dialog2.Show(delegate
						{
							_024this.Reconnect();
						});
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

		private static readonly Log Log = Log.Create(typeof(BoltController));

		[SerializeField]
		private AuthController _authController;

		public event Action ConnectedEvent;

		public event Action ReconnectedEvent;

		public event Action ConnectionFailedEvent;

		private void Awake()
		{
			if (!BoltApi.IsInitialized)
			{
				BoltUnityApi.Init();
			}
			BoltApi.Instance.ConnectionFailedEvent.AddListener(OnConnectionFailedEvent);
		}

		public void ConnectToBolt()
		{
			if (!BoltApi.Instance.IsAuthenticated)
			{
				_authController.Authenticate(OnConnectedEvent);
			}
			else if (BoltApi.Instance.IsConnectedAndReady)
			{
				OnConnectedEvent();
			}
			else
			{
				Reconnect();
			}
		}

		[AsyncStateMachine(typeof(_003CReconnect_003Ec__async0))]
		[DebuggerStepThrough]
		private void Reconnect()
		{
			_003CReconnect_003Ec__async0 stateMachine = default(_003CReconnect_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void OnDestroy()
		{
			if (BoltApi.IsInitialized)
			{
				BoltApi.Instance.ConnectionFailedEvent.RemoveListener(OnConnectionFailedEvent);
			}
		}

		protected virtual void OnConnectedEvent()
		{
			if (this.ConnectedEvent != null)
			{
				this.ConnectedEvent();
			}
		}

		protected virtual void OnReconnectedEvent()
		{
			if (this.ReconnectedEvent != null)
			{
				this.ReconnectedEvent();
			}
		}

		protected virtual void OnConnectionFailedEvent()
		{
			if (this.ConnectionFailedEvent != null)
			{
				this.ConnectionFailedEvent();
			}
			Reconnect();
		}
	}
}
