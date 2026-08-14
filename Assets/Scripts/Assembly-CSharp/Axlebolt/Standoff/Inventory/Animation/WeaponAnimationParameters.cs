using System.Collections.Generic;
using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponAnimationParameters : ScriptableObject
	{
		public int sourceAnimationStatesLayer = 1;

		public RuntimeAnimatorController animatorController;

		public RuntimeAnimatorController lHandAnimatorController;

		public RuntimeAnimatorController rHandAnimatorController;

		public List<WeaponAnimationClip> animationClipList = new List<WeaponAnimationClip>();

		public TransformTR fpsCamOffset;

		public TransformTR spineFPSDirectorTR;

		public TransformTR weaponDirectorOnStand;

		public TransformTR weaponDirectorOnCrouch;

		public float ArmsShakeMult;

		public string magazine1AnimationName = string.Empty;

		public string magazine2AnimationName = string.Empty;

		public string magazine3AnimationName = string.Empty;

		public string gunlock1AnimationName = string.Empty;

		public string gunlock2AnimationName = string.Empty;

		public string sightAnimationName = string.Empty;

		public string collimatorSightAnimationName = string.Empty;

		public string sightLenseAnimationName = string.Empty;

		public string sightReticleAnimationName = string.Empty;

		public string cartridge1AnimationName = string.Empty;

		public string cartridge2AnimationName = string.Empty;

		public bool BlendedTPSShoot;

		public WeaponRecoilAnimation RecoilAnimation;

		[Header("Sound Parameters")]
		[Space(20f)]
		public int audioSourcesCount;

		public int animationAudioSourcesCount = 2;

		public AudioClip shotSound;

		public float shotSoundVolume = 1f;

		public List<AnimationSoundEvent> soundEventlist = new List<AnimationSoundEvent>();
	}
}
