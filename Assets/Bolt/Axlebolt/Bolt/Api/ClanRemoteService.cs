using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("ClanRemoteService")]
	public class ClanRemoteService : RpcService
	{
		public ClanRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("createClan")]
		public void CreateClan(string clanName, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { clanName }, ct);
		}

		[Rpc("assignRoleToMember")]
		public void AssignRoleToMember(string memberId, ClanMemberRole newRole, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { memberId, newRole }, ct);
		}

		[Rpc("inviteToClan")]
		public void InviteToClan(string playerId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { playerId }, ct);
		}

		[Rpc("findClan")]
		public Clan[] FindClan(string clanName, CancellationToken ct = default(CancellationToken))
		{
			return (Clan[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { clanName }, ct);
		}

		[Rpc("requestToJoinClan")]
		public void RequestToJoinClan(string clanId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { clanId }, ct);
		}

		[Rpc("getClan")]
		public Clan GetClan(string clanId, CancellationToken ct = default(CancellationToken))
		{
			return (Clan)Invoke(MethodBase.GetCurrentMethod(), new object[1] { clanId }, ct);
		}

		[Rpc("sendMsgToClan")]
		public void SendMsgToClan(string message, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { message }, ct);
		}
	}
}
