using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Inventory
{
	public class BoltInventoryService : BoltService<BoltInventoryService>
	{
		private readonly InventoryRemoteService _inventoryRemoteService;

		private BoltPlayerInventoryCache _playerInventoryCache;

		private Dictionary<int, BoltInventoryItemCache> _boltInventoryItemsCache;

		public BoltInventoryService()
		{
			_inventoryRemoteService = new InventoryRemoteService(BoltApi.Instance.ClientService);
			_playerInventoryCache = new BoltPlayerInventoryCache();
			_boltInventoryItemsCache = new Dictionary<int, BoltInventoryItemCache>();
		}

		public BoltPlayerInventory GetPlayerInventory()
		{
			return new BoltPlayerInventory
			{
				InventoryItemStacks = _playerInventoryCache.InventoryItemStacks.Values.ToList(),
				CurrencyAmounts = _playerInventoryCache.CurrencyAmounts.Values.ToList()
			};
		}

		public List<BoltInventoryItem> GetPlayerInventoryItems()
		{
			return _boltInventoryItemsCache.Values.Select((BoltInventoryItemCache value) => new BoltInventoryItem
			{
				Id = value.Id,
				DisplayName = value.DisplayName,
				SellPrice = value.SellPrice.Values.ToList(),
				BuyPrice = value.BuyPrice.Values.ToList()
			}).ToList();
		}

		public Task<BoltInventoryItemStack> ConsumeInventoryItem(int inventoryItemStackId, int quantity)
		{
			return BoltApi.Instance.Async(delegate
			{
				InventoryItemStack proto = _inventoryRemoteService.ConsumeInventoryItem(inventoryItemStackId, quantity);
				BoltInventoryItemStack boltInventoryItemStack = InventoryItemStackMapper.Instancce.ToOriginal(proto);
				_playerInventoryCache.InventoryItemStacks[inventoryItemStackId] = boltInventoryItemStack;
				return boltInventoryItemStack;
			});
		}

		public Task TransferInventoryItems(int fromStackId, int toStackId, int quantity)
		{
			return BoltApi.Instance.Async(delegate
			{
				InventoryItemStack[] beans = _inventoryRemoteService.TransferInventoryItems(fromStackId, toStackId, quantity);
				BoltInventoryItemStack[] source = InventoryItemStackMapper.Instancce.ToOriginalList(beans).ToArray();
				Dictionary<int, BoltInventoryItemStack> dictionary = source.ToDictionary((BoltInventoryItemStack stack) => stack.StackId, (BoltInventoryItemStack stack) => stack);
				_playerInventoryCache.InventoryItemStacks[fromStackId] = dictionary[fromStackId];
				if (_playerInventoryCache.InventoryItemStacks.ContainsKey(toStackId))
				{
					_playerInventoryCache.InventoryItemStacks[toStackId] = dictionary[toStackId];
				}
				else
				{
					_playerInventoryCache.InventoryItemStacks.Add(toStackId, dictionary[toStackId]);
				}
			});
		}

		public Task<BoltInventoryItemStack> BuyInventoryItemStack(int inventoryItemId, int quantity, int currencyId)
		{
			return BoltApi.Instance.Async(delegate
			{
				InventoryItemStack proto = _inventoryRemoteService.BuyInventoryItem(inventoryItemId, quantity, currencyId);
				BoltInventoryItemStack boltInventoryItemStack = InventoryItemStackMapper.Instancce.ToOriginal(proto);
				_playerInventoryCache.InventoryItemStacks.Add(boltInventoryItemStack.StackId, boltInventoryItemStack);
				int num = _boltInventoryItemsCache[inventoryItemId].BuyPrice[currencyId].Value * quantity;
				_playerInventoryCache.CurrencyAmounts[currencyId].Value = _playerInventoryCache.CurrencyAmounts[currencyId].Value - num;
				return boltInventoryItemStack;
			});
		}

		public Task<BoltInventoryItemStack> SellInventoryItemStack(int inventoryItemStackId, int quantity, int currencyId)
		{
			return BoltApi.Instance.Async(delegate
			{
				InventoryItemStack proto = _inventoryRemoteService.SellInventoryItem(inventoryItemStackId, quantity, currencyId);
				BoltInventoryItemStack boltInventoryItemStack = InventoryItemStackMapper.Instancce.ToOriginal(proto);
				_playerInventoryCache.InventoryItemStacks[inventoryItemStackId] = boltInventoryItemStack;
				int id = boltInventoryItemStack.InventoryItem.Id;
				int num = _boltInventoryItemsCache[id].SellPrice[currencyId].Value * quantity;
				_playerInventoryCache.CurrencyAmounts[currencyId].Value = _playerInventoryCache.CurrencyAmounts[currencyId].Value + num;
				return boltInventoryItemStack;
			});
		}

		public Task<BoltExchangeResult> ExchangeInventoryItems(string recipeCode, BoltCurrencyAmount[] currencies, int[] inventoryItemStackIds)
		{
			return BoltApi.Instance.Async(delegate
			{
				CurrencyAmount[] currencies2 = CurrencyAmountMapper.Instancce.ToProtoArray(currencies);
				ExchangeResult proto = _inventoryRemoteService.ExchangeInventoryItems(recipeCode, currencies2, inventoryItemStackIds);
				BoltExchangeResult boltExchangeResult = BoltExchangeResultMapper.Instance.ToOriginal(proto);
				BoltCurrencyAmount[] array = currencies;
				foreach (BoltCurrencyAmount boltCurrencyAmount in array)
				{
					if (_playerInventoryCache.CurrencyAmounts[boltCurrencyAmount.CurrencyId] != null)
					{
						_playerInventoryCache.CurrencyAmounts[boltCurrencyAmount.CurrencyId].Value = _playerInventoryCache.CurrencyAmounts[boltCurrencyAmount.CurrencyId].Value - boltCurrencyAmount.Value;
					}
				}
				int[] array2 = inventoryItemStackIds;
				foreach (int key in array2)
				{
					_playerInventoryCache.InventoryItemStacks.Remove(key);
				}
				foreach (BoltCurrencyAmount value in boltExchangeResult.CurrencyAmounts.Values)
				{
					_playerInventoryCache.CurrencyAmounts[value.CurrencyId].Value = _playerInventoryCache.CurrencyAmounts[value.CurrencyId].Value + value.Value;
				}
				foreach (BoltInventoryItemStack value2 in boltExchangeResult.InventoryItemStacks.Values)
				{
					_playerInventoryCache.InventoryItemStacks.Add(value2.StackId, value2);
				}
				return boltExchangeResult;
			});
		}

		internal override void Load()
		{
			_playerInventoryCache = new BoltPlayerInventoryCache { CurrencyAmounts = new Dictionary<int, BoltCurrencyAmount>(), InventoryItemStacks = new Dictionary<int, BoltInventoryItemStack>() };
			_playerInventoryCache.CurrencyAmounts[101] = new BoltCurrencyAmount { CurrencyId = 101, Value = 1 };
			_playerInventoryCache.CurrencyAmounts[102] = new BoltCurrencyAmount { CurrencyId = 102, Value = 1 };
			_playerInventoryCache.InventoryItemStacks[203] = new BoltInventoryItemStack { InventoryItem = new BoltInventoryItem { Id = 203, DisplayName = "GITLER" }, Quantity = 1, StackId = 203 };
			_boltInventoryItemsCache = new Dictionary<int, BoltInventoryItemCache> { { 203, new BoltInventoryItemCache { Id = 203, DisplayName = "GITLER" } } };
		}

		private BoltPlayerInventoryCache LoadInventory()
		{
			return PlayerInventoryMapper.Instancce.ToOriginal(_inventoryRemoteService.GetPlayerInventory());
		}

		private IEnumerable<BoltInventoryItemCache> LoadPlayerInventoryItems()
		{
			List<BoltInventoryItem> source = PlayerInventoryItemMapper.Instancce.ToOriginalList(_inventoryRemoteService.GetInventoryItems());
			return source.Select((BoltInventoryItem item) => new BoltInventoryItemCache
			{
				Id = item.Id,
				DisplayName = item.DisplayName,
				BuyPrice = item.BuyPrice.ToDictionary((BoltCurrencyAmount curr) => curr.CurrencyId, (BoltCurrencyAmount curr) => curr),
				SellPrice = item.SellPrice.ToDictionary((BoltCurrencyAmount curr) => curr.CurrencyId, (BoltCurrencyAmount curr) => curr)
			}).ToList();
		}
	}
}
