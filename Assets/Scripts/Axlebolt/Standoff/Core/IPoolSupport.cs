using UnityEngine;

namespace Axlebolt.Standoff.Core
{
	public interface IPoolSupport
	{
		bool PoolContains(string prefabId);

		GameObject GetFromPool(string prefabId);

		void ReturnToPool(GameObject gameObject);
	}
}
