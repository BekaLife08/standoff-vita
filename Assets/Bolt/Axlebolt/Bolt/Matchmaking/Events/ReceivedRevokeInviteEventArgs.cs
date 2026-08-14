using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class ReceivedRevokeInviteEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CInviteSender_003Ek__BackingField;

		public BoltFriend InviteSender
		{
			[CompilerGenerated]
			get
			{
				return _003CInviteSender_003Ek__BackingField;
			}
		}

		public ReceivedRevokeInviteEventArgs(BoltFriend inviteSender)
		{
			_003CInviteSender_003Ek__BackingField = inviteSender;
		}
	}
}
