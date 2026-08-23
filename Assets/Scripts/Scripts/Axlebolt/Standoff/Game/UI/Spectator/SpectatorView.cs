using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Spectator
{
	public class SpectatorView : View
	{
		[SerializeField]
		private Button _previousButton;

		[SerializeField]
		private Button _nextButton;

		[SerializeField]
		private PlayerView _playerView;

		public Action NextHandler
		{
			get;
			set;
		}

		public Action PreviousHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			_previousButton.onClick.AddListener(delegate
			{
				PreviousHandler?.Invoke();
			});
			_nextButton.onClick.AddListener(delegate
			{
				NextHandler?.Invoke();
			});
		}

		public void Show(PhotonPlayer player)
		{
			_playerView.Show(player);
			Show();
		}
	}
}
