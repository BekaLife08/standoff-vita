using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsScoreView : View
	{
		[SerializeField]
		private Text _scoreText;

		[SerializeField]
		private Color _activeColor;

		[SerializeField]
		private Color _inactiveColor;

		public bool Active
		{
			set
			{
				_scoreText.color = ((!value) ? _inactiveColor : _activeColor);
			}
		}

		public string Score
		{
			set
			{
				_scoreText.text = value;
			}
		}
	}
}
