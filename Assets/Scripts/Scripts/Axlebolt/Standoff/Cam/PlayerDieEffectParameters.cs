using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	[CreateAssetMenu(fileName = "NewCameraOnDieEffectParameters", menuName = "Standoff/Camera/Create PlayerDieEffectParameters", order = 1)]
	public class PlayerDieEffectParameters : ScriptableObject
	{
		[SerializeField]
		private Vector3 _initialOffset;

		[SerializeField]
		private AnimationCurve _yAxisProjectionCurve;

		[SerializeField]
		private AnimationCurve _distancingCurve;

		[SerializeField]
		private AnimationCurve _xAxisRotationCurve;

		[SerializeField]
		private float _effectDuration;

		[SerializeField]
		private Texture _lutBlendTexture;

		[SerializeField]
		private RuntimeAnimatorController _animatorController;

		public Vector3 InitialOffset
		{
			[CompilerGenerated]
			get
			{
				return _initialOffset;
			}
		}

		public AnimationCurve YAxisProjectionCurve
		{
			[CompilerGenerated]
			get
			{
				return _yAxisProjectionCurve;
			}
		}

		public AnimationCurve XAxisRotationCurve
		{
			[CompilerGenerated]
			get
			{
				return _xAxisRotationCurve;
			}
		}

		public AnimationCurve DistancingCurve
		{
			[CompilerGenerated]
			get
			{
				return _distancingCurve;
			}
		}

		public float EffectDuration
		{
			[CompilerGenerated]
			get
			{
				return _effectDuration;
			}
		}

		public Texture LutBlendTexture
		{
			[CompilerGenerated]
			get
			{
				return _lutBlendTexture;
			}
		}

		public RuntimeAnimatorController AnimatorController
		{
			[CompilerGenerated]
			get
			{
				return _animatorController;
			}
		}
	}
}
