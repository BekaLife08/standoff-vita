using ExitGames.Client.Photon;

namespace Standoff.Inventory
{
	public class InventoryNetworkInfo
	{
		public Hashtable data = new Hashtable();

		public Hashtable GetHashedInfo()
		{
			return data;
		}

		public static InventoryNetworkInfo GetUnhashedInfo(Hashtable hashtable)
		{
			InventoryNetworkInfo inventoryNetworkInfo = new InventoryNetworkInfo();
			inventoryNetworkInfo.data = hashtable;
			return inventoryNetworkInfo;
		}
	}
}
