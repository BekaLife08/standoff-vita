using System;
using UnityEngine;

namespace RootMotion.Dynamics
{
	[Serializable]
	public class SubBehaviourCOM : SubBehaviourBase
	{
		[Serializable]
		public enum Mode
		{
			FeetCentroid,
			CenterOfPressure
		}

		public Mode mode;

		public float velocityDamper = 1f;

		public float velocityLerpSpeed = 5f;

		public float velocityMax = 1f;

		public float centerOfPressureSpeed = 5f;

		public Vector3 offset;

		private LayerMask groundLayers;

		public Vector3 position
		{
			get;
			private set;
		}

		public Vector3 direction
		{
			get;
			private set;
		}

		public float angle
		{
			get;
			private set;
		}

		public Vector3 velocity
		{
			get;
			private set;
		}

		public Vector3 centerOfPressure
		{
			get;
			private set;
		}

		public Quaternion rotation
		{
			get;
			private set;
		}

		public Quaternion inverseRotation
		{
			get;
			private set;
		}

		public bool isGrounded
		{
			get;
			private set;
		}

		public bool[] groundContacts
		{
			get;
			private set;
		}

		public Vector3[] groundContactPoints
		{
			get;
			private set;
		}

		public float lastGroundedTime
		{
			get;
			private set;
		}

		public void Initiate(BehaviourBase behaviour, LayerMask groundLayers)
		{
			base.behaviour = behaviour;
			this.groundLayers = groundLayers;
			rotation = Quaternion.identity;
			groundContacts = new bool[behaviour.puppetMaster.muscles.Length];
			groundContactPoints = new Vector3[groundContacts.Length];
			behaviour.OnPreActivate = (BehaviourBase.BehaviourDelegate)Delegate.Combine(behaviour.OnPreActivate, new BehaviourBase.BehaviourDelegate(OnPreActivate));
			behaviour.OnPreLateUpdate = (BehaviourBase.BehaviourDelegate)Delegate.Combine(behaviour.OnPreLateUpdate, new BehaviourBase.BehaviourDelegate(OnPreLateUpdate));
			behaviour.OnPreDisable = (BehaviourBase.BehaviourDelegate)Delegate.Combine(behaviour.OnPreDisable, new BehaviourBase.BehaviourDelegate(OnPreDisable));
			behaviour.OnPreMuscleCollision = (BehaviourBase.CollisionDelegate)Delegate.Combine(behaviour.OnPreMuscleCollision, new BehaviourBase.CollisionDelegate(OnPreMuscleCollision));
			behaviour.OnPreMuscleCollisionExit = (BehaviourBase.CollisionDelegate)Delegate.Combine(behaviour.OnPreMuscleCollisionExit, new BehaviourBase.CollisionDelegate(OnPreMuscleCollisionExit));
		}

		private void OnPreMuscleCollision(MuscleCollision c)
		{
			if (LayerMaskExtensions.Contains(groundLayers, c.collision.gameObject.layer) && c.collision.contacts.Length != 0)
			{
				lastGroundedTime = Time.time;
				groundContacts[c.muscleIndex] = true;
				if (mode == Mode.CenterOfPressure)
				{
					groundContactPoints[c.muscleIndex] = GetCollisionCOP(c.collision);
				}
			}
		}

		private void OnPreMuscleCollisionExit(MuscleCollision c)
		{
			if (LayerMaskExtensions.Contains(groundLayers, c.collision.gameObject.layer))
			{
				groundContacts[c.muscleIndex] = false;
				groundContactPoints[c.muscleIndex] = Vector3.zero;
			}
		}

		private void OnPreActivate()
		{
			position = GetCenterOfMass();
			centerOfPressure = GetFeetCentroid();
			direction = position - centerOfPressure;
			angle = Vector3.Angle(direction, Vector3.up);
			velocity = Vector3.zero;
		}

		private void OnPreLateUpdate()
		{
			isGrounded = IsGrounded();
			if (mode == Mode.FeetCentroid || !isGrounded)
			{
				centerOfPressure = GetFeetCentroid();
			}
			else
			{
				Vector3 vector = (!isGrounded) ? GetFeetCentroid() : GetCenterOfPressure();
				centerOfPressure = ((!(centerOfPressureSpeed <= 2f)) ? Vector3.Lerp(centerOfPressure, vector, Time.deltaTime * centerOfPressureSpeed) : vector);
			}
			position = GetCenterOfMass();
			Vector3 centerOfMassVelocity = GetCenterOfMassVelocity();
			Vector3 vector2 = centerOfMassVelocity - position;
			vector2.y = 0f;
			vector2 = Vector3.ClampMagnitude(vector2, velocityMax);
			velocity = ((!(velocityLerpSpeed <= 0f)) ? Vector3.Lerp(velocity, vector2, Time.deltaTime * velocityLerpSpeed) : vector2);
			position += velocity * velocityDamper;
			position += behaviour.puppetMaster.targetRoot.rotation * offset;
			direction = position - centerOfPressure;
			rotation = Quaternion.FromToRotation(Vector3.up, direction);
			inverseRotation = Quaternion.Inverse(rotation);
			angle = Quaternion.Angle(Quaternion.identity, rotation);
		}

		private void OnPreDisable()
		{
			velocity = Vector3.zero;
		}

		private Vector3 GetCollisionCOP(Collision collision)
		{
			Vector3 a = Vector3.zero;
			for (int i = 0; i < collision.contacts.Length; i++)
			{
				a += collision.contacts[i].point;
			}
			return a / collision.contacts.Length;
		}

		private bool IsGrounded()
		{
			for (int i = 0; i < groundContacts.Length; i++)
			{
				if (groundContacts[i])
				{
					return true;
				}
			}
			return false;
		}

		private Vector3 GetCenterOfMass()
		{
			Vector3 a = Vector3.zero;
			float num = 0f;
			Muscle[] muscles = behaviour.puppetMaster.muscles;
			foreach (Muscle muscle in muscles)
			{
				a += muscle.rigidbody.worldCenterOfMass * muscle.rigidbody.mass;
				num += muscle.rigidbody.mass;
			}
			return a /= num;
		}

		private Vector3 GetCenterOfMassVelocity()
		{
			Vector3 a = Vector3.zero;
			float num = 0f;
			Muscle[] muscles = behaviour.puppetMaster.muscles;
			foreach (Muscle muscle in muscles)
			{
				a += muscle.rigidbody.worldCenterOfMass * muscle.rigidbody.mass;
				a += muscle.rigidbody.velocity * muscle.rigidbody.mass;
				num += muscle.rigidbody.mass;
			}
			return a /= num;
		}

		private Vector3 GetMomentum()
		{
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < behaviour.puppetMaster.muscles.Length; i++)
			{
				vector += behaviour.puppetMaster.muscles[i].rigidbody.velocity * behaviour.puppetMaster.muscles[i].rigidbody.mass;
			}
			return vector;
		}

		private Vector3 GetCenterOfPressure()
		{
			Vector3 a = Vector3.zero;
			int num = 0;
			for (int i = 0; i < groundContacts.Length; i++)
			{
				if (groundContacts[i])
				{
					a += groundContactPoints[i];
					num++;
				}
			}
			return a / num;
		}

		private Vector3 GetFeetCentroid()
		{
			Vector3 a = Vector3.zero;
			int num = 0;
			for (int i = 0; i < behaviour.puppetMaster.muscles.Length; i++)
			{
				if (behaviour.puppetMaster.muscles[i].props.group == Muscle.Group.Foot)
				{
					a += behaviour.puppetMaster.muscles[i].rigidbody.worldCenterOfMass;
					num++;
				}
			}
			return a / num;
		}
	}
}
