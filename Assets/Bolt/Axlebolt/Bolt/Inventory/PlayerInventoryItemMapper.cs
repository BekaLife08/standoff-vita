using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Inventory
{
	public class PlayerInventoryItemMapper : MessageMapper<InventoryItem, BoltInventoryItem>
	{
		public static readonly PlayerInventoryItemMapper Instancce = new PlayerInventoryItemMapper();

		public override BoltInventoryItem ToOriginal(InventoryItem proto)
		{
			return new BoltInventoryItem
			{
				Id = proto.Id,
				DisplayName = proto.DisplayName,
				BuyPrice = CurrencyAmountMapper.Instancce.ToOriginalList(proto.BuyPrice),
				SellPrice = CurrencyAmountMapper.Instancce.ToOriginalList(proto.SellPrice)
			};
		}
	}
}
