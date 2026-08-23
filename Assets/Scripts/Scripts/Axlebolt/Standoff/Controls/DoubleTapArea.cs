using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class DoubleTapArea : InteractableZone
	{
		public enum Tap
		{
			FirstTapEnter,
			FirstTapExit,
			NoTap
		}

		public delegate void OnTapAction();

		[SerializeField]
		private float _nextTapWaitTime;

		[SerializeField]
		private float _firstTapTriggerDeviation;

		[SerializeField]
		private float _secondTapTriggerDeviation;

		private Tap _tapState = Tap.NoTap;

		private Vector3 _firstEnterTouch;

		private float _firstTouchEnterTime;

		public event OnTapAction onFirstTapEnterEvent = delegate
		{
		};

		public event OnTapAction onFirstTapExitEvent = delegate
		{
		};

		public event OnTapAction onSecondTapEvent = delegate
		{
		};

		private bool IsInTriggerArea(Vector3 point)
		{
			return Vector3.Distance(point, _firstEnterTouch) < _firstTapTriggerDeviation;
		}

		public override void OnTouchDown(TouchData touchData)
		{
			base.OnTouchDown(touchData);
			if (_tapState == Tap.NoTap)
			{
				_firstEnterTouch = touchData.positionConverted;
				_firstTouchEnterTime = Time.time;
				_tapState = Tap.FirstTapEnter;
				this.onFirstTapEnterEvent();
			}
			else if (_tapState == Tap.FirstTapExit && Time.time - _firstTouchEnterTime < _nextTapWaitTime && Vector3.Distance(_firstEnterTouch, touchData.positionConverted) < _secondTapTriggerDeviation)
			{
				_tapState = Tap.NoTap;
				this.onSecondTapEvent();
			}
			else
			{
				_tapState = Tap.NoTap;
			}
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			base.OnTouchEnd(touchData);
			if (_tapState == Tap.FirstTapEnter && Time.time - _firstTouchEnterTime < _nextTapWaitTime && IsInTriggerArea(touchData.positionConverted))
			{
				_tapState = Tap.FirstTapExit;
				this.onFirstTapExitEvent();
			}
			else
			{
				_tapState = Tap.NoTap;
			}
		}
	}
}
