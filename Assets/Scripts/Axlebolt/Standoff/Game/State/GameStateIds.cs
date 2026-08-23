namespace Axlebolt.Standoff.Game.State
{
	public static class GameStateIds
	{
		public const byte MaxInitStateId = 19;

		public const byte MaxRunningStateId = 199;

		public const byte None = 0;

		public const byte WaitingPlayers = 10;

		public const byte WarmUp = 11;

		public const byte Starting = 21;

		public const byte RoundStarting = 22;

		public const byte Running = 30;

		public const byte RoundRunning = 31;

		public const byte BombPlanted = 40;

		public const byte RoundEnding = 101;

		public const byte Winners = 200;

		public const byte FinalStatistics = 201;

		public const byte NextLevelVoting = 202;

		public const byte NextLevelVotingResult = 203;

		public const byte GameFinished = byte.MaxValue;
	}
}
