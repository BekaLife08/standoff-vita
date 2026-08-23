using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class SightTypeField : SelectField<DefaultSightType>
	{
		[SerializeField]
		private Sprite _defaultDynimic;

		[SerializeField]
		private Sprite _classicDynamic;

		[SerializeField]
		private Sprite _classicStatic;

		protected override void Format(Image image, DefaultSightType value)
		{
			switch (value)
			{
			case DefaultSightType.DefaultDynamic:
				image.sprite = _defaultDynimic;
				break;
			case DefaultSightType.ClassicDynamic:
				image.sprite = _classicDynamic;
				break;
			case DefaultSightType.ClassicStatic:
				image.sprite = _classicStatic;
				break;
			}
		}

		public void SetColor(string htmlColor)
		{
			if (ColorUtility.TryParseHtmlString(htmlColor, out Color color))
			{
				base.Image.color = color;
			}
		}
	}
}
