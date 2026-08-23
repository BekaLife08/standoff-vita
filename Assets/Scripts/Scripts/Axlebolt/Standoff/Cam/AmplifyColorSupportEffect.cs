using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	public abstract class AmplifyColorSupportEffect : MonoBehaviour
	{
		private AmplifyColorEffect _amplifyColorEffect;

		private Animator _animator;

		protected virtual void Awake()
		{
			_amplifyColorEffect = this.GetRequireComponent<AmplifyColorEffect>();
			_animator = this.GetRequireComponent<Animator>();
		}

		protected void ApplyImageEffect()
		{
			_amplifyColorEffect.BlendAmount = 0f;
			_amplifyColorEffect.LutBlendTexture = GetLutBlendTexture();
			_amplifyColorEffect.enabled = true;
			_animator.runtimeAnimatorController = GetRuntimeAnimatorController();
		}

		protected void ClearImageEffect()
		{
			if (_amplifyColorEffect.LutBlendTexture == GetLutBlendTexture())
			{
				_amplifyColorEffect.LutBlendTexture = null;
				_amplifyColorEffect.enabled = false;
			}
			if (_animator.runtimeAnimatorController == GetRuntimeAnimatorController())
			{
				_animator.runtimeAnimatorController = null;
			}
		}

		protected abstract RuntimeAnimatorController GetRuntimeAnimatorController();

		protected abstract Texture GetLutBlendTexture();
	}
}
