using System;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Stats;
using Axlebolt.Standoff.Bolt;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class PlayerGeneralStatsView : View
	{
		[SerializeField]
		private Text _overallPlayTime;

		[SerializeField]
		private Text _matchesPlayed;

		[SerializeField]
		private Text _accuracy;

		[SerializeField]
		private Text _headshotsPercentage;

		[SerializeField]
		private Image _deathsDiagram;

		[SerializeField]
		private Image _killsDiagram;

		[SerializeField]
		private Text _kdr;

		[SerializeField]
		private Text _kills;

		[SerializeField]
		private Text _deaths;

		[SerializeField]
		private Text _assists;

		[SerializeField]
		private Text _shots;

		[SerializeField]
		private Text _hits;

		[SerializeField]
		private Text _headshots;

		public void SetPlayer(BoltPlayer player, BoltPlayerStats stats)
		{
			_overallPlayTime.text = FormatPlayTime(player.TimeInGame);
			_matchesPlayed.text = stats.GetGamesPlayed().ToString();
			_accuracy.text = stats.GetAccuracyPercentage().ToString("0.0");
			_headshotsPercentage.text = stats.GetHeadshotsPercentage().ToString("0.0");
			_kdr.text = stats.GetKd().ToString("0.00");
			float num = stats.GetKills() + stats.GetDeaths();
			_killsDiagram.fillAmount = ((!(num > 0f)) ? 0f : ((float)stats.GetKills() / num));
			_deathsDiagram.fillAmount = ((!(num > 0f)) ? 0f : ((float)stats.GetDeaths() / num));
			_kills.text = stats.GetKills().ToString();
			_deaths.text = stats.GetDeaths().ToString();
			_assists.text = stats.GetAssists().ToString();
			_shots.text = stats.GetShots().ToString();
			_hits.text = stats.GetHits().ToString();
			_headshots.text = stats.GetHeadshots().ToString();
		}

		private string FormatPlayTime(int timeInSeconds)
		{
			double totalHours = TimeSpan.FromSeconds(timeInSeconds).TotalHours;
			int hours = TimeSpan.FromSeconds(timeInSeconds).Hours;
			return (!(totalHours >= 100.0)) ? totalHours.ToString("0.0") : hours.ToString();
		}
	}
}
