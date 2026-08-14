using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Game
{
	public class GamePlayer
	{
		private readonly HashSet<HitLogs> _hits;

		public PhotonPlayer PhotonPlayer
		{
			get;
		}

		public GamePlayer(PhotonPlayer photonPlayer)
		{
			PhotonPlayer = photonPlayer;
			_hits = new HashSet<HitLogs>();
		}

		public void ClearHits()
		{
			_hits.Clear();
		}

		public void AddHit(PhotonPlayer shooter, int damage)
		{
			GetHits(shooter).Damages.Add(damage);
		}

		[NotNull]
		public HitLogs GetHits(PhotonPlayer filter)
		{
			HitLogs hitLogs = (from hit in _hits
				where hit.PlayerId == filter.ID
				select hit).ElementAtOrDefault(0);
			if (hitLogs == null)
			{
				hitLogs = new HitLogs(filter);
				_hits.Add(hitLogs);
			}
			return hitLogs;
		}

		[CanBeNull]
		public HitLogs GetAssistHits(PhotonPlayer killer, int minTotalDamage)
		{
			HitLogs hitLogs = (from hit in _hits
				where hit.PlayerId != killer.ID
				orderby hit.Damages.Sum() descending
				select hit).ElementAtOrDefault(0);
			if (hitLogs != null && hitLogs.Damages.Sum() < minTotalDamage)
			{
				hitLogs = null;
			}
			return hitLogs;
		}
	}
}
