using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[Serializable]
	public class AnimationSoundEvent
	{
		public WeaponAnimationEventType eventTipe;

		public AudioClip audioClip;

		public AnimationAudioConfig audioClipConfig;
	}
}
