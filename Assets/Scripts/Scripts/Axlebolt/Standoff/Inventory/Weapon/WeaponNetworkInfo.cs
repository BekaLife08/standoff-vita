using ExitGames.Client.Photon;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class WeaponNetworkInfo
	{
		public Hashtable weaponData = new Hashtable();

		public List<ShotInfo> shotsBuffer = new List<ShotInfo>();

		public Hashtable GetHashedInfo()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("WD", weaponData);
			hashtable.Add("L", shotsBuffer.Count);
			int num = 0;
			foreach (ShotInfo item in shotsBuffer)
			{
				hashtable.Add(num, item.GetHashedInfo());
			}
			return hashtable;
		}

		public static WeaponNetworkInfo GetUnhashedInfo(Hashtable hashtable)
		{
			WeaponNetworkInfo weaponNetworkInfo = new WeaponNetworkInfo();
			weaponNetworkInfo.weaponData = (Hashtable)hashtable["WD"];
			int num = (int)hashtable["L"];
			for (int i = 0; i < num; i++)
			{
				weaponNetworkInfo.shotsBuffer.Add(ShotInfo.GetUnhashedInfo((Hashtable)hashtable[i]));
			}
			return weaponNetworkInfo;
		}
	}
}
