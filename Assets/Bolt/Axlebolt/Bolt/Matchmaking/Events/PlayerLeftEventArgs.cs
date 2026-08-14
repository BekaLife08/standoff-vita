using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Matchmaking.Events
{
	public class PlayerLeftEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CLeftPlayer_003Ek__BackingField;

		public BoltFriend LeftPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CLeftPlayer_003Ek__BackingField;
			}
		}

		public PlayerLeftEventArgs(BoltFriend leftPlayer)
		{
			_003CLeftPlayer_003Ek__BackingField = leftPlayer;
		}
	}
}
