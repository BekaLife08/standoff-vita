using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class WeaponCategoriesView : View
	{
		public const int MaxCategories = 6;

		[SerializeField]
		[NotNull]
		private WeaponCategoryView _categoryPrefab;

		private ViewPool<WeaponCategoryView> _pool;

		public Action<WeaponCategory> OnCategorySelected
		{
			get;
			set;
		}

		private void Awake()
		{
			_pool = new ViewPool<WeaponCategoryView>(_categoryPrefab, 6);
		}

		public void SetCategories(WeaponCategory[] categories)
		{
			int num = Math.Min(categories.Length, 6);
			WeaponCategoryView[] items = _pool.GetItems(num);
			for (int i = 0; i < num; i++)
			{
				WeaponCategory category = categories[i];
				WeaponCategoryView item = items[i];
				item.Name = Localize(category);
				item.OnSelected = delegate
				{
					OnSelected(category, item);
				};
				item.IsSelected = false;
			}
		}

		public void SelectFirst()
		{
			if (_pool.Items.Length > 0)
			{
				_pool.Items[0].IsSelected = true;
			}
		}

		private void OnSelected(WeaponCategory category, WeaponCategoryView selectedItem)
		{
			WeaponCategoryView[] items = _pool.Items;
			foreach (WeaponCategoryView weaponCategoryView in items)
			{
				if (weaponCategoryView.IsSelected && weaponCategoryView != selectedItem)
				{
					weaponCategoryView.IsSelected = false;
				}
			}
			OnCategorySelected(category);
		}

		private static string Localize(WeaponCategory category)
		{
			switch (category)
			{
			case WeaponCategory.Pistols:
				return ScriptLocalization.WeaponCategories.Pistols;
			case WeaponCategory.Heavy:
				return ScriptLocalization.WeaponCategories.Heavy;
			case WeaponCategory.Rifels:
				return ScriptLocalization.WeaponCategories.Rifles;
			case WeaponCategory.Grenades:
				return ScriptLocalization.WeaponCategories.Grenade;
			default:
				return category.ToString();
			}
		}
	}
}
