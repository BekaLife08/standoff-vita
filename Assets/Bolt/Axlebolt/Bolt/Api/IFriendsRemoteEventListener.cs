using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[EventListener("FriendsRemoteEventListener")]
	public interface IFriendsRemoteEventListener
	{
		[Event("onNewFriendshipRequest")]
		void OnNewFriendshipRequest(PlayerFriend friend);

		[Event("onFriendAvatarChanged")]
		void OnFriendAvatarChanged(string friendId, string avatarId);

		[Event("onPlayerStatusChanged")]
		void OnPlayerStatusChanged(string friendId, PlayerStatus newStatus);

		[Event("onFriendAdded")]
		void OnFriendAdded(PlayerFriend friend);

		[Event("onFriendRemoved")]
		void OnFriendRemoved(string friendId);

		[Event("onRevokeFriendshipRequest")]
		void OnRevokeFriendshipRequest(string friendId);

		[Event("onFriendNameChanged")]
		void OnFriendNameChanged(string friendId, string newName);
	}
}
