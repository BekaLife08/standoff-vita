using System;
using System.Collections;
using System.Threading.Tasks;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Utils
{
	public class AsyncUtility
	{
		private static readonly Log Log = Log.Create(typeof(AsyncUtility));

		public static Task Async(View view, Task task)
		{
			return AsyncInternal(view, task);
		}

		private static async Task AsyncInternal(View view, Task task)
		{
			view.Show();
			try
			{
				await task;
			}
			finally
			{
				view.Hide();
			}
		}

		public static Task AsyncComplete(Task task)
		{
			return AsyncCompleteInternal(task);
		}

		public static Task AsyncComplete(IEnumerator task)
		{
			return AsyncCompleteInternal(task);
		}

		private static async Task AsyncCompleteInternal(Task task)
		{
			Dialog dialog = Dialogs.Create(ScriptLocalization.Dialogs.Processing, ScriptLocalization.Common.PleaseWait);
			dialog.Background = false;
			dialog.Show();
			try
			{
				await task;
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
				dialog.Hide();
			}
		}

		private static async Task AsyncCompleteInternal(IEnumerator task)
		{
			Dialog dialog = Dialogs.Create(ScriptLocalization.Dialogs.Processing, ScriptLocalization.Common.PleaseWait);
			dialog.Background = false;
			dialog.Show();
			try
			{
				await task;
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
				dialog.Hide();
			}
		}

		public static IEnumerator WaitFrame()
		{
			yield return null;
		}

		public static void StartCoroutine(IEnumerator enumerator)
		{
			StartCoroutineInternal(enumerator);
		}

		private static async void StartCoroutineInternal(IEnumerator enumerator)
		{
			try
			{
				await enumerator;
			}
			catch (Exception exception)
			{
				Log.Error(exception);
			}
		}
	}
}
