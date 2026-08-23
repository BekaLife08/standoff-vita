using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Main.Messages;
using Axlebolt.Standoff.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class SendFriendMessageAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.SendMessage;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.SendMessage;
			}
		}

		public Task Execute(BoltFriend friend)
		{
			TabController<MessagesController>.Instance.OpenWith(friend);
			return Task.FromResult(0);
		}

		public bool IsSupported(BoltFriend friend)
		{
			return !friend.IsLocal();
		}
	}
}
