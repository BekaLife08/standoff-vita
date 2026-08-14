using Axlebolt.Standoff.UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Inventory.Medals
{
	[RequireComponent(typeof(Button))]
	public class MedalItemView : View
	{
		[SerializeField]
		private InventoryItemId _id;

		[SerializeField]
		private Color _boughtBackgroundColor;

		[SerializeField]
		private Color _boughtLineColor;

		[SerializeField]
		private Color _boughtImageColor;

		[SerializeField]
		private Color _boughtTextColor;

		[SerializeField]
		private Color _normalBackgroundColor;

		[SerializeField]
		private Color _normalLineColor;

		[SerializeField]
		private Color _normalImageColor;

		[SerializeField]
		private Color _normalTextColor;

		[SerializeField]
		private Color _selectedBackgroundColor;

		[SerializeField]
		private Color _selectedLineColor;

		[SerializeField]
		private Image _backgroundImage;

		[SerializeField]
		private Image _lineImage;

		[SerializeField]
		private Image _medalImage;

		[SerializeField]
		private Text _medalText;

		[SerializeField]
		private Image _unlockImage;

		public InventoryItemId Id
		{
			[CompilerGenerated]
			get
			{
				return _id;
			}
		}

		public Action<InventoryItemId> ClickHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			this.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				ClickHandler?.Invoke(Id);
			});
		}

		public void SetState(bool isBought, bool isCanBought, bool isSelected)
		{
			if (isBought)
			{
				_backgroundImage.color = _boughtBackgroundColor;
				_lineImage.color = _boughtLineColor;
				_medalImage.color = _boughtImageColor;
				_medalText.color = _boughtTextColor;
			}
			else
			{
				_backgroundImage.color = _normalBackgroundColor;
				_lineImage.color = _normalLineColor;
				_medalImage.color = _normalImageColor;
				_medalText.color = _normalTextColor;
			}
			_unlockImage.gameObject.SetActive(!isBought && !isCanBought);
			if (isSelected)
			{
				_backgroundImage.color = _selectedBackgroundColor;
				_lineImage.color = _selectedLineColor;
			}
		}
	}
}
