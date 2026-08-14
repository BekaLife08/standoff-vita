using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Stats
{
	public class BoltPlayerStats
	{
		private readonly int _magicInt;

		private readonly Dictionary<string, PlayerStat> _stats;

		private readonly Dictionary<string, StorePlayerStat> _changedStats;

		public int Count
		{
			get
			{
				return _stats.Count;
			}
		}

		public BoltPlayerStats(Axlebolt.Bolt.Protobuf.Stats stats)
		{
			if (stats == null)
			{
				throw new ArgumentNullException("stats");
			}
			_magicInt = new Random().Next(1, 1000);
			_stats = new Dictionary<string, PlayerStat>();
			_changedStats = new Dictionary<string, StorePlayerStat>();
			SetStats(stats);
		}

		internal void SetStats(Axlebolt.Bolt.Protobuf.Stats stats)
		{
			if (stats == null)
			{
				throw new ArgumentNullException("stats");
			}
			_stats.Clear();
			if (stats.Stat != null)
			{
				foreach (PlayerStat item in stats.Stat)
				{
					_stats[item.Name] = item;
					if (item.Type == StatDefType.Int)
					{
						item.IntValue += _magicInt;
					}
				}
			}
			foreach (KeyValuePair<string, StorePlayerStat> changedStat in _changedStats)
			{
				if (_stats[changedStat.Key].Type == StatDefType.Int)
				{
					_stats[changedStat.Key].IntValue = changedStat.Value.StoreInt;
				}
				else
				{
					_stats[changedStat.Key].FloatValue = changedStat.Value.StoreFloat;
				}
			}
		}

		public float GetFloatStat(string apiName)
		{
			return FindStat(apiName, StatDefType.Float, StatDefType.Avgrate).FloatValue;
		}

		public int GetIntStat(string apiName)
		{
			return FindStat(apiName, default(StatDefType)).IntValue - _magicInt;
		}

		public void SetFloatStat(string apiName, float value)
		{
			FindStat(apiName, StatDefType.Float).FloatValue = value;
			_changedStats[apiName] = new StorePlayerStat
			{
				Name = apiName,
				StoreFloat = value
			};
		}

		public void SetIntStat(string apiName, int value)
		{
			FindStat(apiName, default(StatDefType)).IntValue = value + _magicInt;
			_changedStats[apiName] = new StorePlayerStat
			{
				Name = apiName,
				StoreInt = value + _magicInt
			};
		}

		public void IncrementStat(string apiName)
		{
			SetIntStat(apiName, GetIntStat(apiName) + 1);
		}

		public void UpdateAvgRateStat(string apiName, float countThisSession, double sessionLength)
		{
			PlayerStat playerStat = FindStat(apiName, StatDefType.Avgrate);
			float floatValue = playerStat.FloatValue;
			float window = playerStat.Window;
			floatValue = (playerStat.FloatValue = (float)((double)floatValue - (double)floatValue * sessionLength / (double)window + (double)countThisSession * sessionLength / (double)window));
			_changedStats[apiName] = new StorePlayerStat
			{
				Name = apiName,
				StoreFloat = floatValue
			};
		}

		private PlayerStat FindStat(string apiName, params StatDefType[] types)
		{
			if (!_stats.ContainsKey(apiName))
			{
				return new PlayerStat { Name = apiName, Type = StatDefType.Int };
			}
			PlayerStat stat = _stats[apiName];
			if (types.Any((StatDefType type) => stat.Type == type))
			{
				return stat;
			}
			string[] value = types.Select((StatDefType type) => type.ToString()).ToArray();
			throw new BoltPlayerStatsException(string.Format("Stat {0} is not {1}", apiName, string.Join(",", value)));
		}

		internal bool HasChanges()
		{
			return _changedStats.Count > 0;
		}

		internal StorePlayerStat[] GetChanges()
		{
			foreach (KeyValuePair<string, StorePlayerStat> changedStat in _changedStats)
			{
				if (_stats[changedStat.Key].Type == StatDefType.Int)
				{
					changedStat.Value.StoreInt -= _magicInt;
				}
			}
			return _changedStats.Values.ToArray();
		}

		internal void ResetChanges()
		{
			_changedStats.Clear();
		}
	}
}
