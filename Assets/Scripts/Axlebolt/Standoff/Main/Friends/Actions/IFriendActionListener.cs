using Axlebolt.Bolt.Friends;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public interface IFriendActionListener
	{
		void OnFriendActionExecuted(FriendActionId friendAction, BoltFriend friend);
	}
}
