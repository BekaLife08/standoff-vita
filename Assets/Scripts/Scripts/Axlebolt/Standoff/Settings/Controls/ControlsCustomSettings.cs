using System;

namespace Axlebolt.Standoff.Settings.Controls
{
	[Serializable]
	public class ControlsCustomSettings
	{
		[Serializable]
		public class Element
		{
			public float PosX;

			public float PosY;

			public float SizeX;

			public float Alpha;
		}

		[Serializable]
		public class ButtonElement
		{
			public string Name;

			public Element Element;
		}

		public float FireZone;

		public Element Joystick;

		public Element AmmoView;

		public Element WeaponInfoView;

		public ButtonElement[] Buttons = new ButtonElement[0];
	}
}
