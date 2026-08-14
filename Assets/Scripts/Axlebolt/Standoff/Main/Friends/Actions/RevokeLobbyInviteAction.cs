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
	public class RevokeLobbyInviteAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.RevokeLobbyInvite;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.RevokeLobbyInvite;
			}
		}

		public Task Execute(BoltFriend friend)
		{
			return AsyncUtility.AsyncComplete(BoltService<BoltMatchmakingService>.Instance.RevokeInvitationToLobby(friend.Id));
		}

		public bool IsSupported(BoltFriend boltFriend)
		{
			BoltMatchmakingService instance = BoltService<BoltMatchmakingService>.Instance;
			return instance.IsInLobby() && instance.IsLobbyOwner() && instance.CurrentLobby.IsMemberInvited(boltFriend) && !boltFriend.IsLocal();
		}
	}
}
