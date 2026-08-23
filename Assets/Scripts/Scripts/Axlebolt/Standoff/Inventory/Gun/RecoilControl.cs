using Axlebolt.Standoff.Inventory.Weapon;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Gun
{
	public class RecoilControl
	{
		private float _lastShotTime;

		private float _prevShotProgress;

		private Vector2 _previousActualPoint = Vector2.zero;

		private Vector2 _currentActualPoint = Vector2.zero;

		private float _approachDeltaDist;

		private Vector2 _currentRelativeDispersion = Vector2.zero;

		private float _localTime;

		private readonly RecoilData _recoilData = new RecoilData();

		private readonly GunParameters _gunParameters;

		private readonly RecoilParameters _recoilParameters;

		public float Progress
		{
			get;
			private set;
		}

		public RecoilControl(GunParameters gunParametrs)
		{
			_gunParameters = gunParametrs;
			_recoilParameters = gunParametrs.RecoilParameters;
		}

		private Vector2 GetEstimatedPoint(float progress)
		{
			Vector2 zero = Vector2.zero;
			zero.x = _recoilParameters.xAxisDeviation.Evaluate(progress) * _recoilParameters.verticalRange;
			zero.y = _recoilParameters.yAxisDeviation.Evaluate(progress) * _recoilParameters.horizontalRange;
			return zero;
		}

		private Vector2 GetRelativeDispertion()
		{
			float max = 1f;
			float angle = Random.Range(0f, 360f);
			Vector3 a = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
			a *= Random.Range(0f, max);
			return new Vector2(a.x, a.y);
		}

		public RecoilData GetNextShot(bool isContinuousShot, float time)
		{
			_localTime = time;
			float num = (float)_gunParameters.FireRate / 60f;
			float num2 = 1f / num;
			float num3 = (!isContinuousShot) ? (_localTime - _lastShotTime) : num2;
			float num4 = _recoilParameters.recoilAccelerationCurve.Evaluate(1f) * _recoilParameters.recoilAccelStep;
			float num5 = _prevShotProgress + num4;
			if (_recoilParameters.recoilAccelDuration > num3)
			{
				Progress = _prevShotProgress + _recoilParameters.recoilAccelerationCurve.Evaluate(num3 / _recoilParameters.recoilAccelDuration) * _recoilParameters.recoilAccelStep;
				Vector2 vector = GetEstimatedPoint(num5) + _currentRelativeDispersion - _previousActualPoint;
				_currentActualPoint = _previousActualPoint + vector.normalized * (_approachDeltaDist * (num3 / _recoilParameters.recoilAccelDuration));
			}
			else
			{
				num3 -= _recoilParameters.recoilAccelDuration;
				Progress = (_prevShotProgress + num4) * _recoilParameters.progressFallbackCurve.Evaluate(num3 / (_recoilParameters.maxFallbackDuration * _recoilParameters.fallbackDurationCurve.Evaluate(num5)));
				Vector2 vector2 = GetEstimatedPoint(num5) + _currentRelativeDispersion - _previousActualPoint;
				_currentActualPoint = (_previousActualPoint + vector2.normalized * _approachDeltaDist) * _recoilParameters.progressFallbackCurve.Evaluate(num3 / (_recoilParameters.maxFallbackDuration * _recoilParameters.fallbackDurationCurve.Evaluate(num5)));
			}
			Vector2 vector3 = GetEstimatedPoint(Progress + num4) + _currentRelativeDispersion - _currentActualPoint;
			Vector2 vector4 = vector3.normalized * _recoilParameters.maxApproachSpeed;
			_approachDeltaDist = ((!(vector4.magnitude > vector3.magnitude)) ? vector4.magnitude : vector3.magnitude);
			_previousActualPoint = _currentActualPoint;
			_prevShotProgress = Progress;
			_lastShotTime = _localTime;
			_recoilData.XDeviation = _currentActualPoint.x;
			_recoilData.YDeviation = _currentActualPoint.y;
			_recoilData.AngleDispertion = _recoilParameters.relativeDispertionCurve.Evaluate(Progress);
			_recoilData.RelativeDispersion = GetRelativeDispertion();
			return _recoilData;
		}

		public RecoilData GetCurrentDeviation(float time)
		{
			_localTime = time;
			float num = time - _lastShotTime;
			float num2 = _recoilParameters.recoilAccelerationCurve.Evaluate(1f) * _recoilParameters.recoilAccelStep;
			float num3 = _prevShotProgress + num2;
			if (_recoilParameters.recoilAccelDuration > num)
			{
				Progress = _prevShotProgress + _recoilParameters.recoilAccelerationCurve.Evaluate(num / _recoilParameters.recoilAccelDuration) * _recoilParameters.recoilAccelStep;
				Vector2 vector = GetEstimatedPoint(num3) - _previousActualPoint;
				_currentActualPoint = _previousActualPoint + vector.normalized * (_approachDeltaDist * (num / _recoilParameters.recoilAccelDuration));
			}
			else
			{
				num -= _recoilParameters.recoilAccelDuration;
				Progress = (_prevShotProgress + num2) * _recoilParameters.progressFallbackCurve.Evaluate(num / (_recoilParameters.maxFallbackDuration * _recoilParameters.fallbackDurationCurve.Evaluate(num3)));
				Vector2 vector2 = GetEstimatedPoint(num3) + _currentRelativeDispersion - _previousActualPoint;
				_currentActualPoint = (_previousActualPoint + vector2.normalized * _approachDeltaDist) * _recoilParameters.progressFallbackCurve.Evaluate(num / (_recoilParameters.maxFallbackDuration * _recoilParameters.fallbackDurationCurve.Evaluate(num3)));
			}
			_recoilData.XDeviation = _currentActualPoint.x;
			_recoilData.YDeviation = _currentActualPoint.y;
			_recoilData.AngleDispertion = _recoilParameters.relativeDispertionCurve.Evaluate(Progress);
			return _recoilData;
		}
	}
}
