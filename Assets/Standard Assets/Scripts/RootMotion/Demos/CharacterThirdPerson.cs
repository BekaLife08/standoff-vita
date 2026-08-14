using System;
using UnityEngine;

namespace RootMotion.Demos
{
	public class CharacterThirdPerson : CharacterBase
	{
		[Serializable]
		public enum MoveMode
		{
			Directional,
			Strafe
		}

		public struct AnimState
		{
			public Vector3 moveDirection;

			public bool jump;

			public bool crouch;

			public bool onGround;

			public bool isStrafing;

			public float yVelocity;
		}

		[Header("References")]
		public CharacterAnimationBase characterAnimation;

		public UserControlThirdPerson userControl;

		public CameraController cam;

		[Header("Movement")]
		public MoveMode moveMode;

		public bool smoothPhysics = true;

		public float smoothAccelerationTime = 0.2f;

		public float linearAccelerationSpeed = 3f;

		public float platformFriction = 7f;

		public float groundStickyEffect = 4f;

		public float maxVerticalVelocityOnGround = 3f;

		public float velocityToGroundTangentWeight;

		[Header("Rotation")]
		public bool lookInCameraDirection;

		public float turnSpeed = 5f;

		public float stationaryTurnSpeedMlp = 1f;

		[Header("Jumping and Falling")]
		public float airSpeed = 6f;

		public float airControl = 2f;

		public float jumpPower = 12f;

		public float jumpRepeatDelayTime;

		[Header("Wall Running")]
		[SerializeField]
		private LayerMask wallRunLayers;

		public float wallRunMaxLength = 1f;

		public float wallRunMinMoveMag = 0.6f;

		public float wallRunMinVelocityY = -1f;

		public float wallRunRotationSpeed = 1.5f;

		public float wallRunMaxRotationAngle = 70f;

		public float wallRunWeightSpeed = 5f;

		[Header("Crouching")]
		public float crouchCapsuleScaleMlp = 0.6f;

		public AnimState animState = default(AnimState);

		protected Vector3 moveDirection;

		private Animator animator;

		private Vector3 normal;

		private Vector3 platformVelocity;

		private Vector3 platformAngularVelocity;

		private RaycastHit hit;

		private float jumpLeg;

		private float jumpEndTime;

		private float forwardMlp;

		private float groundDistance;

		private float lastAirTime;

		private float stickyForce;

		private Vector3 wallNormal = Vector3.up;

		private Vector3 moveDirectionVelocity;

		private float wallRunWeight;

		private float lastWallRunWeight;

		private Vector3 fixedDeltaPosition;

		private Quaternion fixedDeltaRotation;

		private bool fixedFrame;

		private float wallRunEndTime;

		private Vector3 gravity;

		private Vector3 verticalVelocity;

		private float velocityY;

		public bool onGround
		{
			get;
			private set;
		}

		protected override void Start()
		{
			base.Start();
			animator = GetComponent<Animator>();
			if (animator == null)
			{
				animator = characterAnimation.GetComponent<Animator>();
			}
			wallNormal = -gravity.normalized;
			onGround = true;
			animState.onGround = true;
			if (cam != null)
			{
				cam.enabled = false;
			}
		}

		private void OnAnimatorMove()
		{
			Move(animator.deltaPosition, animator.deltaRotation);
		}

		public override void Move(Vector3 deltaPosition, Quaternion deltaRotation)
		{
			fixedDeltaPosition += deltaPosition;
			fixedDeltaRotation *= deltaRotation;
		}

		private void FixedUpdate()
		{
			gravity = GetGravity();
			verticalVelocity = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
			velocityY = verticalVelocity.magnitude;
			if (Vector3.Dot(verticalVelocity, gravity) > 0f)
			{
				velocityY = 0f - velocityY;
			}
			if (animator != null && animator.updateMode == AnimatorUpdateMode.AnimatePhysics)
			{
				smoothPhysics = false;
				characterAnimation.smoothFollow = false;
			}
			r.interpolation = (smoothPhysics ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None);
			characterAnimation.smoothFollow = smoothPhysics;
			MoveFixed(fixedDeltaPosition);
			fixedDeltaPosition = Vector3.zero;
			base.transform.rotation *= fixedDeltaRotation;
			fixedDeltaRotation = Quaternion.identity;
			Rotate();
			GroundCheck();
			if (userControl.state.move == Vector3.zero && groundDistance < airborneThreshold * 0.5f)
			{
				HighFriction();
			}
			else
			{
				ZeroFriction();
			}
			if (gravityTarget != null)
			{
				r.useGravity = false;
				r.AddForce(gravity);
			}
			if (onGround)
			{
				animState.jump = Jump();
			}
			else
			{
				r.AddForce(gravity * gravityMultiplier);
			}
			ScaleCapsule((!userControl.state.crouch) ? 1f : crouchCapsuleScaleMlp);
			fixedFrame = true;
		}

		protected virtual void Update()
		{
			animState.onGround = onGround;
			animState.moveDirection = GetMoveDirection();
			animState.yVelocity = Mathf.Lerp(animState.yVelocity, velocityY, Time.deltaTime * 10f);
			animState.crouch = userControl.state.crouch;
			animState.isStrafing = (moveMode == MoveMode.Strafe);
		}

		protected virtual void LateUpdate()
		{
			if (!(cam == null))
			{
				cam.UpdateInput();
				if (fixedFrame || r.interpolation != 0)
				{
					cam.UpdateTransform((r.interpolation != 0) ? Time.deltaTime : Time.fixedDeltaTime);
					fixedFrame = false;
				}
			}
		}

		private void MoveFixed(Vector3 deltaPosition)
		{
			WallRun();
			Vector3 a = deltaPosition / Time.deltaTime;
			a += V3Tools.ExtractHorizontal(platformVelocity, gravity, 1f);
			if (onGround)
			{
				if (velocityToGroundTangentWeight > 0f)
				{
					Quaternion b = Quaternion.FromToRotation(base.transform.up, normal);
					a = Quaternion.Lerp(Quaternion.identity, b, velocityToGroundTangentWeight) * a;
				}
			}
			else
			{
				Vector3 b2 = V3Tools.ExtractHorizontal(userControl.state.move * airSpeed, gravity, 1f);
				a = Vector3.Lerp(r.velocity, b2, Time.deltaTime * airControl);
			}
			if (onGround && Time.time > jumpEndTime)
			{
				r.velocity -= base.transform.up * stickyForce * Time.deltaTime;
			}
			Vector3 vector = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
			Vector3 a2 = V3Tools.ExtractHorizontal(a, gravity, 1f);
			if (onGround && Vector3.Dot(vector, gravity) < 0f)
			{
				vector = Vector3.ClampMagnitude(vector, maxVerticalVelocityOnGround);
			}
			r.velocity = a2 + vector;
			float b3 = onGround ? GetSlopeDamper(-deltaPosition / Time.deltaTime, normal) : 1f;
			forwardMlp = Mathf.Lerp(forwardMlp, b3, Time.deltaTime * 5f);
		}

		private void WallRun()
		{
			bool flag = CanWallRun();
			if (wallRunWeight > 0f && !flag)
			{
				wallRunEndTime = Time.time;
			}
			if (Time.time < wallRunEndTime + 0.5f)
			{
				flag = false;
			}
			wallRunWeight = Mathf.MoveTowards(wallRunWeight, (!flag) ? 0f : 1f, Time.deltaTime * wallRunWeightSpeed);
			if (wallRunWeight <= 0f && lastWallRunWeight > 0f)
			{
				Vector3 forward = V3Tools.ExtractHorizontal(base.transform.forward, gravity, 1f);
				base.transform.rotation = Quaternion.LookRotation(forward, -gravity);
				wallNormal = -gravity.normalized;
			}
			lastWallRunWeight = wallRunWeight;
			if (!(wallRunWeight <= 0f))
			{
				if (onGround && velocityY < 0f)
				{
					r.velocity = V3Tools.ExtractHorizontal(r.velocity, gravity, 1f);
				}
				Vector3 vector = V3Tools.ExtractHorizontal(base.transform.forward, gravity, 1f);
				RaycastHit hitInfo = default(RaycastHit);
				hitInfo.normal = -gravity.normalized;
				Physics.Raycast((!onGround) ? capsule.bounds.center : base.transform.position, vector, out hitInfo, 3f, wallRunLayers);
				wallNormal = Vector3.Lerp(wallNormal, hitInfo.normal, Time.deltaTime * wallRunRotationSpeed);
				wallNormal = Vector3.RotateTowards(-gravity.normalized, wallNormal, wallRunMaxRotationAngle * ((float)Math.PI / 180f), 0f);
				Vector3 tangent = base.transform.forward;
				Vector3 vector2 = wallNormal;
				Vector3.OrthoNormalize(ref vector2, ref tangent);
				base.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(vector, -gravity), Quaternion.LookRotation(tangent, wallNormal), wallRunWeight);
			}
		}

		private bool CanWallRun()
		{
			if (Time.time < jumpEndTime - 0.1f)
			{
				return false;
			}
			if (Time.time > jumpEndTime - 0.1f + wallRunMaxLength)
			{
				return false;
			}
			if (velocityY < wallRunMinVelocityY)
			{
				return false;
			}
			if (userControl.state.move.magnitude < wallRunMinMoveMag)
			{
				return false;
			}
			return true;
		}

		private Vector3 GetMoveDirection()
		{
			switch (moveMode)
			{
			case MoveMode.Directional:
				moveDirection = Vector3.SmoothDamp(moveDirection, new Vector3(0f, 0f, userControl.state.move.magnitude), ref moveDirectionVelocity, smoothAccelerationTime);
				moveDirection = Vector3.MoveTowards(moveDirection, new Vector3(0f, 0f, userControl.state.move.magnitude), Time.deltaTime * linearAccelerationSpeed);
				return moveDirection * forwardMlp;
			case MoveMode.Strafe:
				moveDirection = Vector3.SmoothDamp(moveDirection, userControl.state.move, ref moveDirectionVelocity, smoothAccelerationTime);
				moveDirection = Vector3.MoveTowards(moveDirection, userControl.state.move, Time.deltaTime * linearAccelerationSpeed);
				return base.transform.InverseTransformDirection(moveDirection);
			default:
				return Vector3.zero;
			}
		}

		protected virtual void Rotate()
		{
			if (gravityTarget != null)
			{
				base.transform.rotation = Quaternion.FromToRotation(base.transform.up, base.transform.position - gravityTarget.position) * base.transform.rotation;
			}
			if (platformAngularVelocity != Vector3.zero)
			{
				base.transform.rotation = Quaternion.Euler(platformAngularVelocity) * base.transform.rotation;
			}
			float num = GetAngleFromForward(GetForwardDirection());
			if (userControl.state.move == Vector3.zero)
			{
				num *= (1.01f - Mathf.Abs(num) / 180f) * stationaryTurnSpeedMlp;
			}
			RigidbodyRotateAround(characterAnimation.GetPivotPoint(), base.transform.up, num * Time.deltaTime * turnSpeed);
		}

		private Vector3 GetForwardDirection()
		{
			bool flag = userControl.state.move != Vector3.zero;
			switch (moveMode)
			{
			case MoveMode.Directional:
				if (flag)
				{
					return userControl.state.move;
				}
				return (!lookInCameraDirection) ? base.transform.forward : (userControl.state.lookPos - r.position);
			case MoveMode.Strafe:
				if (flag)
				{
					return userControl.state.lookPos - r.position;
				}
				return (!lookInCameraDirection) ? base.transform.forward : (userControl.state.lookPos - r.position);
			default:
				return Vector3.zero;
			}
		}

		protected virtual bool Jump()
		{
			if (!userControl.state.jump)
			{
				return false;
			}
			if (userControl.state.crouch)
			{
				return false;
			}
			if (!characterAnimation.animationGrounded)
			{
				return false;
			}
			if (Time.time < lastAirTime + jumpRepeatDelayTime)
			{
				return false;
			}
			onGround = false;
			jumpEndTime = Time.time + 0.1f;
			Vector3 velocity = userControl.state.move * airSpeed;
			r.velocity = velocity;
			r.velocity += base.transform.up * jumpPower;
			return true;
		}

		private void GroundCheck()
		{
			Vector3 b = Vector3.zero;
			platformAngularVelocity = Vector3.zero;
			float num = 0f;
			hit = GetSpherecastHit();
			normal = base.transform.up;
			groundDistance = Vector3.Project(r.position - hit.point, base.transform.up).magnitude;
			if (Time.time > jumpEndTime && velocityY < jumpPower * 0.5f)
			{
				bool onGround = this.onGround;
				this.onGround = false;
				float num2 = onGround ? airborneThreshold : (airborneThreshold * 0.5f);
				float magnitude = V3Tools.ExtractHorizontal(r.velocity, gravity, 1f).magnitude;
				if (groundDistance < num2)
				{
					num = groundStickyEffect * magnitude * num2;
					if (hit.rigidbody != null)
					{
						b = hit.rigidbody.GetPointVelocity(hit.point);
						platformAngularVelocity = Vector3.Project(hit.rigidbody.angularVelocity, base.transform.up);
					}
					this.onGround = true;
				}
			}
			platformVelocity = Vector3.Lerp(platformVelocity, b, Time.deltaTime * platformFriction);
			stickyForce = num;
			if (!this.onGround)
			{
				lastAirTime = Time.time;
			}
		}
	}
}
