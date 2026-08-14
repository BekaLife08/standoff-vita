using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Friends
{
	public class FriendshipRequestCountChangedArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int _003CFriendshiptRequestCount_003Ek__BackingField;

		public int FriendshiptRequestCount
		{
			[CompilerGenerated]
			get
			{
				return _003CFriendshiptRequestCount_003Ek__BackingField;
			}
		}

		public FriendshipRequestCountChangedArgs(int сount)
		{
			_003CFriendshiptRequestCount_003Ek__BackingField = сount;
		}
	}
}
