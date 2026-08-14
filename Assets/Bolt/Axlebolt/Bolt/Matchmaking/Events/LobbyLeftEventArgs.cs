using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyLeftEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltLobby _003CLobbyLeft_003Ek__BackingField;

		public BoltLobby LobbyLeft
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyLeft_003Ek__BackingField;
			}
		}

		public LobbyLeftEventArgs(BoltLobby lobbyLeft)
		{
			_003CLobbyLeft_003Ek__BackingField = lobbyLeft;
		}
	}
}
