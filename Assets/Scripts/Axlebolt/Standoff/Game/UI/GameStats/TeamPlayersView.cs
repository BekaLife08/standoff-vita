using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class TeamPlayersView : View
	{
		private const int MaxItems = 5;

		[SerializeField]
		private Sprite _deathIcon;

		[SerializeField]
		private TeamPlayerView _playerViewPrefab;

		private ViewPool<TeamPlayerView> _itemsPool;

		public void Refresh(PhotonPlayer[] players)
		{
			int lenght = Math.Min(5, players.Length);
			if (_itemsPool == null)
			{
				_itemsPool = new ViewPool<TeamPlayerView>(_playerViewPrefab, 5);
			}
			TeamPlayerView[] items = _itemsPool.GetItems(lenght);
			for (int i = 0; i < items.Length; i++)
			{
				PhotonPlayer player = players[i];
				items[i].Refresh(GetIcon(player), player.IsDead());
			}
		}

		private Sprite GetIcon(PhotonPlayer player)
		{
			return (!player.IsDead()) ? AvatarSupport.GetAvatar(player) : _deathIcon;
		}
	}
}
