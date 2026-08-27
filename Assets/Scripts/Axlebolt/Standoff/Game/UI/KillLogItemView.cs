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

		[SerializeField]
		private Image _airborneImage;

		[SerializeField]
		private Image _noScopeImage;

		private Image _background;

		public float Time
		{
			get;
			private set;
		}

		private void Awake()
		{
			_background = this.GetRequireComponent<Image>();
			if (_airborneImage == null)
			{
				_airborneImage = CreateIcon("AirborneIcon", new Color(0.3f, 0.8f, 1f, 1f));
			}
			if (_noScopeImage == null)
			{
				_noScopeImage = CreateIcon("NoScopeIcon", new Color(1f, 0.85f, 0.2f, 1f));
			}
		}

		private Image CreateIcon(string iconName, Color tintColor)
		{
			GameObject iconGO = new GameObject(iconName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
			iconGO.transform.SetParent(_penetratedImage.transform.parent, false);
			RectTransform rt = iconGO.GetComponent<RectTransform>();
			rt.anchorMin = _penetratedImage.rectTransform.anchorMin;
			rt.anchorMax = _penetratedImage.rectTransform.anchorMax;
			rt.pivot = _penetratedImage.rectTransform.pivot;
			rt.sizeDelta = _penetratedImage.rectTransform.sizeDelta;
			rt.localScale = _penetratedImage.rectTransform.localScale;
			Image img = iconGO.GetComponent<Image>();
			img.color = tintColor;
			img.sprite = _penetratedImage.sprite;
			img.type = Image.Type.Simple;
			img.preserveAspect = true;
			iconGO.SetActive(false);
			return img;
		}

		public void Set(PhotonPlayer killer, PhotonPlayer assist, PhotonPlayer dead, WeaponParameters weapon, bool headShot, bool penetrated)
		{
			Set(killer, assist, dead, weapon, headShot, penetrated, false, false);
		}

		public void Set(PhotonPlayer killer, PhotonPlayer assist, PhotonPlayer dead, WeaponParameters weapon, bool headShot, bool penetrated, bool airborne, bool noScope)
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
			if (_airborneImage != null)
			{
				_airborneImage.gameObject.SetActive(airborne);
			}
			if (_noScopeImage != null)
			{
				_noScopeImage.gameObject.SetActive(noScope);
			}
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
