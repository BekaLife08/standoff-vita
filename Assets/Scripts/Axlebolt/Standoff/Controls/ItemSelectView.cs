using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class ItemSelectView : View
	{
		[SerializeField]
		private List<ItemSelectViewItem> _selectableItems;

		[SerializeField]
		private Color _reloadImgColorSelected;

		[SerializeField]
		private Color _reloadImgColorUnselected;

		[SerializeField]
		private Image _reloadImg;

		public override void Show()
		{
			base.Show();
			foreach (ItemSelectViewItem selectableItem in _selectableItems)
			{
				selectableItem.SetSelected(isSelected: false);
				selectableItem.FadeIn();
			}
		}

		public void SetWeaponList(WeaponController[] weapons)
		{
			if (weapons.Length > 4)
			{
				UnityEngine.Debug.LogError("Slot Length Can't Be More Than > 4");
			}
			for (int i = 0; i < _selectableItems.Count; i++)
			{
				_selectableItems[i].SetWeapon(weapons[i]);
			}
		}

		public void SetSelectedSlot(int slotIndex)
		{
			foreach (ItemSelectViewItem selectableItem in _selectableItems)
			{
				selectableItem.SetSelected(isSelected: false);
			}
			if (slotIndex == 0)
			{
				_reloadImg.color = _reloadImgColorSelected;
				return;
			}
			_reloadImg.color = _reloadImgColorUnselected;
			_selectableItems[slotIndex - 1].SetSelected(isSelected: true);
		}
	}
}
