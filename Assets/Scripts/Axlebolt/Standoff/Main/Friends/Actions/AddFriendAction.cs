using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Utils;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class AddFriendAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.AddFriend;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.AddFriend;
			}
		}

		public Task Execute(BoltFriend boltFriend)
		{
			Task task;
			switch (boltFriend.Relationship)
			{
			case RelationshipStatus.RequestInitiator:
			case RelationshipStatus.Ignored:
				task = BoltService<BoltFriendsService>.Instance.AcceptFriendRequest(boltFriend);
				break;
			case RelationshipStatus.None:
				task = BoltService<BoltFriendsService>.Instance.SendFriendRequest(boltFriend);
				break;
			default:
				throw new NotSupportedException(string.Format("Not supported RelationshipStatus {0}", boltFriend.Relationship));
			}
			return AsyncUtility.AsyncComplete(task);
		}

		public bool IsSupported(BoltFriend boltFriend)
		{
			return !boltFriend.IsLocal() && (boltFriend.Relationship == RelationshipStatus.Ignored || boltFriend.Relationship == RelationshipStatus.RequestInitiator || boltFriend.Relationship == RelationshipStatus.None);
		}
	}
}
