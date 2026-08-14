using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyMaxMembersChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int _003CMaxMembers_003Ek__BackingField;

		public int MaxMembers
		{
			[CompilerGenerated]
			get
			{
				return _003CMaxMembers_003Ek__BackingField;
			}
		}

		public LobbyMaxMembersChangedEventArgs(int maxMembers)
		{
			_003CMaxMembers_003Ek__BackingField = maxMembers;
		}
	}
}
