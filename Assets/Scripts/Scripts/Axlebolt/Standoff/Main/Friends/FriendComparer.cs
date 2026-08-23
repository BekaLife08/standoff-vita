using System;
using System.Collections.Generic;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;

namespace Axlebolt.Standoff.Main.Friends
{
	public class FriendComparer : IComparer<BoltFriend>
	{
		public int Compare(BoltFriend friend1, BoltFriend friend2)
		{
			if (friend1 == null && friend2 == null)
			{
				return 0;
			}
			if (friend1 == null)
			{
				return -1;
			}
			if (friend2 == null)
			{
				return 1;
			}
			if (friend1.OnlineStatus == friend2.OnlineStatus)
			{
				return string.Compare(friend1.Name, friend2.Name, StringComparison.Ordinal);
			}
			if (friend1.OnlineStatus == OnlineStatus.StateOnline)
			{
				return 1;
			}
			if (friend2.OnlineStatus == OnlineStatus.StateOnline)
			{
				return -1;
			}
			if (friend1.OnlineStatus == OnlineStatus.StateAway)
			{
				return 1;
			}
			return -1;
		}
	}
}
