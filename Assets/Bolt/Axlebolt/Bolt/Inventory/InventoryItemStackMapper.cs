using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Inventory
{
	public class InventoryItemStackMapper : MessageMapper<InventoryItemStack, BoltInventoryItemStack>
	{
		public static readonly InventoryItemStackMapper Instancce = new InventoryItemStackMapper();

		public override BoltInventoryItemStack ToOriginal(InventoryItemStack proto)
		{
			return new BoltInventoryItemStack
			{
				InventoryItem = PlayerInventoryItemMapper.Instancce.ToOriginal(proto.InventoryItem),
				StackId = proto.StackId,
				Quantity = proto.Quantity
			};
		}
	}
}
