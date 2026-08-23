using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.UI;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ControlsSettingsController : SettingsTabController<ControlsSettingsController>
	{
		[SerializeField]
		private ExtendedSlider _sensitivitySlider;

		[SerializeField]
		private ExtendedSlider _scopeSensitiviySlider;

		[SerializeField]
		private ExtendedSlider _accelerationSlider;

		[SerializeField]
		private BooleanButtonGroup _jumpTypeField;

		[SerializeField]
		private JoysticTypeField _joysticTypeField;

		[SerializeField]
		private Toggle _shootButtonEnabledField;

		[SerializeField]
		private SightColorField _sightColorField;

		[SerializeField]
		private SightTypeField _sightTypeField;

		[SerializeField]
		private Button _customizationButton;

		[SerializeField]
		private ControlsCustomizationController _customizationController;

		private ControlsSettings _model;

		public ControlsCustomizationController CustomizationController
		{
			[CompilerGenerated]
			get
			{
				return _customizationController;
			}
		}

		public override void Init()
		{
			base.Init();
			_sensitivitySlider.Slider.minValue = 0f;
			_sensitivitySlider.Slider.maxValue = 3f;
			_scopeSensitiviySlider.Slider.minValue = 0f;
			_scopeSensitiviySlider.Slider.maxValue = 3f;
			_accelerationSlider.Slider.minValue = 0f;
			_accelerationSlider.Slider.maxValue = 3f;
			_jumpTypeField.SetFormatter((bool doubleTap) => (!doubleTap) ? ScriptLocalization.Common.Button : ScriptLocalization.Common.DoubleTap);
			_jumpTypeField.Models = new bool[2]
			{
				true,
				false
			};
			_joysticTypeField.Models = new JoysticType[3]
			{
				JoysticType.FreeTouch,
				JoysticType.Fixed,
				JoysticType.Floating
			};
			_sightColorField.Models = new string[6]
			{
				"#3b9110",
				"#c9850f",
				"#00ffff",
				"#b82b19",
				"#000000",
				"#ffffff"
			};
			_sightTypeField.Models = new DefaultSightType[3]
			{
				DefaultSightType.DefaultDynamic,
				DefaultSightType.ClassicDynamic,
				DefaultSightType.ClassicStatic
			};
			_customizationController.Init();
			_customizationButton.onClick.AddListener(Customization);
		}

		private void Customization()
		{
			Apply(delegate
			{
				_customizationController.Open();
			});
		}

		public override void OnOpen()
		{
			base.OnOpen();
			_model = ControlsSettingsManager.Instance.Model;
			_sensitivitySlider.Value = _model.Sensitivity;
			_sensitivitySlider.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_scopeSensitiviySlider.Value = _model.ScopeSensitivity;
			_scopeSensitiviySlider.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_accelerationSlider.Value = _model.Acceleration;
			_accelerationSlider.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_jumpTypeField.Value = _model.JumpByDoubleTap;
			_jumpTypeField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_joysticTypeField.Value = _model.JoysticType;
			_joysticTypeField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_shootButtonEnabledField.isOn = _model.ShootButtonEnabled;
			_shootButtonEnabledField.onValueChanged.AddListener(OnShootButtonEnabled);
			if (!_sightColorField.Models.Contains(_model.SightColor))
			{
				List<string> list = new List<string>(_sightColorField.Models);
				list.Add(_model.SightColor);
				List<string> list2 = list;
				_sightColorField.Models = list2.ToArray();
			}
			_sightColorField.Value = _model.SightColor;
			_sightColorField.ValueChangedHandler = delegate(string value)
			{
				OnDirty();
				_sightTypeField.SetColor(value);
			};
			_sightTypeField.Value = _model.SightType;
			_sightTypeField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_sightTypeField.SetColor(_model.SightColor);
		}

		private void OnShootButtonEnabled(bool value)
		{
			OnDirty();
		}

		public override void OnClose()
		{
			_sensitivitySlider.ValueChangedHandler = null;
			_scopeSensitiviySlider.ValueChangedHandler = null;
			_accelerationSlider.ValueChangedHandler = null;
			_jumpTypeField.ValueChangedHandler = null;
			_joysticTypeField.ValueChangedHandler = null;
			_sightColorField.ValueChangedHandler = null;
			_sightTypeField.ValueChangedHandler = null;
			_shootButtonEnabledField.onValueChanged.RemoveListener(OnShootButtonEnabled);
		}

		protected override IEnumerator ApplyInternal()
		{
			_model.Sensitivity = _sensitivitySlider.Value;
			_model.ScopeSensitivity = _scopeSensitiviySlider.Value;
			_model.Acceleration = _accelerationSlider.Value;
			_model.JumpByDoubleTap = _jumpTypeField.Value;
			_model.JoysticType = _joysticTypeField.Value;
			_model.ShootButtonEnabled = _shootButtonEnabledField.isOn;
			_model.SightColor = _sightColorField.Value;
			_model.SightType = _sightTypeField.Value;
			yield return ControlsSettingsManager.Instance.Save(_model);
		}

		public override void ResetDefaults()
		{
			ControlsSettings controlsSettings = ControlsSettingsManager.DefaultValue();
			_sensitivitySlider.Value = controlsSettings.Sensitivity;
			_scopeSensitiviySlider.Value = controlsSettings.ScopeSensitivity;
			_accelerationSlider.Value = controlsSettings.Acceleration;
			_jumpTypeField.Value = controlsSettings.JumpByDoubleTap;
			_joysticTypeField.Value = controlsSettings.JoysticType;
			_shootButtonEnabledField.isOn = controlsSettings.ShootButtonEnabled;
			_sightColorField.Value = controlsSettings.SightColor;
			_sightTypeField.Value = controlsSettings.SightType;
		}
	}
}
