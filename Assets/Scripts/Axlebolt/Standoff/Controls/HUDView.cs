using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Settings.Controls;
using Axlebolt.Standoff.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class HUDView : View
	{
		public AimView AimView;

		public ItemSelectView ItemSelectView;

		public WeaponInfoView WeaponInfoView;

		public PlayerStatsView PlayerStatsView;

		public HitIndicatorView HitIndicatorView;

		public ProgressView ProgressView;

		public ShootButton ShootButton;

		public ActionButton ActionButton;

		public DropButton DropButton;

		public PlayerDebugView PlayerDebugView;

		private List<IHudComponentView> _views;

		private PlayerController _playerController;

		private TouchController _touchController;

		public PlayerController PlayerController
		{
			set
			{
				_playerController = value;
				foreach (IHudComponentView view in _views)
				{
					view.SetPlayerController(_playerController);
				}
				base.gameObject.SetActive(_playerController != null);
			}
		}

		private void Awake()
		{
			_views = new List<IHudComponentView>
			{
				AimView,
				WeaponInfoView,
				PlayerStatsView,
				HitIndicatorView,
				ProgressView,
				ShootButton,
				ActionButton,
				DropButton
			};
		}

		public void Initialize(TouchController touchController)
		{
			_touchController = touchController;
			_touchController.WeaponSelectArea.onItemSelect += OnWeaponSelect;
			_touchController.WeaponSelectArea.onItemSelected += OnWeaponSelected;
			_touchController.WeaponSelectArea.onTouchDownEvent += OnWeaponTouchDown;
			_touchController.GrenadeSelectArea.onItemSelect += OnGrenadeSelect;
			_touchController.GrenadeSelectArea.onItemSelected += OnGrenadeSelected;
			_touchController.GrenadeSelectArea.onTouchDownEvent += OnGreandeTouchDown;
		}

		private void OnWeaponSelect(int slotIndex)
		{
			ItemSelectView.SetSelectedSlot(slotIndex);
		}

		private void OnWeaponSelected(int slotIndex)
		{
			ItemSelectView.Hide();
		}

		private void OnWeaponTouchDown()
		{
			WeaponController[] weapons = _playerController.WeaponryController.GetWeapons();
			WeaponController[] array = new WeaponController[4];
			WeaponController[] array2 = weapons;
			foreach (WeaponController weaponController in array2)
			{
				array[weaponController.SlotIndex - 1] = weaponController;
			}
			ItemSelectView.Show();
			ItemSelectView.SetWeaponList(array);
		}

		private void OnGrenadeSelect(int slotIndex)
		{
		}

		private void OnGrenadeSelected(int slotIndex)
		{
		}

		public void OnGreandeTouchDown()
		{
		}

		private void Update()
		{
			if (!(_playerController == null))
			{
				foreach (IHudComponentView view in _views)
				{
					view.UpdateView(_playerController);
				}
			}
		}

		public void SetControlsSettings(ControlsSettings settings)
		{
			AimView.SetDefaultSightType(settings.SightType);
			AimView.SetSightColor(settings.SightColor);
			ControlsCustomSettings customSettings = settings.CustomSettings;
			if (customSettings != null)
			{
				ControlsSettingsManager.SetControlSettings(WeaponInfoView.GetRequireComponent<RectTransform>(), customSettings.WeaponInfoView);
				RectTransform requireComponent = WeaponInfoView.AmmoView.GetRequireComponent<RectTransform>();
				Vector2 sizeDelta = requireComponent.sizeDelta;
				float y = sizeDelta.y;
				ControlsSettingsManager.SetControlSettings(requireComponent, customSettings.AmmoView);
				RectTransform rectTransform = requireComponent;
				Vector2 sizeDelta2 = requireComponent.sizeDelta;
				rectTransform.sizeDelta = new Vector2(sizeDelta2.x, y);
			}
		}

		public void RegisterHud(IHudComponentView hudComponentView)
		{
			_views.Add(hudComponentView);
		}
	}
}
