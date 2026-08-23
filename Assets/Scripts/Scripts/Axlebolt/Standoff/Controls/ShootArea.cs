namespace Axlebolt.Standoff.Controls
{
	public class ShootArea : InteractableZone
	{
		private bool _isShoting;

		public bool IsShooting
		{
			get
			{
				return _isShoting;
			}
			set
			{
				_isShoting = value;
			}
		}

		public override void OnTouchDown(TouchData touchData)
		{
			_isShoting = true;
			base.OnTouchDown(touchData);
		}

		public override void OnTouchStayActive(TouchData touchData)
		{
			_isShoting = true;
			base.OnTouchStayActive(touchData);
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			_isShoting = false;
			base.OnTouchEnd(touchData);
		}
	}
}
