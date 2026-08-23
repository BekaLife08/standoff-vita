using Axlebolt.Standoff.Common;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	[Serializable]
	public class GunSightViewParameters
	{
		[SerializeField]
		private float _aimingStartDuration;

		[SerializeField]
		private float _aimingEndDuration;

		[SerializeField]
		private TransformTR _cameraNrearedTR;

		[SerializeField]
		private AnimationCurve _cameraTRNearingCurve;

		[SerializeField]
		private AnimationCurve _cameraFOVNearingCurve;

		[SerializeField]
		private float _defaultFOV;

		[SerializeField]
		private float _aimingFOV;

		[SerializeField]
		private float _sightDisbleNT;

		[SerializeField]
		private float _sightEnableNT;

		[SerializeField]
		private float _sightLenseDisbleNT;

		[SerializeField]
		private float _sightLenseEnableNT;

		[SerializeField]
		private AnimationCurve _shakeAnimationSpeedCurve;

		[SerializeField]
		private AnimationCurve _shakeAnimationMultCurve;

		[SerializeField]
		private AnimationCurve _xAxisShakeAnimationCurve;

		[SerializeField]
		private AnimationCurve _yAxisShakeAnimationCurve;

		[SerializeField]
		private AnimationCurve _rotationDeviationCurve;

		[SerializeField]
		private AnimationCurve _xAxisShotAnimationCurve;

		[SerializeField]
		private AnimationCurve _yAxisShotAnimationCurve;

		[SerializeField]
		private AnimationCurve _zAxisShotAnimationCurve;

		[SerializeField]
		private float _transitionOffset;

		[SerializeField]
		private float _transitionDuration;

		public float AimingEndDuration
		{
			[CompilerGenerated]
			get
			{
				return _aimingEndDuration;
			}
		}

		public float AimingStartDuration
		{
			[CompilerGenerated]
			get
			{
				return _aimingStartDuration;
			}
		}

		public TransformTR CameraNrearedTR
		{
			[CompilerGenerated]
			get
			{
				return _cameraNrearedTR;
			}
		}

		public AnimationCurve CameraTRNearingCurve
		{
			[CompilerGenerated]
			get
			{
				return _cameraTRNearingCurve;
			}
		}

		public AnimationCurve CameraFovNearingCurve
		{
			[CompilerGenerated]
			get
			{
				return _cameraFOVNearingCurve;
			}
		}

		public float DefaultFov
		{
			[CompilerGenerated]
			get
			{
				return _defaultFOV;
			}
		}

		public float AimingFov
		{
			[CompilerGenerated]
			get
			{
				return _aimingFOV;
			}
		}

		public float SightDisbleNt
		{
			[CompilerGenerated]
			get
			{
				return _sightDisbleNT;
			}
		}

		public float SightEnableNt
		{
			[CompilerGenerated]
			get
			{
				return _sightEnableNT;
			}
		}

		public float SightLenseDisbleNt
		{
			[CompilerGenerated]
			get
			{
				return _sightLenseDisbleNT;
			}
		}

		public float SightLenseEnableNt
		{
			[CompilerGenerated]
			get
			{
				return _sightLenseEnableNT;
			}
		}

		public AnimationCurve ShakeAnimationSpeedCurve
		{
			[CompilerGenerated]
			get
			{
				return _shakeAnimationSpeedCurve;
			}
		}

		public AnimationCurve ShakeAnimationMultCurve
		{
			[CompilerGenerated]
			get
			{
				return _shakeAnimationMultCurve;
			}
		}

		public AnimationCurve XAxisShakeAnimationCurve
		{
			[CompilerGenerated]
			get
			{
				return _xAxisShakeAnimationCurve;
			}
		}

		public AnimationCurve YAxisShakeAnimationCurve
		{
			[CompilerGenerated]
			get
			{
				return _yAxisShakeAnimationCurve;
			}
		}

		public AnimationCurve RotationDeviationCurve
		{
			[CompilerGenerated]
			get
			{
				return _rotationDeviationCurve;
			}
		}

		public AnimationCurve XAxisShotAnimationCurve
		{
			[CompilerGenerated]
			get
			{
				return _xAxisShotAnimationCurve;
			}
		}

		public AnimationCurve YAxisShotAnimationCurve
		{
			[CompilerGenerated]
			get
			{
				return _yAxisShotAnimationCurve;
			}
		}

		public AnimationCurve ZAxisShotAnimationCurve
		{
			[CompilerGenerated]
			get
			{
				return _zAxisShotAnimationCurve;
			}
		}

		public float TransitionOffset
		{
			[CompilerGenerated]
			get
			{
				return _transitionOffset;
			}
		}

		public float TransitionDuration
		{
			[CompilerGenerated]
			get
			{
				return _transitionDuration;
			}
		}
	}
}
