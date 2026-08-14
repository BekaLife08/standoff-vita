using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyDataChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Dictionary<string, string> _003CLobbyData_003Ek__BackingField;

		public Dictionary<string, string> LobbyData
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyData_003Ek__BackingField;
			}
		}

		public LobbyDataChangedEventArgs(Dictionary<string, string> lobbyData)
		{
			_003CLobbyData_003Ek__BackingField = lobbyData;
		}
	}
}
