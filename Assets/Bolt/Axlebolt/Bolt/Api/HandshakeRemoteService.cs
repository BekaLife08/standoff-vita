using System.Reflection;
using System.Threading;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("HandshakeRemoteService")]
	public class HandshakeRemoteService : RpcService
	{
		public HandshakeRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("logout")]
		public void Logout(CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("handshake")]
		public void Handshake(string ticket, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { ticket }, ct);
		}
	}
}
