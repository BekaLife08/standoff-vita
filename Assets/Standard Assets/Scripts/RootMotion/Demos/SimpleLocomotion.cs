using System;
using UnityEngine;

namespace RootMotion.Demos
{
	[RequireComponent(typeof(Animator))]
	public class SimpleLocomotion : MonoBehaviour
	{
		[Serializable]
		public enum RotationMode
		{
			Smooth,
			Linear
		}

		[Tooltip("The component that updates the camera.")]
		[SerializeField]
		private CameraController cameraController;

		[Tooltip("Acceleration of movement.")]
		[SerializeField]
		private float accelerationTime = 0.2f;

		[Tooltip("Turning speed.")]
		[SerializeField]
		private float turnTime = 0.2f;

		[Tooltip("If true, will run on left shift, if not will walk on left shift.")]
		[SerializeField]
		private bool walkByDefault = true;

		[Tooltip("Smooth or linear rotation.")]
		[SerializeField]
		private RotationMode rotationMode;

		[Tooltip("Procedural motion speed (if not using root motion).")]
		[SerializeField]
		private float moveSpeed = 3f;

		private Animator animator;

		private float speed;

		private float angleVel;

		private float speedVel;

		private Vector3 linearTargetDirection;

		private CharacterController characterController;

		public bool isGrounded
		{
			get;
			private set;
		}

		private void Start()
		{
			animator = GetComponent<Animator>();
			characterController = GetComponent<CharacterController>();
			cameraController.enabled = false;
		}

		private void Update()
		{
			Vector3 position = base.transform.position;
			isGrounded = (position.y < 0.1f);
			Rotate();
			Move();
		}

		private void LateUpdate()
		{
			cameraController.UpdateInput();
			cameraController.UpdateTransform();
		}

		private void Rotate()
		{
			if (!isGrounded)
			{
				return;
			}
			Vector3 inputVector = GetInputVector();
			if (inputVector == Vector3.zero)
			{
				return;
			}
			Vector3 vector = base.transform.forward;
			switch (rotationMode)
			{
			case RotationMode.Smooth:
			{
				Vector3 vector2 = cameraController.transform.rotation * inputVector;
				float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				float target = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
				float angle = Mathf.SmoothDampAngle(current, target, ref angleVel, turnTime);
				base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
				break;
			}
			case RotationMode.Linear:
			{
				Vector3 inputVectorRaw = GetInputVectorRaw();
				if (inputVectorRaw != Vector3.zero)
				{
					linearTargetDirection = cameraController.transform.rotation * inputVectorRaw;
				}
				vector = Vector3.RotateTowards(vector, linearTargetDirection, Time.deltaTime * (1f / turnTime), 1f);
				vector.y = 0f;
				base.transform.rotation = Quaternion.LookRotation(vector);
				break;
			}
			}
		}

		private void Move()
		{
			float target = walkByDefault ? ((!Input.GetKey(KeyCode.LeftShift)) ? 0.5f : 1f) : ((!Input.GetKey(KeyCode.LeftShift)) ? 1f : 0.5f);
			speed = Mathf.SmoothDamp(speed, target, ref speedVel, accelerationTime);
			float num = GetInputVector().magnitude * speed;
			animator.SetFloat("Speed", num);
			if (!animator.hasRootMotion && isGrounded)
			{
				Vector3 a = base.transform.forward * num * moveSpeed;
				if (characterController != null)
				{
					characterController.SimpleMove(a);
				}
				else
				{
					base.transform.position += a * Time.deltaTime;
				}
			}
		}

		private Vector3 GetInputVector()
		{
			Vector3 result = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
			result.z += Mathf.Abs(result.x) * 0.05f;
			result.x -= Mathf.Abs(result.z) * 0.05f;
			return result;
		}

		private Vector3 GetInputVectorRaw()
		{
			return new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0f, UnityEngine.Input.GetAxisRaw("Vertical"));
		}
	}
}
