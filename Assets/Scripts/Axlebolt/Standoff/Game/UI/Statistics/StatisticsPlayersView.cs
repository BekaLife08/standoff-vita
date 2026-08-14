using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsPlayersView : View
	{
		[SerializeField]
		private Color _normalColor;

		[SerializeField]
		private Color _currentColor;

		[SerializeField]
		private Color _deathColor;

		[SerializeField]
		private Color _normalTextColor;

		[SerializeField]
		private Color _currentTextColor;

		[SerializeField]
		private Color _deathTextColor;

		[SerializeField]
		private StatisticsPlayerView _playerViewPrefab;

		[SerializeField]
		private Image _teamImage;

		[SerializeField]
		private Sprite _deathSprite;

		[SerializeField]
		private Color _deathIconColor;

		private ViewPool<StatisticsPlayerView> _playerViewPool;

		public Func<PhotonPlayer, Sprite> StateProvider
		{
			get;
			set;
		}

		private void Awake()
		{
			_playerViewPool = new ViewPool<StatisticsPlayerView>(_playerViewPrefab, 5);
		}

		public void Refresh(PhotonPlayer[] players, bool moneyEnabled)
		{
			int num = Math.Min(players.Length, 5);
			StatisticsPlayerView[] items = _playerViewPool.GetItems(num);
			for (int i = 0; i < num; i++)
			{
				PhotonPlayer photonPlayer = players[i];
				bool flag = photonPlayer.Equals(PhotonNetwork.player);
				Color color = GetColor(flag, photonPlayer);
				bool flag2 = photonPlayer.GetTeam() == PhotonNetwork.player.GetTeam();
				Color textColor = GetTextColor(flag, photonPlayer);
				Sprite stateSprite = GetStateSprite(photonPlayer, flag2);
				Color stateColor = GetStateColor(photonPlayer, textColor);
				items[i].Refresh(photonPlayer, color, textColor, flag, flag2 && moneyEnabled, stateSprite, stateColor);
			}
		}

		private Color GetStateColor(PhotonPlayer player, Color textColor)
		{
			return (!player.IsDead()) ? textColor : _deathIconColor;
		}

		private Sprite GetStateSprite(PhotonPlayer player, bool isTeammate)
		{
			if (isTeammate)
			{
				return player.IsDead() ? _deathSprite : StateProvider?.Invoke(player);
			}
			return null;
		}

		private Color GetColor(bool isCurrentPlayer, PhotonPlayer player)
		{
			if (isCurrentPlayer)
			{
				return _currentColor;
			}
			return (!player.IsDead()) ? _normalColor : _deathColor;
		}

		private Color GetTextColor(bool isCurrentPlayer, PhotonPlayer player)
		{
			if (isCurrentPlayer)
			{
				return _currentTextColor;
			}
			return (!player.IsDead()) ? _normalTextColor : _deathTextColor;
		}

		public void SetTeamColor(Color color)
		{
			_teamImage.color = color;
		}
	}
}
