using Axlebolt.Common.States;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class GunSightViewControl
	{
		public enum States
		{
			Neared,
			Approaching,
			MovingAway,
			NotStated,
			Default
		}

		private GunParameters _gunParameters;

		private float _progress;

		private float _cameraFOV;

		private TransformTR _cameraTR = new TransformTR();

		private StateSimple<States> _state = new StateSimple<States>();

		private States _targetState;

		private float _cameraFOVProgress;

		private float _cameraNearingProgress;

		public float CameraFov
		{
			[CompilerGenerated]
			get
			{
				return _cameraFOV;
			}
		}

		public TransformTR CameraTR
		{
			[CompilerGenerated]
			get
			{
				return _cameraTR;
			}
		}

		public float Progress
		{
			[CompilerGenerated]
			get
			{
				return _progress;
			}
		}

		public States TargetState
		{
			[CompilerGenerated]
			get
			{
				return _targetState;
			}
		}

		public GunSightViewControl(GunParameters gunParameters)
		{
			_gunParameters = gunParameters;
		}

		public void StartAiming(float localTime)
		{
			_state.SetState(States.Approaching, localTime);
		}

		public void FinishAiming(float localTime)
		{
			_state.SetState(States.MovingAway, localTime);
		}

		private void CalculateParameters(float fromFOV, float toFOV, TransformTR fromCameraTR, TransformTR toCameraTR, float progress)
		{
			_cameraFOV = Mathf.Lerp(fromFOV, toFOV, _gunParameters.GunSightViewParameters.CameraFovNearingCurve.Evaluate(progress));
			_cameraTR.pos = Vector3.Lerp(fromCameraTR.pos, toCameraTR.pos, _gunParameters.GunSightViewParameters.CameraTRNearingCurve.Evaluate(progress));
			_cameraTR.rot = VectorAngle.LerpEulerAngle(fromCameraTR.rot, toCameraTR.rot, _gunParameters.GunSightViewParameters.CameraTRNearingCurve.Evaluate(progress));
		}

		private void Approaching()
		{
			CalculateParameters(_gunParameters.GunSightViewParameters.DefaultFov, _gunParameters.GunSightViewParameters.AimingFov, new TransformTR
			{
				pos = Vector3.zero,
				rot = Vector3.zero
			}, _gunParameters.GunSightViewParameters.CameraNrearedTR, _progress);
		}

		private void MovingAway()
		{
			CalculateParameters(_gunParameters.GunSightViewParameters.AimingFov, _gunParameters.GunSightViewParameters.DefaultFov, _gunParameters.GunSightViewParameters.CameraNrearedTR, new TransformTR
			{
				pos = Vector3.zero,
				rot = Vector3.zero
			}, _progress);
		}

		public void Reset()
		{
		}

		public void Evaluate(float localTime, float dTime, float progress, States targetState)
		{
			_progress = progress;
			_targetState = targetState;
			if (_state.curState != States.Approaching)
			{
			}
		}
	}
}
