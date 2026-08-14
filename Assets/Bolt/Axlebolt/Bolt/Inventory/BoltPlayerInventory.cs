using System.Collections.Generic;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltPlayerInventory
	{
		public List<BoltInventoryItemStack> InventoryItemStacks { get; set; }

		public List<BoltCurrencyAmount> CurrencyAmounts { get; set; }
	}
}
