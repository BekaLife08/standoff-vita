using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyTypeChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltLobby.LobbyType _003CLobbyType_003Ek__BackingField;

		public BoltLobby.LobbyType LobbyType
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyType_003Ek__BackingField;
			}
		}

		public LobbyTypeChangedEventArgs(BoltLobby.LobbyType lobbyType)
		{
			_003CLobbyType_003Ek__BackingField = lobbyType;
		}
	}
}
