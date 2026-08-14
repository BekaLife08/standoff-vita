using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Player;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyPhotonGameChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltPhotonGame _003CPhotonGame_003Ek__BackingField;

		public BoltPhotonGame PhotonGame
		{
			[CompilerGenerated]
			get
			{
				return _003CPhotonGame_003Ek__BackingField;
			}
		}

		public LobbyPhotonGameChangedEventArgs(BoltPhotonGame photonGame)
		{
			_003CPhotonGame_003Ek__BackingField = photonGame;
		}
	}
}
