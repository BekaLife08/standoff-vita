using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class Trash : Singleton<Trash>
	{
		private static readonly Log Log = Log.Create(typeof(Trash));

		private readonly Dictionary<Transform, List<Transform>> _trash = new Dictionary<Transform, List<Transform>>();

		public void Drop(GameObject item)
		{
			Drop(item.transform);
		}

		public void Drop([NotNull] Transform itemTransform)
		{
			if (itemTransform == null)
			{
				throw new ArgumentNullException("itemTransform");
			}
			Transform parent = itemTransform.parent;
			if (!(parent == base.transform))
			{
				if (!_trash.TryGetValue(parent, out List<Transform> value))
				{
					value = new List<Transform>();
					_trash[parent] = value;
				}
				value.Add(itemTransform);
				itemTransform.gameObject.SetActive(value: false);
				itemTransform.SetParent(base.transform, worldPositionStays: false);
			}
		}

		public bool Contains(Transform item, Transform parent)
		{
			return _trash.ContainsKey(parent) && _trash[parent].Contains(item);
		}

		public void ReturnAll(Transform parent)
		{
			if (!_trash.ContainsKey(parent))
			{
				Log.Warning("Trash items for parent {0} not found", parent.name);
				return;
			}
			foreach (Transform item in _trash[parent])
			{
				item.SetParent(parent, worldPositionStays: false);
				item.gameObject.SetActive(value: true);
			}
			_trash.Remove(parent);
		}

		public void Return(Transform item, Transform parent)
		{
			if (!_trash.ContainsKey(parent))
			{
				Log.Warning("Trash items for parent {0} not found", parent.name);
				return;
			}
			_trash[parent].Remove(item);
			item.transform.SetParent(parent, worldPositionStays: false);
			item.gameObject.SetActive(value: true);
		}
	}
}
