using Axlebolt.Bolt;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Inventory.Medals
{
	public class MedalsController : TabController<MedalsController>
	{
		[SerializeField]
		private MedalItemView _bronzeView;

		[SerializeField]
		private MedalItemView _silverView;

		[SerializeField]
		private MedalItemView _goldView;

		[SerializeField]
		private MedalItemView _platinumView;

		[SerializeField]
		private MedalItemView _brilliantView;

		[SerializeField]
		private GameObject _bronzeModel;

		[SerializeField]
		private GameObject _silverModel;

		[SerializeField]
		private GameObject _goldModel;

		[SerializeField]
		private GameObject _platinumModel;

		[SerializeField]
		private GameObject _brilliantModel;

		[SerializeField]
		private CloseButton _closeButton;

		[SerializeField]
		private GameObject _buyPanel;

		[SerializeField]
		private Text _unavailableText;

		[SerializeField]
		private GameObject _boughtPanel;

		[SerializeField]
		private Button _buyButton;

		[SerializeField]
		private Text _priceText;

		private InventoryItemId _selectedItemId;

		private InventoryItemId _buyItemId;

		private void Awake()
		{
			_bronzeView.ClickHandler = OnSelectedHandler;
			_silverView.ClickHandler = OnSelectedHandler;
			_goldView.ClickHandler = OnSelectedHandler;
			_platinumView.ClickHandler = OnSelectedHandler;
			_brilliantView.ClickHandler = OnSelectedHandler;
			_buyButton.onClick.AddListener(BuyMedal);
			_closeButton.CloseHandler = Close;
		}

		public override void OnOpen()
		{
			ResetSelectedItem();
			ResetButItem();
			Refresh();
		}

		private void ResetSelectedItem()
		{
			_selectedItemId = Singleton<InventoryManager>.Instance.GetMainBadgeId();
			if (_selectedItemId == InventoryItemId.None)
			{
				_selectedItemId = InventoryItemId.MedalAssistanceBronze;
			}
			else if (_selectedItemId != InventoryItemId.MedalAssistanceBrilliant)
			{
				_selectedItemId++;
			}
		}

		private void ResetButItem()
		{
			_buyItemId = Singleton<InventoryManager>.Instance.GetMainBadgeId();
			if (_buyItemId == InventoryItemId.None)
			{
				_buyItemId = InventoryItemId.MedalAssistanceBronze;
			}
			else if (_buyItemId != InventoryItemId.MedalAssistanceBrilliant)
			{
				_buyItemId++;
			}
			else
			{
				_buyItemId = InventoryItemId.None;
			}
		}

		private void Refresh()
		{
			bool flag = Singleton<InventoryManager>.Instance.HasInventoryItemById(InventoryItemId.MedalAssistanceBronze);
			bool flag2 = Singleton<InventoryManager>.Instance.HasInventoryItemById(InventoryItemId.MedalAssistanceSilver);
			bool flag3 = Singleton<InventoryManager>.Instance.HasInventoryItemById(InventoryItemId.MedalAssistanceGold);
			bool flag4 = Singleton<InventoryManager>.Instance.HasInventoryItemById(InventoryItemId.MedalAssistancePlatinum);
			bool isBought = Singleton<InventoryManager>.Instance.HasInventoryItemById(InventoryItemId.MedalAssistanceBrilliant);
			bool flag5 = _selectedItemId == InventoryItemId.MedalAssistanceBronze;
			bool flag6 = _selectedItemId == InventoryItemId.MedalAssistanceSilver;
			bool flag7 = _selectedItemId == InventoryItemId.MedalAssistanceGold;
			bool flag8 = _selectedItemId == InventoryItemId.MedalAssistancePlatinum;
			bool flag9 = _selectedItemId == InventoryItemId.MedalAssistanceBrilliant;
			_bronzeView.SetState(flag, true, flag5);
			_bronzeModel.SetActive(flag5);
			_silverView.SetState(flag2, flag, flag6);
			_silverModel.SetActive(flag6);
			_goldView.SetState(flag3, flag2, flag7);
			_goldModel.SetActive(flag7);
			_platinumView.SetState(flag4, flag3, flag8);
			_platinumModel.SetActive(flag8);
			_brilliantView.SetState(isBought, flag4, flag9);
			_brilliantModel.SetActive(flag9);
			_buyPanel.gameObject.SetActive(_selectedItemId == _buyItemId);
			if (_buyPanel.gameObject.activeInHierarchy)
			{
				_priceText.text = ScriptLocalization.Common.Cost + " " + Singleton<InAppManager>.Instance.GetIntentoryItemPrice(_buyItemId);
			}
			if (_buyItemId == InventoryItemId.None)
			{
				_boughtPanel.gameObject.SetActive(value: true);
				_unavailableText.gameObject.SetActive(value: false);
				return;
			}
			_unavailableText.gameObject.SetActive(_selectedItemId > _buyItemId);
			if (_unavailableText.gameObject.activeSelf)
			{
				_unavailableText.text = string.Format(ScriptLocalization.Medals.Unavailable, GetMedalTitle(_selectedItemId - 1));
			}
			_boughtPanel.gameObject.SetActive(_selectedItemId < _buyItemId);
		}

		private string GetMedalTitle(InventoryItemId id)
		{
			switch (id)
			{
			case InventoryItemId.MedalAssistanceBronze:
				return "Bronze";
			case InventoryItemId.MedalAssistanceSilver:
				return "Silver";
			case InventoryItemId.MedalAssistanceGold:
				return "Gold";
			case InventoryItemId.MedalAssistancePlatinum:
				return "Platinum";
			case InventoryItemId.MedalAssistanceBrilliant:
				return "Brilliant";
			default:
				throw new ArgumentOutOfRangeException("id", id, null);
			}
		}

		private void OnSelectedHandler(InventoryItemId id)
		{
			_selectedItemId = id;
			Refresh();
		}

		private void BuyMedal()
		{
			if (_selectedItemId != _buyItemId)
			{
				throw new Exception("Invalid state!");
			}
			Dialog dialog = Dialogs.Create(ScriptLocalization.Medals.Purchasing, ScriptLocalization.Common.PleaseWait);
			dialog.Show();
			Singleton<InAppManager>.Instance.BuyInventoryItem(_buyItemId, delegate(bool success)
			{
				dialog.Hide();
				if (success)
				{
					string name = BoltService<BoltPlayerService>.Instance.Player.Name;
					string message = string.Format(ScriptLocalization.Medals.ThanksFor, name);
					Dialogs.Message(ScriptLocalization.Common.Success, message);
				}
				ResetButItem();
				Refresh();
			});
		}

		public override void OnClose()
		{
		}
	}
}
