using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class LobbyOwnerChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003COwner_003Ek__BackingField;

		public BoltFriend Owner
		{
			[CompilerGenerated]
			get
			{
				return _003COwner_003Ek__BackingField;
			}
		}

		public LobbyOwnerChangedEventArgs(BoltFriend owner)
		{
			_003COwner_003Ek__BackingField = owner;
		}
	}
}
