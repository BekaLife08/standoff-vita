using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt.Api.Exception;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Analytics;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public class AuthController : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CAuthenticateBolt_003Ec__async0 : IAsyncStateMachine
		{
			internal Dialog _003Cdialog_003E__1;

			internal AuthService service;

			internal AuthController _0024this;

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
								_003Cdialog_003E__1 = Dialogs.Create(ScriptLocalization.Common.PleaseWait, ScriptLocalization.Dialogs.LoadingPlayerProfile);
								_003Cdialog_003E__1.Background = false;
								_0024awaiter0 = AsyncUtility.Async(_003Cdialog_003E__1, service.AuthenticateBolt()).GetAwaiter();
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
						_0024this.CallbackResult();
					}
					catch (ConnectionFailedException ex)
					{
						_0024this.BoltConnectionFailed(service, ex);
					}
					catch (GoogleBadRequestRpcException ex2)
					{
						_0024this.GooglePlayerServerException(service, ex2);
					}
					catch (GoogleInvalidCodeRpcException ex3)
					{
						_0024this.GooglePlayerServerException(service, ex3);
					}
					catch (GoogleCodeAlreadyRedeemedRpcException ex4)
					{
						_0024this.GooglePlayerServerException(service, ex4);
					}
					catch (Exception ex5)
					{
						_0024this.BoltUnknwonError(service, ex5);
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

		private static readonly Log Log = Log.Create(typeof(AuthController));

		[SerializeField]
		private AuthService _gpgsService;

		[SerializeField]
		private AuthService _testService;

		private Action _callback;

		public void Authenticate([NotNull] Action callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (_callback != null)
			{
				throw new InvalidOperationException("Authentication already in progress");
			}
			_callback = callback;
			Authenticate((Application.platform != RuntimePlatform.Android) ? _testService : _gpgsService);
		}

		private void Authenticate(AuthService service)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Authenticate {0}", service));
			}
			Dialog dialog = Dialogs.Create(ScriptLocalization.Auth.Connecting, service.Sprite);
			dialog.Background = false;
			dialog.Show();
			service.Authenticate(delegate (bool result, string errorMsg)
			{
				dialog.Hide();
				if (Log.DebugEnabled)
				{
					Log.Debug(string.Format("Service result {0}", result));
				}
				if (result)
				{
					AuthenticateBolt(service);
				}
				else
				{
					ServiceError(service, errorMsg);
				}
			});
		}

		private void ServiceError(AuthService service, string msg)
		{
			DialogButton dialogButton = new DialogButton(ScriptLocalization.Common.Ok);
			Dialog dialog = ((!string.IsNullOrEmpty(msg)) ? Dialogs.Create(ScriptLocalization.Auth.ConnectionError, msg, dialogButton) : Dialogs.Create(ScriptLocalization.Auth.ConnectionError, service.Sprite, dialogButton));
			dialog.Background = false;
			dialog.Show(delegate
			{
				service.Logout();
				Authenticate(service);
			});
		}

		[AsyncStateMachine(typeof(_003CAuthenticateBolt_003Ec__async0))]
		[DebuggerStepThrough]
		private void AuthenticateBolt(AuthService service)
		{
			_003CAuthenticateBolt_003Ec__async0 stateMachine = default(_003CAuthenticateBolt_003Ec__async0);
			stateMachine.service = service;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void BoltUnknwonError(AuthService service, Exception ex)
		{
			Log.Error(ex);
			Dialog dialog = Dialogs.Message(ScriptLocalization.Dialogs.ConnectionFailed, ex.Message, delegate
			{
				service.Logout();
				Authenticate(service);
			});
			dialog.Background = false;
			AnalyticsManager.BoltAuthError(ex.GetType().Name);
		}

		private void GooglePlayerServerException(AuthService service, Exception ex)
		{
			Log.Debug(ex);
			Dialogs.Message(ScriptLocalization.Dialogs.ConnectionFailed, ScriptLocalization.Auth.GooglePlayerServerError, delegate
			{
				service.Logout();
				Authenticate(service);
			}).Background = false;
			AnalyticsManager.BoltAuthError((ex != null) ? ex.GetType().Name : null);
		}

		private void BoltConnectionFailed(AuthService service, Exception ex)
		{
			Log.Debug(ex);
			Dialog dialog = Dialogs.Message(ScriptLocalization.Dialogs.ConnectionFailed, ScriptLocalization.Dialogs.CheckInternetConnection, delegate
			{
				AuthenticateBolt(service);
			});
			dialog.Background = false;
		}

		private void CallbackResult()
		{
			Action callback = _callback;
			_callback = null;
			callback();
		}
	}
}
