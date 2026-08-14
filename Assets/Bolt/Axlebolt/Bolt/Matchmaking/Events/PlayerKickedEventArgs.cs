using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class PlayerKickedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CKickInitiator_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CKickedPlayer_003Ek__BackingField;

		public BoltFriend KickInitiator
		{
			[CompilerGenerated]
			get
			{
				return _003CKickInitiator_003Ek__BackingField;
			}
		}

		public BoltFriend KickedPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CKickedPlayer_003Ek__BackingField;
			}
		}

		public PlayerKickedEventArgs(BoltFriend kickInitiator, BoltFriend kickedPlayer)
		{
			_003CKickInitiator_003Ek__BackingField = kickInitiator;
			_003CKickedPlayer_003Ek__BackingField = kickedPlayer;
		}
	}
}
