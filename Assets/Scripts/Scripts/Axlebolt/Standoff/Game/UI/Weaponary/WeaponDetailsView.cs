using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class WeaponDetailsView : View
	{
		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Image _previewImage;

		[SerializeField]
		private Text _costTitle;

		[SerializeField]
		private Text _costText;

		[SerializeField]
		private GunParametersView _gunParamtersView;

		private void Awake()
		{
			_gunParamtersView.Show();
			Clear();
		}

		public void SetInventories(WeaponParameters[] inventories)
		{
			GunParameters[] guns = (from inventory in inventories
				where inventory is GunParameters
				select inventory).Cast<GunParameters>().ToArray();
			_gunParamtersView.SetGuns(guns);
		}

		public void ShowDeatils(WeaponParameters weapon)
		{
			_nameText.text = weapon.DisplayName;
			_previewImage.gameObject.SetActive(value: true);
			_costTitle.gameObject.SetActive(value: true);
			_previewImage.sprite = WeaponUtility.GetWeaponPreview(weapon, GetSkin(weapon));
			_costText.text = "$" + weapon.Cost;
			GunParameters gunParameters = weapon as GunParameters;
			if (gunParameters != null)
			{
				_gunParamtersView.ShowGun(gunParameters);
			}
		}

		private static SkinDefinition GetSkin(WeaponParameters weapon)
		{
			InventoryItemId selectedSkinId = Singleton<InventoryManager>.Instance.GetSelectedSkinId(weapon.Id);
			return (selectedSkinId == InventoryItemId.None) ? null : Singleton<InventoryManager>.Instance.GetSkinDefinition(selectedSkinId);
		}

		public void Clear()
		{
			_nameText.text = string.Empty;
			_previewImage.gameObject.SetActive(value: false);
			_costTitle.gameObject.SetActive(value: false);
			_costText.text = string.Empty;
			_gunParamtersView.Hide();
		}
	}
}
