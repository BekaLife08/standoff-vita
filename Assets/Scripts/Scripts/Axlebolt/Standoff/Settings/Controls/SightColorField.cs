using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class SightColorField : SelectField<string>
	{
		protected override void Format(Image image, string value)
		{
			if (ColorUtility.TryParseHtmlString(value, out Color color))
			{
				image.color = color;
			}
			else
			{
				UnityEngine.Debug.LogError($"Can't parse color ${color}");
			}
		}
	}
}
