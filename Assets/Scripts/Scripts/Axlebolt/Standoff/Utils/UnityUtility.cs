using System;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Utils
{
	public class UnityUtility
	{
		public static Transform FindDeepChild(Transform aParent, string aName)
		{
			Transform transform = aParent.Find(aName);
			if (transform != null)
			{
				return transform;
			}
			IEnumerator enumerator = aParent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform aParent2 = (Transform)enumerator.Current;
					transform = FindDeepChild(aParent2, aName);
					if (transform != null)
					{
						return transform;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return null;
		}

		public static void SetLayerRecursively(GameObject go, int layer)
		{
			go.layer = layer;
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(includeInactive: true);
			foreach (Transform transform in componentsInChildren)
			{
				transform.gameObject.layer = layer;
			}
		}
	}
}
