using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[EventListener("MessagesRemoteEventListener")]
	public interface IMessagesRemoteEventListener
	{
		[Event("onMsgFromFriend")]
		void OnMsgFromFriend(UserMessage userMessage);
	}
}
