using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Utils;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class KickLobbyMemberAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.KickLobbyMember;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.KickLobbyMember;
			}
		}

		public Task Execute(BoltFriend friend)
		{
			return AsyncUtility.AsyncComplete(BoltService<BoltMatchmakingService>.Instance.KickFromLobby(friend.Id));
		}

		public bool IsSupported(BoltFriend boltFriend)
		{
			BoltMatchmakingService instance = BoltService<BoltMatchmakingService>.Instance;
			return instance.IsInLobby() && instance.IsLobbyOwner() && instance.CurrentLobby.IsLobbyMember(boltFriend) && !boltFriend.IsLocal();
		}
	}
}
