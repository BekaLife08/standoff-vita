using UnityEngine;

namespace StandOff.Debug.Player
{
	public class PlayerFollowCamera : MonoBehaviour
	{
		public enum RotationType
		{
			FixedRotation = 1,
			FreeRotation,
			ThirdPersonControl
		}

		public enum FollowType
		{
			Fixed,
			Lerped
		}

		public RotationType rotationType = RotationType.FixedRotation;

		public FollowType followType = FollowType.Lerped;

		public float followSpeed_l;

		public float distance;

		public float borderRadius;

		public float sensitivityX = 1f;

		public float sensitivityY = 1f;

		private Vector3 _deltaOffsetPos = Vector3.zero;

		private Transform _playerTransform;

		private Transform _cameraTransform;

		private void Awake()
		{
			_cameraTransform = base.transform;
		}

		public void SetFollowType(int type)
		{
		}

		public void SetCamera(GameObject cameraGO)
		{
			_cameraTransform = cameraGO.transform;
		}

		public void SetPlayerTransform(Transform plTransform)
		{
			_playerTransform = plTransform;
			base.enabled = true;
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (rotationType == RotationType.FixedRotation)
			{
			}
			if (rotationType == RotationType.FreeRotation)
			{
				Vector3 eulerAngles = _cameraTransform.eulerAngles;
				float num = eulerAngles.y + UnityEngine.Input.GetAxis("Mouse X") * sensitivityY;
				Vector3 eulerAngles2 = _cameraTransform.eulerAngles;
				float num2 = 0f - eulerAngles2.x;
				if (num < -180f)
				{
					num += 360f;
				}
				num2 += UnityEngine.Input.GetAxis("Mouse Y") * sensitivityX;
				if (num2 < -180f)
				{
					num2 += 360f;
				}
				_cameraTransform.eulerAngles = new Vector3(0f - num2, num, 0f);
			}
			if (followType == FollowType.Fixed)
			{
			}
			if (followType == FollowType.Lerped)
			{
				_deltaOffsetPos = Vector3.Lerp(_deltaOffsetPos, _playerTransform.position, Time.deltaTime * followSpeed_l);
				if ((_deltaOffsetPos - _playerTransform.position).magnitude > borderRadius)
				{
					Vector3 vector = _deltaOffsetPos - _playerTransform.position;
					_deltaOffsetPos = _playerTransform.position + vector.normalized * borderRadius;
				}
				Vector3 b = _cameraTransform.forward * distance;
				_cameraTransform.position = _deltaOffsetPos - b + Vector3.up * 2f;
			}
		}
	}
}
