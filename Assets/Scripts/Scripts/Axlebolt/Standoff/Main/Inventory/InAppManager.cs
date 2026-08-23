using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using System;
using UnityEngine.Purchasing;

namespace Axlebolt.Standoff.Main.Inventory
{
	public class InAppManager : Singleton<InAppManager>
	{
		private class StoreListener : IStoreListener
		{
			public InAppManager InAppManager
			{
				private get;
				set;
			}

			public void OnInitializeFailed(InitializationFailureReason error)
			{
				InAppManager.OnInitializeFailed(error);
			}

			public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
			{
				return InAppManager.ProcessPurchase(e);
			}

			public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
			{
				InAppManager.OnPurchaseFailed(i, p);
			}

			public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
			{
				InAppManager.OnInitialized(controller, extensions);
			}
		}

		private static readonly Log Log = Log.Create(typeof(InAppManager));

		private static IStoreController _storeController;

		private static IExtensionProvider _storeExtensionProvider;

		private static StoreListener _storeListener;

		private Action<bool> _resultCallback;

		public event Action InitializedEvent;

		public event Action PurchaseSuccessfullyEvent;

		public void Init()
		{
			BindStoreListener();
			if (!IsInitialized())
			{
				ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
				configurationBuilder.AddProduct("medal_assistance_bronze", ProductType.NonConsumable);
				configurationBuilder.AddProduct("medal_assistance_silver", ProductType.NonConsumable);
				configurationBuilder.AddProduct("medal_assistance_gold", ProductType.NonConsumable);
				configurationBuilder.AddProduct("medal_assistance_platinum", ProductType.NonConsumable);
				configurationBuilder.AddProduct("medal_assistance_brilliant", ProductType.NonConsumable);
				UnityPurchasing.Initialize(_storeListener, configurationBuilder);
			}
		}

		private void BindStoreListener()
		{
			if (_storeListener == null)
			{
				StoreListener storeListener = new StoreListener();
				storeListener.InAppManager = this;
				_storeListener = storeListener;
			}
			else
			{
				_storeListener.InAppManager = this;
			}
		}

		private bool IsInitialized()
		{
			return _storeController != null && _storeExtensionProvider != null;
		}

		public void BuyInventoryItem(InventoryItemId id, [NotNull] Action<bool> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (_resultCallback != null)
			{
				throw new Exception("Purchasing already in progress");
			}
			if (!IsInitialized())
			{
				Log.Error("BuyInventoryItem fail. Not initialized.");
				return;
			}
			_resultCallback = callback;
			string text = ToProductId(id);
			Product product = _storeController.products.WithID(text);
			if (product != null && product.availableToPurchase)
			{
				Log.Debug($"Purchasing product asychronously: {product.definition.id}");
				_storeController.InitiatePurchase(product);
			}
			else
			{
				Log.Error($"{text} either is not found or is not available for purchase");
			}
		}

		public bool IsInventoryItemBought(InventoryItemId id)
		{
			if (IsInitialized())
			{
				string id2 = ToProductId(id);
				return _storeController.products.WithID(id2)?.hasReceipt ?? false;
			}
			Log.Error("BuyInventoryItem fail. Not initialized.");
			return false;
		}

		public string GetIntentoryItemPrice(InventoryItemId id)
		{
			if (IsInitialized())
			{
				string id2 = ToProductId(id);
				return _storeController.products.WithID(id2)?.metadata.localizedPriceString;
			}
			Log.Error("GetIntentoryItemPrice fail. Not initialized.");
			return null;
		}

		private string ToProductId(InventoryItemId id)
		{
			switch (id)
			{
			case InventoryItemId.MedalAssistanceBronze:
				return "medal_assistance_bronze";
			case InventoryItemId.MedalAssistanceSilver:
				return "medal_assistance_silver";
			case InventoryItemId.MedalAssistanceGold:
				return "medal_assistance_gold";
			case InventoryItemId.MedalAssistancePlatinum:
				return "medal_assistance_platinum";
			case InventoryItemId.MedalAssistanceBrilliant:
				return "medal_assistance_brilliant";
			default:
				throw new ArgumentOutOfRangeException("id", id, null);
			}
		}

		protected void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			_storeController = controller;
			_storeExtensionProvider = extensions;
			if (this.InitializedEvent != null)
			{
				this.InitializedEvent();
			}
		}

		protected void OnInitializeFailed(InitializationFailureReason error)
		{
			Log.Error("OnInitializeFailed InitializationFailureReason:" + error);
		}

		protected PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
		{
			Log.Debug("ProcessPurchase");
			if (_resultCallback == null)
			{
				return PurchaseProcessingResult.Complete;
			}
			Action<bool> resultCallback = _resultCallback;
			_resultCallback = null;
			resultCallback(obj: true);
			if (this.PurchaseSuccessfullyEvent != null)
			{
				this.PurchaseSuccessfullyEvent();
			}
			return PurchaseProcessingResult.Complete;
		}

		protected void OnPurchaseFailed(Product i, PurchaseFailureReason p)
		{
			Log.Debug("OnPurchaseFailed");
			Action<bool> resultCallback = _resultCallback;
			_resultCallback = null;
			resultCallback(obj: false);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_storeListener.InAppManager = null;
		}
	}
}
