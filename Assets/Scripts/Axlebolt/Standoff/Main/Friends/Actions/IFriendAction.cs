using System.Threading.Tasks;
using Axlebolt.Bolt.Friends;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public interface IFriendAction
	{
		FriendActionId Id { get; }

		string LocalizedText { get; }

		Task Execute(BoltFriend friend);

		bool IsSupported(BoltFriend friend);
	}
}
