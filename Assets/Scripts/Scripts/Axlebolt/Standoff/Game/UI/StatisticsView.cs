using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI.Statistics;
using Axlebolt.Standoff.UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI
{
	public class StatisticsView : View, IPlayerPropSensitiveView
	{
		private static readonly string[] PlayerProps = new string[8]
		{
			"team",
			"isDeath",
			"score",
			"kills",
			"death",
			"ping",
			"money",
			"mvp"
		};

		[SerializeField]
		private StatisticsHeaderView _header;

		[SerializeField]
		private StatisticsScoreView _ctScore;

		[SerializeField]
		private Color _ctTeamWinColor;

		[SerializeField]
		private Color _ctTeamColor;

		[SerializeField]
		private TextView _ctWinView;

		[SerializeField]
		private StatisticsPlayersView _ctPlayerStatistics;

		[SerializeField]
		private StatisticsScoreView _trScore;

		[SerializeField]
		private StatisticsPlayersView _trPlayerStatistics;

		[SerializeField]
		private Color _trTeamWinColor;

		[SerializeField]
		private Color _trTeamColor;

		[SerializeField]
		private TextView _trWinView;

		private bool _gameRunning;

		public Func<PhotonPlayer, Sprite> StateProvider
		{
			set
			{
				_trPlayerStatistics.StateProvider = value;
				_ctPlayerStatistics.StateProvider = value;
			}
		}

		public string GameDescription
		{
			set
			{
				_header.Description = value;
			}
		}

		public bool ScoreEnabled
		{
			set
			{
				StatisticsScoreView ctScore = _ctScore;
				bool active = value;
				_trScore.Active = active;
				ctScore.Active = active;
			}
		}

		public int CtScore
		{
			set
			{
				_ctScore.Score = value.ToString();
			}
		}

		public int TrScore
		{
			set
			{
				_trScore.Score = value.ToString();
			}
		}

		public string CtHighlightText
		{
			set
			{
				_ctWinView.Text = value;
				_ctWinView.Show();
				_ctPlayerStatistics.SetTeamColor(_ctTeamWinColor);
			}
		}

		public string TrHighlighText
		{
			set
			{
				_trWinView.Text = value;
				_trWinView.Show();
				_trPlayerStatistics.SetTeamColor(_trTeamWinColor);
			}
		}

		public double TimeLeft
		{
			set
			{
				_header.Time = value;
			}
		}

		public bool MoneyEnabled
		{
			get;
			set;
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
			_ctWinView.Hide();
			_ctPlayerStatistics.SetTeamColor(_ctTeamColor);
			_trWinView.Hide();
			_trPlayerStatistics.SetTeamColor(_trTeamColor);
		}

		public void Init(string gameMode, string levelName)
		{
			_gameRunning = true;
			_header.GameMode = gameMode;
			_header.Level = levelName;
		}

		public void GameFinished(PhotonPlayer[] finalPlayers)
		{
			_gameRunning = false;
			Refresh(finalPlayers.OrderByScore());
		}

		public override void Show()
		{
			if (!base.IsVisible)
			{
				Refresh();
				base.Show();
			}
		}

		public void Refresh()
		{
			if (_gameRunning)
			{
				Refresh(PhotonNetwork.playerList.OrderByScore());
			}
		}

		public void Refresh(PhotonPlayer[] orderedPlayers)
		{
			PhotonPlayer[] byTeam = orderedPlayers.GetByTeam(Team.Ct);
			PhotonPlayer[] byTeam2 = orderedPlayers.GetByTeam(Team.Tr);
			_ctPlayerStatistics.Refresh(byTeam, MoneyEnabled);
			_trPlayerStatistics.Refresh(byTeam2, MoneyEnabled);
		}
	}
}
