using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class PlayerWeaponsView : View
	{
		public const int MaxWeaponCount = 5;

		[SerializeField]
		private PlayerWeaponView _weaponPrefab;

		[SerializeField]
		private Text _moneyText;

		private ViewPool<PlayerWeaponView> _pool;

		public bool MoneyEnabled
		{
			set
			{
				_moneyText.gameObject.SetActive(value);
			}
		}

		private void Awake()
		{
			_pool = new ViewPool<PlayerWeaponView>(_weaponPrefab, 5);
			MoneyEnabled = false;
		}

		public void SetWeapons(WeaponParameters[] weapons)
		{
			int num = Math.Min(weapons.Length, 5);
			PlayerWeaponView[] items = _pool.GetItems(num);
			for (int i = 0; i < num; i++)
			{
				items[i].Weapon = weapons[i];
			}
		}

		public void SetMoney(int money)
		{
			_moneyText.text = "$" + money;
		}
	}
}
