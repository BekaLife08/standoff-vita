using System.Collections.Generic;
using System.Linq;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltExchangeResultMapper : MessageMapper<ExchangeResult, BoltExchangeResult>
	{
		public static readonly BoltExchangeResultMapper Instance = new BoltExchangeResultMapper();

		public override BoltExchangeResult ToOriginal(ExchangeResult proto)
		{
			Dictionary<int, BoltInventoryItemStack> inventoryItemStacks = InventoryItemStackMapper.Instancce.ToOriginalList(proto.InventoryItems).ToDictionary((BoltInventoryItemStack stack) => stack.StackId, (BoltInventoryItemStack stack) => stack);
			Dictionary<int, BoltCurrencyAmount> currencyAmounts = CurrencyAmountMapper.Instancce.ToOriginalList(proto.Currencies).ToDictionary((BoltCurrencyAmount curr) => curr.CurrencyId, (BoltCurrencyAmount curr) => curr);
			return new BoltExchangeResult
			{
				InventoryItemStacks = inventoryItemStacks,
				CurrencyAmounts = currencyAmounts
			};
		}
	}
}
