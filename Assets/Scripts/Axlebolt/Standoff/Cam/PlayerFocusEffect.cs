using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Cam
{
	public class PlayerFocusEffect : MonoBehaviour
	{
		private PlayerFocusEffectParameters _parameters;

		private Transform _cameraHelper;

		private Transform _cameraTransform;

		private Transform _characterTransform;

		private RawImage _screenshotBackground;

		private float _effectStartTime;

		private float _effectDuration;

		private float _screenshotTime;

		private Vector3 _cameraInitialLocalAngles;

		private Vector3 _cameraStartPosition;

		private Camera _camera;

		private RawImage _screenshotSprite;

		private Texture2D _screenShot;

		private void Awake()
		{
			_parameters = ResourcesUtility.Load<PlayerFocusEffectParameters>("Camera/PlayerFocusEffectParameters");
			GameObject gameObject = new GameObject();
			gameObject.name = "OnCharacterFocusCameraHelper";
			GameObject gameObject2 = gameObject;
			_cameraHelper = gameObject2.transform;
			_screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			_camera = this.GetRequireComponent<Camera>();
		}

		public IEnumerator PlayEffect([NotNull] Transform characterTransform, [NotNull] RawImage screenshotBackground)
		{
			if (characterTransform == null)
			{
				throw new ArgumentNullException("characterTransform");
			}
			if (screenshotBackground == null)
			{
				throw new ArgumentNullException("screenshotBackground");
			}
			_cameraTransform = _camera.transform;
			_characterTransform = characterTransform;
			_screenshotBackground = screenshotBackground;
			_effectStartTime = Time.time;
			Vector3 dir = characterTransform.position - _cameraTransform.position;
			_cameraHelper.position = _cameraTransform.position;
			_cameraHelper.rotation = Quaternion.LookRotation(dir);
			_cameraTransform.SetParent(_cameraHelper, worldPositionStays: true);
			_cameraInitialLocalAngles = _cameraTransform.localEulerAngles;
			_effectDuration = _parameters.DurationCurve.Evaluate(Vector3.Distance(_cameraTransform.position, characterTransform.position));
			_cameraStartPosition = _cameraHelper.position;
			yield return EffectLoop();
		}

		private IEnumerator EffectLoop()
		{
			while (Time.time - _effectStartTime <= _effectDuration)
			{
				float dTime = (Time.time - _effectStartTime) / _effectDuration;
				float distancingCoeff = _parameters.DistancingCurve.Evaluate(dTime);
				Vector3 characterPosition = _characterTransform.position + Vector3.up * _parameters.CharacterHeightOffset;
				Vector3 targetPosition = characterPosition + (_cameraHelper.transform.position - characterPosition).normalized * _parameters.CharacterForwardOffset;
				_cameraHelper.position = Vector3.Lerp(_cameraStartPosition, targetPosition, distancingCoeff);
				Vector3 dir = (characterPosition - _cameraHelper.position).normalized;
				_cameraHelper.rotation = Quaternion.Lerp(_cameraHelper.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15f);
				float localAngleCorrectCoeff = _parameters.CameraLocalAnglesCorrectingCurve.Evaluate(dTime);
				_cameraTransform.localEulerAngles = VectorAngle.LerpEulerAngle(_cameraInitialLocalAngles, Vector3.zero, localAngleCorrectCoeff);
				yield return null;
			}
			TakeScreenshot();
		}

		private void TakeScreenshot()
		{
			int width = Screen.width;
			int height = Screen.height;
			RenderTexture renderTexture = new RenderTexture(width, height, 24);
			_camera.targetTexture = renderTexture;
			_camera.Render();
			RenderTexture.active = renderTexture;
			_screenShot.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			_camera.targetTexture = null;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(renderTexture);
			_screenShot.Apply();
			_screenshotBackground.texture = _screenShot;
			_screenshotBackground.gameObject.SetActive(value: true);
			_screenshotBackground.enabled = true;
		}

		public void FinishFocusing()
		{
			_screenshotBackground.gameObject.SetActive(value: false);
		}
	}
}
