using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Game.UI;
using I2.Loc;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.State
{
	public class StartingState : TimeBasedGameState
	{
		private GameStateView _gameStateView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 21;
			}
		}

		public StartingState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameStateView = gameController.GameView.GetView<GameStateView>();
		}

		public override void Enter()
		{
			_gameStateView.Show();
			PlayerControls.Instance.RequestPartialDisable(this);
		}

		public override void Update()
		{
			_gameStateView.Text = ScriptLocalization.GameState.StartingGame + " " + StringUtils.FormatWithDots(base.TimeLeft);
		}

		public override void Exit()
		{
			PlayerControls.Instance.RequestPartialEnable(this);
			_gameStateView.Hide();
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
