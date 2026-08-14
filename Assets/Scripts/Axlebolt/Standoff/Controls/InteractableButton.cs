using System;

namespace Axlebolt.Standoff.Controls
{
	public class InteractableButton : InteractableZone
	{
		public bool IsButton
		{
			get;
			private set;
		}

		public event Action OnButtonDownEvent = delegate
		{
		};

		public event Action OnButtonStayActiveEvent = delegate
		{
		};

		public event Action OnButtonUpEvent = delegate
		{
		};

		public override void OnTouchDown(TouchData data)
		{
			IsButton = true;
			this.OnButtonDownEvent();
		}

		public override void OnTouchStayActive(TouchData data)
		{
			IsButton = true;
			this.OnButtonStayActiveEvent();
		}

		public override void OnTouchEnd(TouchData data)
		{
			IsButton = false;
			this.OnButtonUpEvent();
		}
	}
}
