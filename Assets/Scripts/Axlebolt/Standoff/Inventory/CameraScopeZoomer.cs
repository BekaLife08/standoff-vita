using Axlebolt.Common.States;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class CameraScopeZoomer : MonoBehaviour
	{
		public enum States
		{
			Zoomed,
			Zooming,
			NotZoomed
		}

		private readonly StateSimple<States> _zoomState = new StateSimple<States>();

		private PlayerController _playerController;

		private Camera _camera;

		private const float ZoomedFov = 20f;

		public PlayerController PlayerController
		{
			get
			{
				return _playerController;
			}
			set
			{
				_playerController = value;
				SetNotZoomed();
			}
		}

		private void Awake()
		{
			_camera = GetComponent<Camera>();
			SetNotZoomed();
		}

		public void SetNotZoomed()
		{
			if (Mathf.Abs(_camera.fieldOfView - 60f) > 0.001f)
			{
				_camera.fieldOfView = 60f;
			}
			_zoomState.SetState(States.NotZoomed, Time.time);
		}

		private void SetZooming(float targetFOV, float lerpSpeed)
		{
			if (Math.Abs(_camera.fieldOfView - 20f) > 0.01f)
			{
				_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 20f, Time.deltaTime * lerpSpeed);
			}
		}

		private void SetUnzooming()
		{
			if (Math.Abs(_camera.fieldOfView - 60f) > 0.01f)
			{
				_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60f, Time.deltaTime * 20f);
			}
		}

		private void Update()
		{
			if (PlayerController != null && PlayerController.WeaponryController.CurrentWeapon != null && PlayerController.WeaponryController.CurrentWeapon is GunController)
			{
				GunController gunController = (GunController)PlayerController.WeaponryController.CurrentWeapon;
				if (gunController.GunParameters.SightType == SightType.SniperScope && gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
				{
					SetZooming(20f, 20f);
					return;
				}
				if (gunController.GunParameters.SightType == SightType.CollimatorSight)
				{
					if (gunController.CurrentAimingMode == GunController.AimingMode.Aiming || gunController.CurrentAimingMode == GunController.AimingMode.StartingAiming)
					{
						SetZooming(30f, 17f);
						return;
					}
					if (gunController.CurrentAimingMode == GunController.AimingMode.FinishingAiming)
					{
						SetUnzooming();
						return;
					}
				}
			}
			SetNotZoomed();
		}
	}
}
