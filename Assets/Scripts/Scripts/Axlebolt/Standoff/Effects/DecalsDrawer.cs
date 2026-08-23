using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public abstract class DecalsDrawer : MonoBehaviour
	{
		public abstract void DrawDecal(RaycastHit raycastHit, Vector2 size, int decalIndex);

		public abstract void Clear();
	}
}
