using Axlebolt.Standoff.UI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Sidebar
{
	[RequireComponent(typeof(Button))]
	public class SidebarButton : TabButton
	{
		[SerializeField]
		private Image _glowImage;

		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private Sprite _sprite;

		[SerializeField]
		private Sprite _selectedSprite;

		[SerializeField]
		private Color _notificationIconColor;

		[SerializeField]
		private Text _notificationText;

		private Color _defaultColor;

		public override bool Selected
		{
			set
			{
				_iconImage.sprite = ((!value) ? _sprite : _selectedSprite);
				_glowImage.color = ((!value) ? Color.clear : Color.white);
			}
		}

		public override int NotificationCount
		{
			set
			{
				if (!(_notificationText == null))
				{
					_notificationText.gameObject.SetActive(value > 0);
					_notificationText.text = value.ToString();
					_iconImage.color = ((value <= 0) ? _defaultColor : _notificationIconColor);
				}
			}
		}

		public override Button Button
		{
			[CompilerGenerated]
			get
			{
				return this.GetRequireComponent<Button>();
			}
		}

		private void Awake()
		{
			_defaultColor = _iconImage.color;
		}
	}
}
