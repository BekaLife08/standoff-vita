using System;
using UnityEngine;
using UnityEngine.Events;

namespace RootMotion.Dynamics
{
	public abstract class BehaviourBase : MonoBehaviour
	{
		public delegate void BehaviourDelegate();

		public delegate void HitDelegate(MuscleHit hit);

		public delegate void CollisionDelegate(MuscleCollision collision);

		[Serializable]
		public struct PuppetEvent
		{
			[Tooltip("Another Puppet Behaviour to switch to on this event. This must be the exact Type of the the Behaviour, careful with spelling.")]
			public string switchToBehaviour;

			[Tooltip("Animations to cross-fade to on this event. This is separate from the UnityEvent below because UnityEvents can't handle calls with more than one parameter such as Animator.CrossFade.")]
			public AnimatorEvent[] animations;

			[Tooltip("The UnityEvent to invoke on this event.")]
			public UnityEvent unityEvent;

			private const string empty = "";

			public bool switchBehaviour => switchToBehaviour != string.Empty && switchToBehaviour != string.Empty;

			public void Trigger(PuppetMaster puppetMaster, bool switchBehaviourEnabled = true)
			{
				unityEvent.Invoke();
				AnimatorEvent[] array = animations;
				foreach (AnimatorEvent animatorEvent in array)
				{
					animatorEvent.Activate(puppetMaster.targetAnimator, puppetMaster.targetAnimation);
				}
				if (!switchBehaviour)
				{
					return;
				}
				bool flag = false;
				BehaviourBase[] behaviours = puppetMaster.behaviours;
				foreach (BehaviourBase behaviourBase in behaviours)
				{
					if (behaviourBase != null && behaviourBase.GetType().ToString() == "RootMotion.Dynamics." + switchToBehaviour)
					{
						flag = true;
						behaviourBase.Activate();
						break;
					}
				}
				if (!flag)
				{
					UnityEngine.Debug.LogWarning("No Puppet Behaviour of type '" + switchToBehaviour + "' was found. Can not switch to the behaviour, please check the spelling (also for empty spaces).");
				}
			}
		}

		[Serializable]
		public class AnimatorEvent
		{
			public string animationState;

			public float crossfadeTime = 0.3f;

			public int layer;

			public bool resetNormalizedTime;

			private const string empty = "";

			public void Activate(Animator animator, Animation animation)
			{
				if (animator != null)
				{
					Activate(animator);
				}
				if (animation != null)
				{
					Activate(animation);
				}
			}

			private void Activate(Animator animator)
			{
				if (animationState == string.Empty)
				{
					return;
				}
				if (resetNormalizedTime)
				{
					if (crossfadeTime > 0f)
					{
						animator.CrossFadeInFixedTime(animationState, crossfadeTime, layer, 0f);
					}
					else
					{
						animator.Play(animationState, layer, 0f);
					}
				}
				else if (crossfadeTime > 0f)
				{
					animator.CrossFadeInFixedTime(animationState, crossfadeTime, layer);
				}
				else
				{
					animator.Play(animationState, layer);
				}
			}

			private void Activate(Animation animation)
			{
				if (!(animationState == string.Empty))
				{
					if (resetNormalizedTime)
					{
						animation[animationState].normalizedTime = 0f;
					}
					animation[animationState].layer = layer;
					animation.CrossFade(animationState, crossfadeTime);
				}
			}
		}

		public BehaviourDelegate OnPreActivate;

		public BehaviourDelegate OnPreInitiate;

		public BehaviourDelegate OnPreFixedUpdate;

		public BehaviourDelegate OnPreUpdate;

		public BehaviourDelegate OnPreLateUpdate;

		public BehaviourDelegate OnPreDisable;

		public BehaviourDelegate OnPreFixTransforms;

		public BehaviourDelegate OnPreRead;

		public BehaviourDelegate OnPreWrite;

		public HitDelegate OnPreMuscleHit;

		public CollisionDelegate OnPreMuscleCollision;

		public CollisionDelegate OnPreMuscleCollisionExit;

		public BehaviourDelegate OnPostActivate;

		public BehaviourDelegate OnPostInitiate;

		public BehaviourDelegate OnPostFixedUpdate;

		public BehaviourDelegate OnPostUpdate;

		public BehaviourDelegate OnPostLateUpdate;

		public BehaviourDelegate OnPostDisable;

		public BehaviourDelegate OnPostDrawGizmos;

		public BehaviourDelegate OnPostFixTransforms;

		public BehaviourDelegate OnPostRead;

		public BehaviourDelegate OnPostWrite;

		public HitDelegate OnPostMuscleHit;

		public CollisionDelegate OnPostMuscleCollision;

		public CollisionDelegate OnPostMuscleCollisionExit;

		[HideInInspector]
		public bool deactivated;

		private bool initiated;

		public PuppetMaster puppetMaster
		{
			get;
			private set;
		}

		public bool forceActive
		{
			get;
			protected set;
		}

		public abstract void OnReactivate();

		public virtual void Resurrect()
		{
		}

		public virtual void Freeze()
		{
		}

		public virtual void Unfreeze()
		{
		}

		public virtual void KillStart()
		{
		}

		public virtual void KillEnd()
		{
		}

		public virtual void OnMuscleAdded(Muscle m)
		{
		}

		public virtual void OnMuscleRemoved(Muscle m)
		{
		}

		protected virtual void OnActivate()
		{
		}

		protected virtual void OnDeactivate()
		{
		}

		protected virtual void OnInitiate()
		{
		}

		protected virtual void OnFixedUpdate()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnLateUpdate()
		{
		}

		protected virtual void OnDisableBehaviour()
		{
		}

		protected virtual void OnDrawGizmosBehaviour()
		{
		}

		protected virtual void OnFixTransformsBehaviour()
		{
		}

		protected virtual void OnReadBehaviour()
		{
		}

		protected virtual void OnWriteBehaviour()
		{
		}

		protected virtual void OnMuscleHitBehaviour(MuscleHit hit)
		{
		}

		protected virtual void OnMuscleCollisionBehaviour(MuscleCollision collision)
		{
		}

		protected virtual void OnMuscleCollisionExitBehaviour(MuscleCollision collision)
		{
		}

		public void Initiate(PuppetMaster puppetMaster)
		{
			this.puppetMaster = puppetMaster;
			initiated = true;
			if (OnPreInitiate != null)
			{
				OnPreInitiate();
			}
			OnInitiate();
			if (OnPostInitiate != null)
			{
				OnPostInitiate();
			}
		}

		public void Activate()
		{
			if (!initiated)
			{
				UnityEngine.Debug.LogError("Trying to activate a puppet behaviour that has not initiated yet.", base.transform);
				return;
			}
			BehaviourBase[] behaviours = puppetMaster.behaviours;
			foreach (BehaviourBase behaviourBase in behaviours)
			{
				if (behaviourBase != this && behaviourBase.enabled)
				{
					behaviourBase.enabled = false;
					behaviourBase.OnDeactivate();
				}
			}
			base.enabled = true;
			if (OnPreActivate != null)
			{
				OnPreActivate();
			}
			OnActivate();
			if (OnPostActivate != null)
			{
				OnPostActivate();
			}
		}

		public void OnFixTransforms()
		{
			if (initiated && base.enabled)
			{
				if (OnPreFixTransforms != null)
				{
					OnPreFixTransforms();
				}
				OnFixTransformsBehaviour();
				if (OnPostFixTransforms != null)
				{
					OnPostFixTransforms();
				}
			}
		}

		public void OnRead()
		{
			if (initiated && base.enabled)
			{
				if (OnPreRead != null)
				{
					OnPreRead();
				}
				OnReadBehaviour();
				if (OnPostRead != null)
				{
					OnPostRead();
				}
			}
		}

		public void OnWrite()
		{
			if (initiated && base.enabled)
			{
				if (OnPreWrite != null)
				{
					OnPreWrite();
				}
				OnWriteBehaviour();
				if (OnPostWrite != null)
				{
					OnPostWrite();
				}
			}
		}

		public void OnMuscleHit(MuscleHit hit)
		{
			if (initiated)
			{
				if (OnPreMuscleHit != null)
				{
					OnPreMuscleHit(hit);
				}
				OnMuscleHitBehaviour(hit);
				if (OnPostMuscleHit != null)
				{
					OnPostMuscleHit(hit);
				}
			}
		}

		public void OnMuscleCollision(MuscleCollision collision)
		{
			if (initiated)
			{
				if (OnPreMuscleCollision != null)
				{
					OnPreMuscleCollision(collision);
				}
				OnMuscleCollisionBehaviour(collision);
				if (OnPostMuscleCollision != null)
				{
					OnPostMuscleCollision(collision);
				}
			}
		}

		public void OnMuscleCollisionExit(MuscleCollision collision)
		{
			if (initiated)
			{
				if (OnPreMuscleCollisionExit != null)
				{
					OnPreMuscleCollisionExit(collision);
				}
				OnMuscleCollisionExitBehaviour(collision);
				if (OnPostMuscleCollisionExit != null)
				{
					OnPostMuscleCollisionExit(collision);
				}
			}
		}

		private void OnDisable()
		{
			if (initiated)
			{
				if (OnPreDisable != null)
				{
					OnPreDisable();
				}
				OnDisableBehaviour();
				if (OnPostDisable != null)
				{
					OnPostDisable();
				}
			}
		}

		private void FixedUpdate()
		{
			if (initiated && puppetMaster.muscles.Length > 0)
			{
				if (OnPreFixedUpdate != null && base.enabled)
				{
					OnPreFixedUpdate();
				}
				OnFixedUpdate();
				if (OnPostFixedUpdate != null && base.enabled)
				{
					OnPostFixedUpdate();
				}
			}
		}

		private void Update()
		{
			if (initiated && puppetMaster.muscles.Length > 0)
			{
				if (OnPreUpdate != null && base.enabled)
				{
					OnPreUpdate();
				}
				OnUpdate();
				if (OnPostUpdate != null && base.enabled)
				{
					OnPostUpdate();
				}
			}
		}

		private void LateUpdate()
		{
			if (initiated && puppetMaster.muscles.Length > 0)
			{
				if (OnPreLateUpdate != null && base.enabled)
				{
					OnPreLateUpdate();
				}
				OnLateUpdate();
				if (OnPostLateUpdate != null && base.enabled)
				{
					OnPostLateUpdate();
				}
			}
		}

		protected virtual void OnDrawGizmos()
		{
			if (initiated)
			{
				OnDrawGizmosBehaviour();
				if (OnPostDrawGizmos != null)
				{
					OnPostDrawGizmos();
				}
			}
		}

		protected void RotateTargetToRootMuscle()
		{
			Vector3 point = Quaternion.Inverse(puppetMaster.muscles[0].target.rotation) * puppetMaster.targetRoot.forward;
			Vector3 forward = puppetMaster.muscles[0].rigidbody.rotation * point;
			forward.y = 0f;
			puppetMaster.targetRoot.rotation = Quaternion.LookRotation(forward);
		}

		protected void TranslateTargetToRootMuscle(float maintainY)
		{
			Transform target = puppetMaster.muscles[0].target;
			Vector3 position = puppetMaster.muscles[0].transform.position;
			float x = position.x;
			Vector3 position2 = puppetMaster.muscles[0].transform.position;
			float y = position2.y;
			Vector3 position3 = puppetMaster.muscles[0].target.position;
			float y2 = Mathf.Lerp(y, position3.y, maintainY);
			Vector3 position4 = puppetMaster.muscles[0].transform.position;
			target.position = new Vector3(x, y2, position4.z);
		}

		protected void RemoveMusclesOfGroup(Muscle.Group group)
		{
			while (MusclesContainsGroup(group))
			{
				for (int i = 0; i < puppetMaster.muscles.Length; i++)
				{
					if (puppetMaster.muscles[i].props.group == group)
					{
						puppetMaster.RemoveMuscleRecursive(puppetMaster.muscles[i].joint, attachTarget: true);
						break;
					}
				}
			}
		}

		protected void GroundTarget(LayerMask layers)
		{
			Ray ray = new Ray(puppetMaster.targetRoot.position + puppetMaster.targetRoot.up, -puppetMaster.targetRoot.up);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 4f, layers))
			{
				puppetMaster.targetRoot.position = hitInfo.point;
			}
		}

		protected bool MusclesContainsGroup(Muscle.Group group)
		{
			Muscle[] muscles = puppetMaster.muscles;
			foreach (Muscle muscle in muscles)
			{
				if (muscle.props.group == group)
				{
					return true;
				}
			}
			return false;
		}
	}
}
