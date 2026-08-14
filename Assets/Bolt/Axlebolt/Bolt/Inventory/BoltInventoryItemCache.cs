using System.Collections.Generic;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltInventoryItemCache
	{
		public int Id { get; set; }

		public string DisplayName { get; set; }

		public Dictionary<int, BoltCurrencyAmount> BuyPrice { get; set; }

		public Dictionary<int, BoltCurrencyAmount> SellPrice { get; set; }
	}
}
