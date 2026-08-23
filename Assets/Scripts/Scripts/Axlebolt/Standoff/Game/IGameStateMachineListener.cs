using Axlebolt.Standoff.Game.State;

namespace Axlebolt.Standoff.Game
{
	public interface IGameStateMachineListener
	{
		void OnGameInit();

		void OnGameFinishedState();

		void OnGameStateChanged(IGameState state);
	}
}
