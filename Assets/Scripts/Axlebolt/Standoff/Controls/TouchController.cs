using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Settings.Controls;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class TouchController : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _interactiveArea;

		[SerializeField]
		private AimingArea _aimingArea;

		public ItemSelectArea WeaponSelectArea;

		public ItemSelectArea GrenadeSelectArea;

		[SerializeField]
		private FloatingJoystick _joystick;

		[SerializeField]
		private ShootArea _shootArea;

		[SerializeField]
		private InteractableButton _shootButton;

		[SerializeField]
		private InteractableButton _jumpButton;

		[SerializeField]
		private InteractableButton _aimButton;

		[SerializeField]
		private InteractableButton _crouchButton;

		[SerializeField]
		private InteractableButton _dropButton;

		[SerializeField]
		private InteractableButton _actionButton;

		[SerializeField]
		private ClingingButton _crouchButtonClinger;

		[SerializeField]
		private DoubleTapArea _jumpDoubleTapArea;

		private TouchHandler _touchHandler;

		private bool _isAimButtonActive = true;

		private PlayerInputs _playerInputs = new PlayerInputs();

		private bool _buttonsVisible = true;

		public InteractableButton AimButton
		{
			get
			{
				return _aimButton;
			}
			set
			{
				_aimButton = value;
			}
		}

		public InteractableButton ShootButton
		{
			[CompilerGenerated]
			get
			{
				return _shootButton;
			}
		}

		public InteractableButton JumpButton
		{
			[CompilerGenerated]
			get
			{
				return _jumpButton;
			}
		}

		private void Awake()
		{
			Initialize();
		}

		public void Initialize()
		{
			_touchHandler = base.gameObject.AddComponent<TouchHandler>();
			_touchHandler.interactiveArea = _interactiveArea;
			_touchHandler.interactableZoneList.Add(_aimingArea);
			_touchHandler.interactableZoneList.Add(WeaponSelectArea);
			_touchHandler.interactableZoneList.Add(GrenadeSelectArea);
			_touchHandler.interactableZoneList.Add(_joystick);
			_touchHandler.interactableZoneList.Add(_shootArea);
			_touchHandler.interactableZoneList.Add(_shootButton);
			_touchHandler.interactableZoneList.Add(_jumpButton);
			_touchHandler.interactableZoneList.Add(_aimButton);
			_touchHandler.interactableZoneList.Add(_crouchButton);
			_touchHandler.interactableZoneList.Add(_dropButton);
			_touchHandler.interactableZoneList.Add(_actionButton);
			_touchHandler.interactableZoneList.Add(_jumpDoubleTapArea);
			GrenadeSelectArea.IsActive = false;
			_touchHandler.Initialize();
			WeaponSelectArea.onItemSelect += OnWeaponSelect;
			WeaponSelectArea.onItemSelected += OnWeaponSelected;
			GrenadeSelectArea.onItemSelect += OnGrenadeSelect;
			GrenadeSelectArea.onItemSelected += OnGrenadeSelected;
			_aimButton.OnButtonDownEvent += OnAimClicked;
			_jumpDoubleTapArea.onSecondTapEvent += OnJumpDoubleTouch;
			_dropButton.OnButtonDownEvent += OnDropClicked;
		}

		public void OnWeaponSelect(int slotIndex)
		{
		}

		private void OnWeaponSelected(int slotIndex)
		{
			if (slotIndex == 0)
			{
				_playerInputs.IsToReload = true;
			}
			else
			{
				_playerInputs.SwitchToWeapon = slotIndex;
			}
		}

		private void OnGrenadeSelect(int slotIndex)
		{
		}

		private void OnGrenadeSelected(int slotIndex)
		{
		}

		public void SetAimButtonActive(bool isActive)
		{
			_aimButton.gameObject.SetActive(isActive);
			_isAimButtonActive = isActive;
		}

		private void OnAimClicked()
		{
			_playerInputs.IsToAim = true;
		}

		private void OnJumpDoubleTouch()
		{
			_playerInputs.IsToJump = true;
		}

		private void OnDropClicked()
		{
			_playerInputs.IsToDrop = true;
		}

		public PlayerInputs GetPlayerInputs()
		{
			return _playerInputs;
		}

		public void HandleTouches()
		{
			_playerInputs = new PlayerInputs();
			_touchHandler.Handle();
			PlayerInputs playerInputs = _playerInputs;
			Vector3 joystickAxis = _joystick.JoystickAxis;
			playerInputs.Horizontal = joystickAxis.x;
			PlayerInputs playerInputs2 = _playerInputs;
			Vector3 joystickAxis2 = _joystick.JoystickAxis;
			playerInputs2.Vertical = joystickAxis2.y;
			Vector2 aimingAxis = _aimingArea.AimingAxis;
			_playerInputs.DeltaAimAngles.x = aimingAxis.y;
			Vector2 aimingAxis2 = _aimingArea.AimingAxis;
			_playerInputs.DeltaAimAngles.y = aimingAxis2.x;
			_playerInputs.IsToFire = (_shootArea.IsShooting || _shootButton.IsButton);
			if (_jumpButton.IsActive)
			{
				_playerInputs.IsToJump = _jumpButton.IsButton;
			}
			if (_actionButton.IsActive)
			{
				_playerInputs.IsToAction = _actionButton.IsButton;
			}
			_playerInputs.IsCrouching = _crouchButtonClinger.IsClicked;
		}

		public void SetButtonsVisible(bool isVisible)
		{
			if (isVisible != _buttonsVisible)
			{
				_buttonsVisible = isVisible;
				if (_shootButton.IsActive)
				{
					_shootButton.gameObject.SetActive(isVisible);
				}
				if (_isAimButtonActive)
				{
					_aimButton.gameObject.SetActive(isVisible);
				}
				_crouchButton.gameObject.SetActive(isVisible);
				if (_jumpButton.IsActive)
				{
					_jumpButton.gameObject.SetActive(isVisible);
				}
			}
		}

		public void SetControlsSettings(ControlsSettings settings)
		{
			_aimingArea.Acceleration = settings.Acceleration;
			_jumpButton.IsActive = !settings.JumpByDoubleTap;
			_jumpButton.gameObject.SetActive(_jumpButton.IsActive && _buttonsVisible);
			_jumpDoubleTapArea.IsActive = settings.JumpByDoubleTap;
			_shootButton.IsActive = settings.ShootButtonEnabled;
			_shootButton.gameObject.SetActive(_shootButton.IsActive && _buttonsVisible);
			ApplyCustomization(settings.CustomSettings);
			_joystick.SetJoystickType(settings.JoysticType);
		}

		private void ApplyCustomization(ControlsCustomSettings settings)
		{
			if (settings == null)
			{
				return;
			}
			ControlsSettingsManager.SetControlSettings(_joystick.joystickCurcleRect, settings.Joystick);
			Vector2 anchorMax = _joystick.touchTriggerArea.anchorMax;
			float x = anchorMax.x;
			_joystick.touchTriggerArea.anchorMax = new Vector2(x, settings.FireZone);
			Vector2 anchorMin = _shootArea.interactiveZone.anchorMin;
			float x2 = anchorMin.x;
			_shootArea.interactiveZone.anchorMin = new Vector2(x2, settings.FireZone);
			InteractableButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<InteractableButton>(includeInactive: false);
			foreach (InteractableButton interactableButton in componentsInChildren)
			{
				ControlsCustomSettings.ButtonElement buttonElement = settings.Buttons.FirstOrDefault((ControlsCustomSettings.ButtonElement s) => s.Name == interactableButton.gameObject.name);
				if (buttonElement != null)
				{
					ControlsSettingsManager.SetControlSettings(interactableButton.interactiveZone, buttonElement.Element);
				}
			}
		}
	}
}
