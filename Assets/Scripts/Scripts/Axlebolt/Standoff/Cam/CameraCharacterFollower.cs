using Axlebolt.Standoff.Controls;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	public class CameraCharacterFollower : MonoBehaviour
	{
		public const float HeightOffset = 1.5f;

		public const float MaxDistancing = 3f;

		public const float RotationSensitivity = 1f;

		private Transform _characterTransform;

		private void Awake()
		{
			base.enabled = false;
		}

		public void Follow(Transform characterTransform)
		{
			_characterTransform = characterTransform;
			base.transform.LookAt(_characterTransform.forward);
			base.enabled = true;
		}

		public void Stop()
		{
			_characterTransform = null;
			base.enabled = false;
		}

		private Vector2 GetCameraRotationAxis()
		{
			Vector2 zero = Vector2.zero;
			if (PlayerControls.Instance == null)
			{
				return zero;
			}
			return (!Application.isMobilePlatform) ? PlayerControls.Instance.GetKeyboardInputs().DeltaAimAngles : PlayerControls.Instance.GetTouchInputs().DeltaAimAngles;
		}

		private void RotateCamera(Vector2 rotationAxis)
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			float num = eulerAngles.y + rotationAxis.y;
			float num2 = 0f - eulerAngles.x;
			if (num < -180f)
			{
				num += 360f;
			}
			num2 += rotationAxis.x;
			if (num2 < -180f)
			{
				num2 += 360f;
			}
			num2 = Mathf.Clamp(num2, -60f, 60f);
			eulerAngles = new Vector3(0f - num2, num, 0f);
			base.transform.eulerAngles = eulerAngles;
		}

		private void Update()
		{
			if (!(_characterTransform == null))
			{
				RotateCamera(GetCameraRotationAxis());
				Vector3 vector = _characterTransform.position + Vector3.up * 1.5f;
				RaycastHit hitInfo;
				Vector3 position = (!Physics.Raycast(vector, -base.transform.forward, out hitInfo, 3f, 1)) ? (vector - base.transform.forward * 3f) : (hitInfo.point + base.transform.forward * 0.3f);
				base.transform.position = position;
			}
		}
	}
}
