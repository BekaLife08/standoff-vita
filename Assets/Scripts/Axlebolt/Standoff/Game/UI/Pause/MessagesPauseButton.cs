using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Pause
{
	public class MessagesPauseButton : View
	{
		[SerializeField]
		private Text _messagesCountText;

		[SerializeField]
		private Image _messagesImage;

		[SerializeField]
		private Image _pauseImage;

		private int _messagesCount = -1;

		public int MessageCount
		{
			set
			{
				if (value > 0)
				{
					if (_messagesCount <= 0)
					{
						_messagesCountText.gameObject.SetActive(value: true);
						_messagesImage.gameObject.SetActive(value: true);
						_pauseImage.gameObject.SetActive(value: false);
					}
					if (_messagesCount != value)
					{
						_messagesCountText.text = value.ToString();
					}
				}
				else if (_messagesCount != value)
				{
					_messagesCountText.gameObject.SetActive(value: false);
					_messagesImage.gameObject.SetActive(value: false);
					_pauseImage.gameObject.SetActive(value: true);
				}
				_messagesCount = value;
			}
		}

		private void Awake()
		{
			MessageCount = 0;
		}
	}
}
