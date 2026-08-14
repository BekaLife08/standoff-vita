using System.Collections.Generic;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltInventoryItem
	{
		public int Id { get; set; }

		public string DisplayName { get; set; }

		public List<BoltCurrencyAmount> BuyPrice { get; set; }

		public List<BoltCurrencyAmount> SellPrice { get; set; }
	}
}
