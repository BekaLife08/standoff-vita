using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	[RequireComponent(typeof(Image))]
	public class StatisticsPlayerView : View
	{
		[SerializeField]
		private Image _avatarImage;

		[SerializeField]
		private Text _pingText;

		[SerializeField]
		private Image _stateImage;

		[SerializeField]
		private Image _avatarBackgroundImage;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Text _moneyText;

		[SerializeField]
		private Text _killsText;

		[SerializeField]
		private Text _assistsText;

		[SerializeField]
		private Text _deathText;

		[SerializeField]
		private Text _scoreText;

		[SerializeField]
		private Image _badgeImage;

		[SerializeField]
		private float _mvpTopMargin;

		[SerializeField]
		private View _mvpIcon;

		private ViewPool<View> _mvpIconPool;

		private RectTransform _nameRectTransform;

		private Image _image;

		private void Awake()
		{
			_image = GetComponent<Image>();
			_mvpIconPool = new ViewPool<View>(_mvpIcon, 20);
			_mvpIcon.gameObject.SetActive(value: false);
			_nameRectTransform = (RectTransform)_nameText.transform;
			RectTransform nameRectTransform = _nameRectTransform;
			Vector3 localPosition = _nameRectTransform.localPosition;
			nameRectTransform.localPosition = new Vector3(localPosition.x, 0f);
		}

		public void Refresh(PhotonPlayer player, Color color, Color textColor, bool currentPlayer, bool moneyEnabled, Sprite stateSprite, Color stateColor)
		{
			_avatarBackgroundImage.gameObject.SetActive(currentPlayer);
			_avatarImage.sprite = AvatarSupport.GetAvatar(player);
			_stateImage.enabled = (stateSprite != null);
			if (_stateImage.enabled)
			{
				_stateImage.sprite = stateSprite;
				_stateImage.color = stateColor;
			}
			_nameText.text = player.NickName;
			_nameText.color = textColor;
			_pingText.text = player.GetPing().ToString();
			_pingText.color = textColor;
			_moneyText.gameObject.SetActive(moneyEnabled);
			_moneyText.text = "$" + player.GetMoney();
			_moneyText.color = textColor;
			_killsText.text = player.GetKills().ToString();
			_killsText.color = textColor;
			_assistsText.text = player.GetAssists().ToString();
			_assistsText.color = textColor;
			_deathText.text = player.GetDeath().ToString();
			_deathText.color = textColor;
			_scoreText.text = player.GetScore().ToString();
			_scoreText.color = textColor;
			_mvpIconPool.GetItems(player.GetMvp());
			RectTransform nameRectTransform = _nameRectTransform;
			Vector3 localPosition2;
			if (player.GetMvp() == 0)
			{
				Vector3 localPosition = _nameRectTransform.localPosition;
				localPosition2 = new Vector3(localPosition.x, 0f);
			}
			else
			{
				Vector3 localPosition3 = _nameRectTransform.localPosition;
				localPosition2 = new Vector3(localPosition3.x, _mvpTopMargin);
			}
			nameRectTransform.localPosition = localPosition2;
			_image.color = color;
			BadgeUtility.SetBadge(_badgeImage, player);
		}
	}
}
