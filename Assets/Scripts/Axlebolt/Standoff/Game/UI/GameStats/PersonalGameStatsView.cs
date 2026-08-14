using Axlebolt.Standoff.UI;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class PersonalGameStatsView : View, IPlayerPropSensitiveView
	{
		private static readonly string[] PlayerProps = new string[3]
		{
			"team",
			"score",
			"isDeath"
		};

		[SerializeField]
		private TextView _timeText;

		[SerializeField]
		private PersonalLeaderBoardView _personalLeaderBoardView;

		public double Time
		{
			set
			{
				_timeText.Text = StringUtils.FormatTwoDigit(value);
			}
		}

		public bool ScoreVisible
		{
			get;
			set;
		}

		public bool TimeVisible
		{
			set
			{
				if (value)
				{
					_timeText.Show();
				}
				else
				{
					_timeText.Hide();
				}
			}
		}

		public string[] SensitivePlayerProperties
		{
			[CompilerGenerated]
			get
			{
				return PlayerProps;
			}
		}

		private void Awake()
		{
			_timeText.Text = string.Empty;
		}

		public override void Show()
		{
			base.Show();
			Refresh();
		}

		public void Refresh()
		{
			_personalLeaderBoardView.Refresh(ScoreVisible);
		}
	}
}
