using Axlebolt.Standoff.UI;
using I2.Loc;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class AlertView : View
	{
		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private Text _titleText;

		[SerializeField]
		private Text _messageText;

		[SerializeField]
		private Sprite _attentionSprite;

		[SerializeField]
		private float _time;

		private void Awake()
		{
			_titleText.text = ScriptLocalization.Common.Attention.ToUpper();
		}

		public void ShowAttention(string message)
		{
			base.gameObject.SetActive(value: true);
			StopAllCoroutines();
			_messageText.text = message;
			_iconImage.sprite = _attentionSprite;
			StartCoroutine(HideCoroutine());
		}

		private IEnumerator HideCoroutine()
		{
			yield return new WaitForSeconds(_time);
			Hide();
		}
	}
}
