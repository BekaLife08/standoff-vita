using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("InventoryRemoteService")]
	public class InventoryRemoteService : RpcService
	{
		public InventoryRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("tradeInventoryItems")]
		public InventoryItemStack[] TradeInventoryItems(int[] localStackId, CurrencyAmount[] localCurrencyAmount, long? remotePlayerId, int[] remoteStackId, CurrencyAmount[] remoteCurrencyAmount, CancellationToken ct = default(CancellationToken))
		{
			return (InventoryItemStack[])Invoke(MethodBase.GetCurrentMethod(), new object[5] { localStackId, localCurrencyAmount, remotePlayerId, remoteStackId, remoteCurrencyAmount }, ct);
		}

		[Rpc("consumeInventoryItem")]
		public InventoryItemStack ConsumeInventoryItem(int inventoryItemStackId, int quantity, CancellationToken ct = default(CancellationToken))
		{
			return (InventoryItemStack)Invoke(MethodBase.GetCurrentMethod(), new object[2] { inventoryItemStackId, quantity }, ct);
		}

		[Rpc("transferInventoryItems")]
		public InventoryItemStack[] TransferInventoryItems(int fromStackId, int toStackId, int quantity, CancellationToken ct = default(CancellationToken))
		{
			return (InventoryItemStack[])Invoke(MethodBase.GetCurrentMethod(), new object[3] { fromStackId, toStackId, quantity }, ct);
		}

		[Rpc("getInventoryItems")]
		public InventoryItem[] GetInventoryItems(CancellationToken ct = default(CancellationToken))
		{
			return (InventoryItem[])Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("buyInventoryItem")]
		public InventoryItemStack BuyInventoryItem(int inventoryItemId, int quantity, int currencyId, CancellationToken ct = default(CancellationToken))
		{
			return (InventoryItemStack)Invoke(MethodBase.GetCurrentMethod(), new object[3] { inventoryItemId, quantity, currencyId }, ct);
		}

		[Rpc("getPlayerInventory")]
		public PlayerInventory GetPlayerInventory(CancellationToken ct = default(CancellationToken))
		{
			return (PlayerInventory)Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("exchangeInventoryItems")]
		public ExchangeResult ExchangeInventoryItems(string recipeCode, CurrencyAmount[] currencies, int[] inventoryItemStackIds, CancellationToken ct = default(CancellationToken))
		{
			return (ExchangeResult)Invoke(MethodBase.GetCurrentMethod(), new object[3] { recipeCode, currencies, inventoryItemStackIds }, ct);
		}

		[Rpc("sellInventoryItem")]
		public InventoryItemStack SellInventoryItem(int inventoryItemStackId, int quantity, int currencyId, CancellationToken ct = default(CancellationToken))
		{
			return (InventoryItemStack)Invoke(MethodBase.GetCurrentMethod(), new object[3] { inventoryItemStackId, quantity, currencyId }, ct);
		}
	}
}
