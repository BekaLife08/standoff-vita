using System.Linq;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Inventory
{
	public class PlayerInventoryMapper : MessageMapper<PlayerInventory, BoltPlayerInventoryCache>
	{
		public static readonly PlayerInventoryMapper Instancce = new PlayerInventoryMapper();

		public override BoltPlayerInventoryCache ToOriginal(PlayerInventory proto)
		{
			return new BoltPlayerInventoryCache
			{
				InventoryItemStacks = InventoryItemStackMapper.Instancce.ToOriginalList(proto.InventoryItems).ToDictionary((BoltInventoryItemStack stack) => stack.StackId, (BoltInventoryItemStack stack) => stack),
				CurrencyAmounts = CurrencyAmountMapper.Instancce.ToOriginalList(proto.Currencies).ToDictionary((BoltCurrencyAmount curr) => curr.CurrencyId, (BoltCurrencyAmount curr) => curr)
			};
		}
	}
}
