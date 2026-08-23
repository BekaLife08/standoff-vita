using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class GunParametersView : View
	{
		public const int ParametersCount = 7;

		[NotNull]
		[SerializeField]
		private Text _ammoTitle;

		[SerializeField]
		[NotNull]
		private Text _ammoText;

		[NotNull]
		[SerializeField]
		private GunParameterView _parameterPrefab;

		private ViewPool<GunParameterView> _pool;

		private readonly float[] _maxValues = new float[7];

		private void Awake()
		{
			_pool = new ViewPool<GunParameterView>(_parameterPrefab, 7);
			_ammoTitle.gameObject.SetActive(value: false);
			_ammoText.gameObject.SetActive(value: false);
		}

		public void SetGuns(GunParameters[] guns)
		{
			foreach (GunParameters gunParameters in guns)
			{
				_maxValues[0] = Math.Max(_maxValues[0], gunParameters.Damage.ChestAndArmsDamage);
				_maxValues[1] = Math.Max(_maxValues[1], gunParameters.FireRate);
				_maxValues[2] = Math.Max(_maxValues[2], gunParameters.RecoilControl);
				_maxValues[3] = Math.Max(_maxValues[3], gunParameters.AccurateRange);
				_maxValues[4] = Math.Max(_maxValues[4], gunParameters.MovementRate);
				_maxValues[5] = Math.Max(_maxValues[5], gunParameters.ArmorPenetration);
				_maxValues[6] = Math.Max(_maxValues[6], gunParameters.PenetrationPower);
			}
		}

		public void ShowGun(GunParameters gun)
		{
			Show();
			_ammoTitle.gameObject.SetActive(value: true);
			_ammoText.gameObject.SetActive(value: true);
			_ammoText.text = gun.Ammunition.MagazineCapacity + "/" + gun.Ammunition.Capacity;
			GunParameterView[] items = _pool.GetItems(7);
			int num = 0;
			items[num].Name = ScriptLocalization.Gun.Damage;
			items[num].Value = gun.Damage.ChestAndArmsDamage.ToString();
			items[num].ValueProgress = (float)gun.Damage.ChestAndArmsDamage / _maxValues[num];
			num++;
			items[num].Name = ScriptLocalization.Gun.FireRate;
			items[num].Value = string.Empty;
			items[num].ValueProgress = (float)gun.FireRate / _maxValues[num];
			num++;
			items[num].Name = ScriptLocalization.Gun.RecoilControl;
			items[num].Value = string.Empty;
			items[num].ValueProgress = (float)gun.RecoilControl / _maxValues[num];
			num++;
			items[num].Name = ScriptLocalization.Gun.AccurateRange;
			items[num].Value = gun.AccurateRange + "m";
			items[num].ValueProgress = (float)gun.AccurateRange / _maxValues[num];
			num++;
			items[num].Name = ScriptLocalization.Gun.MovementRate;
			items[num].Value = gun.MovementRate.ToString();
			items[num].ValueProgress = (float)gun.MovementRate / _maxValues[num];
			num++;
			items[num].Name = ScriptLocalization.Gun.ArmorPenetration;
			items[num].Value = gun.ArmorPenetration.ToString();
			items[num].ValueProgress = gun.ArmorPenetration / _maxValues[num];
			num++;
			items[num].Name = ScriptLocalization.Gun.PenetrationPower;
			items[num].Value = gun.PenetrationPower.ToString();
			items[num].ValueProgress = (float)gun.PenetrationPower / _maxValues[num];
		}
	}
}
