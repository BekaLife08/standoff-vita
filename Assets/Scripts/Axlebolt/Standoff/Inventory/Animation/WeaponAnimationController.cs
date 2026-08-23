using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponAnimationController : MonoBehaviour
	{
		public WeaponAnimationParameters AnimationParameters;

		public GameObject weaponHierarchy;

		public Animator animator;

		public Animator rightHandAnimator;

		public Animator leftHandAnimator;

		private WeaponAnimationClip currentAnimClip = new WeaponAnimationClip();

		private WeaponMap weaponMap;

		public WeaponAnimationEventController eventController;

		public WeaponController weapon;

		private bool _isActive;

		private BipedMap bipedMap;

		private float _shotTime;

		private float _prevProgress;

		public void PreInitialize(WeaponController weapon)
		{
			this.weapon = weapon;
			weaponMap = GetComponent<WeaponMap>();
			AnimationParameters = weapon.AnimationParameters;
			weaponHierarchy = weapon.WeaponHierarchy;
			if (weaponMap.Magazine1 != null && AnimationParameters.magazine1AnimationName != string.Empty)
			{
				weaponMap.Magazine1.name = AnimationParameters.magazine1AnimationName;
			}
			if (weaponMap.Magazine2 != null && AnimationParameters.magazine2AnimationName != string.Empty)
			{
				weaponMap.Magazine2.name = AnimationParameters.magazine2AnimationName;
			}
			if (weaponMap.Magazine3 != null && AnimationParameters.magazine3AnimationName != string.Empty)
			{
				weaponMap.Magazine3.name = AnimationParameters.magazine3AnimationName;
			}
			if (weaponMap.Gunlock1 != null && AnimationParameters.gunlock1AnimationName != string.Empty)
			{
				weaponMap.Gunlock1.name = AnimationParameters.gunlock1AnimationName;
			}
			if (weaponMap.Gunlock2 != null && AnimationParameters.gunlock2AnimationName != string.Empty)
			{
				weaponMap.Gunlock2.name = AnimationParameters.gunlock2AnimationName;
			}
			if (weaponMap.Sight != null && AnimationParameters.sightAnimationName != string.Empty)
			{
				weaponMap.Sight.name = AnimationParameters.sightAnimationName;
			}
			if (weaponMap.CollimatorSight != null && AnimationParameters.collimatorSightAnimationName != string.Empty)
			{
				weaponMap.CollimatorSight.name = AnimationParameters.collimatorSightAnimationName;
			}
			if (weaponMap.SightLense != null && AnimationParameters.sightLenseAnimationName != string.Empty)
			{
				weaponMap.SightLense.name = AnimationParameters.sightLenseAnimationName;
			}
			if (weaponMap.SightReticle != null && AnimationParameters.sightReticleAnimationName != string.Empty)
			{
				weaponMap.SightReticle.name = AnimationParameters.sightReticleAnimationName;
			}
			if (weaponMap.Cartridge1 != null && AnimationParameters.cartridge1AnimationName != string.Empty)
			{
				weaponMap.Cartridge1.name = AnimationParameters.cartridge1AnimationName;
			}
			if (weaponMap.Cartridge2 != null && AnimationParameters.cartridge2AnimationName != string.Empty)
			{
				weaponMap.Cartridge2.name = AnimationParameters.cartridge2AnimationName;
			}
			if (weaponHierarchy.GetComponent<Animator>() == null)
			{
				animator = weaponHierarchy.AddComponent<Animator>();
			}
			else
			{
				animator = weaponHierarchy.GetComponent<Animator>();
			}
			animator.runtimeAnimatorController = AnimationParameters.animatorController;
			if (weaponHierarchy.GetComponent<WeaponAnimationEventController>() == null)
			{
				eventController = weaponHierarchy.AddComponent<WeaponAnimationEventController>();
			}
			eventController.Initialize(weapon);
		}

		public void OnPlayerSet(PlayerController playerController)
		{
			bipedMap = playerController.BipedMap;
			_shotTime = (_prevProgress = 0f);
		}

		private void InitializeHandAnimators()
		{
			leftHandAnimator = bipedMap.LeftHand.GetComponent<Animator>();
			if (leftHandAnimator == null)
			{
				leftHandAnimator = bipedMap.LeftHand.gameObject.AddComponent<Animator>();
			}
			leftHandAnimator.runtimeAnimatorController = AnimationParameters.lHandAnimatorController;
			rightHandAnimator = bipedMap.RightHand.GetComponent<Animator>();
			if (rightHandAnimator == null)
			{
				rightHandAnimator = bipedMap.RightHand.gameObject.AddComponent<Animator>();
			}
			rightHandAnimator.runtimeAnimatorController = AnimationParameters.rHandAnimatorController;
		}

		public void OnCharacterSkinSet(BipedMap bipedMap)
		{
			this.bipedMap = bipedMap;
			InitializeHandAnimators();
		}

		private WeaponAnimationClip FindWeaponAnimationClip(int sourceStateNameHash)
		{
			if (currentAnimClip != null && currentAnimClip.sourceAnimationStateNameHashList.Count > 0 && currentAnimClip.sourceAnimationStateNameHashList[0] == sourceStateNameHash)
			{
				return currentAnimClip;
			}
			foreach (WeaponAnimationClip animationClip in AnimationParameters.animationClipList)
			{
				if (animationClip.sourceAnimationStateNameHashList[0] == sourceStateNameHash)
				{
					return animationClip;
				}
			}
			return null;
		}

		public void OnBecomePrimary()
		{
			InitializeHandAnimators();
		}

		public void SetActive(bool isActive)
		{
			_isActive = isActive;
			if (animator != null)
			{
				animator.enabled = isActive;
			}
		}

		public void OnWeaponShoot(float time)
		{
			_shotTime = time;
			_prevProgress = EvaluateRecoilAnimationProgress(time);
		}

		private float EvaluateRecoilAnimationProgress(float time)
		{
			WeaponRecoilAnimation recoilAnimation = AnimationParameters.RecoilAnimation;
			float a = _prevProgress * recoilAnimation.Duration;
			float num = time - _shotTime;
			float t = num / recoilAnimation.BlendDuration;
			float b = recoilAnimation.BlendOffset + num;
			float num2 = Mathf.Lerp(a, b, t);
			return num2 / recoilAnimation.Duration;
		}

		public float EvaluateRecoilAnimation(float time)
		{
			return AnimationParameters.RecoilAnimation.AnimationProgressCurve.Evaluate(EvaluateRecoilAnimationProgress(time));
		}

		public void Play(int sourceStateNameHash, float normalizedTime, bool isSynchronized, bool isVisible, float deltaTime)
		{
			if (!_isActive)
			{
				return;
			}
			currentAnimClip = FindWeaponAnimationClip(sourceStateNameHash);
			if (currentAnimClip == null)
			{
				return;
			}
			if (isVisible)
			{
				animator.Play(currentAnimClip.targetAnimationStateNameHash, 0, normalizedTime);
				if (!isSynchronized)
				{
					animator.Update(0f);
				}
				if (leftHandAnimator != null && leftHandAnimator.runtimeAnimatorController != null)
				{
					leftHandAnimator.Play(currentAnimClip.targetAnimationStateNameHash, 0, normalizedTime);
				}
				if (rightHandAnimator != null && rightHandAnimator.runtimeAnimatorController != null)
				{
					rightHandAnimator.Play(currentAnimClip.targetAnimationStateNameHash, 0, normalizedTime);
				}
			}
			if (eventController != null)
			{
				eventController.Evaluate(currentAnimClip, normalizedTime, deltaTime);
			}
		}
	}
}
