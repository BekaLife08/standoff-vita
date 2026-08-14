using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("AccountLinkRemoteService")]
	public class AccountLinkRemoteService : RpcService
	{
		public AccountLinkRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("createLinkTicket")]
		public string CreateLinkTicket(CancellationToken ct = default(CancellationToken))
		{
			return (string)Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("unlinkAccount")]
		public void UnlinkAccount(string playerId, AuthType authType, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { playerId, authType }, ct);
		}

		[Rpc("getPlayerByTicket")]
		public Axlebolt.Bolt.Protobuf.Player GetPlayerByTicket(string ticket, CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Player)Invoke(MethodBase.GetCurrentMethod(), new object[1] { ticket }, ct);
		}

		[Rpc("linkAccount")]
		public void LinkAccount(string ticket, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { ticket }, ct);
		}
	}
}
