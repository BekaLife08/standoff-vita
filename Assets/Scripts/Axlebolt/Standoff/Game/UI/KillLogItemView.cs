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
				_airborneImage = CreateTextIcon("AirborneIcon", "\u2191", new Color(0.4f, 0.9f, 1f, 1f));
			}
			if (_noScopeImage == null)
			{
				_noScopeImage = CreateTextIcon("NoScopeIcon", "\u25CE", new Color(1f, 0.9f, 0.2f, 1f));
			}
		}

		private Image CreateTextIcon(string iconName, string symbol, Color tintColor)
		{
			GameObject iconGO = new GameObject(iconName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
			iconGO.transform.SetParent(_penetratedImage.transform.parent, false);
			RectTransform myRt = iconGO.GetComponent<RectTransform>();
			RectTransform refRt = _penetratedImage.rectTransform;
			myRt.anchorMin = refRt.anchorMin;
			myRt.anchorMax = refRt.anchorMax;
			myRt.pivot = refRt.pivot;
			myRt.sizeDelta = refRt.sizeDelta;
			myRt.localScale = refRt.localScale;
			myRt.anchoredPosition = refRt.anchoredPosition;
			Image bg = iconGO.GetComponent<Image>();
			bg.color = new Color(0f, 0f, 0f, 0.6f);
			bg.type = Image.Type.Simple;
			bg.preserveAspect = true;
			GameObject textGO = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
			textGO.transform.SetParent(iconGO.transform, false);
			RectTransform textRt = textGO.GetComponent<RectTransform>();
			textRt.anchorMin = Vector2.zero;
			textRt.anchorMax = Vector2.one;
			textRt.sizeDelta = Vector2.zero;
			textRt.offsetMin = Vector2.zero;
			textRt.offsetMax = Vector2.zero;
			Text text = textGO.GetComponent<Text>();
			text.text = symbol;
			text.color = tintColor;
			text.fontSize = 14;
			text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text.alignment = TextAnchor.MiddleCenter;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			iconGO.SetActive(false);
			return bg;
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
