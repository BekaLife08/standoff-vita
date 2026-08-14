using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Utils;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class BlockFriendAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.BlockFriend;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.Block;
			}
		}

		public Task Execute(BoltFriend boltFriend)
		{
			return AsyncUtility.AsyncComplete(BoltService<BoltFriendsService>.Instance.BlockFriend(boltFriend));
		}

		public bool IsSupported(BoltFriend boltFriend)
		{
			return (!boltFriend.IsLocal() && boltFriend.Relationship == RelationshipStatus.None) || boltFriend.Relationship == RelationshipStatus.RequestRecipient || boltFriend.Relationship == RelationshipStatus.Ignored;
		}
	}
}
