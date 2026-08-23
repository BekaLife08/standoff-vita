using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class PersonalLeaderBoardView : View
	{
		private const int MaxItems = 6;

		[SerializeField]
		private Color _trColor;

		[SerializeField]
		private Color _ctColor;

		[SerializeField]
		private Color _currentPlayerColor;

		[SerializeField]
		private Sprite _deathIcon;

		[SerializeField]
		private PersonalLeaderBoardPlayerView _playerViewPrefab;

		private ViewPool<PersonalLeaderBoardPlayerView> _itemsPool;

		public void Refresh(bool scoreVisible)
		{
			PhotonPlayer[] array = PhotonNetwork.playerList.OrderByScore();
			int lenght = Math.Min(6, array.Length);
			if (_itemsPool == null)
			{
				_itemsPool = new ViewPool<PersonalLeaderBoardPlayerView>(_playerViewPrefab, 6);
			}
			PersonalLeaderBoardPlayerView[] items = _itemsPool.GetItems(lenght);
			for (int i = 0; i < items.Length; i++)
			{
				PhotonPlayer player = array[i];
				Refresh(items[i], player, scoreVisible);
			}
		}

		private void Refresh(PersonalLeaderBoardPlayerView view, PhotonPlayer player, bool scoreVisible)
		{
			Sprite icon = GetIcon(player);
			Color color = GetColor(player);
			view.Refresh(icon, color, player.GetScore(), scoreVisible);
		}

		private Sprite GetIcon(PhotonPlayer player)
		{
			if (player.IsDead())
			{
				return _deathIcon;
			}
			return AvatarSupport.GetAvatar(player);
		}

		private Color GetColor(PhotonPlayer player)
		{
			if (player.IsDead())
			{
				return Color.clear;
			}
			if (object.Equals(player, PhotonNetwork.player))
			{
				return _currentPlayerColor;
			}
			if (player.GetTeam() == Team.Ct)
			{
				return _ctColor;
			}
			return _trColor;
		}
	}
}
