using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Pause
{
	public class MessagesButton : View, IPointerDownHandler, IEventSystemHandler
	{
		[SerializeField]
		private Text _messagesCountText;

		[SerializeField]
		private Image _messagesImage;

		[SerializeField]
		private Image _defaultImage;

		private int _messagesCount = -1;

		public Action OnClick
		{
			get;
			set;
		}

		public int MessageCount
		{
			set
			{
				if (value != _messagesCount)
				{
					if (_messagesCount == 0 || value == 0)
					{
						_messagesCountText.gameObject.SetActive(value > 0);
					}
					if (value > 0)
					{
						_messagesCountText.text = value.ToString();
					}
					if (_messagesCount == 0 || value == 0)
					{
						_messagesImage.gameObject.SetActive(value > 0);
						_defaultImage.gameObject.SetActive(value == 0);
					}
					_messagesCount = value;
				}
			}
		}

		private void Awake()
		{
			MessageCount = 0;
			Hide();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			OnClick?.Invoke();
		}
	}
}
