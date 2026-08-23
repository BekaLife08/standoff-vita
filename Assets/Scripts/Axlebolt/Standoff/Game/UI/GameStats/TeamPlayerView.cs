using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class TeamPlayerView : View
	{
		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private Image _borderImage;

		[SerializeField]
		private Image _healthImage;

		public void Refresh(Sprite icon, bool isDead)
		{
			_iconImage.sprite = icon;
			Image borderImage = _borderImage;
			bool enabled = !isDead;
			_healthImage.enabled = enabled;
			borderImage.enabled = enabled;
		}
	}
}
