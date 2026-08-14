using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class BombSite : BaseZone
	{
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(base.transform.position, base.transform.localScale);
		}
	}
}
