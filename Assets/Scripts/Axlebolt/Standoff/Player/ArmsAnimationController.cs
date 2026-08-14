using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.State;
using Axlebolt.Standoff.Player.Weaponry;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(MecanimController))]
	[RequireComponent(typeof(PlayerController))]
	[RequireComponent(typeof(MovementController))]
	public class ArmsAnimationController : MonoBehaviour, IMovementEvents, IAimingEvents, IInventoryEvents
	{
		public enum JLStates
		{
			Jumped,
			Landed,
			NotStated
		}

		public enum FootstepTraceState
		{
			InProgress,
			InDeadzone,
			NotStated,
			Starting
		}

		public enum FootstepTraceStateFPS
		{
			NotStated,
			Starting,
			Tracing
		}

		[Serializable]
		public class OffsetPoints
		{
			public TransformTRS centerTR;

			public Transform[] point;

			public TransformTRS[] pointTR;
		}

		[Serializable]
		public class ArmsOffsetInfo
		{
			[HideInInspector]
			public Vector3 curAngularSpeed = Vector3.zero;

			[HideInInspector]
			public Vector3 prevAngles = Vector3.zero;

			public AnimationCurve offsetCoeffCurve;

			public float relativeSpeed;

			[HideInInspector]
			public float curVer;

			[HideInInspector]
			public float curHor;

			[HideInInspector]
			public float curMov;

			[HideInInspector]
			public TransformTR curFPSCamTR = new TransformTR();

			[HideInInspector]
			public float timeJumpStarted;

			public AnimationCurve jumpAnimCurve;

			public float jumpDuration;

			[HideInInspector]
			public float timeLandStarted;

			public AnimationCurve landAnimCurve;

			public float landDuration;

			public AnimationCurve FallDurationMultCurve;
		}

		[Serializable]
		public class FootstepTraceParameters
		{
			public float DeadzoneDuration;

			public float TracingStartMinSpeed;

			public AnimationCurve FootstepCycleSpeedCurve;

			public float FootstepCycleCorrectorSpeed;

			public AnimationCurve FootstepCycleSpeedCurveFPS;
		}

		[Serializable]
		public class ArmsAnimationParametres
		{
			public AnimationCurve XAxisAnimtion;

			public AnimationCurve YAxisAnimtion;

			public AnimationCurve ZAxisAnimtion;

			public AnimationCurve ArmsShakingMultiplier;
		}

		public OffsetPoints offsetPoints;

		public ArmsOffsetInfo armsOffsetInfo;

		public Transform fpsDirective;

		public Transform fpsCamera;

		public int targetPointNo = -1;

		public bool resetToCamPos;

		public bool isTuningMode;

		[SerializeField]
		private FootstepTraceParameters _footstepTraceParameters;

		[SerializeField]
		private ArmsAnimationParametres _armsAnimationParametres;

		private PlayerController _playerController;

		private MecanimController _mecanimController;

		private MovementController _movementController;

		private BipedMap _bipedMap;

		private Transform _fpsCamPlaceholder;

		private JLStates _jlState = JLStates.NotStated;

		private float _fallDurationMult;

		private FootstepTraceState _footstepTraceState = FootstepTraceState.NotStated;

		private FootstepTraceStateFPS _footstepTraceStateFPS;

		private TransformTR _additiveCameraAnimation = new TransformTR();

		private float _footstepTraceStartValue;

		private float _footstepTraceDeadzoneEnterTime;

		public float _curFootstepTraceProgress;

		private float _footstepTracePrevValue;

		public float _curFootstepCycleProgress;

		public float _halfOffsetFootstepCycleProgress;

		private Vector3 _additiveArmsAnimation = Vector3.zero;

		private float _armsShakeWeaponMult;

		private float _armsShakeMult;

		private float _footstepTraceStartTimeFPS;

		private ViewMode _viewMode;

		public bool IsInitialized
		{
			get;
			private set;
		}

		public TransformTR AdditiveCameraAnimation
		{
			[CompilerGenerated]
			get
			{
				return _additiveCameraAnimation;
			}
		}

		public void PreInitialize()
		{
			base.enabled = false;
			IsInitialized = true;
			_playerController = GetComponent<PlayerController>();
			_playerController.CharacterSkinSetEvent += CharacterSkinSet;
			_mecanimController = _playerController.MecanimController;
			_movementController = GetComponent<MovementController>();
			_bipedMap = _playerController.BipedMap;
			_fpsCamPlaceholder = new GameObject().transform;
			_fpsCamPlaceholder.SetParent(_bipedMap.Spine2);
			_fpsCamPlaceholder.name = "FPS cam placeholder";
			GetComponent<MecanimController>().onAnimatorUpdated += LocalLateUpdate;
			for (int i = 0; i < offsetPoints.point.Length - 1; i++)
			{
				VectorAngle.ApplyLocalTRTransform(offsetPoints.point[i], offsetPoints.pointTR[i]);
			}
			offsetPoints.pointTR = new TransformTRS[offsetPoints.point.Length];
			int num = 0;
			Transform[] point = offsetPoints.point;
			foreach (Transform transform in point)
			{
				offsetPoints.pointTR[num] = new TransformTRS();
				offsetPoints.pointTR[num].pos = transform.localPosition;
				offsetPoints.pointTR[num].rot = transform.localEulerAngles;
				num++;
			}
		}

		public void Initialize()
		{
			armsOffsetInfo.prevAngles = fpsCamera.eulerAngles;
			_footstepTraceState = FootstepTraceState.NotStated;
			_footstepTraceStateFPS = FootstepTraceStateFPS.NotStated;
			_footstepTraceStartValue = 0f;
			_footstepTraceDeadzoneEnterTime = 0f;
			_curFootstepTraceProgress = 0f;
			_footstepTracePrevValue = 0f;
			_curFootstepCycleProgress = 0f;
			_halfOffsetFootstepCycleProgress = 0f;
			_additiveArmsAnimation = Vector3.zero;
			_armsShakeWeaponMult = 0f;
			base.enabled = true;
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			_bipedMap = bipedMap;
			_fpsCamPlaceholder.SetParent(bipedMap.Spine2, worldPositionStays: false);
		}

		private void CalculateParameters()
		{
			Vector3 zero = Vector3.zero;
			zero = VectorAngle.DeltaAngle3(fpsDirective.eulerAngles, armsOffsetInfo.prevAngles) / Time.deltaTime;
			armsOffsetInfo.curAngularSpeed = Vector3.Lerp(armsOffsetInfo.curAngularSpeed, zero, Time.deltaTime * 15f);
			float f = armsOffsetInfo.curAngularSpeed.y / armsOffsetInfo.relativeSpeed;
			float f2 = armsOffsetInfo.curAngularSpeed.x / armsOffsetInfo.relativeSpeed;
			Vector3 velocity = _movementController.Velocity;
			velocity.y = 0f;
			float b = velocity.magnitude / 4f;
			f = armsOffsetInfo.offsetCoeffCurve.Evaluate(Mathf.Abs(f)) * Mathf.Sign(f);
			f2 = armsOffsetInfo.offsetCoeffCurve.Evaluate(Mathf.Abs(f2)) * Mathf.Sign(f2);
			if (_jlState == JLStates.Jumped)
			{
				float num = Time.time - armsOffsetInfo.timeJumpStarted;
				f2 -= CurvedValue.GetCurveValue(armsOffsetInfo.jumpAnimCurve, armsOffsetInfo.jumpDuration, armsOffsetInfo.timeJumpStarted, Time.time);
				if (num > armsOffsetInfo.jumpDuration)
				{
					_jlState = JLStates.NotStated;
				}
			}
			if (_jlState == JLStates.Landed)
			{
				float num2 = Time.time - armsOffsetInfo.timeLandStarted;
				f2 -= CurvedValue.GetCurveValue(armsOffsetInfo.landAnimCurve, armsOffsetInfo.landDuration, armsOffsetInfo.timeLandStarted, Time.time) * armsOffsetInfo.FallDurationMultCurve.Evaluate(_fallDurationMult);
				if (num2 > armsOffsetInfo.landDuration)
				{
					_jlState = JLStates.NotStated;
				}
			}
			Vector3 rot = offsetPoints.pointTR[0].rot;
			Vector3 pos = offsetPoints.pointTR[0].pos;
			armsOffsetInfo.curHor = Mathf.Lerp(armsOffsetInfo.curHor, f, Time.deltaTime * 10f);
			armsOffsetInfo.curVer = Mathf.Lerp(armsOffsetInfo.curVer, f2, Time.deltaTime * 10f);
			armsOffsetInfo.curMov = Mathf.Lerp(armsOffsetInfo.curMov, b, Time.deltaTime * 13f);
			f = armsOffsetInfo.curHor;
			f2 = armsOffsetInfo.curVer;
			pos.z = Mathf.LerpAngle(offsetPoints.pointTR[0].pos.z, offsetPoints.pointTR[5].pos.z, armsOffsetInfo.curMov);
			if (f > 0f)
			{
				rot.y = Mathf.LerpAngle(offsetPoints.pointTR[0].rot.y, offsetPoints.pointTR[2].rot.y, f);
				pos.x = Mathf.LerpAngle(offsetPoints.pointTR[0].pos.x, offsetPoints.pointTR[2].pos.x, f);
			}
			else
			{
				rot.y = Mathf.LerpAngle(offsetPoints.pointTR[0].rot.y, offsetPoints.pointTR[4].rot.y, 0f - f);
				pos.x = Mathf.LerpAngle(offsetPoints.pointTR[0].pos.x, offsetPoints.pointTR[4].pos.x, 0f - f);
			}
			if (f2 > 0f)
			{
				rot.x = Mathf.LerpAngle(offsetPoints.pointTR[0].rot.x, offsetPoints.pointTR[3].rot.x, f2);
				pos.y = Mathf.LerpAngle(offsetPoints.pointTR[0].pos.y, offsetPoints.pointTR[3].pos.y, f2);
			}
			else
			{
				rot.x = Mathf.LerpAngle(offsetPoints.pointTR[0].rot.x, offsetPoints.pointTR[1].rot.x, 0f - f2);
				pos.y = Mathf.LerpAngle(offsetPoints.pointTR[0].pos.y, offsetPoints.pointTR[1].pos.y, 0f - f2);
			}
			Vector3 a = pos - offsetPoints.pointTR[0].pos;
			Vector3 rot2 = rot - offsetPoints.pointTR[0].rot;
			_additiveCameraAnimation.pos = a + _additiveArmsAnimation;
			_additiveCameraAnimation.rot = rot2;
			fpsCamera.position = _fpsCamPlaceholder.position;
			fpsCamera.rotation = _fpsCamPlaceholder.rotation;
			armsOffsetInfo.prevAngles = fpsDirective.eulerAngles;
		}

		public void SetFPSCameraPosition(TransformTR cameraDirectorLocalTR)
		{
			_fpsCamPlaceholder.localPosition = cameraDirectorLocalTR.pos;
			_fpsCamPlaceholder.localEulerAngles = cameraDirectorLocalTR.rot;
			fpsCamera.position = _fpsCamPlaceholder.position;
			fpsCamera.rotation = _fpsCamPlaceholder.rotation;
		}

		public TransformTR GetCameraDirectorLocalTR()
		{
			_fpsCamPlaceholder.position = fpsCamera.position;
			_fpsCamPlaceholder.rotation = fpsCamera.rotation;
			TransformTR transformTR = new TransformTR();
			transformTR.pos = _fpsCamPlaceholder.localPosition;
			transformTR.rot = _fpsCamPlaceholder.localEulerAngles;
			return transformTR;
		}

		private void FootstepTraceControl()
		{
			float currentSpeedXZ = _playerController.MovementController.translationData.currentSpeedXZ;
			float num = _curFootstepTraceProgress = _mecanimController.GetWalkAnimationProgress();
			if (currentSpeedXZ < _footstepTraceParameters.TracingStartMinSpeed)
			{
				_footstepTraceState = FootstepTraceState.NotStated;
			}
			else if (_footstepTraceState == FootstepTraceState.NotStated)
			{
				if (currentSpeedXZ > _footstepTraceParameters.TracingStartMinSpeed)
				{
					_footstepTraceState = FootstepTraceState.Starting;
					_footstepTraceStartValue = _curFootstepTraceProgress;
				}
			}
			else if (_footstepTraceState == FootstepTraceState.Starting)
			{
				if (_curFootstepTraceProgress > _footstepTraceStartValue)
				{
					_footstepTraceState = FootstepTraceState.InProgress;
				}
				else
				{
					_footstepTraceState = FootstepTraceState.InDeadzone;
					_footstepTraceDeadzoneEnterTime = Time.time;
					_curFootstepTraceProgress = 0f;
				}
				_footstepTracePrevValue = _curFootstepTraceProgress;
				StartFootstepTraceFollow();
			}
			else if (_footstepTraceState == FootstepTraceState.InProgress)
			{
				if (_footstepTracePrevValue > num)
				{
					_footstepTraceState = FootstepTraceState.InDeadzone;
					_footstepTraceDeadzoneEnterTime = Time.time;
				}
				_footstepTracePrevValue = num;
			}
			else if (_footstepTraceState == FootstepTraceState.InDeadzone && Time.time - _footstepTraceDeadzoneEnterTime > _footstepTraceParameters.DeadzoneDuration)
			{
				_footstepTraceState = FootstepTraceState.InProgress;
				_footstepTracePrevValue = _curFootstepTraceProgress;
			}
		}

		private void StartFootstepTraceFollow()
		{
			_curFootstepCycleProgress = _curFootstepTraceProgress;
			if (_curFootstepCycleProgress < 0.5f)
			{
				_halfOffsetFootstepCycleProgress = _curFootstepCycleProgress + 0.5f;
			}
			else
			{
				_halfOffsetFootstepCycleProgress = _curFootstepCycleProgress - 0.5f;
			}
		}

		private void FootstepTPSCycleControl()
		{
			FootstepTraceControl();
			if (_footstepTraceState == FootstepTraceState.InProgress || _footstepTraceState == FootstepTraceState.InDeadzone)
			{
				float currentSpeedXZ = _movementController.translationData.currentSpeedXZ;
				float num = _footstepTraceParameters.FootstepCycleSpeedCurve.Evaluate(currentSpeedXZ);
				float curFootstepCycleProgress = _curFootstepCycleProgress;
				_curFootstepCycleProgress += Time.deltaTime * num;
				if (_footstepTraceState == FootstepTraceState.InProgress)
				{
					float num2 = _curFootstepTraceProgress - _curFootstepCycleProgress;
					float b = Time.deltaTime * _footstepTraceParameters.FootstepCycleCorrectorSpeed;
					b = Mathf.Min(Math.Abs(num2), b) * Mathf.Sign(num2);
					_curFootstepCycleProgress += b;
				}
				_halfOffsetFootstepCycleProgress += _curFootstepCycleProgress - curFootstepCycleProgress;
				if (_curFootstepCycleProgress > 1f)
				{
					_curFootstepCycleProgress -= 1f;
					OnRightFootDown();
				}
				if (_halfOffsetFootstepCycleProgress > 1f)
				{
					_halfOffsetFootstepCycleProgress -= 1f;
					OnLeftFootDown();
				}
			}
			else
			{
				_curFootstepCycleProgress = (_halfOffsetFootstepCycleProgress = 0f);
			}
		}

		private void FootstepCycleControlFPS()
		{
			float currentSpeedXZ = _movementController.translationData.currentSpeedXZ;
			if (_footstepTraceStateFPS == FootstepTraceStateFPS.NotStated && currentSpeedXZ > _footstepTraceParameters.TracingStartMinSpeed)
			{
				_footstepTraceStartTimeFPS = Time.time;
				_footstepTraceStateFPS = FootstepTraceStateFPS.Tracing;
				_curFootstepCycleProgress = 0f;
				_halfOffsetFootstepCycleProgress = 0.5f;
			}
			if (_footstepTraceStateFPS == FootstepTraceStateFPS.Tracing)
			{
				float num = _footstepTraceParameters.FootstepCycleSpeedCurveFPS.Evaluate(currentSpeedXZ);
				_armsShakeMult = _armsAnimationParametres.ArmsShakingMultiplier.Evaluate(Time.time - _footstepTraceStartTimeFPS);
				_curFootstepCycleProgress += Time.deltaTime * num;
				_halfOffsetFootstepCycleProgress += Time.deltaTime * num;
				if (_curFootstepCycleProgress > 1f)
				{
					_curFootstepCycleProgress -= 1f;
					OnRightFootDown();
				}
				if (_halfOffsetFootstepCycleProgress > 1f)
				{
					_halfOffsetFootstepCycleProgress -= 1f;
					OnLeftFootDown();
				}
				if (currentSpeedXZ < _footstepTraceParameters.TracingStartMinSpeed)
				{
					_footstepTraceStateFPS = FootstepTraceStateFPS.NotStated;
				}
			}
		}

		private void OnRightFootDown()
		{
		}

		private void OnLeftFootDown()
		{
		}

		private void ArmsAnimationControl()
		{
			if (_footstepTraceStateFPS == FootstepTraceStateFPS.Tracing)
			{
				Vector3 a = default(Vector3);
				a.x = _armsAnimationParametres.XAxisAnimtion.Evaluate(_curFootstepCycleProgress) / 100f;
				a.y = _armsAnimationParametres.YAxisAnimtion.Evaluate(_curFootstepCycleProgress) / 100f;
				a.z = _armsAnimationParametres.ZAxisAnimtion.Evaluate(_curFootstepCycleProgress) / 100f;
				float d = _armsShakeWeaponMult * _armsShakeMult;
				_additiveArmsAnimation = Vector3.Lerp(_additiveArmsAnimation, a * d, Time.deltaTime * 20f);
			}
			else
			{
				_additiveArmsAnimation = Vector3.Lerp(_additiveArmsAnimation, Vector3.zero, Time.deltaTime * 20f);
			}
		}

		private void LocalLateUpdate()
		{
			if (_viewMode == ViewMode.TPS)
			{
				FootstepTPSCycleControl();
			}
			if (_viewMode == ViewMode.FPS)
			{
				FootstepCycleControlFPS();
				ArmsAnimationControl();
				CalculateParameters();
			}
		}

		public void OnViewModeSet(ViewMode viewMode)
		{
			_viewMode = viewMode;
		}

		public void OnMovementInit(MovementController movementController)
		{
		}

		public void OnJump(MovementController moveModuleInfo)
		{
			_jlState = JLStates.Jumped;
			armsOffsetInfo.timeJumpStarted = Time.time;
		}

		public void OnLand(MovementController moveModule, float fallDuration)
		{
			_jlState = JLStates.Landed;
			armsOffsetInfo.timeLandStarted = Time.time;
			_fallDurationMult = fallDuration;
		}

		public void OnCrouch(MovementController moveModule)
		{
		}

		public void OnStand(MovementController moveModule)
		{
		}

		public void OnWeaponSet(WeaponController weapon)
		{
			armsOffsetInfo.curFPSCamTR = weapon.AnimationParameters.fpsCamOffset.Clone();
			_fpsCamPlaceholder.localPosition = armsOffsetInfo.curFPSCamTR.pos;
			_fpsCamPlaceholder.localEulerAngles = armsOffsetInfo.curFPSCamTR.rot;
			_armsShakeWeaponMult = weapon.AnimationParameters.ArmsShakeMult;
		}

		public void OnNewWeaponSet(WeaponController weapon)
		{
			armsOffsetInfo.curFPSCamTR = weapon.AnimationParameters.fpsCamOffset.Clone();
			_fpsCamPlaceholder.localPosition = armsOffsetInfo.curFPSCamTR.pos;
			_fpsCamPlaceholder.localEulerAngles = armsOffsetInfo.curFPSCamTR.rot;
		}

		public void OnDropWeapon(WeaponController weapon)
		{
		}
	}
}
