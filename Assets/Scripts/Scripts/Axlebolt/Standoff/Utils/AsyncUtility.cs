using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Utils
{
	public class AsyncUtility
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CAsync_003Ec__async0 : IAsyncStateMachine
		{
			internal View view;

			internal Task task;

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
						view.Show();
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
							_0024awaiter0 = task.GetAwaiter();
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
					finally
					{
						if (!flag)
						{
							view.Hide();
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
		private struct _003CAsyncComplete_003Ec__async1 : IAsyncStateMachine
		{
			internal Dialog _003Cview_003E__0;

			internal Task task;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			private static Action _003C_003Ef__am_0024cache0;

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
						_003Cview_003E__0 = Dialogs.Create(ScriptLocalization.Dialogs.Processing, ScriptLocalization.Common.PleaseWait);
						_003Cview_003E__0.Background = false;
						_003Cview_003E__0.Show();
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
							_0024awaiter0 = task.GetAwaiter();
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
					catch (Exception message)
					{
						Log.Error(message);
						Dialogs.Message(ScriptLocalization.Common.Error, ScriptLocalization.Dialogs.RequestFailed, delegate
						{
						});
					}
					finally
					{
						if (!flag)
						{
							_003Cview_003E__0.Hide();
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

			private static void _003C_003Em__0()
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CAsyncComplete_003Ec__async2 : IAsyncStateMachine
		{
			internal Dialog _003Cview_003E__0;

			internal IEnumerator task;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

			private static Action _003C_003Ef__am_0024cache0;

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
						_003Cview_003E__0 = Dialogs.Create(ScriptLocalization.Dialogs.Processing, ScriptLocalization.Common.PleaseWait);
						_003Cview_003E__0.Background = false;
						_003Cview_003E__0.Show();
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
							_0024awaiter0 = task.GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						case 1u:
							break;
						}
						_0024awaiter0.GetResult();
					}
					catch (Exception message)
					{
						Log.Error(message);
						Dialogs.Message(ScriptLocalization.Common.Error, ScriptLocalization.Dialogs.RequestFailed, delegate
						{
						});
					}
					finally
					{
						if (!flag)
						{
							_003Cview_003E__0.Hide();
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

			private static void _003C_003Em__0()
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CStartCoroutine_003Ec__async4 : IAsyncStateMachine
		{
			internal IEnumerator enumerator;

			internal AsyncVoidMethodBuilder _0024builder;

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
						_0024awaiter0 = enumerator.GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
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

		private static readonly Log Log = Log.Create(typeof(AsyncUtility));

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CAsync_003Ec__async0))]
		public static Task Async(View view, Task task)
		{
			_003CAsync_003Ec__async0 stateMachine = default(_003CAsync_003Ec__async0);
			stateMachine.view = view;
			stateMachine.task = task;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CAsyncComplete_003Ec__async1))]
		[DebuggerStepThrough]
		public static Task AsyncComplete(Task task)
		{
			_003CAsyncComplete_003Ec__async1 stateMachine = default(_003CAsyncComplete_003Ec__async1);
			stateMachine.task = task;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CAsyncComplete_003Ec__async2))]
		[DebuggerStepThrough]
		public static Task AsyncComplete(IEnumerator task)
		{
			_003CAsyncComplete_003Ec__async2 stateMachine = default(_003CAsyncComplete_003Ec__async2);
			stateMachine.task = task;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public static IEnumerator WaitFrame()
		{
			yield return null;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CStartCoroutine_003Ec__async4))]
		public static void StartCoroutine(IEnumerator enumerator)
		{
			_003CStartCoroutine_003Ec__async4 stateMachine = default(_003CStartCoroutine_003Ec__async4);
			stateMachine.enumerator = enumerator;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}

