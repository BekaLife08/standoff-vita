using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public abstract class BaseDialog : View
	{
		[SerializeField]
		private CloseButton _closeButton;

		private Transform _canvasTransform;

		public CloseButton CloseButton
		{
			[CompilerGenerated]
			get
			{
				return _closeButton;
			}
		}

		protected virtual void Awake()
		{
			_canvasTransform = ViewUtility.GetCanvas(base.transform).transform;
			if (_closeButton != null)
			{
				_closeButton.CloseHandler = OnClose;
			}
		}

		protected virtual void OnClose()
		{
			Hide();
		}

		public override void Show()
		{
			base.Show();
			base.transform.SetParent(_canvasTransform, worldPositionStays: false);
			base.transform.SetAsLastSibling();
		}
	}
}
