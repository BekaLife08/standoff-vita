using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	[RequireComponent(typeof(Image))]
	public class KillLogItemView : View
	{
		[SerializeField]
		private Color _ctColor;

		[SerializeField]
		private Color _trColor;

		[SerializeField]
		private Sprite _currentPlayerSprite;

		[SerializeField]
		private Sprite _otherPlayerSprite;

		[SerializeField]
		[NotNull]
		private Text _killNameText;

		[NotNull]
		[SerializeField]
		private Text _assistNameText;

		[NotNull]
		[SerializeField]
		private Text _plusText;

		[NotNull]
		[SerializeField]
		private Text _deadNameText;

		[SerializeField]
		[NotNull]
		private Image _weaponImage;

		[SerializeField]
		[NotNull]
		private Image _headShotImage;

		[NotNull]
		[SerializeField]
		private Image _penetratedImage;

		private Image _background;

		public float Time
		{
			get;
			private set;
		}

		private void Awake()
		{
			_background = this.GetRequireComponent<Image>();
		}

		public void Set(PhotonPlayer killer, PhotonPlayer assist, PhotonPlayer dead, WeaponParameters weapon, bool headShot, bool penetrated)
		{
			string nickName = killer.NickName;
			bool flag = assist != null;
			_assistNameText.gameObject.SetActive(flag);
			_plusText.gameObject.SetActive(flag);
			_killNameText.text = nickName;
			SetColor(_killNameText, killer.GetTeam());
			if (flag)
			{
				_assistNameText.text = assist.NickName;
				SetColor(_assistNameText, assist.GetTeam());
			}
			_deadNameText.text = dead.NickName;
			SetColor(_deadNameText, dead.GetTeam());
			_weaponImage.sprite = weapon.Sprites.Icon;
			_headShotImage.gameObject.SetActive(headShot);
			_penetratedImage.gameObject.SetActive(penetrated);
			if (object.Equals(killer, PhotonNetwork.player) || object.Equals(assist, PhotonNetwork.player) || object.Equals(dead, PhotonNetwork.player))
			{
				_background.sprite = _currentPlayerSprite;
			}
			else
			{
				_background.sprite = _otherPlayerSprite;
			}
			Time = UnityEngine.Time.time;
		}

		private void SetColor(Graphic text, Team team)
		{
			text.color = ((team != Team.Ct) ? _trColor : _ctColor);
		}
	}
}
