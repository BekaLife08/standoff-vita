using System.Collections.Generic;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltExchangeResult
	{
		public Dictionary<int, BoltInventoryItemStack> InventoryItemStacks { get; set; }

		public Dictionary<int, BoltCurrencyAmount> CurrencyAmounts { get; set; }
	}
}
