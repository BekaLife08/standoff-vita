using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyChatMessageEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltLobbyMessage _003CLobbyMessage_003Ek__BackingField;

		public BoltLobbyMessage LobbyMessage
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyMessage_003Ek__BackingField;
			}
		}

		public LobbyChatMessageEventArgs(BoltLobbyMessage lobbyMessage)
		{
			_003CLobbyMessage_003Ek__BackingField = lobbyMessage;
		}
	}
}
