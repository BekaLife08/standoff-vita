using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponAnimationTool : MonoBehaviour
	{
		public WeaponAnimationParameters WeaponAnimationParameters;

		[Space(5f)]
		[NotNull]
		public Animator characterAnimator;

		[Space(5f)]
		[NotNull]
		public PlayerController playerController;

		[Space(5f)]
		[NotNull]
		public Transform weaponDirection;

		[Space(5f)]
		[NotNull]
		public WeaponAnimationController weaponAnimationController;

		[HideInInspector]
		public WeaponAnimationClip currentAnimationClip;

		[HideInInspector]
		public float currentPlayback;

		private string _defaultFullBodyAnimation = "Walking";

		public void AddNewClip(WeaponAnimationClip animationClip)
		{
			animationClip.sourceAnimationStateNameHashList = new List<int>();
			foreach (string sourceAnimationStateName in animationClip.sourceAnimationStateNameList)
			{
				animationClip.sourceAnimationStateNameHashList.Add(Animator.StringToHash(sourceAnimationStateName));
			}
			animationClip.targetAnimationStateNameHash = Animator.StringToHash(animationClip.targetAnimationStateName);
			WeaponAnimationParameters.animationClipList.Add(animationClip);
		}

		public void RemoveClip(string targetStateName)
		{
			WeaponAnimationClip weaponAnimationClip = null;
			foreach (WeaponAnimationClip animationClip in WeaponAnimationParameters.animationClipList)
			{
				if (animationClip.targetAnimationStateName == targetStateName)
				{
					weaponAnimationClip = animationClip;
					break;
				}
			}
			if (weaponAnimationClip != null)
			{
				if (currentAnimationClip == weaponAnimationClip)
				{
					currentAnimationClip = null;
				}
				WeaponAnimationParameters.animationClipList.Remove(weaponAnimationClip);
			}
		}

		public void Play()
		{
			characterAnimator.Play(Animator.StringToHash(_defaultFullBodyAnimation), 0, 0.5f);
			characterAnimator.SetFloat("Horizontal", 0f);
			characterAnimator.SetFloat("Vertical", 0f);
			characterAnimator.Update(0f);
			weaponAnimationController.AnimationParameters = WeaponAnimationParameters;
			weaponAnimationController.Play(currentAnimationClip.sourceAnimationStateNameHashList[0], currentPlayback, false, true, Time.deltaTime);
			characterAnimator.Play(currentAnimationClip.sourceAnimationStateNameHashList[0], 1, currentPlayback);
			characterAnimator.Update(0f);
		}
	}
}
