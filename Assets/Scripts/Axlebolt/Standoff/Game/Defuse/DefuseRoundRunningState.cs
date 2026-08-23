using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Game.UI.GameStats;
using Axlebolt.Standoff.Inventory.Bomb;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class DefuseRoundRunningState : TimeBasedGameState
	{
		private TeamGameStatsView _teamGameStatsView;

		private DefuseController _gameController;

		private StatisticsView _statisticsView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 31;
			}
		}

		public DefuseRoundRunningState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = (DefuseController)gameController;
			_teamGameStatsView = gameController.GameView.GetView<TeamGameStatsView>();
			_statisticsView = gameController.GameView.GetView<StatisticsView>();
		}

		public override void Enter()
		{
			PhotonNetwork.player.ResetRoundAssists();
			PhotonNetwork.player.ResetRoundKills();
			ScenePhotonBehavior<GameStateHelper>.Instance.ClearRpc();
			_teamGameStatsView.SetTimeVisible(visible: true);
			_teamGameStatsView.SetCriticalTime(isCriticalTime: false);
			_teamGameStatsView.Refresh();
		}

		public override bool CheckExit()
		{
			if (ScenePhotonBehavior<BombManager>.Instance.IsBombPlanted())
			{
				return true;
			}
			bool flag = PhotonNetwork.playerList.IsAllDead(Team.Ct);
			bool flag2 = PhotonNetwork.playerList.IsAllDead(Team.Tr);
			if (base.CheckExit() || flag || flag2)
			{
				return ScenePhotonBehavior<GameStateHelper>.Instance.ServerCallback();
			}
			return false;
		}

		public override void Update()
		{
			_teamGameStatsView.Time = base.TimeLeft;
			_statisticsView.TimeLeft = base.TimeLeft;
		}

		public override void Exit()
		{
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
