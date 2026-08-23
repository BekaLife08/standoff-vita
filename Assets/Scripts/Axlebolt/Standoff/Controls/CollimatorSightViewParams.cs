using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	[Serializable]
	public class CollimatorSightViewParams
	{
		[SerializeField]
		private AnimationCurve _backgroundFadeInCurve;

		[SerializeField]
		private AnimationCurve _backgroundFadeOutCurve;

		[SerializeField]
		private AnimationCurve _sightPointFadeInCurve;

		[SerializeField]
		private float _backgroundAlpha;

		public AnimationCurve BackgroundFadeInCurve
		{
			[CompilerGenerated]
			get
			{
				return _backgroundFadeInCurve;
			}
		}

		public AnimationCurve BackgroundFadeOutCurve
		{
			[CompilerGenerated]
			get
			{
				return _backgroundFadeOutCurve;
			}
		}

		public AnimationCurve SightPointFadeInCurve
		{
			[CompilerGenerated]
			get
			{
				return _sightPointFadeInCurve;
			}
		}

		public float BackgroundAlpha
		{
			[CompilerGenerated]
			get
			{
				return _backgroundAlpha;
			}
		}
	}
}
