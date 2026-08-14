using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class NewPlayerJoinedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CJoinedPlayer_003Ek__BackingField;

		public BoltFriend JoinedPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CJoinedPlayer_003Ek__BackingField;
			}
		}

		public NewPlayerJoinedEventArgs(BoltFriend joinedPlayer)
		{
			_003CJoinedPlayer_003Ek__BackingField = joinedPlayer;
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
			return obj.GetType() == GetType() && JoinedPlayer.Equals(obj);
		}

		public override int GetHashCode()
		{
			return ((JoinedPlayer != null) ? JoinedPlayer.GetHashCode() : 0) * 397;
		}
	}
}
