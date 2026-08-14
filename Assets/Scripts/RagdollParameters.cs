using Axlebolt.Standoff.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RagdollParameters : ScriptableObject
{
	[Serializable]
	public class BoneImpulseEffectParameter
	{
		public BipedMap.Bip bone;

		public float impulseMult;

		public float impulseMultY;
	}

	[Serializable]
	public class BoneAffectGroup
	{
		[HideInInspector]
		public bool isVisible = true;

		public BipedMap.Bip targetBone;

		public float targetBoneImpulseMult;

		public List<BoneImpulseEffectParameter> affectedBones = new List<BoneImpulseEffectParameter>();
	}

	public List<BoneAffectGroup> boneAffectGroup = new List<BoneAffectGroup>();

	[Header("General Drop Data")]
	public AnimationCurve inertialVelocityDistribution;

	public AnimationCurve weaponInertialVelocityDistribution;

	public float weaponAdditiveAngSpeed;

	public AnimationCurve weaponAdditiveAngSpeedDistributionAngular;

	public AnimationCurve weaponAdditiveAngSpeedDistributionIntertial;

	public float referenceInertialVelocity;
}
