using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class PlayerRevokedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CInviteSender_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CRevokedPlayer_003Ek__BackingField;

		public BoltFriend InviteSender
		{
			[CompilerGenerated]
			get
			{
				return _003CInviteSender_003Ek__BackingField;
			}
		}

		public BoltFriend RevokedPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CRevokedPlayer_003Ek__BackingField;
			}
		}

		public PlayerRevokedEventArgs(BoltFriend inviteSender, BoltFriend revokedPlayer)
		{
			_003CInviteSender_003Ek__BackingField = inviteSender;
			_003CRevokedPlayer_003Ek__BackingField = revokedPlayer;
		}
	}
}
