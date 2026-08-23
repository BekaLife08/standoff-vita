using Axlebolt.Common.States;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.Occlusion;
using Axlebolt.Standoff.Player.State;
using Axlebolt.Standoff.Player.Weaponry;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Aim
{
	[RequireComponent(typeof(PlayerController))]
	[RequireComponent(typeof(MecanimController))]
	[RequireComponent(typeof(MovementController))]
	public class AimController : BaseController<AimSnapshot, AimControlCmd>, IInventoryEvents, IMovementEvents, IAimingEvents, IMecanimEvents
	{
		public enum ObservationState
		{
			Invisible,
			Visible,
			Disabled
		}

		public enum WeaponOffsetState
		{
			NotStated,
			Translation
		}

		public enum MoveState
		{
			NotStated,
			Crouch,
			Stand,
			FinishedCrouch,
			FinishedStand
		}

		public delegate void AimingModeEvets(ViewMode viewMode);

		[Serializable]
		public class AimingParameters
		{
			[Serializable]
			public class GeneralData
			{
				public CurvedValue curve01;

				public CurvedValue curve00;

				public CurvedValue curve11;
			}

			public TransformTRS FPSOffsetStand;

			public TransformTRS FPSOffsetCrouch;

			public AnimationCurve FPSOffsetCurveSC;

			public AnimationCurve FPSOffsetCurveCS;

			public CurvedValue FPSOffsetCurveStandToCrouch;

			public CurvedValue FPSOffsetCurveCrouchToStand;

			public float FPSTDuration;

			public CurvedValue fpsAdditiveOffsetOnLand;

			public AnimationCurve weaponPHTCurve;

			public float weaponPHTDuration;

			public AnimationCurve weaponMTCurve;

			public AnimationCurve weaponMTCurveOpp;

			public float weaponMTDuration;

			public GeneralData generalData;

			public TransformTRS defaultWeaponOffset;

			public Vector3 defaultHeadRotation;

			public Vector3 headOffset;

			public float verticalClamp;

			public float horizontalClamp;
		}

		public class InterpolatorsBunch
		{
			public ValueBlender additiveFPSOffset = new ValueBlender();

			public ValueBlender FPSOffset = new ValueBlender();
		}

		[Serializable]
		public class TuningParams
		{
			public bool disableSpinesRotation;
		}

		private ObservationState _observationState;

		public const string tagNetwork = "AMM";

		public float sensitivityX;

		public float sensitivityY;

		public float minimumX;

		public float maximumX;

		public GameObject FPSP_go;

		public Transform spineDirector;

		public Transform FPSCamera;

		private Transform _relative;

		public Transform camTransform;

		public AimingParameters aimingParameters;

		public AimingData aimingData = new AimingData();

		public InterpolatorsBunch interpolatorsBunch = new InterpolatorsBunch();

		public IAimingEvents[] _aimingEventsImpl;

		public TuningParams tuningParams;

		private PlayerController _playerController;

		private MovementController _movementController;

		private PlayerOcclusionController _occlusionController;

		private Transform _movementDirector;

		private MecanimController _mecanimController;

		private Transform _helper;

		private Transform _gunSubstitude;

		private Transform _headDirective;

		private BipedMap _bipedMap;

		private Transform _characterTransform;

		private bool _isGpOffsetInitialized;

		private AimControlCmd _aimCmd = new AimControlCmd();

		private readonly StateSimple<ViewMode> _aimingMode = new StateSimple<ViewMode>(ViewMode.NotStated);

		private Vector3 _curSpineAimAngle;

		private readonly StateSimple<WeaponOffsetState> _weaponOffsetState = new StateSimple<WeaponOffsetState>(WeaponOffsetState.NotStated);

		private readonly StateSimple<MoveState> _moveState = new StateSimple<MoveState>(MoveState.NotStated);

		private readonly TransformTR _weaponDirOnStand = new TransformTR();

		private readonly TransformTR _weaponDirOnCrouch = new TransformTR();

		private TransformTR _currentGphOffsetOnStand = new TransformTR();

		private TransformTR _currentGphOffsetOnCrouch = new TransformTR();

		private readonly AimSnapshot _syncData = new AimSnapshot();

		private Action _onAnimatorUpdatedAction1;

		private Action _onAnimatorUpdatedAction2;

		private Transform _spine1RotateHelper;

		private Transform _spine2RotateHelper;

		public float LocalTime
		{
			get;
			private set;
		}

		public float DeltaTime
		{
			get;
			private set;
		}

		public ViewMode ViewMode => _aimingMode.curState;

		public event Action OnSpinePostRotation = delegate
		{
		};

		public event Action onAllChangesApplied;

		public AimController()
		{
			DeltaTime = 0f;
		}

		public override void PreInitialize()
		{
			InitializeParameters();
			_playerController = GetComponent<PlayerController>();
			_bipedMap = _playerController.BipedMap;
			_characterTransform = _playerController.BipedMap.transform;
			_playerController.CharacterSkinSetEvent += CharacterSkinSet;
			_mecanimController = GetComponent<MecanimController>();
			_mecanimController.onAnimatorUpdated += OnAnimatorUpdated;
			_movementController = GetComponent<MovementController>();
			_movementDirector = _movementController.MovementDirector;
			_occlusionController = GetComponent<PlayerOcclusionController>();
			_occlusionController.OnOcclusionBecameInvisible += OnOcclusionBecameInvisible;
			_occlusionController.OnOcclusionPostBecameVisible += OnOcclusionBecameVisible;
			GameObject gameObject = new GameObject();
			_gunSubstitude = gameObject.transform;
			_gunSubstitude.SetParent(_bipedMap.Spine2);
			_gunSubstitude.transform.localPosition = aimingParameters.defaultWeaponOffset.pos;
			_gunSubstitude.transform.localEulerAngles = aimingParameters.defaultWeaponOffset.rot;
			aimingData.curEulerAngles = _movementDirector.eulerAngles;
			gameObject = new GameObject();
			_helper = gameObject.transform;
			gameObject = new GameObject();
			_headDirective = gameObject.transform;
			_headDirective.LookAt(base.transform.forward);
			_headDirective.SetParent(_bipedMap.Head);
			_headDirective.localPosition = Vector3.zero;
			_aimingEventsImpl = GetComponents<IAimingEvents>();
			interpolatorsBunch.additiveFPSOffset.InitializeBlending(aimingParameters.generalData.curve00.id, 1f, 0f);
			interpolatorsBunch.FPSOffset.InitializeBlending(aimingParameters.generalData.curve11.id, 1f, 0f);
			_playerController.BipedMap.Head.localEulerAngles = aimingParameters.defaultHeadRotation;
			_spine1RotateHelper = new GameObject().transform;
			_spine1RotateHelper.name = "_characterSpineHelper";
			_spine2RotateHelper = new GameObject().transform;
			_spine2RotateHelper.name = "_characterSpineHelper";
			_spine2RotateHelper.SetParent(_spine1RotateHelper);
			_spine1RotateHelper.SetParent(base.transform);
			Singleton<Trash>.Instance.Drop(_spine1RotateHelper);
		}

		public override void Initialize()
		{
			_isGpOffsetInitialized = false;
			interpolatorsBunch.additiveFPSOffset.InitializeBlending(aimingParameters.generalData.curve00.id, 1f, 0f);
			interpolatorsBunch.FPSOffset.InitializeBlending(aimingParameters.generalData.curve11.id, 1f, 0f);
			_observationState = ObservationState.Visible;
		}

		private void InitializeParameters()
		{
			CurvesContainer.RegisterCurve(aimingParameters.fpsAdditiveOffsetOnLand);
			CurvesContainer.RegisterCurve(aimingParameters.FPSOffsetCurveStandToCrouch);
			CurvesContainer.RegisterCurve(aimingParameters.FPSOffsetCurveCrouchToStand);
			CurvesContainer.RegisterCurve(aimingParameters.generalData.curve00);
			CurvesContainer.RegisterCurve(aimingParameters.generalData.curve01);
			CurvesContainer.RegisterCurve(aimingParameters.generalData.curve11);
		}

		public override void ExecuteCommands(AimControlCmd commands, float duration, float time)
		{
			_aimCmd = commands;
			DeltaTime = duration;
			LocalTime = time;
			RotationControl();
			OffsetControl();
			_onAnimatorUpdatedAction1 = AimingControl;
		}

		public override void SetSnapshot(AimSnapshot parameters)
		{
			if (_observationState != 0)
			{
				aimingData = parameters.aimingData.Clone();
				ApplyRotation();
				if (_aimingMode.curState == ViewMode.FPS)
				{
					_onAnimatorUpdatedAction1 = FPSControl;
				}
				if (_aimingMode.curState == ViewMode.TPS)
				{
					_onAnimatorUpdatedAction1 = TPSControl;
				}
			}
		}

		public override AimSnapshot GetSnapshot()
		{
			_syncData.aimingData = aimingData.Clone();
			return _syncData;
		}

		private void OnOcclusionBecameInvisible()
		{
			_observationState = ObservationState.Invisible;
		}

		private void OnOcclusionBecameVisible()
		{
			_observationState = ObservationState.Visible;
		}

		public void OnAnimatorUpdated()
		{
			if (_onAnimatorUpdatedAction1 != null)
			{
				_onAnimatorUpdatedAction1();
				_onAnimatorUpdatedAction1 = null;
			}
			if (_onAnimatorUpdatedAction2 != null)
			{
				_onAnimatorUpdatedAction2();
				_onAnimatorUpdatedAction2 = null;
			}
			if (this.onAllChangesApplied != null)
			{
				this.onAllChangesApplied();
				this.onAllChangesApplied = null;
			}
		}

		internal void SetFPSView()
		{
			SetViewMode(ViewMode.FPS);
		}

		internal void SetTPSView()
		{
			SetViewMode(ViewMode.TPS);
		}

		private void SetViewMode(ViewMode viewMode)
		{
			if (viewMode == ViewMode.FPS)
			{
				_mecanimController.SetFPSViewMode();
			}
			if (viewMode == ViewMode.TPS)
			{
				_mecanimController.SetTPSViewMode();
			}
			OnViewModeSetEvent(viewMode);
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			_characterTransform = bipedMap.transform;
			Vector3 localPosition = _gunSubstitude.localPosition;
			Quaternion localRotation = _gunSubstitude.localRotation;
			_bipedMap = bipedMap;
			_gunSubstitude.SetParent(bipedMap.Spine2);
			_gunSubstitude.transform.localPosition = localPosition;
			_gunSubstitude.transform.localRotation = localRotation;
			Vector3 localEulerAngles = _headDirective.localEulerAngles;
			_headDirective.SetParent(bipedMap.Head);
			_headDirective.localEulerAngles = localEulerAngles;
			_headDirective.localPosition = Vector3.zero;
		}

		public void ResetWeaponSwitchBlending()
		{
			aimingData.prevGPHOffsetOnCrouch = _weaponDirOnStand;
			aimingData.prevGPHOffsetOnStand = _weaponDirOnCrouch;
		}

		private void RotationControl()
		{
			float num = aimingData.curEulerAngles.y + _aimCmd.DeltaAimAngles.y;
			float num2 = 0f - aimingData.curAimAngle.x;
			if (num < -180f)
			{
				num += 360f;
			}
			num2 += _aimCmd.DeltaAimAngles.x;
			if (num2 < -180f)
			{
				num2 += 360f;
			}
			num2 = Mathf.Clamp(num2, minimumX, maximumX);
			aimingData.curEulerAngles = new Vector3(0f, num, 0f);
			aimingData.curAimAngle = new Vector3(0f - num2, num, 0f);
			ApplyRotation();
		}

		private void ApplyRotation()
		{
			FPSP_go.transform.localEulerAngles = new Vector3(aimingData.curAimAngle.x, 0f, 0f);
			_movementDirector.eulerAngles = aimingData.curEulerAngles;
		}

		private void OffsetControl()
		{
			if (_weaponOffsetState.curState == WeaponOffsetState.Translation)
			{
				if (_weaponOffsetState.isJustSwitched)
				{
					_weaponOffsetState.isJustSwitched = false;
				}
				float num = LocalTime - _weaponOffsetState.timeSwitched;
				aimingData.curWTCoeff = CurvedValue.GetCurveValue(aimingParameters.weaponPHTCurve, aimingParameters.weaponPHTDuration, _weaponOffsetState.timeSwitched, LocalTime);
				if (num > aimingParameters.weaponPHTDuration)
				{
					_weaponOffsetState.SetState(WeaponOffsetState.NotStated, LocalTime);
				}
			}
			Vector3 b = _currentGphOffsetOnStand.pos = Vector3.Lerp(aimingData.prevGPHOffsetOnStand.pos, _weaponDirOnStand.pos, aimingData.curWTCoeff);
			Vector3 b2 = _currentGphOffsetOnStand.rot = VectorAngle.LerpEulerAngle(aimingData.prevGPHOffsetOnStand.rot, _weaponDirOnStand.rot, aimingData.curWTCoeff);
			Vector3 a = _currentGphOffsetOnCrouch.pos = Vector3.Lerp(aimingData.prevGPHOffsetOnCrouch.pos, _weaponDirOnCrouch.pos, aimingData.curWTCoeff);
			Vector3 a2 = _currentGphOffsetOnCrouch.rot = VectorAngle.LerpEulerAngle(aimingData.prevGPHOffsetOnCrouch.rot, _weaponDirOnCrouch.rot, aimingData.curWTCoeff);
			float actual = _movementController.translationData.standTypeCoeff.actual;
			float num2 = Mathf.Min(0f, _movementController.translationData.jumpTypeCoeff.blended) / 3f;
			num2 = num2 * 2f + 1f;
			float num3 = Mathf.Min(actual, num2);
			float num4 = (1f + num3) * 0.5f;
			aimingData.standTypeCoeff = num4;
			aimingData.gunSubstitude.pos = Vector3.Lerp(a, b, num4);
			aimingData.gunSubstitude.rot = VectorAngle.LerpEulerAngle(a2, b2, num4);
			_mecanimController.SetArmsOffsetCoeff(1f - aimingData.standTypeCoeff);
			if (_moveState.curState == MoveState.Stand)
			{
				if (_moveState.isJustSwitched)
				{
					_moveState.isJustSwitched = false;
					interpolatorsBunch.FPSOffset.BlendValue(aimingParameters.FPSOffsetCurveStandToCrouch.duration, aimingParameters.FPSOffsetCurveStandToCrouch, aimingData.curMTCoeff, LocalTime);
				}
				float timeSwitched = _moveState.timeSwitched;
				if (timeSwitched > aimingParameters.weaponMTDuration)
				{
					_moveState.SetState(MoveState.FinishedStand, LocalTime);
				}
			}
			if (_moveState.curState == MoveState.Crouch)
			{
				if (_moveState.isJustSwitched)
				{
					_moveState.isJustSwitched = false;
					interpolatorsBunch.FPSOffset.BlendValue(aimingParameters.FPSOffsetCurveCrouchToStand.duration, aimingParameters.FPSOffsetCurveCrouchToStand, aimingData.curMTCoeff, LocalTime);
				}
				float num5 = LocalTime - _moveState.timeSwitched;
				if (num5 > aimingParameters.weaponMTDuration)
				{
					_moveState.SetState(MoveState.FinishedCrouch, LocalTime);
				}
			}
		}

		private void TPSControl()
		{
			if (_aimingMode.isJustSwitched)
			{
				_aimingMode.isJustSwitched = false;
			}
			_gunSubstitude.localPosition = aimingData.gunSubstitude.pos;
			_gunSubstitude.localEulerAngles = aimingData.gunSubstitude.rot;
			Vector3 a = Vector3.zero;
			Vector3 eulerAngles = FPSP_go.transform.eulerAngles;
			float x = eulerAngles.x;
			Vector3 eulerAngles2 = _gunSubstitude.eulerAngles;
			a.x = VectorAngle.DeltaAngle(x, eulerAngles2.x);
			Vector3 eulerAngles3 = FPSP_go.transform.eulerAngles;
			float y = eulerAngles3.y;
			Vector3 eulerAngles4 = _gunSubstitude.eulerAngles;
			a.y = VectorAngle.DeltaAngle(y, eulerAngles4.y);
			Vector3 zero = Vector3.zero;
			Vector3 eulerAngles5 = _characterTransform.eulerAngles;
			float x2 = eulerAngles5.x;
			Vector3 eulerAngles6 = _gunSubstitude.eulerAngles;
			zero.x = VectorAngle.DeltaAngle(x2, eulerAngles6.x);
			Vector3 eulerAngles7 = _characterTransform.eulerAngles;
			float y2 = eulerAngles7.y;
			Vector3 eulerAngles8 = _gunSubstitude.eulerAngles;
			zero.y = VectorAngle.DeltaAngle(y2, eulerAngles8.y);
			a.x = Mathf.Clamp(a.x - zero.x, 0f - aimingParameters.verticalClamp, aimingParameters.verticalClamp);
			a.y = Mathf.Clamp(a.y - zero.y, 0f - aimingParameters.horizontalClamp, aimingParameters.horizontalClamp);
			a += zero;
			_curSpineAimAngle.x = Mathf.Lerp(_curSpineAimAngle.x, a.x, 1f);
			_curSpineAimAngle.y = Mathf.Lerp(_curSpineAimAngle.y, a.y, 1f);
			ApplySpineRotation();
			this.OnSpinePostRotation();
			Transform bone = _playerController.BipedMap.GetBone(BipedMap.Bip.Head);
			_helper.rotation = Quaternion.LookRotation(_gunSubstitude.forward);
			Vector3 zero2 = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			_relative = _bipedMap.Head;
			Transform gunSubstitude = _gunSubstitude;
			Vector3 eulerAngles9 = gunSubstitude.eulerAngles;
			float y3 = eulerAngles9.y;
			Vector3 eulerAngles10 = _headDirective.eulerAngles;
			zero2.y = VectorAngle.DeltaAngle(y3, eulerAngles10.y);
			bone.rotation *= Quaternion.AngleAxis(aimingParameters.headOffset.y + zero2.y, _relative.InverseTransformDirection(base.transform.up));
			Vector3 eulerAngles11 = gunSubstitude.eulerAngles;
			float x3 = eulerAngles11.x;
			Vector3 eulerAngles12 = _headDirective.eulerAngles;
			zero2.x = VectorAngle.DeltaAngle(x3, eulerAngles12.x);
			bone.rotation *= Quaternion.AngleAxis(aimingParameters.headOffset.x + zero2.x, _relative.InverseTransformDirection(_helper.right));
			Vector3 eulerAngles13 = _gunSubstitude.eulerAngles;
			float z = eulerAngles13.z;
			Vector3 eulerAngles14 = _headDirective.eulerAngles;
			zero2.z = VectorAngle.DeltaAngle(z, eulerAngles14.z);
			bone.rotation *= Quaternion.AngleAxis(aimingParameters.headOffset.z + zero2.z, _relative.InverseTransformDirection(_helper.forward));
		}

		private void ApplySpineRotation()
		{
			Transform bone = _playerController.BipedMap.GetBone(BipedMap.Bip.Spine1);
			Transform bone2 = _playerController.BipedMap.GetBone(BipedMap.Bip.Spine2);
			if (!tuningParams.disableSpinesRotation)
			{
				_spine1RotateHelper.position = bone.position;
				_spine1RotateHelper.rotation = bone.rotation;
				_spine2RotateHelper.position = bone2.position;
				_spine2RotateHelper.rotation = bone2.rotation;
				Vector3 localPosition = _gunSubstitude.localPosition;
				Quaternion localRotation = _gunSubstitude.localRotation;
				_gunSubstitude.SetParent(_spine2RotateHelper);
				_gunSubstitude.localPosition = localPosition;
				_gunSubstitude.localRotation = localRotation;
				_spine1RotateHelper.rotation *= Quaternion.AngleAxis(_curSpineAimAngle.y * 0.5f, _spine1RotateHelper.InverseTransformDirection(base.transform.up));
				_spine2RotateHelper.rotation *= Quaternion.AngleAxis(_curSpineAimAngle.y * 0.5f, _spine2RotateHelper.InverseTransformDirection(base.transform.up));
				_helper.position = base.transform.position;
				_helper.rotation = Quaternion.LookRotation(_gunSubstitude.forward);
				Transform helper = _helper;
				Vector3 eulerAngles = _helper.eulerAngles;
				float x = eulerAngles.x;
				Vector3 eulerAngles2 = _helper.eulerAngles;
				helper.eulerAngles = new Vector3(x, eulerAngles2.y, 0f);
				_spine1RotateHelper.rotation *= Quaternion.AngleAxis(_curSpineAimAngle.x * 0.5f, _spine1RotateHelper.InverseTransformDirection(_helper.right));
				_spine2RotateHelper.rotation *= Quaternion.AngleAxis(_curSpineAimAngle.x * 0.5f, _spine2RotateHelper.InverseTransformDirection(_helper.right));
				Vector3 eulerAngles3 = _gunSubstitude.eulerAngles;
				float b = VectorAngle.DeltaAngle(0f, eulerAngles3.z);
				_helper.rotation = Quaternion.LookRotation(_gunSubstitude.forward);
				aimingData.currentSpineZAxisOffset = Mathf.LerpAngle(aimingData.currentSpineZAxisOffset, b, 5f);
				_spine1RotateHelper.rotation *= Quaternion.AngleAxis(aimingData.currentSpineZAxisOffset * 0.8f, _spine1RotateHelper.InverseTransformDirection(_helper.forward));
				_spine2RotateHelper.rotation *= Quaternion.AngleAxis(aimingData.currentSpineZAxisOffset * 0.2f, _spine2RotateHelper.InverseTransformDirection(_helper.forward));
				bone.rotation = _spine1RotateHelper.rotation;
				bone2.rotation = _spine2RotateHelper.rotation;
				_gunSubstitude.SetParent(bone2);
				_gunSubstitude.localPosition = localPosition;
				_gunSubstitude.localRotation = localRotation;
			}
		}

		private void FPSControl()
		{
			if (_aimingMode.isJustSwitched)
			{
				_aimingMode.isJustSwitched = false;
				_mecanimController.SetArmsOffsetCoeff(0f);
			}
			ApplyFPSRotation();
		}

		private void ApplyFPSRotation()
		{
			aimingData.curMTCoeff = interpolatorsBunch.FPSOffset.GetValue(LocalTime);
			float value = interpolatorsBunch.additiveFPSOffset.GetValue(LocalTime);
			Vector3 a = VectorAngle.LerpEulerAngle(aimingParameters.FPSOffsetCrouch.pos, aimingParameters.FPSOffsetStand.pos, aimingData.curMTCoeff);
			a += Vector3.up * value;
			FPSP_go.transform.localPosition = a;
			Transform bone = _playerController.BipedMap.GetBone(BipedMap.Bip.Spine2);
			bone.position = spineDirector.position;
			bone.eulerAngles = spineDirector.eulerAngles;
		}

		public void SetSpineTR()
		{
			Transform bone = _playerController.BipedMap.GetBone(BipedMap.Bip.Spine2);
			bone.position = spineDirector.position;
			bone.eulerAngles = spineDirector.eulerAngles;
		}

		private void AimingControl()
		{
			switch (_aimingMode.curState)
			{
			case ViewMode.FPS:
				FPSControl();
				break;
			case ViewMode.TPS:
				TPSControl();
				break;
			}
		}

		private void OnViewModeSetEvent(ViewMode viewMode)
		{
			IAimingEvents[] aimingEventsImpl = _aimingEventsImpl;
			foreach (IAimingEvents aimingEvents in aimingEventsImpl)
			{
				aimingEvents.OnViewModeSet(viewMode);
			}
		}

		public void OnViewModeSet(ViewMode viewMode)
		{
			if (viewMode == ViewMode.FPS)
			{
				_aimingMode.SetState(ViewMode.FPS, LocalTime);
				_mecanimController.SetFPSViewMode();
			}
			if (viewMode == ViewMode.TPS)
			{
				_aimingMode.SetState(ViewMode.TPS, LocalTime);
				_mecanimController.SetTPSViewMode();
			}
		}

		public void OnAnimatorReset()
		{
			if (_aimingMode.curState == ViewMode.FPS)
			{
				_mecanimController.SetFPSViewMode();
			}
			if (_aimingMode.curState == ViewMode.TPS)
			{
				_mecanimController.SetTPSViewMode();
			}
		}

		public void OnMovementInit(MovementController movementController)
		{
			_movementDirector = movementController.MovementDirector;
			_movementController = movementController;
		}

		public void OnJump(MovementController moveModule)
		{
			if (_moveState.curState == MoveState.Crouch || _moveState.curState == MoveState.FinishedCrouch)
			{
				_moveState.SetState(MoveState.Stand, LocalTime);
			}
		}

		public void OnLand(MovementController moveModule, float fallDuration)
		{
			float value = interpolatorsBunch.additiveFPSOffset.GetValue(LocalTime);
			interpolatorsBunch.additiveFPSOffset.BlendValue(0.1f, aimingParameters.fpsAdditiveOffsetOnLand, value, LocalTime);
		}

		public void OnCrouch(MovementController moveModule)
		{
			if (_moveState.curState == MoveState.Stand || _moveState.curState == MoveState.FinishedStand)
			{
				_moveState.SetState(MoveState.Crouch, LocalTime);
			}
		}

		public void OnStand(MovementController moveModule)
		{
			if (_moveState.curState == MoveState.Crouch || _moveState.curState == MoveState.FinishedCrouch || _moveState.curState == MoveState.NotStated)
			{
				_moveState.SetState(MoveState.Stand, LocalTime);
			}
		}

		public void OnWeaponSet(WeaponController weapon)
		{
			aimingData.prevGPHOffsetOnCrouch = _currentGphOffsetOnCrouch.Clone();
			aimingData.prevGPHOffsetOnStand = _currentGphOffsetOnStand.Clone();
			_weaponDirOnStand.pos = weapon.AnimationParameters.weaponDirectorOnStand.pos;
			_weaponDirOnStand.rot = weapon.AnimationParameters.weaponDirectorOnStand.rot;
			_weaponDirOnCrouch.pos = weapon.AnimationParameters.weaponDirectorOnCrouch.pos;
			_weaponDirOnCrouch.rot = weapon.AnimationParameters.weaponDirectorOnCrouch.rot;
			spineDirector.localEulerAngles = weapon.AnimationParameters.spineFPSDirectorTR.rot;
			if (!_isGpOffsetInitialized)
			{
				_gunSubstitude.localPosition = weapon.AnimationParameters.weaponDirectorOnStand.pos;
				_gunSubstitude.localEulerAngles = weapon.AnimationParameters.weaponDirectorOnStand.rot;
				aimingData.prevGPHOffsetOnCrouch = weapon.AnimationParameters.weaponDirectorOnCrouch;
				aimingData.prevGPHOffsetOnStand = weapon.AnimationParameters.weaponDirectorOnStand;
				_currentGphOffsetOnStand = weapon.AnimationParameters.weaponDirectorOnStand.Clone();
				_currentGphOffsetOnCrouch = weapon.AnimationParameters.weaponDirectorOnCrouch.Clone();
				_isGpOffsetInitialized = true;
			}
			_weaponOffsetState.SetState(WeaponOffsetState.Translation, LocalTime);
		}

		public void OnNewWeaponSet(WeaponController weapon)
		{
		}

		public void OnDropWeapon(WeaponController weapon)
		{
		}
	}
}
