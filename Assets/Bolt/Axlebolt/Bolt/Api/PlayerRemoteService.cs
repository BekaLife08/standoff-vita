using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("PlayerRemoteService")]
	public class PlayerRemoteService : RpcService
	{
		public PlayerRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("setOnlineStatus")]
		public void SetOnlineStatus(CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("setAwayStatus")]
		public void SetAwayStatus(CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("setPlayerName")]
		public void SetPlayerName(string newName, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { newName }, ct);
		}

		[Rpc("setPlayerFirebaseToken")]
		public void SetPlayerFirebaseToken(string token, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { token }, ct);
		}

		[Rpc("setPlayerAvatar")]
		public string SetPlayerAvatar(byte[] avatar, CancellationToken ct = default(CancellationToken))
		{
			return (string)Invoke(MethodBase.GetCurrentMethod(), new object[1] { avatar }, ct);
		}

		[Rpc("getPlayer")]
		public Axlebolt.Bolt.Protobuf.Player GetCurrentPlayer(CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Player)Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}
	}
}
