using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class NewPlayerInvitedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CInviteSender_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CInvitedPlayer_003Ek__BackingField;

		public BoltFriend InviteSender
		{
			[CompilerGenerated]
			get
			{
				return _003CInviteSender_003Ek__BackingField;
			}
		}

		public BoltFriend InvitedPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CInvitedPlayer_003Ek__BackingField;
			}
		}

		public NewPlayerInvitedEventArgs(BoltFriend inviteSender, BoltFriend invitedPlayer)
		{
			_003CInviteSender_003Ek__BackingField = inviteSender;
			_003CInvitedPlayer_003Ek__BackingField = invitedPlayer;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			return obj.GetType() == GetType() && InvitedPlayer.Equals(obj);
		}

		public override int GetHashCode()
		{
			return ((InvitedPlayer != null) ? InvitedPlayer.GetHashCode() : 0) * 397;
		}
	}
}
