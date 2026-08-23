using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ControlsCustomizationController : SettingsTabController<ControlsCustomizationController>, IPointerClickHandler, IEventSystemHandler
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CCreate_003Ec__async0 : IAsyncStateMachine
		{
			internal ControlsCustomizationController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						_0024this.InitContainer();
						_0024awaiter0 = AsyncUtility.AsyncComplete(_0024this.LoadHud()).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		private Canvas _canvas;

		[SerializeField]
		private Button _applyButton;

		[SerializeField]
		private CloseButton _closeButton;

		[SerializeField]
		private Button _resetButton;

		[SerializeField]
		private ControlsCustomizationField _sizeCustomizationField;

		[SerializeField]
		private ControlsCustomizationField _colorCustomizationField;

		[SerializeField]
		private ZoneEditor _zoneEditor;

		private RectTransform _containerTransform;

		private ControlsCustomization _joystickCustomization;

		private ControlsCustomization _ammoCustomization;

		private ControlsCustomization _weaponCustomization;

		private ControlsCustomization _currentSelected;

		private ControlsCustomization[] _buttonsCustomizations;

		private ControlsSettings _model;

		private ControlsCustomSettings _settings;

		public override void Init()
		{
			base.Init();
			_canvas = ViewUtility.GetCanvas(base.transform);
			_applyButton.onClick.AddListener(base.Apply);
			_closeButton.CloseHandler = TryClose;
			_resetButton.onClick.AddListener(ResetDefaults);
			base.DirtyChangedEvent = (Action<bool>)Delegate.Combine(base.DirtyChangedEvent, (Action<bool>)delegate(bool isDirty)
			{
				_applyButton.interactable = isDirty;
			});
		}

		public override void OnOpen()
		{
			base.OnOpen();
			_model = ControlsSettingsManager.Instance.Model;
			_settings = _model.CustomSettings;
			Create();
		}

		public override void OnClose()
		{
			Clear();
			_model = null;
			_settings = null;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCreate_003Ec__async0))]
		private void Create()
		{
			_003CCreate_003Ec__async0 stateMachine = default(_003CCreate_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void TryClose()
		{
			CanClose(delegate(bool canClose)
			{
				if (canClose)
				{
					Close();
				}
			});
		}

		private void Clear()
		{
			UnityEngine.Object.Destroy(_containerTransform.gameObject);
			UnSelect();
			_zoneEditor.ValueChangedHandler = null;
			_containerTransform = null;
			_joystickCustomization = null;
			_weaponCustomization = null;
			_ammoCustomization = null;
			_buttonsCustomizations = null;
		}

		protected override IEnumerator ApplyInternal()
		{
			if (_settings == null)
			{
				_settings = new ControlsCustomSettings();
			}
			_settings.FireZone = _zoneEditor.Value;
			_settings.Joystick = GetModel(_joystickCustomization);
			_settings.AmmoView = GetModel(_ammoCustomization);
			_settings.WeaponInfoView = GetModel(_weaponCustomization);
			_settings.Buttons = _buttonsCustomizations.Select((ControlsCustomization customization) => new ControlsCustomSettings.ButtonElement
			{
				Name = customization.gameObject.name,
				Element = GetModel(customization)
			}).ToArray();
			_model.CustomSettings = _settings;
			UnSelect();
			yield return ControlsSettingsManager.Instance.Save(_model);
		}

		private static ControlsCustomSettings.Element GetModel(ControlsCustomization customization)
		{
			ControlsCustomSettings.Element element = new ControlsCustomSettings.Element();
			element.PosX = customization.RectTransform.position.x;
			element.PosY = customization.RectTransform.position.y;
			element.Alpha = customization.GetAlpha();
			element.SizeX = customization.RectTransform.sizeDelta.x;
			return element;
		}

		private static void SetModel(ControlsCustomization customization, ControlsCustomSettings.Element model)
		{
			if (model != null)
			{
				customization.RectTransform.position = new Vector3(model.PosX, model.PosY, 0f);
				customization.SetAlpha(model.Alpha);
				float num = customization.RectTransform.sizeDelta.y / customization.RectTransform.sizeDelta.x;
				customization.RectTransform.sizeDelta = new Vector2(model.SizeX, model.SizeX * num);
			}
		}

		public override void ResetDefaults()
		{
			Clear();
			_settings = null;
			Create();
			OnDirty();
		}

		private void InitContainer()
		{
			GameObject gameObject = new GameObject("Container");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.SetAsFirstSibling();
			_containerTransform = gameObject.AddComponent<RectTransform>();
			_containerTransform.anchorMin = Vector3.zero;
			_containerTransform.anchorMax = Vector3.one;
			_containerTransform.sizeDelta = Vector2.zero;
		}

		private IEnumerator LoadHud()
		{
			yield return Singleton<ScenePrefab>.Instance.LoadPrefab<PlayerControls>("Controls");
			PlayerControls instance = Singleton<ScenePrefab>.Instance.Singleton<PlayerControls>(_canvas.transform);
			CopyPlayerControls(instance);
			UnityEngine.Object.Destroy(instance.gameObject);
		}

		private void CopyPlayerControls(PlayerControls controls)
		{
			ProcessWeaponInfo(controls);
			ProcessInteractableButtons(controls);
			ProcessJoystick(controls);
		}

		private void ProcessJoystick(PlayerControls controls)
		{
			FloatingJoystick floatingJoystick = Copy<FloatingJoystick>(controls.gameObject);
			floatingJoystick.joystickCurcleRect.gameObject.SetActive(true);
			_joystickCustomization = CreateControlCustomization(floatingJoystick.joystickCurcleRect.gameObject, ControlType.Joystick);
			if (_settings != null)
			{
				SetModel(_joystickCustomization, _settings.Joystick);
				float x = floatingJoystick.touchTriggerArea.anchorMax.x;
				floatingJoystick.touchTriggerArea.anchorMax = new Vector2(x, _settings.FireZone);
			}
			_zoneEditor.Value = floatingJoystick.touchTriggerArea.anchorMax.y;
			_zoneEditor.ZoneEditorControl.SelectHandler = UnSelect;
			_zoneEditor.ValueChangedHandler = OnZoneValueChaged;
		}

		private void ProcessInteractableButtons(PlayerControls controls)
		{
			TouchController touchController = controls.GetRequireComponent<TouchController>();
			InteractableButton[] source = CopyAll(controls.gameObject, delegate(InteractableButton button)
			{
				if (button == touchController.JumpButton)
				{
					return !_model.JumpByDoubleTap;
				}
				return !(button == touchController.ShootButton) || _model.ShootButtonEnabled;
			});
			_buttonsCustomizations = source.Select((InteractableButton button) => CreateControlCustomization(button.gameObject, ControlType.InteractableButton)).ToArray();
			if (_settings == null)
			{
				return;
			}
			ControlsCustomization[] buttonsCustomizations = _buttonsCustomizations;
			foreach (ControlsCustomization buttonCustomization in buttonsCustomizations)
			{
				ControlsCustomSettings.ButtonElement buttonElement = _settings.Buttons.FirstOrDefault((ControlsCustomSettings.ButtonElement button) => button.Name == buttonCustomization.gameObject.name);
				SetModel(buttonCustomization, (buttonElement != null) ? buttonElement.Element : null);
			}
		}

		private void ProcessWeaponInfo(PlayerControls controls)
		{
			GunParameters gunParameters = (GunParameters)WeaponUtility.LoadWeapon(WeaponId.Deagle);
			AmmoView ammoView = Copy<AmmoView>(controls.gameObject);
			ammoView.SetTextViewVisible(true);
			ammoView.SetCriticalMagazineAmmo(false);
			ammoView.SetCurMagazineAmmo(gunParameters.Ammunition.MagazineCapacity);
			ammoView.SetRestAmmoAmount(gunParameters.Ammunition.Capacity);
			_ammoCustomization = CreateControlCustomization(ammoView.gameObject, ControlType.AmmoView);
			WeaponInfoView weaponInfoView = Copy<WeaponInfoView>(controls.gameObject);
			weaponInfoView.enabled = false;
			weaponInfoView.WeaponImage.sprite = gunParameters.Sprites.Icon;
			weaponInfoView.WeaponImage.enabled = true;
			_weaponCustomization = CreateControlCustomization(weaponInfoView.gameObject, ControlType.WeaponInfoView);
			if (_settings != null)
			{
				SetModel(_ammoCustomization, _settings.AmmoView);
				SetModel(_weaponCustomization, _settings.WeaponInfoView);
			}
		}

		private ControlsCustomization CreateControlCustomization(GameObject controlGameObject, ControlType controlType)
		{
			controlGameObject.gameObject.SetActive(true);
			ControlsCustomization controlsCustomization = controlGameObject.AddComponent<ControlsCustomization>();
			controlsCustomization.ControlType = controlType;
			controlsCustomization.SelectHandler = OnSelectedHandler;
			controlsCustomization.DragHandler = OnDragHandler;
			controlsCustomization.PosXValidator = ValidatePosX;
			controlsCustomization.PosYValidator = ValidatePosY;
			return controlsCustomization;
		}

		private T Copy<T>(GameObject go) where T : MonoBehaviour
		{
			T componentInChildren = go.GetComponentInChildren<T>();
			T result = UnityEngine.Object.Instantiate(componentInChildren, _containerTransform);
			result.name = go.name;
			return result;
		}

		private T[] CopyAll<T>(GameObject go, Func<T, bool> selector) where T : MonoBehaviour
		{
			return go.GetComponentsInChildren<T>(true).Where(selector).Select(delegate(T copy)
			{
				T result = UnityEngine.Object.Instantiate(copy, _containerTransform);
				result.name = copy.name;
				return result;
			})
				.ToArray();
		}

		private void OnSelectedHandler(ControlsCustomization controlsCustomization)
		{
			UnSelect();
			_currentSelected = controlsCustomization;
			RectTransform rectTransform = _currentSelected.RectTransform;
			if (controlsCustomization.ControlType != ControlType.AmmoView)
			{
				_sizeCustomizationField.BindTo(rectTransform);
				_sizeCustomizationField.Value = GetSizeByX(rectTransform.sizeDelta.x, _currentSelected.ControlType);
				_sizeCustomizationField.ValueChangedHandler = OnSizeChanged;
			}
			_colorCustomizationField.BindTo(rectTransform);
			_colorCustomizationField.Value = _currentSelected.GetAlpha();
			_colorCustomizationField.ValueChangedHandler = OnColorChanged;
		}

		private void UnSelect()
		{
			_currentSelected = null;
			_sizeCustomizationField.Unbind();
			_sizeCustomizationField.ValueChangedHandler = null;
			_colorCustomizationField.Unbind();
			_colorCustomizationField.ValueChangedHandler = null;
		}

		private void OnDragHandler(ControlsCustomization controlsCustomization)
		{
			_sizeCustomizationField.UpdatePosition();
			_colorCustomizationField.UpdatePosition();
			OnDirty();
		}

		private void OnSizeChanged(float size)
		{
			RectTransform rectTransform = _currentSelected.RectTransform;
			float num = rectTransform.sizeDelta.y / rectTransform.sizeDelta.x;
			float xBySize = GetXBySize(size, _currentSelected.ControlType);
			rectTransform.sizeDelta = new Vector2(xBySize, xBySize * num);
			if (_currentSelected.ControlType == ControlType.WeaponInfoView)
			{
				_ammoCustomization.RectTransform.sizeDelta = new Vector2(xBySize, _ammoCustomization.RectTransform.sizeDelta.y);
			}
			_sizeCustomizationField.UpdatePosition();
			_colorCustomizationField.UpdatePosition();
			OnDirty();
		}

		private static float GetXBySize(float size, ControlType controlType)
		{
			switch (controlType)
			{
			case ControlType.InteractableButton:
				return 100f + 300f * size;
			case ControlType.WeaponInfoView:
				return 320f + 380f * size;
			case ControlType.Joystick:
				return 200f + 300f * size;
			default:
				throw new ArgumentOutOfRangeException("controlType", controlType, null);
			}
		}

		private static float GetSizeByX(float x, ControlType controlType)
		{
			switch (controlType)
			{
			case ControlType.InteractableButton:
				return (x - 100f) / 300f;
			case ControlType.WeaponInfoView:
				return (x - 320f) / 380f;
			case ControlType.Joystick:
				return (x - 200f) / 300f;
			default:
				throw new ArgumentOutOfRangeException("controlType", controlType, null);
			}
		}

		private float ValidatePosX(ControlsCustomization controlsCustomization, float posX)
		{
			if (posX < 0f)
			{
				return 0f;
			}
			if (controlsCustomization.ControlType == ControlType.Joystick && posX > (float)Screen.width * 0.5f)
			{
				return (float)Screen.width * 0.5f;
			}
			return (!(posX > (float)Screen.width)) ? posX : ((float)Screen.width);
		}

		private float ValidatePosY(ControlsCustomization controlsCustomization, float posY)
		{
			if (posY < 0f)
			{
				return 0f;
			}
			if (controlsCustomization.ControlType == ControlType.Joystick)
			{
				float value = _zoneEditor.Value;
				if (posY > value * (float)Screen.height)
				{
					return value * (float)Screen.height;
				}
			}
			return (!(posY > (float)Screen.height)) ? posY : ((float)Screen.height);
		}

		private void OnZoneValueChaged(float value)
		{
			RectTransform rectTransform = _joystickCustomization.RectTransform;
			Vector3 position = rectTransform.position;
			float x = ValidatePosX(_joystickCustomization, position.x);
			float y = ValidatePosY(_joystickCustomization, position.y);
			rectTransform.position = new Vector3(x, y, position.z);
			OnDirty();
		}

		private void OnColorChanged(float alhpa)
		{
			_currentSelected.SetAlpha(alhpa);
			OnDirty();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			UnSelect();
		}
	}
}
