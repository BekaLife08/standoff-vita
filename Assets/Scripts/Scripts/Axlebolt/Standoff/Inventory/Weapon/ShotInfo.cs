using ExitGames.Client.Photon;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class ShotInfo
	{
		public Vector3 startPositioon;

		public Vector3 direction;

		public float shotTime;

		public Hashtable GetHashedInfo()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("S", startPositioon);
			hashtable.Add("D", direction);
			hashtable.Add("T", shotTime);
			return hashtable;
		}

		public static ShotInfo GetUnhashedInfo(Hashtable hashtable)
		{
			ShotInfo shotInfo = new ShotInfo();
			shotInfo.startPositioon = (Vector3)hashtable["S"];
			shotInfo.direction = (Vector3)hashtable["D"];
			shotInfo.shotTime = (float)hashtable["T"];
			return shotInfo;
		}
	}
}
