using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Winners
{
	public class WinPlayersView : View
	{
		[SerializeField]
		private Text _titleText;

		[SerializeField]
		private Text _firstPlaceText;

		[SerializeField]
		private Image _firstPlaceAvatarBorderImage;

		[SerializeField]
		private Image _firstPlaceAvatar;

		[SerializeField]
		private Image _firstPlaceBadge;

		[SerializeField]
		private GameObject _secondPlaceTitle;

		[SerializeField]
		private Text _secondPlaceText;

		[SerializeField]
		private Image _secondPlaceAvatar;

		[SerializeField]
		private GameObject _thirdPlaceTitle;

		[SerializeField]
		private Text _thirdPlaceText;

		[SerializeField]
		private Image _thirdPlaceAvatar;

		[SerializeField]
		private Color _ctBorderColor;

		[SerializeField]
		private Color _trBorderColor;

		public void Show([NotNull] PhotonPlayer firstPlace, PhotonPlayer secondPlace, PhotonPlayer thirdPlace)
		{
			if (firstPlace == null)
			{
				throw new ArgumentNullException("firstPlace");
			}
			base.Show();
			_titleText.text = ScriptLocalization.Winners.FirstPlace;
			_firstPlaceAvatar.sprite = AvatarSupport.GetAvatar(firstPlace);
			_firstPlaceAvatarBorderImage.color = ((firstPlace.GetTeam() != Team.Ct) ? _trBorderColor : _ctBorderColor);
			_firstPlaceAvatar.gameObject.SetActive(value: true);
			BadgeUtility.SetBadge(_firstPlaceBadge, firstPlace);
			_firstPlaceText.text = firstPlace.NickName;
			_firstPlaceText.gameObject.SetActive(value: true);
			if (secondPlace != null)
			{
				_secondPlaceTitle.SetActive(value: true);
				_secondPlaceAvatar.sprite = AvatarSupport.GetAvatar(secondPlace);
				_secondPlaceAvatar.gameObject.SetActive(value: true);
				_secondPlaceText.text = secondPlace.NickName;
				_secondPlaceText.gameObject.SetActive(value: true);
			}
			else
			{
				_secondPlaceTitle.SetActive(value: false);
				_secondPlaceText.gameObject.SetActive(value: false);
				_secondPlaceAvatar.gameObject.SetActive(value: false);
			}
			if (thirdPlace != null)
			{
				_thirdPlaceTitle.SetActive(value: true);
				_thirdPlaceAvatar.sprite = AvatarSupport.GetAvatar(thirdPlace);
				_thirdPlaceAvatar.gameObject.SetActive(value: true);
				_thirdPlaceText.text = thirdPlace.NickName;
				_thirdPlaceText.gameObject.SetActive(value: true);
			}
			else
			{
				_thirdPlaceTitle.SetActive(value: false);
				_thirdPlaceText.gameObject.SetActive(value: false);
				_thirdPlaceAvatar.gameObject.SetActive(value: false);
			}
		}
	}
}
