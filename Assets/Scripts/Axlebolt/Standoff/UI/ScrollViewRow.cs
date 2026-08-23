using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.UI
{
	public class ScrollViewRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public Action<PointerEventData> ClickHandler
		{
			get;
			set;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			ClickHandler?.Invoke(eventData);
		}
	}
}
