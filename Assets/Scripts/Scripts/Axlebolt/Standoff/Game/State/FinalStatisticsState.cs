using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI;
using I2.Loc;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.State
{
	public class FinalStatisticsState : TimeBasedGameState
	{
		private GameController _gameController;

		private StatisticsView _statisticsView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 201;
			}
		}

		public FinalStatisticsState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_statisticsView = _gameController.GameView.GetView<StatisticsView>();
		}

		public override void Enter()
		{
			PlayerControls.Instance.RequestPartialEnable(this);
			PhotonPlayer[] finalPlayers = PhotonNetwork.room.GetFinalPlayers();
			FinalWinTeam finalWinTeam = PhotonNetwork.room.GetFinalWinTeam();
			_statisticsView.GameFinished(finalPlayers);
			_statisticsView.CtScore = PhotonNetwork.room.GetScore(Team.Ct);
			_statisticsView.TrScore = PhotonNetwork.room.GetScore(Team.Tr);
			if (finalWinTeam.IsDraw)
			{
				_statisticsView.CtHighlightText = ScriptLocalization.Statistics.Draw;
				_statisticsView.TrHighlighText = ScriptLocalization.Statistics.Draw;
			}
			else if (finalWinTeam.Team == Team.Ct)
			{
				_statisticsView.CtHighlightText = ScriptLocalization.Statistics.Winner;
			}
			else
			{
				_statisticsView.TrHighlighText = ScriptLocalization.Statistics.Winner;
			}
			_statisticsView.Show();
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
			PlayerControls.Instance.RequestPartialEnable(this);
			GameManager.Instance.GameFinished();
		}

		public override void Complete()
		{
		}

		public override bool CanSpawnPlayer()
		{
			return false;
		}
	}
}
