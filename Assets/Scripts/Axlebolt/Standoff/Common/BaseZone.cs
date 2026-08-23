using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public abstract class BaseZone : MonoBehaviour
	{
		private Bounds _bounds;

		private void Awake()
		{
			_bounds = new Bounds(base.transform.position, base.transform.localScale);
		}

		public bool IsInZone(Vector3 point)
		{
			return _bounds.Contains(point);
		}
	}
}
