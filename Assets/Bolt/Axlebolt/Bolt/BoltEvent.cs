using System;
using System.Collections.Generic;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt
{
	public class BoltEvent<T>
	{
		private readonly List<Action<T>> _listeners;

		public BoltEvent()
		{
			_listeners = new List<Action<T>>();
		}

		internal void Invoke(T arg)
		{
			BoltApi.Instance.ExecuteInMainThread(delegate
			{
				Action<T>[] array = _listeners.ToArray();
				foreach (Action<T> action in array)
				{
					try
					{
						action(arg);
					}
					catch (Exception arg2)
					{
						Logger.Error(string.Format("EventListener ({0}) failed. {1}", action, arg2));
					}
				}
			});
		}

		public void AddListener(Action<T> listener)
		{
			if (_listeners.Contains(listener))
			{
				Logger.Error(string.Format("EventListener ({0}) already added", listener));
			}
			else
			{
				_listeners.Add(listener);
			}
		}

		public void RemoveListener(Action<T> listener)
		{
			_listeners.Remove(listener);
		}
	}
	public class BoltEvent
	{
		private readonly List<Action> _listeners;

		public BoltEvent()
		{
			_listeners = new List<Action>();
		}

		internal void Invoke()
		{
			BoltApi.Instance.ExecuteInMainThread(delegate
			{
				Action[] array = _listeners.ToArray();
				foreach (Action action in array)
				{
					try
					{
						action();
					}
					catch (Exception arg)
					{
						Logger.Error(string.Format("EventListener ({0}) failed. {1}", action, arg));
					}
				}
			});
		}

		public void AddListener(Action listener)
		{
			if (_listeners.Contains(listener))
			{
				Logger.Error(string.Format("EventListener ({0}) already added", listener));
			}
			else
			{
				_listeners.Add(listener);
			}
		}

		public void RemoveListener(Action listener)
		{
			_listeners.Remove(listener);
		}
	}
}
