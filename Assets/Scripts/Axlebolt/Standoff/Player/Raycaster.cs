using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class Raycaster : MonoBehaviour
	{
		public static bool Raycast(Vector3 startPos, Vector3 direction, float dist, out RaycastHit hit, string[] hitObj)
		{
			if (Physics.Raycast(startPos, direction, out hit, dist))
			{
				return true;
			}
			return false;
		}
	}
}
