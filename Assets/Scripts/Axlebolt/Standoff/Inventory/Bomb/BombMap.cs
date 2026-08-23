using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class BombMap : MonoBehaviour
	{
		public List<MeshRenderer> Displays;

		public MeshRenderer Indicator;

		public MeshRenderer IndicatorLighted;

		public void DisableParts()
		{
			foreach (MeshRenderer display in Displays)
			{
				display.enabled = false;
			}
			MeshRenderer indicator = Indicator;
			bool enabled = false;
			IndicatorLighted.enabled = enabled;
			indicator.enabled = enabled;
		}

		public void SetActivated()
		{
			DisableParts();
			Displays[Displays.Count - 1].enabled = true;
			Indicator.enabled = false;
			IndicatorLighted.enabled = true;
		}

		public void SetLayer(int layer)
		{
			foreach (MeshRenderer display in Displays)
			{
				display.gameObject.layer = layer;
			}
			GameObject gameObject = Indicator.gameObject;
			IndicatorLighted.gameObject.layer = layer;
			gameObject.layer = layer;
		}
	}
}
