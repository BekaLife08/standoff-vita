using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class WeaponBuyView : View
	{
		private static readonly Log Log = Log.Create(typeof(WeaponBuyView));

		[SerializeField]
		private Text _timeLeftTitle;

		[SerializeField]
		private Text _timeLeftText;

		[SerializeField]
		private Color _timeLeftColor;

		[SerializeField]
		private Color _timeLeftRedColor;

		[SerializeField]
		private WeaponCategoriesView _categoriesView;

		[SerializeField]
		private WeaponItemsView _itemsView;

		[SerializeField]
		private WeaponDetailsView _detailsView;

		[SerializeField]
		private PlayerWeaponsView _weaponView;

		[SerializeField]
		private Button _closeButton;

		private GameController _gameController;

		private Dictionary<WeaponCategory, List<WeaponParameters>> _weaponByCategory;

		public UnityAction Closed;

		public bool MoneyEnabled
		{
			set
			{
				_weaponView.MoneyEnabled = value;
				_itemsView.WeaponCostEnabled = value;
			}
		}

		public string TimeLeftTitle
		{
			set
			{
				_timeLeftTitle.text = value;
			}
		}

		public float TimeLeft
		{
			set
			{
				_timeLeftText.color = ((!(value < 10f)) ? _timeLeftColor : _timeLeftRedColor);
				_timeLeftText.text = StringUtils.FormatTwoDigit(value);
			}
		}

		private void Awake()
		{
			_closeButton.onClick.AddListener(delegate
			{
				Closed();
			});
		}

		public void Init([NotNull] GameController gameController)
		{
			if (gameController == null)
			{
				throw new ArgumentNullException("gameController");
			}
			_gameController = gameController;
			WeaponParameters[] allWeapons = _gameController.GetAllWeapons();
			_weaponByCategory = GroupWeapons(allWeapons);
			_categoriesView.OnCategorySelected = OnCategorySelected;
			_itemsView.Init(gameController);
			_itemsView.OnInventorySelected = OnInventorySelected;
			_categoriesView.SetCategories((from key in _weaponByCategory.Keys
				orderby key
				select key).ToArray());
			_detailsView.SetInventories(allWeapons);
		}

		private Dictionary<WeaponCategory, List<WeaponParameters>> GroupWeapons([NotNull] WeaponParameters[] weapons)
		{
			if (weapons == null)
			{
				throw new ArgumentNullException("weapons");
			}
			Dictionary<WeaponCategory, List<WeaponParameters>> dictionary = new Dictionary<WeaponCategory, List<WeaponParameters>>();
			foreach (WeaponParameters weaponParameters in weapons)
			{
				WeaponCategory? category = GetCategory(weaponParameters);
				if (category.HasValue)
				{
					WeaponCategory value = category.Value;
					if (!dictionary.ContainsKey(value))
					{
						dictionary.Add(value, new List<WeaponParameters>());
					}
					dictionary[value].Add(weaponParameters);
				}
			}
			return dictionary;
		}

		private WeaponCategory? GetCategory(WeaponParameters weapon)
		{
			GunParameters gunParameters = weapon as GunParameters;
			if (gunParameters != null)
			{
				return GetCategory(gunParameters);
			}
			if (weapon is GrenadeParameters)
			{
				return WeaponCategory.Grenades;
			}
			return null;
		}

		private static WeaponCategory? GetCategory(GunParameters gun)
		{
			switch (gun.GunType)
			{
			case GunType.Pistol:
				return WeaponCategory.Pistols;
			case GunType.Heavy:
				return WeaponCategory.Heavy;
			case GunType.Shotgun:
				return WeaponCategory.Heavy;
			case GunType.Smg:
				return WeaponCategory.Smg;
			case GunType.Rifels:
				return WeaponCategory.Rifels;
			default:
				Log.Error($"{gun.GunType} not supported");
				return null;
			}
		}

		private void OnCategorySelected(WeaponCategory category)
		{
			_itemsView.SetWeapons((from weapon in _weaponByCategory[category]
				orderby weapon.Cost
				select weapon).ToArray());
		}

		private void OnInventorySelected(WeaponParameters inventory)
		{
			_detailsView.ShowDeatils(inventory);
			_gameController.BuyWeapon(inventory);
			_weaponView.SetWeapons(_gameController.GetPlayerWeapons());
			_weaponView.SetMoney(_gameController.Player.GetMoney());
			_itemsView.Refresh();
		}

		public override void Show()
		{
			base.Show();
			_categoriesView.SelectFirst();
			_detailsView.Clear();
		}
	}
}
