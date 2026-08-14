using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Core
{
	public class Event<T>
	{
		private static readonly Log Log = Log.Create(typeof(Event<T>));

		private readonly List<Action<T>> _listeners = new List<Action<T>>();

		public void AddListener([NotNull] Action<T> listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Add(listener);
		}

		public void Invoke(T arg)
		{
			for (int i = 0; i < _listeners.Count; i++)
			{
				try
				{
					_listeners[i](arg);
				}
				catch (Exception message)
				{
					Log.Error(message);
				}
			}
		}
	}
	public class Event<T0, T1>
	{
		private static readonly Log Log = Log.Create(typeof(Event<T0, T1>));

		private readonly List<Action<T0, T1>> _listeners = new List<Action<T0, T1>>();

		public void AddListener([NotNull] Action<T0, T1> listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Add(listener);
		}

		public void Invoke(T0 arg0, T1 arg1)
		{
			for (int i = 0; i < _listeners.Count; i++)
			{
				try
				{
					_listeners[i](arg0, arg1);
				}
				catch (Exception message)
				{
					Log.Error(message);
				}
			}
		}
	}
}
