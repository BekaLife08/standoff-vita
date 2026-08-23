using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.Utils;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class IgnoreFriendAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.IgnoreFriend;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.Ignore;
			}
		}

		public Task Execute(BoltFriend boltFriend)
		{
			return AsyncUtility.AsyncComplete(BoltService<BoltFriendsService>.Instance.IgnoreFriendRequest(boltFriend));
		}

		public bool IsSupported(BoltFriend boltFriend)
		{
			return boltFriend.Relationship == RelationshipStatus.RequestInitiator;
		}
	}
}
