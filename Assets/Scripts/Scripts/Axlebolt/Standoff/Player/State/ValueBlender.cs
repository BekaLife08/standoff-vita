using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player.State
{
	[Serializable]
	public class ValueBlender
	{
		public enum Type
		{
			CurveValue,
			PointsInterpolation
		}

		public enum BlendType
		{
			BetweenCurves,
			pointCurve
		}

		public enum ProgressType
		{
			Actual,
			Blened
		}

		public Type type;

		public BlendType blendType;

		private CurvedValue _currentCurve = new CurvedValue();

		private float _startValue;

		private float _timeTransitionStarted_fictitiousCurrent;

		private float _timeBlendStarted_relative;

		private float localTime;

		private float _duration;

		private float _blendDuration;

		private float _blendStartPrevValue;

		private float _blendStartCurveValue;

		private bool _isBlending;

		private ValueBlenderParams syncParameters = new ValueBlenderParams();

		private BlendedValue<float> _progress = new BlendedValue<float>();

		private BlendedValue<float> progress
		{
			get
			{
				_progress.actual = GetProgress(localTime);
				return _progress;
			}
			set
			{
				_progress = value;
			}
		}

		private float restTime
		{
			get
			{
				return _duration - (localTime - _timeTransitionStarted_fictitiousCurrent);
			}
			set
			{
			}
		}

		private float pastTime
		{
			get
			{
				return localTime - _timeTransitionStarted_fictitiousCurrent;
			}
			set
			{
			}
		}

		public ValueBlenderParams GetParameters()
		{
			return syncParameters;
		}

		public void SetParameters(ValueBlenderParams listTemp)
		{
		}

		public void InitializeBlending(int curveId, float duration, float time)
		{
			localTime = time;
			_currentCurve = CurvesContainer.GetCurve(curveId);
			_duration = duration;
			type = Type.CurveValue;
			_timeTransitionStarted_fictitiousCurrent = localTime;
		}

		public void BlendValue(float blendDuration, CurvedValue targetCurve, float startValue, float time)
		{
			localTime = time;
			_duration = targetCurve.duration;
			type = Type.CurveValue;
			_timeTransitionStarted_fictitiousCurrent = localTime;
			_blendStartPrevValue = startValue;
			_currentCurve = targetCurve;
			_blendStartCurveValue = GetCurveValue(_currentCurve.curve, _duration, _timeTransitionStarted_fictitiousCurrent, localTime) * _currentCurve.multiplier;
			_blendDuration = blendDuration;
			_timeBlendStarted_relative = localTime - _timeTransitionStarted_fictitiousCurrent;
			float b = _duration - (localTime - _timeTransitionStarted_fictitiousCurrent);
			_blendDuration = Mathf.Min(blendDuration, b);
			if (_blendDuration < 0.001f)
			{
				_blendDuration = 0.001f;
			}
			_isBlending = true;
			blendType = BlendType.BetweenCurves;
		}

		public void BlendValue(float blendDuration, int targetCurveId, float time)
		{
			BlendValue(blendDuration, CurvesContainer.GetCurve(targetCurveId), GetValue(time), time);
		}

		public void BlendValue(float blendDuration, float startValue, float time)
		{
			BlendValue(blendDuration, _currentCurve, startValue, time);
		}

		public void SetProgress(float progressCoeff, float time)
		{
			localTime = time;
			_timeTransitionStarted_fictitiousCurrent = localTime - _duration * progressCoeff;
		}

		public float GetProgress(float time)
		{
			localTime = time;
			return Mathf.Min((localTime - _timeTransitionStarted_fictitiousCurrent) / _duration, 1f);
		}

		private float GetBlendedValue()
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = pastTime - _timeBlendStarted_relative;
			float num4 = Mathf.Min(1f, num3 / _blendDuration);
			float num5 = 0f;
			num2 = GetCurveValue(_currentCurve.curve, _duration, _timeTransitionStarted_fictitiousCurrent, localTime) * _currentCurve.multiplier;
			float num6 = num2 - _blendStartCurveValue;
			num = _blendStartPrevValue + num6;
			num5 = Mathf.Lerp(num, num2, num4);
			if (Mathf.Abs(num4 - 1f) < 0.001f)
			{
				_isBlending = false;
			}
			return num5;
		}

		private float GetCurrentValue()
		{
			float num = 0f;
			if (_isBlending)
			{
				return GetBlendedValue();
			}
			return GetCurveValue(_currentCurve.curve, _duration, _timeTransitionStarted_fictitiousCurrent, localTime) * _currentCurve.multiplier;
		}

		public float GetValue(float time)
		{
			localTime = time;
			return GetCurrentValue();
		}

		public float GetValue(float progress, float time)
		{
			localTime = time;
			float progress2 = GetProgress(localTime);
			SetProgress(progress, localTime);
			float currentValue = GetCurrentValue();
			SetProgress(progress2, localTime);
			return currentValue;
		}

		public static float GetCurveValue(AnimationCurve curve, float duration, float timeStarted, float localTime)
		{
			float num = localTime - timeStarted;
			return curve.Evaluate(num / duration);
		}

		public float GetFinalValue(float time)
		{
			localTime = time;
			if (type == Type.CurveValue)
			{
				return _currentCurve.curve.Evaluate(1f) * _currentCurve.multiplier;
			}
			if (type == Type.PointsInterpolation)
			{
				return 0f;
			}
			return 0f;
		}

		public bool IsFinished(float time)
		{
			localTime = time;
			return _duration < localTime - _timeTransitionStarted_fictitiousCurrent;
		}
	}
}
