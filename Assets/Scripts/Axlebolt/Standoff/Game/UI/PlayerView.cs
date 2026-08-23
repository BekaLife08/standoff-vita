using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class PlayerView : View
	{
		[SerializeField]
		private Text _uidText;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private Image _badgeImage;

		[SerializeField]
		private Image _borderImage;

		[SerializeField]
		private Color _trColor;

		[SerializeField]
		private Color _ctColor;

		public void Show(PhotonPlayer player)
		{
			_nameText.text = player.NickName;
			_uidText.text = "ID " + player.GetUid();
			_iconImage.sprite = AvatarSupport.GetAvatar(player);
			BadgeUtility.SetBadge(_badgeImage, player);
			Text nameText = _nameText;
			Color color = (player.GetTeam() != Team.Ct) ? _trColor : _ctColor;
			_borderImage.color = color;
			nameText.color = color;
			Show();
		}
	}
}
