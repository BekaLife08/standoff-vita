using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class ReceivedInviteEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltLobbyInvite _003CLobbyInvite_003Ek__BackingField;

		public BoltLobbyInvite LobbyInvite
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyInvite_003Ek__BackingField;
			}
		}

		public ReceivedInviteEventArgs(BoltLobbyInvite lobbyInvite)
		{
			_003CLobbyInvite_003Ek__BackingField = lobbyInvite;
		}
	}
}
