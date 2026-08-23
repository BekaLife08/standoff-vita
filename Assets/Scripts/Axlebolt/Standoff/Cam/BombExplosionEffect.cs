using Axlebolt.Standoff.Common;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	public class BombExplosionEffect : AmplifyColorSupportEffect
	{
		private BombExplosionEffectParameters _parameters;

		protected override void Awake()
		{
			base.Awake();
			_parameters = ResourcesUtility.Load<BombExplosionEffectParameters>("Camera/BombExplosionEffectParameters");
		}

		public void PlayEffect()
		{
			StartCoroutine(PlayEffectCoroutine());
		}

		public IEnumerator PlayEffectCoroutine()
		{
			ApplyImageEffect();
			yield return new WaitForSeconds(_parameters.Duration);
			ClearImageEffect();
		}

		protected override RuntimeAnimatorController GetRuntimeAnimatorController()
		{
			return _parameters.AnimatorController;
		}

		protected override Texture GetLutBlendTexture()
		{
			return _parameters.LutBlendTexture;
		}
	}
}
