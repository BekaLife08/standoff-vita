using System.Collections.Generic;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltPlayerInventoryCache
	{
		public Dictionary<int, BoltInventoryItemStack> InventoryItemStacks { get; set; }

		public Dictionary<int, BoltCurrencyAmount> CurrencyAmounts { get; set; }
	}
}
