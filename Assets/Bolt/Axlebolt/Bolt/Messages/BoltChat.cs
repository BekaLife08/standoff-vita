using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Messages
{
	public class BoltChat
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltFriend _003CFriend_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltGroup _003CGroup_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly bool _003CIsGroup_003Ek__BackingField;

		public BoltFriend Friend
		{
			[CompilerGenerated]
			get
			{
				return _003CFriend_003Ek__BackingField;
			}
		}

		public BoltGroup Group
		{
			[CompilerGenerated]
			get
			{
				return _003CGroup_003Ek__BackingField;
			}
		}

		public bool IsGroup
		{
			[CompilerGenerated]
			get
			{
				return _003CIsGroup_003Ek__BackingField;
			}
		}

		public string Message { get; internal set; }

		public long Timestamp { get; internal set; }

		public int UnreadMsgsCount { get; internal set; }

		public BoltChat(BoltFriend friend, string message, long timestamp, int unreadMsgsCount)
		{
			if (friend == null)
			{
				throw new ArgumentNullException();
			}
			_003CFriend_003Ek__BackingField = friend;
			_003CIsGroup_003Ek__BackingField = false;
			Message = message;
			Timestamp = timestamp;
			UnreadMsgsCount = unreadMsgsCount;
		}

		public BoltChat(BoltGroup group, string message, long timesTamp, int unreadMsgsCount)
		{
			if (group == null)
			{
				throw new ArgumentNullException();
			}
			_003CGroup_003Ek__BackingField = group;
			_003CIsGroup_003Ek__BackingField = true;
			Message = message;
			Timestamp = timesTamp;
			UnreadMsgsCount = unreadMsgsCount;
		}
	}
}
