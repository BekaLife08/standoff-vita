using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class WeaponItemsView : View
	{
		public const int MaxItems = 7;

		[SerializeField]
		private WeaponItemView _prefab;

		private ViewPool<WeaponItemView> _pool;

		private GameController _gameController;

		private WeaponItemView _currentSelection;

		private WeaponParameters[] _weapons;

		internal Action<WeaponParameters> OnInventorySelected
		{
			get;
			set;
		}

		public bool WeaponCostEnabled
		{
			get;
			set;
		}

		private void Awake()
		{
			_prefab.IsSelected = false;
			_pool = new ViewPool<WeaponItemView>(_prefab, 7);
		}

		public void Init([NotNull] GameController gameController)
		{
			if (gameController == null)
			{
				throw new ArgumentNullException("gameController");
			}
			_gameController = gameController;
		}

		public void SetWeapons([NotNull] WeaponParameters[] weapons)
		{
			if (weapons == null)
			{
				throw new ArgumentNullException("weapons");
			}
			if (_gameController == null)
			{
				throw new InvalidOperationException("View not initialized");
			}
			_weapons = weapons;
			WeaponParameters[] playerWeapons = _gameController.GetPlayerWeapons();
			int num = Math.Min(weapons.Length, 7);
			WeaponItemView[] items = _pool.GetItems(num);
			for (int i = 0; i < num; i++)
			{
				WeaponItemView weaponItemView = items[i];
				WeaponParameters weaponParameters = weapons[i];
				weaponItemView.Name = weaponParameters.DisplayName;
				weaponItemView.OnSelected = null;
				weaponItemView.IsSelected = playerWeapons.Contains(weaponParameters);
				weaponItemView.WeaponIcon = weaponParameters.Sprites.Icon;
			}
			Refresh();
		}

		public void Refresh()
		{
			for (int i = 0; i < _pool.Items.Length; i++)
			{
				WeaponItemView item = _pool.Items[i];
				WeaponParameters inventory = _weapons[i];
				item.WeaponCost = _weapons[i].Cost;
				item.WeaponCostEnabled = WeaponCostEnabled;
				if (!item.IsSelected)
				{
					item.IsSelectable = _gameController.CanBuyWeapon(inventory);
				}
				item.OnSelected = delegate
				{
					OnSelected(inventory, item);
				};
			}
		}

		private void OnSelected(WeaponParameters weapon, WeaponItemView selected)
		{
			WeaponItemView[] items = _pool.Items;
			foreach (WeaponItemView weaponItemView in items)
			{
				if (weaponItemView.IsSelected && weaponItemView != selected)
				{
					weaponItemView.IsSelected = false;
				}
			}
			OnInventorySelected(weapon);
		}
	}
}
