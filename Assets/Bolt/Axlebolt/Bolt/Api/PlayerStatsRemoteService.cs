using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("PlayerStatsRemoteService")]
	public class PlayerStatsRemoteService : RpcService
	{
		public PlayerStatsRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("getPlayerStats")]
		public Axlebolt.Bolt.Protobuf.Stats GetPlayerStats(string playerId, CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Stats)Invoke(MethodBase.GetCurrentMethod(), new object[1] { playerId }, ct);
		}

		[Rpc("getGlobalStats")]
		public PlayerStat[] GetGlobalStats(int? historicDays, CancellationToken ct = default(CancellationToken))
		{
			return (PlayerStat[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { historicDays }, ct);
		}

		[Rpc("resetAllStats")]
		public void ResetStats(CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("getStats")]
		public Axlebolt.Bolt.Protobuf.Stats GetStats(CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Stats)Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("getNumberOfCurrentPlayers")]
		public int GetNumberOfCurrentPlayers(CancellationToken ct = default(CancellationToken))
		{
			return (int)Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("storeStats")]
		public void StoreStats(StorePlayerStat[] storeStats, StoreAchievement[] storeAchievements, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { storeStats, storeAchievements }, ct);
		}
	}
}
