using Axlebolt.Standoff.UI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Sidebar
{
	[RequireComponent(typeof(Button))]
	public class SidebarMessagesButton : TabButton
	{
		[SerializeField]
		private GameObject _button;

		[SerializeField]
		private GameObject _buttonWithNotification;

		[SerializeField]
		private Text _notificationCountText;

		public override bool Selected
		{
			set
			{
			}
		}

		public override int NotificationCount
		{
			set
			{
				if (value <= 0)
				{
					_button.SetActive(value: true);
					_buttonWithNotification.SetActive(value: false);
				}
				else
				{
					_button.SetActive(value: false);
					_buttonWithNotification.SetActive(value: true);
					_notificationCountText.text = value.ToString();
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
	}
}
