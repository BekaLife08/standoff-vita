using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponAnimationEventController : MonoBehaviour
	{
		public delegate void AnimationEvent();

		public WeaponAnimationParameters AnimationParameters;

		private WeaponAnimationClip currentAnimationClip;

		private WeaponMap weaponMap;

		public WeaponController weapon;

		private int _currentAudioSourceIndex;

		public AnimationEvent OnMagazineThrowEvent = delegate
		{
		};

		public AnimationEvent OnMagazineAttachHandEvent = delegate
		{
		};

		public AnimationEvent OnMagazineInEvent = delegate
		{
		};

		public AnimationEvent OnMagazineOutEvent = delegate
		{
		};

		public AnimationEvent OnGunLock1Event = delegate
		{
		};

		public AnimationEvent OnGunLock2Event = delegate
		{
		};

		public AnimationEvent OnGunLock3Event = delegate
		{
		};

		public AnimationEvent OnGunLock4Event = delegate
		{
		};

		private List<AudioSource> _audioSources => weapon.AnimationAudioSources;

		public void Initialize(WeaponController weapon)
		{
			weaponMap = GetComponent<WeaponMap>();
			this.weapon = weapon;
			AnimationParameters = weapon.AnimationParameters;
			_currentAudioSourceIndex = 0;
		}

		public void Evaluate(WeaponAnimationClip animationClip, float normalizedTime, float deltaTime)
		{
			currentAnimationClip = animationClip;
			if (weaponMap == null)
			{
				weaponMap = GetComponent<WeaponMap>();
				if (weaponMap == null)
				{
					UnityEngine.Debug.LogError("Weapon map is NULL");
				}
			}
			else
			{
				int num = -1;
				foreach (WeaponAnimationSegment segments in currentAnimationClip.segmentsList)
				{
					if (segments.startEventNT <= normalizedTime && normalizedTime <= segments.endEventNT)
					{
						GameObject weaponPart = weaponMap.GetWeaponPart(segments.weaponPart);
						num++;
						if (!(weaponPart == null))
						{
							if (segments.actionType == WeaponAnimationSegment.SegmentActionType.Disable && weaponPart.activeSelf)
							{
								weaponPart.SetActive(value: false);
							}
							if (segments.actionType == WeaponAnimationSegment.SegmentActionType.Enable && !weaponPart.activeSelf)
							{
								weaponPart.SetActive(value: true);
							}
						}
					}
				}
				foreach (WeaponAnimationEvent @event in currentAnimationClip.eventList)
				{
					float num2 = deltaTime / (currentAnimationClip.clipDuration / currentAnimationClip.clipSpeed);
					float num3 = normalizedTime - num2;
					if (num3 <= @event.normalizedTime && @event.normalizedTime <= normalizedTime)
					{
						CallEvent(@event.eventType);
					}
				}
			}
		}

		public void CallEvent(WeaponAnimationEventType eventType)
		{
			switch (eventType)
			{
			case WeaponAnimationEventType.MagazineAttachHand:
				OnMagazineAttachHand();
				break;
			case WeaponAnimationEventType.MagazineThrow:
				OnMagazineThrow();
				break;
			case WeaponAnimationEventType.MagazineIn:
				OnMagazineIn();
				break;
			case WeaponAnimationEventType.MagazineOut:
				OnMagazineOut();
				break;
			case WeaponAnimationEventType.GunLock1:
				OnGunLock1();
				break;
			case WeaponAnimationEventType.GunLock2:
				OnGunLock2();
				break;
			case WeaponAnimationEventType.GunLock3:
				OnGunLock3();
				break;
			case WeaponAnimationEventType.GunLock4:
				OnGunLock4();
				break;
			}
		}

		private AudioSource GetCurrentAudioSource()
		{
			if (_audioSources.Count == 0)
			{
				return null;
			}
			_currentAudioSourceIndex %= _audioSources.Count;
			return _audioSources[_currentAudioSourceIndex];
		}

		private void PlaySoundClip(WeaponAnimationEventType eventType)
		{
			foreach (AnimationSoundEvent item in AnimationParameters.soundEventlist)
			{
				if (item.eventTipe == eventType)
				{
					AudioSource currentAudioSource = GetCurrentAudioSource();
					if (!(currentAudioSource == null))
					{
						currentAudioSource.panStereo = item.audioClipConfig.stereoPan;
						currentAudioSource.volume = item.audioClipConfig.volume;
						currentAudioSource.pitch = item.audioClipConfig.pitch;
						currentAudioSource.PlayOneShot(item.audioClip);
						_currentAudioSourceIndex++;
					}
					break;
				}
			}
		}

		public void OnMagazineThrow()
		{
			PlaySoundClip(WeaponAnimationEventType.MagazineThrow);
			OnMagazineThrowEvent();
		}

		public void OnMagazineAttachHand()
		{
			PlaySoundClip(WeaponAnimationEventType.MagazineAttachHand);
			OnMagazineAttachHandEvent();
		}

		public void OnMagazineIn()
		{
			PlaySoundClip(WeaponAnimationEventType.MagazineIn);
			OnMagazineInEvent();
		}

		public void OnMagazineOut()
		{
			PlaySoundClip(WeaponAnimationEventType.MagazineOut);
			OnMagazineOutEvent();
		}

		public void OnGunLock1()
		{
			PlaySoundClip(WeaponAnimationEventType.GunLock1);
			OnGunLock1Event();
		}

		public void OnGunLock2()
		{
			PlaySoundClip(WeaponAnimationEventType.GunLock2);
			OnGunLock2Event();
		}

		public void OnGunLock3()
		{
			PlaySoundClip(WeaponAnimationEventType.GunLock3);
			OnGunLock3Event();
		}

		public void OnGunLock4()
		{
			PlaySoundClip(WeaponAnimationEventType.GunLock4);
			OnGunLock4Event();
		}
	}
}
