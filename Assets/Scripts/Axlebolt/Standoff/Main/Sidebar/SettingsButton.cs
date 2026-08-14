using Axlebolt.Standoff.UI;
using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Main.Sidebar
{
	public class SettingsButton : View, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeField]
		private PopupMenu _popupMenu;

		public Action VideoSettingsHandler
		{
			get;
			set;
		}

		public Action ControlSettingsHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			PopupItem videoSettings = new PopupItem(ScriptLocalization.VideoSettings.Title);
			PopupItem controlSettings = new PopupItem(ScriptLocalization.ControlSettings.Title);
			_popupMenu.AddItem(videoSettings);
			_popupMenu.AddItem(controlSettings);
			_popupMenu.ActionHandler = delegate(PopupItem item)
			{
				if (item == videoSettings)
				{
					VideoSettingsHandler?.Invoke();
				}
				if (item == controlSettings)
				{
					ControlSettingsHandler?.Invoke();
				}
			};
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_popupMenu.Show(eventData.position);
		}
	}
}
