using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Settings.Controls;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class PlayerControls : MonoBehaviour
	{
		public enum InputSourceType
		{
			Keyboard,
			TouchScreen,
			InputSimulator
		}

		[SerializeField]
		private InputSourceType _sourceType = InputSourceType.TouchScreen;

		private KeyboardControl _keyboardControl;

		private PlayerController _playerController;

		private TouchController _touchController;

		private RandomInputsSimulator _inputsSimulator;

		private readonly HashSet<object> _disableMovementRequests = new HashSet<object>();

		private readonly HashSet<object> _disableFiringRequests = new HashSet<object>();

		private readonly HashSet<object> _disableRequests = new HashSet<object>();

		private ControlsSettings _controlsSettings;

		public static PlayerControls Instance
		{
			get;
			private set;
		}

		public HUDView HudView
		{
			get;
			private set;
		}

		public InputSourceType SourceType
		{
			get
			{
				return _sourceType;
			}
			set
			{
				if (value == InputSourceType.InputSimulator)
				{
					_inputsSimulator = this.GetRequireComponent<RandomInputsSimulator>();
				}
				_sourceType = value;
			}
		}

		public PlayerController PlayerController
		{
			get
			{
				return _playerController;
			}
			set
			{
				_playerController = value;
				HudView.PlayerController = _playerController;
				if (_sourceType == InputSourceType.TouchScreen)
				{
					_touchController.SetButtonsVisible(PlayerController != null);
				}
			}
		}

		private void Awake()
		{
			if (Application.isMobilePlatform)
			{
				_sourceType = InputSourceType.TouchScreen;
			}
			_keyboardControl = base.gameObject.AddComponent<KeyboardControl>();
			_touchController = this.GetRequireComponent<TouchController>();
			_touchController.Initialize();
			HudView = this.GetRequireComponent<HUDView>();
			HudView.Initialize(_touchController);
			SetControlsSettings(new ControlsSettings());
			SourceType = _sourceType;
		}

		public static IEnumerator CreateInstance(Transform canvasTransform)
		{
			if (Instance != null)
			{
				throw new Exception("PlayerControls already exists!");
			}
			yield return Singleton<ScenePrefab>.Instance.LoadPrefab<PlayerControls>("Controls");
			Instance = Singleton<ScenePrefab>.Instance.Singleton<PlayerControls>(canvasTransform);
			Instance.SetControlsSettings(ControlsSettingsManager.Instance.Model);
			ControlsSettingsManager.Instance.SettingsChangedEvent += Instance.SetControlsSettings;
		}

		public void SetControlsSettings([NotNull] ControlsSettings controlsSettings)
		{
			if (controlsSettings == null)
			{
				throw new ArgumentNullException("controlsSettings");
			}
			_controlsSettings = controlsSettings;
			_touchController.SetControlsSettings(controlsSettings);
			HudView.SetControlsSettings(controlsSettings);
		}

		public void RequestMovementDisable(object owner)
		{
			_disableMovementRequests.Add(owner);
		}

		public void RequestMovementEnable(object owner)
		{
			_disableMovementRequests.Remove(owner);
		}

		public void RequestFiringDisable(object owner)
		{
			_disableFiringRequests.Add(owner);
		}

		public void RequestFiringEnable(object owner)
		{
			_disableFiringRequests.Remove(owner);
		}

		public void RequestDisable(object owner)
		{
			_disableRequests.Add(owner);
		}

		public void RequestEnable(object owner)
		{
			_disableRequests.Remove(owner);
		}

		public void RequestPartialDisable(object owner)
		{
			RequestMovementDisable(owner);
			RequestFiringDisable(owner);
		}

		public void RequestPartialEnable(object owner)
		{
			RequestMovementEnable(owner);
			RequestFiringEnable(owner);
		}

		private void Update()
		{
			if (PlayerController == null)
			{
				return;
			}
			PlayerInputs inputs = GetInputs();
			WeaponController currentWeapon = PlayerController.WeaponryController.CurrentWeapon;
			GunController gunController = currentWeapon as GunController;
			if (gunController != null)
			{
				if (gunController.GunParameters.SightType == SightType.CollimatorSight || gunController.GunParameters.SightType == SightType.SniperScope)
				{
					if (gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
					{
						inputs.DeltaAimAngles *= gunController.GunParameters.ScopeAimSensitivityMult * _controlsSettings.ScopeSensitivity;
					}
					else
					{
						inputs.DeltaAimAngles *= _controlsSettings.Sensitivity;
					}
					if (_sourceType == InputSourceType.TouchScreen)
					{
						_touchController.SetAimButtonActive(isActive: true);
					}
				}
				else
				{
					inputs.DeltaAimAngles *= _controlsSettings.Sensitivity;
					if (_sourceType == InputSourceType.TouchScreen)
					{
						_touchController.SetAimButtonActive(isActive: false);
					}
				}
			}
			else if (currentWeapon is KnifeController)
			{
				if (_sourceType == InputSourceType.TouchScreen)
				{
					_touchController.SetAimButtonActive(isActive: false);
				}
				inputs.DeltaAimAngles *= _controlsSettings.Sensitivity;
			}
			if (PlayerController.PhotonView.isMine)
			{
				PlayerController.SetInputs(inputs, Time.deltaTime);
			}
		}

		private PlayerInputs GetInputs()
		{
			if (IsDisabled())
			{
				return new PlayerInputs();
			}
			PlayerInputs inputsInternal = GetInputsInternal();
			IControlAffector controlAffector = PlayerController.WeaponryController.CurrentWeapon as IControlAffector;
			IControlAffector controlAffector2 = PlayerController.WeaponryController.CurrentKit as IControlAffector;
			bool flag = _disableMovementRequests.Count > 0 || (controlAffector != null && controlAffector.IsMovementLocked()) || (controlAffector2?.IsMovementLocked() ?? false);
			bool flag2 = _disableFiringRequests.Count > 0 || (controlAffector != null && controlAffector.IsFiringLocked()) || (controlAffector2?.IsFiringLocked() ?? false);
			bool flag3 = (controlAffector != null && controlAffector.IsDropLocked()) || (controlAffector2?.IsDropLocked() ?? false);
			if (flag)
			{
				inputsInternal.IsToJump = false;
				inputsInternal.Vertical = 0f;
				inputsInternal.Horizontal = 0f;
			}
			if (flag2)
			{
				inputsInternal.IsToFire = false;
			}
			if (flag3)
			{
				inputsInternal.IsToDrop = false;
			}
			return inputsInternal;
		}

		private PlayerInputs GetInputsInternal()
		{
			switch (_sourceType)
			{
			case InputSourceType.TouchScreen:
				_touchController.HandleTouches();
				return _touchController.GetPlayerInputs();
			case InputSourceType.InputSimulator:
				return _inputsSimulator.GetPlayerInputs();
			case InputSourceType.Keyboard:
				return _keyboardControl.GetPlayerInputs();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public PlayerInputs GetKeyboardInputs()
		{
			return _keyboardControl.GetPlayerInputs();
		}

		public PlayerInputs GetTouchInputs()
		{
			return _touchController.GetPlayerInputs();
		}

		private bool IsDisabled()
		{
			return _disableRequests.Count > 0;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				ControlsSettingsManager.Instance.SettingsChangedEvent -= SetControlsSettings;
				Instance = null;
			}
		}
	}
}
