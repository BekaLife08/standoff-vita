using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class CoroutineAsyncExtensions
{
	private class Helper : MonoBehaviour
	{
		private static Helper _instance;

		internal static Helper Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject gameObject = new GameObject("CoroutineAsyncBridge");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					GameObject gameObject2 = gameObject;
					_instance = gameObject2.AddComponent<Helper>();
				}
				return _instance;
			}
		}

		private void OnDestroy()
		{
			_instance = null;
		}
	}

	public class CoroutineAsyncBridge : INotifyCompletion
	{
		private Action _continuation;

		private Exception _exception;

		public bool IsCompleted
		{
			get;
			private set;
		}

		private CoroutineAsyncBridge()
		{
			IsCompleted = false;
		}

		public static CoroutineAsyncBridge Start(IEnumerator awaitTarget)
		{
			CoroutineAsyncBridge coroutineAsyncBridge = new CoroutineAsyncBridge();
			Helper.Instance.StartCoroutine(coroutineAsyncBridge.Run(awaitTarget));
			return coroutineAsyncBridge;
		}

		private IEnumerator Run(IEnumerator coroutine)
		{
			yield return coroutine;
			IsCompleted = true;
			_continuation();
		}

		public void OnCompleted(Action continuation)
		{
			_continuation = continuation;
		}

		public void GetResult()
		{
			if (!IsCompleted)
			{
				throw new InvalidOperationException("coroutine not yet completed");
			}
			if (_exception != null)
			{
				throw _exception;
			}
		}
	}

	public static CoroutineAsyncBridge GetAwaiter(this IEnumerator coroutine)
	{
		return CoroutineAsyncBridge.Start(coroutine);
	}
}
