using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.UI;
using I2.Loc;
using System;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class JoysticTypeField : RadioButtonGroup<JoysticType>
	{
		protected override void InitFormatter()
		{
			SetFormatter(delegate(JoysticType type)
			{
				switch (type)
				{
				case JoysticType.Fixed:
					return ScriptLocalization.ControlSettings.JoystickTypeFixed;
				case JoysticType.Floating:
					return ScriptLocalization.ControlSettings.JoystickTypeFloating;
				case JoysticType.FreeTouch:
					return ScriptLocalization.ControlSettings.JoystickTypeFreeTouch;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
				}
			});
		}
	}
}
