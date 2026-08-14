using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyJoinableChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly bool _003CJoinable_003Ek__BackingField;

		public bool Joinable
		{
			[CompilerGenerated]
			get
			{
				return _003CJoinable_003Ek__BackingField;
			}
		}

		public LobbyJoinableChangedEventArgs(bool joinable)
		{
			_003CJoinable_003Ek__BackingField = joinable;
		}
	}
}
