using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	public class PlayerDieEffect : AmplifyColorSupportEffect
	{
		private float _effectStartTime;

		private Vector3 _direction;

		private PlayerDieEffectParameters _parameters;

		private Transform _cameraHelper;

		private Transform _cameraTransform;

		private Transform _pelvislTransform;

		private Vector3 _pelvisPosition;

		private Camera _camera;

		protected override void Awake()
		{
			base.Awake();
			_parameters = ResourcesUtility.Load<PlayerDieEffectParameters>("Camera/PlayerDieEffectParameters");
			GameObject gameObject = new GameObject();
			gameObject.name = "PlayerDieEffectHelper";
			GameObject gameObject2 = gameObject;
			_cameraHelper = gameObject2.transform;
			_camera = this.GetRequireComponent<Camera>();
		}

		protected override RuntimeAnimatorController GetRuntimeAnimatorController()
		{
			return _parameters.AnimatorController;
		}

		protected override Texture GetLutBlendTexture()
		{
			return _parameters.LutBlendTexture;
		}

		public IEnumerator PlayEffect([NotNull] BipedMap character, [NotNull] Transform killerTransform)
		{
			if (character == null)
			{
				throw new ArgumentNullException("character");
			}
			if (killerTransform == null)
			{
				throw new ArgumentNullException("killerTransform");
			}
			_pelvislTransform = character.Hip;
			Vector3 dir = -(killerTransform.position - _pelvislTransform.position);
			dir.y = 0f;
			_direction = dir.normalized;
			Transform cameraTransform = _camera.transform;
			cameraTransform.SetParent(_cameraHelper);
			cameraTransform.localEulerAngles = Vector3.zero;
			cameraTransform.localPosition = Vector3.zero;
			_cameraTransform = cameraTransform;
			_effectStartTime = Time.time;
			_cameraHelper.rotation = Quaternion.LookRotation(-_direction);
			_cameraHelper.position = _pelvislTransform.position;
			_cameraHelper.position += _cameraHelper.TransformDirection(_parameters.InitialOffset);
			_pelvisPosition = _pelvislTransform.position;
			yield return EffectLoop();
		}

		private IEnumerator EffectLoop()
		{
			while (Time.time - _effectStartTime < _parameters.EffectDuration)
			{
				_pelvisPosition = Vector3.Lerp(_pelvisPosition, _pelvislTransform.position, Time.deltaTime * 20f);
				float dCoeff = (Time.time - _effectStartTime) / _parameters.EffectDuration;
				Vector3 cameraAngles = new Vector3(_parameters.XAxisRotationCurve.Evaluate(dCoeff), 0f, 0f);
				_cameraTransform.localEulerAngles = cameraAngles;
				_cameraHelper.position = _pelvisPosition + _direction * _parameters.DistancingCurve.Evaluate(dCoeff) + Vector3.up * _parameters.YAxisProjectionCurve.Evaluate(dCoeff);
				yield return new WaitForFixedUpdate();
			}
		}
	}
}
