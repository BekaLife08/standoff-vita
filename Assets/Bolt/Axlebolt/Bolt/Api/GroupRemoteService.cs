using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("GroupRemoteService")]
	public class GroupRemoteService : RpcService
	{
		public GroupRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("leaveGroup")]
		public void LeaveGroup(string groupId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { groupId }, ct);
		}

		[Rpc("joinGroup")]
		public Group JoinGroup(string groupId, CancellationToken ct = default(CancellationToken))
		{
			return (Group)Invoke(MethodBase.GetCurrentMethod(), new object[1] { groupId }, ct);
		}

		[Rpc("createGroup")]
		public Group CreateGroup(string[] friendIds, CancellationToken ct = default(CancellationToken))
		{
			return (Group)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendIds }, ct);
		}
	}
}
