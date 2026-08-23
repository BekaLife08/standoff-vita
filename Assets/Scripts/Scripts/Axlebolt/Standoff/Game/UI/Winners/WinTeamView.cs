using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Winners
{
	public class WinTeamView : View
	{
		[SerializeField]
		private Text _titleText;

		[SerializeField]
		private Image _mvpAvatarBorderImage;

		[SerializeField]
		private Image _mvpAvatar;

		[SerializeField]
		private Image _mvpBadgeImage;

		[SerializeField]
		private Text _mvpText;

		[SerializeField]
		private Image _mvpIcon;

		[SerializeField]
		private Image _teamEmblemImage;

		[SerializeField]
		private Color _ctBorderColor;

		[SerializeField]
		private Color _trBorderColor;

		[SerializeField]
		private Sprite _trEmblem;

		[SerializeField]
		private Sprite _ctEmblem;

		public void Show(Team winTeam, PhotonPlayer mvpPlayer, string mvpMessage)
		{
			base.Show();
			_titleText.text = ((winTeam != Team.Ct) ? ScriptLocalization.Winners.TerroristsWin : ScriptLocalization.Winners.CounterTerroristsWin);
			_teamEmblemImage.sprite = ((winTeam != Team.Ct) ? _trEmblem : _ctEmblem);
			if (mvpPlayer != null)
			{
				_mvpAvatar.gameObject.SetActive(value: true);
				_mvpAvatarBorderImage.gameObject.SetActive(value: true);
				_mvpText.gameObject.SetActive(value: true);
				_mvpIcon.gameObject.SetActive(value: true);
				_mvpBadgeImage.gameObject.SetActive(value: true);
				_mvpAvatar.sprite = Singleton<AvatarSupport>.Instance.GetPlayerAvatar(mvpPlayer);
				_mvpAvatarBorderImage.color = ((mvpPlayer.GetTeam() != Team.Ct) ? _trBorderColor : _ctBorderColor);
				_mvpText.text = mvpMessage;
				BadgeUtility.SetBadge(_mvpBadgeImage, mvpPlayer);
			}
			else
			{
				_mvpAvatar.gameObject.SetActive(value: false);
				_mvpAvatarBorderImage.gameObject.SetActive(value: false);
				_mvpText.gameObject.SetActive(value: false);
				_mvpIcon.gameObject.SetActive(value: false);
				_mvpBadgeImage.gameObject.SetActive(value: false);
			}
		}
	}
}
