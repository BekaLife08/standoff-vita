using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsHeaderView : View
	{
		[SerializeField]
		private Text _timeText;

		[SerializeField]
		private Text _descriptionText;

		[SerializeField]
		private Text _gameModeText;

		[SerializeField]
		private Text _levelText;

		public double Time
		{
			set
			{
				_timeText.text = StringUtils.FormatTwoDigit(value);
			}
		}

		public string Description
		{
			set
			{
				_descriptionText.text = value;
			}
		}

		public string GameMode
		{
			set
			{
				_gameModeText.text = value;
			}
		}

		public string Level
		{
			set
			{
				_levelText.text = value;
			}
		}

		private void Awake()
		{
			_timeText.text = string.Empty;
			_descriptionText.text = string.Empty;
			_gameModeText.text = string.Empty;
			_levelText.text = string.Empty;
		}
	}
}
