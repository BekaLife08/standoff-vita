using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsHighlightView : View
	{
		[SerializeField]
		private Text _text;

		public string Text
		{
			set
			{
				_text.text = value;
			}
		}
	}
}
