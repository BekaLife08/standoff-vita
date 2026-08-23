using Axlebolt.Standoff.Controls;
using System;

namespace Axlebolt.Standoff.Settings.Controls
{
	[Serializable]
	public class ControlsSettings
	{
		public const float SensitivityMin = 0f;

		public const float SensitivityMax = 3f;

		public const float AccelerationMin = 0f;

		public const float AccelerationMax = 3f;

		public const float InteractableButtonMin = 100f;

		public const float InteractableButtonMax = 400f;

		public const float WeaponInfoViewMin = 320f;

		public const float WeaponInfoViewMax = 700f;

		public const float JoystickMin = 200f;

		public const float JoystickMax = 500f;

		public const string SightDefaultColor = "#3b9110";

		public float Sensitivity = 1f;

		public float ScopeSensitivity = 1f;

		public float Acceleration = 1f;

		public bool JumpByDoubleTap = true;

		public JoysticType JoysticType = JoysticType.FreeTouch;

		public bool ShootButtonEnabled = true;

		public string SightColor = "#3b9110";

		public DefaultSightType SightType;

		public ControlsCustomSettings CustomSettings;

		public bool CustomSettingsNull = true;
	}
}
