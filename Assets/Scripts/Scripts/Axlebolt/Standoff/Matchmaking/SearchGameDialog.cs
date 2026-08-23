using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Matchmaking
{
	public class SearchGameDialog : SearchGameView
	{
		private Transform _canvasTransform;

		protected override void Awake()
		{
			base.Awake();
			_canvasTransform = ViewUtility.GetCanvas(base.transform).transform;
		}

		public override void Show()
		{
			base.Show();
			base.transform.SetParent(_canvasTransform, worldPositionStays: false);
			base.transform.SetAsLastSibling();
		}
	}
}
