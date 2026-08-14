using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class PlayerRefusedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CInviteSenderPlayer_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CRefusedPlayer_003Ek__BackingField;

		public BoltFriend InviteSenderPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CInviteSenderPlayer_003Ek__BackingField;
			}
		}

		public BoltFriend RefusedPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CRefusedPlayer_003Ek__BackingField;
			}
		}

		public PlayerRefusedEventArgs(BoltFriend inviteSenderPlayer, BoltFriend refusedPlayer)
		{
			_003CInviteSenderPlayer_003Ek__BackingField = inviteSenderPlayer;
			_003CRefusedPlayer_003Ek__BackingField = refusedPlayer;
		}
	}
}
