using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class TeamGameStatsView : View, IPlayerPropSensitiveView
	{
		private static readonly string[] PlayerProps = new string[2]
		{
			"team",
			"isDeath"
		};

		[SerializeField]
		private Color _criticalTimeColor;

		[SerializeField]
		private TextView _timeText;

		[SerializeField]
		private GameObject _bombTimer;

		[SerializeField]
		private TextView _ctScoreText;

		[SerializeField]
		private TextView _trScoreText;

		[SerializeField]
		private TeamPlayersView _ctTeamPlayersView;

		[SerializeField]
		private TeamPlayersView _trTeamPlayersView;

		private Color _timeColor;

		public double Time
		{
			set
			{
				_timeText.Text = StringUtils.FormatTwoDigit(value);
			}
		}

		public int CtScore
		{
			set
			{
				_ctScoreText.Text = value.ToString();
			}
		}

		public int TrScore
		{
			set
			{
				_trScoreText.Text = value.ToString();
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
			_timeColor = _timeText.text.color;
			CtScore = 0;
			TrScore = 0;
			SetTimeVisible(visible: false);
		}

		public void SetTimeVisible(bool visible)
		{
			_timeText.IsVisible = visible;
			_bombTimer.gameObject.SetActive(value: false);
		}

		public void SetBombTimerVisible(bool visible)
		{
			_bombTimer.gameObject.SetActive(visible);
			_timeText.Hide();
		}

		public void SetCriticalTime(bool isCriticalTime)
		{
			_timeText.text.color = ((!isCriticalTime) ? _timeColor : _criticalTimeColor);
		}

		public override void Show()
		{
			base.Show();
			Refresh();
		}

		public void Refresh()
		{
			_ctTeamPlayersView.Refresh(PhotonNetwork.playerList.GetByTeam(Team.Ct));
			_trTeamPlayersView.Refresh(PhotonNetwork.playerList.GetByTeam(Team.Tr));
		}
	}
}
