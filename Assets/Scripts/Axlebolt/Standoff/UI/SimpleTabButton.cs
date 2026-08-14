using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(Button))]
	public class SimpleTabButton : TabButton
	{
		[SerializeField]
		private Image _image;

		[SerializeField]
		private Text _text;

		[SerializeField]
		private Color _selectedColor;

		[SerializeField]
		private Color _unselectedColor;

		[SerializeField]
		private Text _notificationCountText;

		public override bool Selected
		{
			set
			{
				if (_image != null)
				{
					_image.enabled = !value;
				}
				if (_text != null)
				{
					_text.color = ((!value) ? _unselectedColor : _selectedColor);
				}
			}
		}

		public override int NotificationCount
		{
			set
			{
				if (!(_notificationCountText == null))
				{
					_notificationCountText.transform.parent.gameObject.SetActive(value > 0);
					_notificationCountText.text = value.ToString();
				}
			}
		}

		public override Button Button
		{
			[CompilerGenerated]
			get
			{
				return base.gameObject.GetRequireComponent<Button>();
			}
		}
	}
}
