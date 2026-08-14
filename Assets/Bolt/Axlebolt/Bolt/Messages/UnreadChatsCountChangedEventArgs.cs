using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Messages
{
	public class UnreadChatsCountChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int _003CUnreadChatsCount_003Ek__BackingField;

		public int UnreadChatsCount
		{
			[CompilerGenerated]
			get
			{
				return _003CUnreadChatsCount_003Ek__BackingField;
			}
		}

		public UnreadChatsCountChangedEventArgs(int count)
		{
			_003CUnreadChatsCount_003Ek__BackingField = count;
		}
	}
}
