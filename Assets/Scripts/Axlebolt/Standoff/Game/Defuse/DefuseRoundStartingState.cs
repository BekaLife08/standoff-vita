using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Game.UI.GameStats;
using Axlebolt.Standoff.Game.UI.Winners;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Inventory.Drop;
using ExitGames.Client.Photon;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class DefuseRoundStartingState : TimeBasedGameState
	{
		private DefuseController _gameController;

		private TeamGameStatsView _teamGameStatsView;

		private StatisticsView _statisticsView;

		private WinnersView _winnersView;

		private bool _criticalBomberSet;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 22;
			}
		}

		public DefuseRoundStartingState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = (DefuseController)gameController;
			_teamGameStatsView = gameController.GameView.GetView<TeamGameStatsView>();
			_statisticsView = gameController.GameView.GetView<StatisticsView>();
			_winnersView = gameController.GameView.GetView<WinnersView>();
		}

		public override Hashtable Init()
		{
			Hashtable hashtable = base.Init();
			hashtable.SetRound(PhotonNetwork.room.GetRound() + 1);
			hashtable.SetRoundStartTime(PhotonNetwork.time);
			ScenePhotonBehavior<BombManager>.Instance.Clear();
			ScenePhotonBehavior<WeaponDropManager>.Instance.Clear();
			PhotonPlayer photonPlayer = (from player in PhotonNetwork.playerList.GetByTeam(Team.Tr)
				where !player.IsInactive
				select player).Random();
			if (photonPlayer == null)
			{
				return hashtable;
			}
			hashtable.SetInitBomberId(photonPlayer.ID);
			return hashtable;
		}

		public override void Enter()
		{
			Singleton<SurfaceImpactsEmitter>.Instance.ClearDecals();
			_criticalBomberSet = false;
			PhotonNetwork.player.ResetRoundAssists();
			PhotonNetwork.player.ResetRoundKills();
			_teamGameStatsView.SetTimeVisible(visible: true);
			_teamGameStatsView.SetCriticalTime(isCriticalTime: true);
			_winnersView.Hide();
			_gameController.RoundStarting();
			StatisticsView statisticsView = _statisticsView;
			int score = PhotonNetwork.room.GetScore(Team.Ct);
			_teamGameStatsView.CtScore = score;
			statisticsView.CtScore = score;
			StatisticsView statisticsView2 = _statisticsView;
			score = PhotonNetwork.room.GetScore(Team.Tr);
			_teamGameStatsView.TrScore = score;
			statisticsView2.TrScore = score;
			PlayerControls.Instance.RequestPartialDisable(this);
		}

		public override void Update()
		{
			_teamGameStatsView.Time = base.TimeLeft;
			if (base.CheckExit())
			{
				CriticalBomberInit();
			}
		}

		private void CriticalBomberInit()
		{
			if (!_criticalBomberSet && !ScenePhotonBehavior<BombManager>.Instance.IsBomberInitialized() && PhotonNetwork.player.GetTeam() == Team.Tr && !(_gameController.PlayerController == null))
			{
				ScenePhotonBehavior<BombManager>.Instance.SetInitBomberIsMe();
				_criticalBomberSet = true;
			}
		}

		public override void Exit()
		{
			PlayerControls.Instance.RequestPartialEnable(this);
		}

		public override void Complete()
		{
		}

		public override bool CheckExit()
		{
			bool flag = base.CheckExit();
			int ctPlayersCount = PhotonNetwork.playerList.GetCtPlayersCount();
			int trPlayersCount = PhotonNetwork.playerList.GetTrPlayersCount();
			return flag && ctPlayersCount > 0 && trPlayersCount > 0 && ScenePhotonBehavior<BombManager>.Instance.IsBomberInitialized();
		}

		public override bool CanSpawnPlayer()
		{
			return true;
		}
	}
}
