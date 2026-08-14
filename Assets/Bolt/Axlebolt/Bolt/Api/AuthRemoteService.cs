using System.Reflection;
using System.Threading;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("AuthRemoteService")]
	public class AuthRemoteService : RpcService
	{
		public AuthRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("authGPGS")]
		public string AuthGPGS(string gameId, string gameVersion, string tempToken, CancellationToken ct = default(CancellationToken))
		{
			return (string)Invoke(MethodBase.GetCurrentMethod(), new object[3] { gameId, gameVersion, tempToken }, ct);
		}

		[Rpc("authTest")]
		public string AuthTest(string gameId, string gameVersion, string token, CancellationToken ct = default(CancellationToken))
		{
			return (string)Invoke(MethodBase.GetCurrentMethod(), new object[3] { gameId, gameVersion, token }, ct);
		}

		[Rpc("authGC")]
		public string AuthGC(string gameId, string gameVersion, string token, CancellationToken ct = default(CancellationToken))
		{
			return (string)Invoke(MethodBase.GetCurrentMethod(), new object[3] { gameId, gameVersion, token }, ct);
		}

		[Rpc("authFb")]
		public string AuthFb(string gameId, string gameVersion, string token, CancellationToken ct = default(CancellationToken))
		{
			return (string)Invoke(MethodBase.GetCurrentMethod(), new object[3] { gameId, gameVersion, token }, ct);
		}
	}
}
