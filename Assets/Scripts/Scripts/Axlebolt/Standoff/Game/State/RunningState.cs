using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Game.UI.GameStats;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.State
{
	public class RunningState : TimeBasedGameState
	{
		private GameController _gameController;

		private StatisticsView _statisticsView;

		private PersonalGameStatsView _personalGameStatsView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 30;
			}
		}

		public RunningState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_statisticsView = gameController.GameView.GetView<StatisticsView>();
			_personalGameStatsView = gameController.GameView.GetView<PersonalGameStatsView>();
		}

		public override void Enter()
		{
			if (_gameController.GameType == GameType.Personal)
			{
				_personalGameStatsView.TimeVisible = true;
				_personalGameStatsView.ScoreVisible = true;
				_personalGameStatsView.Refresh();
			}
			_gameController.SpawnPlayer();
			_statisticsView.VisibleChanged += RefreshTimer;
		}

		public override void Update()
		{
			RefreshTimer(_statisticsView.IsVisible);
			if (_gameController.GameType == GameType.Personal)
			{
				_personalGameStatsView.Time = base.TimeLeft;
			}
			if (PhotonNetwork.isMasterClient)
			{
				HideRoomIf();
			}
		}

		private void HideRoomIf()
		{
			if (PhotonNetwork.room.IsVisible && base.TimeLeft < 180.0)
			{
				PhotonNetwork.room.IsVisible = false;
			}
		}

		private void RefreshTimer(bool visible)
		{
			if (visible)
			{
				_statisticsView.TimeLeft = base.TimeLeft;
			}
		}

		public override void Exit()
		{
			_statisticsView.VisibleChanged -= RefreshTimer;
			_personalGameStatsView.Hide();
		}

		public override void Complete()
		{
		}

		public override bool CanSpawnPlayer()
		{
			return true;
		}
	}
}
