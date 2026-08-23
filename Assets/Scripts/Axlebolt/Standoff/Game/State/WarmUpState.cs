using Axlebolt.Standoff.Game.UI;
using I2.Loc;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.State
{
	public class WarmUpState : TimeBasedGameState
	{
		private GameStateView _gameStateView;

		private GameController _gameController;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 11;
			}
		}

		public WarmUpState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameStateView = gameController.GameView.GetView<GameStateView>();
			_gameController = gameController;
		}

		public override void Enter()
		{
			PhotonNetwork.player.ResetKills();
			PhotonNetwork.player.ResetAssists();
			PhotonNetwork.player.ResetDeath();
			_gameStateView.Show();
		}

		public override void Update()
		{
			_gameStateView.Text = ScriptLocalization.GameState.WarmUp + " " + StringUtils.FormatTwoDigit(base.TimeLeft);
		}

		public override void Exit()
		{
			_gameStateView.Hide();
		}

		public override bool CheckExit()
		{
			bool flag = base.CheckExit();
			int num = PhotonNetwork.playerList.GetCtPlayersCount() + PhotonNetwork.playerList.GetTrPlayersCount();
			return flag || num >= _gameController.MaxPlayers;
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
