using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	[CreateAssetMenu(fileName = "NewCameraOnDieEffectParameters", menuName = "Standoff/Camera/Create PlayerFocusEffectParameters", order = 2)]
	public class PlayerFocusEffectParameters : ScriptableObject
	{
		[SerializeField]
		private float _characterForwardOffset;

		[SerializeField]
		private float _characterHeightOffset;

		[SerializeField]
		private AnimationCurve _distancingCurve;

		[SerializeField]
		private AnimationCurve _durationCurve;

		[SerializeField]
		private AnimationCurve _cameraLocalAnglesCorrectingCurve;

		public float CharacterForwardOffset
		{
			[CompilerGenerated]
			get
			{
				return _characterForwardOffset;
			}
		}

		public float CharacterHeightOffset
		{
			[CompilerGenerated]
			get
			{
				return _characterHeightOffset;
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

		public AnimationCurve DurationCurve
		{
			[CompilerGenerated]
			get
			{
				return _durationCurve;
			}
		}

		public AnimationCurve CameraLocalAnglesCorrectingCurve
		{
			[CompilerGenerated]
			get
			{
				return _cameraLocalAnglesCorrectingCurve;
			}
		}
	}
}
