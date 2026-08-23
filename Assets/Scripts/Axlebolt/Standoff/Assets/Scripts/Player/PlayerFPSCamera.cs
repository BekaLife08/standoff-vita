using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Inventory.Weapon;
using Axlebolt.Standoff.Player;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Assets.Scripts.Player
{
	public class PlayerFPSCamera : MonoBehaviour
	{
		private Camera _camera;

		private int _lastShotId = -1;

		private Vector2 _prevShotAnimaXYAxisDispertion;

		private Vector2 _curShotAnimaXYAxisDispertion;

		private float _curShotAnimProgress;

		private float _currentCycleOffset;

		private Vector3 _additiveShakeOffset;

		private Vector3 _currentShakeOffset = Vector3.zero;

		private Vector3 _prevCameraEulerAngles;

		private Vector3 _currentRotationDeviation = Vector3.zero;

		private Vector3 _defaultPosition = Vector3.zero;

		private Vector3 _defaultEulerAngles = Vector3.zero;

		public PlayerController PlayerController
		{
			get;
			private set;
		}

		public void AttachToPlayer(PlayerController playerController, Transform fpsCameraPlaceholder)
		{
			if (PlayerController != null)
			{
				DetachFromPlayer();
			}
			PlayerController = playerController;
			base.transform.SetParent(fpsCameraPlaceholder);
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			_lastShotId = -1;
			_prevCameraEulerAngles = base.transform.eulerAngles;
			if (_camera == null)
			{
				_camera = GetComponent<Camera>();
			}
			base.enabled = true;
		}

		public void DetachFromPlayer()
		{
			base.transform.SetParent(null);
			base.enabled = false;
			PlayerController.ResetFpsCamera();
			PlayerController = null;
		}

		private void SetCameraZeroParameters()
		{
			_camera.fieldOfView = 40f;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}

		private void ShakeAnimationControl(GunController gunController)
		{
			GunSightViewParameters gunSightViewParameters = gunController.GunParameters.GunSightViewParameters;
			float currentSpeedXZ = PlayerController.MovementController.translationData.currentSpeedXZ;
			float num = gunSightViewParameters.ShakeAnimationMultCurve.Evaluate(currentSpeedXZ);
			_currentCycleOffset += gunSightViewParameters.ShakeAnimationSpeedCurve.Evaluate(currentSpeedXZ) * Time.deltaTime;
			if (_currentCycleOffset > 1f)
			{
				_currentCycleOffset -= (float)Math.Truncate(_currentCycleOffset);
			}
			Vector3 zero = Vector3.zero;
			zero.x = (0f - gunSightViewParameters.XAxisShakeAnimationCurve.Evaluate(_currentCycleOffset)) * 0.01f * num;
			zero.y = (0f - gunSightViewParameters.YAxisShakeAnimationCurve.Evaluate(_currentCycleOffset)) * 0.01f * num;
			_currentShakeOffset = Vector3.Lerp(_currentShakeOffset, zero, Time.deltaTime * 15f);
			_additiveShakeOffset += _currentShakeOffset;
		}

		private void ShotAnimationControl(GunController gunController)
		{
			GunSightViewParameters gunSightViewParameters = gunController.GunParameters.GunSightViewParameters;
			if (gunController.ShotId != _lastShotId)
			{
				_prevShotAnimaXYAxisDispertion = _curShotAnimaXYAxisDispertion;
				_curShotAnimaXYAxisDispertion = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
				_lastShotId = gunController.ShotId;
			}
			_curShotAnimProgress = gunController.LocalTime - gunController.TimeFired;
			Vector3 zero = Vector3.zero;
			zero.x = gunSightViewParameters.XAxisShotAnimationCurve.Evaluate(_curShotAnimProgress) * _curShotAnimaXYAxisDispertion.x;
			zero.y = gunSightViewParameters.YAxisShotAnimationCurve.Evaluate(_curShotAnimProgress) * _curShotAnimaXYAxisDispertion.y;
			zero.z = gunSightViewParameters.ZAxisShotAnimationCurve.Evaluate(_curShotAnimProgress);
			Vector3 a = default(Vector3);
			a.x = gunSightViewParameters.XAxisShotAnimationCurve.Evaluate(_curShotAnimProgress) * _prevShotAnimaXYAxisDispertion.x;
			a.y = gunSightViewParameters.YAxisShotAnimationCurve.Evaluate(_curShotAnimProgress) * _prevShotAnimaXYAxisDispertion.y;
			a.z = gunSightViewParameters.ZAxisShotAnimationCurve.Evaluate(_curShotAnimProgress);
			float t = (gunController.LocalTime - gunController.TimeFired) / gunSightViewParameters.TransitionDuration;
			Vector3 vector = Vector3.Lerp(a, zero, t) * 0.01f;
			_additiveShakeOffset += vector;
		}

		private void RotationOffsetControl(GunController gunController)
		{
			GunSightViewParameters gunSightViewParameters = gunController.GunParameters.GunSightViewParameters;
			Vector3 vector = VectorAngle.DeltaAngle3(base.transform.eulerAngles, _prevCameraEulerAngles);
			float f = vector.x / Time.deltaTime;
			float f2 = vector.y / Time.deltaTime;
			_prevCameraEulerAngles = base.transform.eulerAngles;
			Vector3 zero = Vector3.zero;
			zero.y = (0f - gunSightViewParameters.RotationDeviationCurve.Evaluate(Mathf.Abs(f) / 360f)) / 100f * Mathf.Sign(f);
			zero.x = gunSightViewParameters.RotationDeviationCurve.Evaluate(Mathf.Abs(f2) / 360f) / 100f * Mathf.Sign(f2);
			_currentRotationDeviation = Vector3.Lerp(_currentRotationDeviation, zero, Time.deltaTime * 20f);
			_additiveShakeOffset += _currentRotationDeviation;
		}

		private void CollimatorSightControl(GunController gunController)
		{
			_additiveShakeOffset = Vector3.zero;
			ShakeAnimationControl(gunController);
			ShotAnimationControl(gunController);
			RotationOffsetControl(gunController);
			_camera.fieldOfView = gunController.SightViewControl.CameraFov;
			GunSightViewParameters gunSightViewParameters = gunController.GunParameters.GunSightViewParameters;
			float time = (gunController.SightViewControl.TargetState != GunSightViewControl.States.Default) ? gunController.SightViewControl.Progress : (1f - gunController.SightViewControl.Progress);
			float num = gunSightViewParameters.CameraTRNearingCurve.Evaluate(time);
			float num2 = gunSightViewParameters.CameraFovNearingCurve.Evaluate(time);
			if (gunController.SightViewControl.TargetState == GunSightViewControl.States.Default)
			{
				num2 = 1f - num2;
				num = 1f - num;
			}
			base.transform.localPosition = Vector3.Lerp(_defaultPosition, gunSightViewParameters.CameraNrearedTR.pos + _additiveShakeOffset, num);
			base.transform.localEulerAngles = Vector3.Lerp(_defaultEulerAngles, gunSightViewParameters.CameraNrearedTR.rot, num);
			_camera.fieldOfView = Mathf.Lerp(gunSightViewParameters.DefaultFov, gunSightViewParameters.AimingFov, num2);
		}

		private void DefaultSightControl()
		{
			base.transform.localPosition = _defaultPosition;
			base.transform.localEulerAngles = _defaultEulerAngles;
			_camera.fieldOfView = 40f;
		}

		private void Update()
		{
			_defaultPosition = Vector3.zero;
			_defaultEulerAngles = Vector3.zero;
			if (PlayerController != null)
			{
				_defaultEulerAngles += PlayerController.ArmsAnimController.AdditiveCameraAnimation.rot;
				_defaultPosition += PlayerController.ArmsAnimController.AdditiveCameraAnimation.pos;
			}
			if (PlayerController != null && PlayerController.WeaponryController.CurrentWeapon != null)
			{
				WeaponController currentWeapon = PlayerController.WeaponryController.CurrentWeapon;
				if (currentWeapon is GunController)
				{
					GunController gunController = (GunController)currentWeapon;
					if (gunController.GunParameters.SightType == SightType.CollimatorSight)
					{
						CollimatorSightControl(gunController);
					}
					else
					{
						DefaultSightControl();
					}
					return;
				}
				if (currentWeapon is KnifeController || currentWeapon is BombController)
				{
					DefaultSightControl();
					return;
				}
			}
			SetCameraZeroParameters();
		}
	}
}
