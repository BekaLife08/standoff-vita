using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	[RequireComponent(typeof(RawImage))]
	public class ScreenshotView : View
	{
		public RawImage RawImage
		{
			get;
			private set;
		}

		private void Awake()
		{
			RawImage = this.GetRequireComponent<RawImage>();
		}
	}
}
