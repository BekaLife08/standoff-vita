using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Stats
{
	public class BoltPlayerStatsService : BoltService<BoltPlayerStatsService>
	{
		private readonly PlayerStatsRemoteService _statsRemoteService;

		public int StatCount
		{
			get
			{
				return Stats.Count;
			}
		}

		public BoltPlayerStats Stats { get; private set; }

		public BoltPlayerStatsService()
		{
			_statsRemoteService = new PlayerStatsRemoteService(BoltApi.Instance.ClientService);
		}

		public Task<Axlebolt.Bolt.Protobuf.Stats> GetPlayerStats(string playerId)
		{
			return BoltApi.Instance.Async(() => _statsRemoteService.GetPlayerStats(playerId));
		}

		public float GetFloatStat(string apiName)
		{
			return Stats.GetFloatStat(apiName);
		}

		public int GetIntStat(string apiName)
		{
			return Stats.GetIntStat(apiName);
		}

		public void SetFloatStat(string apiName, float value)
		{
			Stats.SetFloatStat(apiName, value);
		}

		public void SetIntStat(string apiName, int value)
		{
			Stats.SetIntStat(apiName, value);
		}

		public void IncrementStat(string apiName)
		{
			Stats.IncrementStat(apiName);
		}

		public void UpdateAvgRateStat(string apiName, float countThisSession, double sessionLength)
		{
			Stats.UpdateAvgRateStat(apiName, countThisSession, sessionLength);
		}

		public bool HasChanges()
		{
			return Stats.HasChanges();
		}

		public Task StoreStats()
		{
			return BoltApi.Instance.Async(delegate
			{
				Stats.ResetChanges();
			});
		}

		public Task ResetStats()
		{
			return BoltApi.Instance.Async(delegate
			{
				_statsRemoteService.ResetStats();
			});
		}

		public Task<int> GetNumberOfCurrentPlayers()
		{
			return BoltApi.Instance.Async(() => _statsRemoteService.GetNumberOfCurrentPlayers());
		}

		internal override void Load()
		{
			Stats = new BoltPlayerStats(new Protobuf.Stats());
		}
	}
}
