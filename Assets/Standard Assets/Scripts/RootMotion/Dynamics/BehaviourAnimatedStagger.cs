using System;
using System.Collections;
using UnityEngine;

namespace RootMotion.Dynamics
{
	[AddComponentMenu("Scripts/RootMotion.Dynamics/PuppetMaster/Behaviours/BehaviourAnimatedStagger")]
	public class BehaviourAnimatedStagger : BehaviourBase
	{
		[Serializable]
		public struct FallParams
		{
			public float startPinWeightMlp;

			public float startMuscleWeightMlp;

			public float losePinSpeed;
		}

		[Serializable]
		public struct FallParamsGroup
		{
			public Muscle.Group[] groups;

			public FallParams fallParams;
		}

		[Header("Master Properties")]
		public LayerMask groundLayers;

		public float animationBlendSpeed = 2f;

		public float animationMag = 5f;

		public float momentumMag = 0.1f;

		public float unbalancedMuscleWeightMlp = 0.05f;

		public float unbalancedMuscleDamperAdd = 1f;

		public bool dropProps;

		public float maxGetUpVelocity = 0.3f;

		public float minHipHeight = 0.3f;

		public SubBehaviourCOM centerOfMass;

		[Header("Muscle Group Properties")]
		public FallParams defaults;

		public FallParamsGroup[] groupOverrides;

		[Header("Events")]
		public PuppetEvent onUngrounded;

		public PuppetEvent onFallOver;

		public PuppetEvent onRest;

		[HideInInspector]
		public Vector3 moveVector;

		[HideInInspector]
		public bool isGrounded = true;

		[HideInInspector]
		public Vector3 forward;

		protected override void OnInitiate()
		{
			centerOfMass.Initiate(this, groundLayers);
		}

		protected override void OnActivate()
		{
			StartCoroutine(LoseBalance());
		}

		public override void OnReactivate()
		{
		}

		private IEnumerator LoseBalance()
		{
			Muscle[] muscles = base.puppetMaster.muscles;
			foreach (Muscle muscle in muscles)
			{
				FallParams fallParams = GetFallParams(muscle.props.group);
				muscle.state.pinWeightMlp = Mathf.Min(muscle.state.pinWeightMlp, fallParams.startPinWeightMlp);
				muscle.state.muscleWeightMlp = Mathf.Min(muscle.state.muscleWeightMlp, fallParams.startMuscleWeightMlp);
				muscle.state.muscleDamperAdd = 0f - base.puppetMaster.muscleDamper;
			}
			base.puppetMaster.internalCollisions = true;
			bool done = false;
			while (!done)
			{
				moveVector = Vector3.Lerp(b: Quaternion.Inverse(base.puppetMaster.targetRoot.rotation) * centerOfMass.direction * animationMag, a: moveVector, t: Time.deltaTime * animationBlendSpeed);
				moveVector = Vector3.ClampMagnitude(moveVector, 2f);
				Muscle[] muscles2 = base.puppetMaster.muscles;
				foreach (Muscle muscle2 in muscles2)
				{
					FallParams fallParams2 = GetFallParams(muscle2.props.group);
					muscle2.state.pinWeightMlp = Mathf.MoveTowards(muscle2.state.pinWeightMlp, 0f, Time.deltaTime * fallParams2.losePinSpeed);
					muscle2.state.mappingWeightMlp = Mathf.MoveTowards(muscle2.state.mappingWeightMlp, 1f, Time.deltaTime * animationBlendSpeed);
				}
				done = true;
				Muscle[] muscles3 = base.puppetMaster.muscles;
				foreach (Muscle muscle3 in muscles3)
				{
					if (muscle3.state.pinWeightMlp > 0f || muscle3.state.mappingWeightMlp < 1f)
					{
						done = false;
						break;
					}
				}
				Vector3 position = base.puppetMaster.muscles[0].rigidbody.position;
				float y = position.y;
				Vector3 position2 = base.puppetMaster.targetRoot.position;
				if (y - position2.y < minHipHeight)
				{
					done = true;
				}
				yield return null;
			}
			if (dropProps)
			{
				RemoveMusclesOfGroup(Muscle.Group.Prop);
			}
			if (!isGrounded)
			{
				Muscle[] muscles4 = base.puppetMaster.muscles;
				foreach (Muscle muscle4 in muscles4)
				{
					muscle4.state.pinWeightMlp = 0f;
					muscle4.state.muscleWeightMlp = 1f;
				}
				onUngrounded.Trigger(base.puppetMaster);
				if (onUngrounded.switchBehaviour)
				{
					yield break;
				}
			}
			moveVector = Vector3.zero;
			base.puppetMaster.mappingWeight = 1f;
			Muscle[] muscles5 = base.puppetMaster.muscles;
			foreach (Muscle muscle5 in muscles5)
			{
				muscle5.state.pinWeightMlp = 0f;
				muscle5.state.muscleWeightMlp = unbalancedMuscleWeightMlp;
				muscle5.state.muscleDamperAdd = unbalancedMuscleDamperAdd;
			}
			onFallOver.Trigger(base.puppetMaster);
			if (!onFallOver.switchBehaviour)
			{
				yield return new WaitForSeconds(1f);
				while (base.puppetMaster.muscles[0].rigidbody.velocity.magnitude > maxGetUpVelocity || !isGrounded)
				{
					yield return null;
				}
				onRest.Trigger(base.puppetMaster);
				if (!onRest.switchBehaviour)
				{
				}
			}
		}

		private FallParams GetFallParams(Muscle.Group group)
		{
			FallParamsGroup[] array = groupOverrides;
			for (int i = 0; i < array.Length; i++)
			{
				FallParamsGroup fallParamsGroup = array[i];
				Muscle.Group[] groups = fallParamsGroup.groups;
				foreach (Muscle.Group group2 in groups)
				{
					if (group2 == group)
					{
						return fallParamsGroup.fallParams;
					}
				}
			}
			return defaults;
		}
	}
}
