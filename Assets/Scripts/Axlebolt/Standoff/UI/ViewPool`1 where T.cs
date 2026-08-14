using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class ViewPool<T> where T : View
	{
		private readonly T[] _pool;

		public T[] Items
		{
			get;
			private set;
		} = new T[0];


		public ViewPool([NotNull] T viewPrefab, int size)
		{
			if ((UnityEngine.Object)viewPrefab == (UnityEngine.Object)null)
			{
				throw new ArgumentNullException("viewPrefab");
			}
			_pool = new T[size];
			for (int i = 0; i < size; i++)
			{
				T val = UnityEngine.Object.Instantiate(viewPrefab, viewPrefab.transform.parent, worldPositionStays: false);
				val.Hide();
				_pool[i] = val;
			}
			viewPrefab.Hide();
		}

		public T[] GetItems(int lenght)
		{
			if (lenght > _pool.Length)
			{
				throw new IndexOutOfRangeException("Pool size exceeded, can't return " + lenght + " elements, pool size is " + _pool.Length);
			}
			Items = new T[lenght];
			int i;
			for (i = 0; i < lenght; i++)
			{
				Items[i] = _pool[i];
				Items[i].Show();
			}
			while (i < _pool.Length)
			{
				_pool[i++].Hide();
			}
			return Items;
		}

		public void Clear()
		{
			GetItems(0);
		}
	}
}
