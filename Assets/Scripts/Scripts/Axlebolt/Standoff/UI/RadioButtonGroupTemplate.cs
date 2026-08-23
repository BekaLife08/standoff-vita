using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(Button))]
	public class RadioButtonGroupTemplate : View
	{
		[SerializeField]
		private Text _text;

		[SerializeField]
		private Image _image;

		[SerializeField]
		private Color _selectedImageColor;

		[SerializeField]
		private Color _selectedTextColor;

		[SerializeField]
		private Color _unselectedImageColor;

		[SerializeField]
		private Color _unselectedTextColor;

		public string Text
		{
			set
			{
				_text.text = value;
			}
		}

		public bool Selected
		{
			set
			{
				_image.color = ((!value) ? _unselectedImageColor : _selectedImageColor);
				_text.color = ((!value) ? _unselectedTextColor : _selectedTextColor);
			}
		}

		public Action ActionHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			this.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				ActionHandler?.Invoke();
			});
		}
	}
}
