using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Game.UI.Pause
{
	public class PauseButton : View, IPointerClickHandler, IEventSystemHandler
	{
		public Action OnClick
		{
			get;
			set;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
			{
				OnClick?.Invoke();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (UnityEngine.Input.touchCount <= 1)
			{
				OnClick?.Invoke();
			}
		}
	}
}
