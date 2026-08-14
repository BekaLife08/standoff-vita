using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyJoinedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltLobby _003CLobbyJoined_003Ek__BackingField;

		public BoltLobby LobbyJoined
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyJoined_003Ek__BackingField;
			}
		}

		public LobbyJoinedEventArgs(BoltLobby lobbyJoined)
		{
			_003CLobbyJoined_003Ek__BackingField = lobbyJoined;
		}
	}
}
