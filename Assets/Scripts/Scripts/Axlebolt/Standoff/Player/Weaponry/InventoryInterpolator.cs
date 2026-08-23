using Axlebolt.Networking;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Weaponry
{
	public class InventoryInterpolator : Interpolator
	{
		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			WeaponrySnapshot weaponrySnapshot = (WeaponrySnapshot)a;
			WeaponrySnapshot weaponrySnapshot2 = (WeaponrySnapshot)b;
			weaponrySnapshot2.WeaponSnapshot.Time = Mathf.Lerp(weaponrySnapshot.time, weaponrySnapshot2.time, progress);
			return weaponrySnapshot2;
		}
	}
}
