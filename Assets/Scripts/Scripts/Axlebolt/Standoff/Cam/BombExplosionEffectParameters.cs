using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	[CreateAssetMenu(fileName = "BombExplosionEffectParameters", menuName = "Standoff/Camera/Create BombExplosionEffectParameters")]
	public class BombExplosionEffectParameters : ScriptableObject
	{
		[SerializeField]
		private Texture _lutBlendTexture;

		[SerializeField]
		private RuntimeAnimatorController _animatorController;

		[SerializeField]
		private float _duration;

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

		public float Duration
		{
			[CompilerGenerated]
			get
			{
				return _duration;
			}
		}
	}
}
