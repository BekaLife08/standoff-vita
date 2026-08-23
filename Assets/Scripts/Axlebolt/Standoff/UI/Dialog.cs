using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(Image))]
	public class Dialog : View
	{
		[SerializeField]
		private Text _titleText;

		[SerializeField]
		private Text _contentText;

		[SerializeField]
		private Image _contentImage;

		[SerializeField]
		private Button _buttonPrefab;

		[SerializeField]
		private RectTransform _rectTransform;

		[SerializeField]
		private GameObject _progressIndicator;

		private Image _backgroundImage;

		private Action<DialogButton> _action;

		private bool _hasResult;

		private Color _backgroundColor;

		private int _buttonsCount;

		public string Title
		{
			get
			{
				return _titleText.text;
			}
			set
			{
				_titleText.text = value;
			}
		}

		public string ContentText
		{
			set
			{
				_contentText.text = value;
				_contentText.gameObject.SetActive(value: true);
				_contentImage.gameObject.SetActive(value: false);
			}
		}

		public Sprite ContentSprite
		{
			set
			{
				_contentImage.sprite = value;
				_contentImage.SetNativeSize();
				_contentText.gameObject.SetActive(value: false);
				_contentImage.gameObject.SetActive(value: true);
			}
		}

		public bool Background
		{
			set
			{
				_backgroundImage.color = ((!value) ? Color.clear : _backgroundColor);
			}
		}

		public DialogButton ActionButton
		{
			get;
			private set;
		}

		private void Awake()
		{
			_buttonPrefab.gameObject.SetActive(value: false);
			_backgroundImage = this.GetRequireComponent<Image>();
			_backgroundColor = _backgroundImage.color;
			_progressIndicator.gameObject.SetActive(value: true);
		}

		public void AddButton(DialogButton btn)
		{
			_buttonsCount++;
			Button button = UnityEngine.Object.Instantiate(_buttonPrefab, _buttonPrefab.transform.parent, worldPositionStays: false);
			button.GetComponentInChildren<Text>().text = btn.Name;
			button.onClick.AddListener(delegate
			{
				OnButtonAction(btn);
			});
			button.gameObject.SetActive(value: true);
			_progressIndicator.gameObject.SetActive(value: false);
			if (_buttonsCount > 2)
			{
				Vector2 sizeDelta = button.GetRequireComponent<RectTransform>().sizeDelta;
				float x = sizeDelta.x;
				Vector2 sizeDelta2 = _rectTransform.sizeDelta;
				_rectTransform.sizeDelta = new Vector2(sizeDelta2.x + x, sizeDelta2.y);
			}
		}

		private void OnButtonAction(DialogButton btn)
		{
			ActionButton = btn;
			_hasResult = true;
		}

		public void AddButtons(params DialogButton[] buttons)
		{
			foreach (DialogButton btn in buttons)
			{
				AddButton(btn);
			}
		}

		public void Show([NotNull] Action<DialogButton> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			_action = action;
			base.Show();
			StartCoroutine(ShowAndWait());
		}

		public IEnumerator ShowAndWait()
		{
			Show();
			while (!_hasResult)
			{
				yield return null;
			}
			if (_hasResult && _action != null)
			{
				_action(ActionButton);
			}
			yield return HideNextFrame();
		}

		private IEnumerator HideNextFrame()
		{
			yield return null;
			Hide();
		}

		public override void Hide()
		{
			base.Hide();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
