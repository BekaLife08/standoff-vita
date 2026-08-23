using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsButton : View, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		public Action Down;

		public Action Up;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (Down != null)
			{
				Down();
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (Up != null)
			{
				Up();
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Tab) && Down != null)
			{
				Down();
			}
			if (UnityEngine.Input.GetKeyUp(KeyCode.Tab) && Up != null)
			{
				Up();
			}
		}
	}
}
