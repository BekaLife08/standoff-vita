using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Player;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Core;
using Firebase;
using Firebase.Analytics;
using Firebase.Messaging;

namespace Axlebolt.Standoff.Main
{
	public class FirebaseManager : Singleton<FirebaseManager>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnTokenReceived_003Ec__async0 : IAsyncStateMachine
		{
			internal TokenReceivedEventArgs token;

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
							Log.Debug(string.Format("Received Registration Token: {0}", token.Token));
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
								_0024awaiter0 = BoltService<BoltPlayerService>.Instance.SetFirebaseToken(token.Token).GetAwaiter();
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
					catch (ConnectionFailedException)
					{
					}
					catch (Exception message)
					{
						Log.Error(message);
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

		private static readonly Log Log = Log.Create(typeof(FirebaseManager));

		private void Awake()
		{
		}

		private void InitAnalytics()
		{
			FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
		}

		[AsyncStateMachine(typeof(_003COnTokenReceived_003Ec__async0))]
		[DebuggerStepThrough]
		public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
		{
			_003COnTokenReceived_003Ec__async0 stateMachine = default(_003COnTokenReceived_003Ec__async0);
			stateMachine.token = token;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			Log.Debug(string.Format("Received a new message from: {0}", e.Message));
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
	}
}
