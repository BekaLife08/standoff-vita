using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class ViewUtility
	{
		public static Canvas GetCanvas(Transform transform)
		{
			return transform.GetComponentInParent<Canvas>();
		}
	}
}
