using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("MessagesRemoteService")]
	public class MessagesRemoteService : RpcService
	{
		public MessagesRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("readGroupMsgs")]
		public void ReadGroupMsgs(string groupId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { groupId }, ct);
		}

		[Rpc("sendGroupMsg")]
		public void SendGroupMsg(string groupId, string msg, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { groupId, msg }, ct);
		}

		[Rpc("deleteFriendMsgs")]
		public void DeleteFriendMsgs(string friendId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("getUnreadChatUsersCount")]
		public int GetUnreadChatUsersCount(CancellationToken ct = default(CancellationToken))
		{
			return (int)Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("getChatUsers")]
		public ChatUser[] GetChatUsers(CancellationToken ct = default(CancellationToken))
		{
			return (ChatUser[])Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("getGroupMsgs")]
		public UserMessage[] GetGroupMsgs(string groupId, int page, int pageSize, CancellationToken ct = default(CancellationToken))
		{
			return (UserMessage[])Invoke(MethodBase.GetCurrentMethod(), new object[3] { groupId, page, pageSize }, ct);
		}

		[Rpc("getFriendMsgs")]
		public UserMessage[] GetFriendMsgs(string friendId, int page, int pageSize, CancellationToken ct = default(CancellationToken))
		{
			return (UserMessage[])Invoke(MethodBase.GetCurrentMethod(), new object[3] { friendId, page, pageSize }, ct);
		}

		[Rpc("sendFriendMsg")]
		public void SendFriendMsg(string friendId, string msg, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { friendId, msg }, ct);
		}

		[Rpc("deleteGroupMsgs")]
		public void DeleteGroupMsgs(string groupId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { groupId }, ct);
		}

		[Rpc("readFriendMsgs")]
		public void ReadFriendMsgs(string friendId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}
	}
}
