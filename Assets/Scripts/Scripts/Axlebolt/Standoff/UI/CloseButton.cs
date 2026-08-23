using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.UI
{
	public class CloseButton : View, IPointerClickHandler, IEventSystemHandler
	{
		public Action CloseHandler
		{
			get;
			set;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
			{
				CloseHandler?.Invoke();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			CloseHandler?.Invoke();
		}
	}
}
