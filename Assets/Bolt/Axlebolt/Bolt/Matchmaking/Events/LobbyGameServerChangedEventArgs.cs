using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyGameServerChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltGameServer _003CGameServer_003Ek__BackingField;

		public BoltGameServer GameServer
		{
			[CompilerGenerated]
			get
			{
				return _003CGameServer_003Ek__BackingField;
			}
		}

		public LobbyGameServerChangedEventArgs(BoltGameServer gameServer)
		{
			_003CGameServer_003Ek__BackingField = gameServer;
		}
	}
}
