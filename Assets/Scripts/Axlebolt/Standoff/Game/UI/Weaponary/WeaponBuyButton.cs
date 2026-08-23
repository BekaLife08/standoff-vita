using Axlebolt.Standoff.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class WeaponBuyButton : View, IPointerClickHandler, IEventSystemHandler
	{
		public Action Clicked;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (Clicked != null)
			{
				Clicked();
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKey(KeyCode.B) && Clicked != null)
			{
				Clicked();
			}
		}
	}
}
