using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class AimView : HudComponentView
	{
		[SerializeField]
		private RectTransform _interactiveArea;

		[SerializeField]
		private RectTransform _topAccuracyPanel;

		[SerializeField]
		private RectTransform _topRecoilPanel;

		[SerializeField]
		private RectTransform _bottomAccuracyPanel;

		[SerializeField]
		private RectTransform _bottomRecoilPanel;

		[SerializeField]
		private RectTransform _rightAccuracyPanel;

		[SerializeField]
		private RectTransform _rightRecoilPanel;

		[SerializeField]
		private RectTransform _leftAccuracyPanel;

		[SerializeField]
		private RectTransform _leftRecoilPanel;

		[SerializeField]
		private RectTransform _centerPoint;

		[SerializeField]
		private GameObject _defaultSight;

		[SerializeField]
		private GameObject _sniperSight;

		[SerializeField]
		private GameObject _collimatorSight;

		[SerializeField]
		private CollimatorSightViewParams _collimatorSightViewParams;

		[SerializeField]
		private List<Image> _collimatorBackgrounds;

		[SerializeField]
		private Image _collimatorSightPoint;

		[SerializeField]
		private Image _sniperCrossLineHor;

		[SerializeField]
		private Image _sniperCrossLineVer;

		[SerializeField]
		private RectTransform _sniperCrossBlurHor;

		[SerializeField]
		private RectTransform _sniperCrossBlurVer;

		[SerializeField]
		private AnimationCurve _sniperCrossLineFadeCurve;

		[SerializeField]
		private AnimationCurve _sniperCrossBlupShrinkCurve;

		[SerializeField]
		private float _sniperCrossBlurWidth;

		private Camera _camera;

		private DefaultSightType _defaultSightType;

		public override void SetPlayerController(PlayerController playerController)
		{
			base.gameObject.SetActive(playerController != null && playerController.PhotonView.isMine);
		}

		public override void UpdateView(PlayerController playerController)
		{
			if (!(playerController.WeaponryController.CurrentWeapon != null))
			{
				return;
			}
			WeaponController currentWeapon = playerController.WeaponryController.CurrentWeapon;
			if (currentWeapon is GunController)
			{
				GunController gunController = (GunController)currentWeapon;
				if (gunController.GunParameters.SightType == SightType.Default)
				{
					DrawDefaultRecoilLines(currentWeapon);
				}
				if (gunController.GunParameters.SightType == SightType.SniperScope)
				{
					DrawSniperScope(gunController);
				}
				if (gunController.GunParameters.SightType == SightType.CollimatorSight)
				{
					if (gunController.CurrentAimingMode == GunController.AimingMode.NotAiming)
					{
						DrawDefaultRecoilLines(currentWeapon);
					}
					else
					{
						DrawCollimatorSightView(gunController);
					}
				}
			}
			if (currentWeapon is KnifeController)
			{
				DrawDefaultRecoilLines(currentWeapon);
			}
		}

		private void DrawDefaultRecoilLines(WeaponController weaponController)
		{
			_sniperSight.SetActive(value: false);
			_defaultSight.SetActive(value: true);
			_collimatorSight.SetActive(value: false);
			if (_defaultSightType != DefaultSightType.ClassicStatic)
			{
				AccuracyData accuracyData = weaponController.GetAccuracyData();
				if (_camera == null)
				{
					_camera = Camera.main;
				}
				Vector3 position = Quaternion.AngleAxis(0f - accuracyData.AccuracyAngle, _camera.transform.right) * _camera.transform.forward + _camera.transform.position;
				Vector3 position2 = Quaternion.AngleAxis(0f - accuracyData.RecoilAngle, _camera.transform.right) * _camera.transform.forward + _camera.transform.position;
				Vector3 b = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
				position = _camera.WorldToScreenPoint(position) - b;
				position2 = _camera.WorldToScreenPoint(position2) - b;
				float num = _interactiveArea.rect.yMax - _interactiveArea.rect.yMin;
				float num2 = position.y / (float)Screen.height;
				float num3 = position2.y / (float)Screen.height;
				float num4 = num * num2;
				float a = num * num3;
				a = Mathf.Max(a, num4);
				_topAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
				_topRecoilPanel.localPosition = new Vector3(0f, a, 0f);
				_bottomAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
				_bottomRecoilPanel.localPosition = new Vector3(0f, a, 0f);
				_rightAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
				_rightRecoilPanel.localPosition = new Vector3(0f, a, 0f);
				_leftAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
				_leftRecoilPanel.localPosition = new Vector3(0f, a, 0f);
			}
		}

		public void SetDefaultSightType(DefaultSightType sightType)
		{
			_defaultSightType = sightType;
			_topRecoilPanel.gameObject.SetActive(sightType == DefaultSightType.DefaultDynamic);
			_bottomRecoilPanel.gameObject.SetActive(sightType == DefaultSightType.DefaultDynamic);
			_rightRecoilPanel.gameObject.SetActive(sightType == DefaultSightType.DefaultDynamic);
			_leftRecoilPanel.gameObject.SetActive(sightType == DefaultSightType.DefaultDynamic);
			_centerPoint.gameObject.SetActive(sightType != DefaultSightType.ClassicStatic);
		}

		public void SetSightColor(string htmlColor)
		{
			if (!string.IsNullOrEmpty(htmlColor))
			{
				if (!ColorUtility.TryParseHtmlString(htmlColor, out Color color))
				{
					UnityEngine.Debug.LogError($"Can't parse {htmlColor}");
					return;
				}
				_topAccuracyPanel.GetRequireComponent<Image>().color = color;
				_topRecoilPanel.GetRequireComponent<Image>().color = color;
				_bottomAccuracyPanel.GetRequireComponent<Image>().color = color;
				_bottomRecoilPanel.GetRequireComponent<Image>().color = color;
				_rightAccuracyPanel.GetRequireComponent<Image>().color = color;
				_rightRecoilPanel.GetRequireComponent<Image>().color = color;
				_leftAccuracyPanel.GetRequireComponent<Image>().color = color;
				_leftRecoilPanel.GetRequireComponent<Image>().color = color;
				_centerPoint.GetRequireComponent<Image>().color = color;
			}
		}

		private void DrawSniperScope(GunController gunController)
		{
			if (_defaultSight.activeSelf)
			{
				_defaultSight.SetActive(value: false);
			}
			_collimatorSight.SetActive(value: false);
			if (gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
			{
				if (!_sniperSight.activeSelf)
				{
					_sniperSight.SetActive(value: true);
				}
				float time = gunController.GetAccuracyData().AccuracyAngle / 1f;
				_sniperCrossBlurHor.sizeDelta = new Vector2(_sniperCrossBlurWidth * _sniperCrossBlupShrinkCurve.Evaluate(time), 1080f);
				_sniperCrossBlurVer.sizeDelta = new Vector2(_sniperCrossBlurWidth * _sniperCrossBlupShrinkCurve.Evaluate(time), 1080f);
				Color color = _sniperCrossLineHor.color;
				color.a = _sniperCrossLineFadeCurve.Evaluate(time);
				Image sniperCrossLineHor = _sniperCrossLineHor;
				Color color2 = color;
				_sniperCrossLineVer.color = color2;
				sniperCrossLineHor.color = color2;
			}
			if (gunController.CurrentAimingMode != 0 && _sniperSight.activeSelf)
			{
				_sniperSight.SetActive(value: false);
			}
		}

		private void SetCollimatorBackgroundAplha(float alpha)
		{
			foreach (Image collimatorBackground in _collimatorBackgrounds)
			{
				Color color = collimatorBackground.color;
				color.a = alpha / 255f;
				collimatorBackground.color = color;
			}
		}

		private void SetCollimatorSightpointParams(float alpha, float scale)
		{
			Color color = _collimatorSightPoint.color;
			color.a = alpha;
			_collimatorSightPoint.color = color;
			_collimatorSight.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1f);
		}

		private void DrawCollimatorSightView(GunController gunController)
		{
			_defaultSight.SetActive(value: false);
			_sniperSight.SetActive(value: false);
			_collimatorSight.SetActive(value: true);
			if (gunController.CurrentAimingMode == GunController.AimingMode.NotAiming)
			{
				SetCollimatorBackgroundAplha(0f);
				SetCollimatorSightpointParams(0f, 1f);
			}
			if (gunController.CurrentAimingMode == GunController.AimingMode.StartingAiming)
			{
				float num = _collimatorSightViewParams.BackgroundFadeInCurve.Evaluate(gunController.SightViewControl.Progress);
				float alpha = _collimatorSightViewParams.SightPointFadeInCurve.Evaluate(gunController.SightViewControl.Progress);
				SetCollimatorBackgroundAplha(num * _collimatorSightViewParams.BackgroundAlpha);
				SetCollimatorSightpointParams(alpha, 1f);
			}
			if (gunController.CurrentAimingMode == GunController.AimingMode.FinishingAiming)
			{
				float num2 = _collimatorSightViewParams.BackgroundFadeOutCurve.Evaluate(gunController.SightViewControl.Progress);
				SetCollimatorBackgroundAplha(num2 * _collimatorSightViewParams.BackgroundAlpha);
				float alpha2 = _collimatorSightViewParams.SightPointFadeInCurve.Evaluate(gunController.SightViewControl.Progress);
				SetCollimatorSightpointParams(alpha2, 1f);
			}
			if (gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
			{
				SetCollimatorBackgroundAplha(1f * _collimatorSightViewParams.BackgroundAlpha);
				SetCollimatorSightpointParams(1f, 1f);
			}
		}
	}
}
