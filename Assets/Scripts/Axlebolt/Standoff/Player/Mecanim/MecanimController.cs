using Axlebolt.Standoff.Player.Occlusion;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimController : BaseController<MecanimSnapshot, MecanimControllerCmd>
	{
		public delegate void AnimatorUpdateCall();

		private ObservationState _observationState;

		[HideInInspector]
		private PlayerController _controlModule;

		private PlayerOcclusionController _occlusionController;

		public Animator animator;

		public RuntimeAnimatorController runtimeAnimatorController;

		public PlayerMecanimConfig config;

		public MecanimSync mecanimSync;

		private MecanimUpdateMode _updateMode;

		private ViewMode _viewMode;

		private ExecutionType _executionType = ExecutionType.NotStated;

		private MecanimLayerInfo _layerInfo = new MecanimLayerInfo();

		private IMecanimEvents[] _mecanimEventsImpl;

		private MecanimSnapshot syncParameters = new MecanimSnapshot();

		public MecanimUpdateMode UpdateMode
		{
			get
			{
				return _updateMode;
			}
			set
			{
				_updateMode = value;
				if (_updateMode == MecanimUpdateMode.OnDemand)
				{
					animator.enabled = false;
				}
				if (_updateMode == MecanimUpdateMode.GameLoop)
				{
					animator.enabled = true;
				}
			}
		}

		public event AnimatorUpdateCall onAnimatorUpdated = delegate
		{
		};

		public event AnimatorUpdateCall onAnimatorStateSet = delegate
		{
		};

		public override void PreInitialize()
		{
			_controlModule = GetComponent<PlayerController>();
			_mecanimEventsImpl = GetComponents<IMecanimEvents>();
			_controlModule.CharacterSkinSetEvent += CharacterSkinSet;
			_occlusionController = GetComponent<PlayerOcclusionController>();
			_occlusionController.OnOcclusionBecameVisible += OnOcclusionBecameVisible;
			_occlusionController.OnOcclusionBecameInvisible += OnOcclusionBecameInvisible;
			animator = GetAnimator(_controlModule.BipedMap);
			mecanimSync = new MecanimSync();
			mecanimSync.PreInitialize(animator, config);
		}

		public override void Initialize()
		{
			animator.runtimeAnimatorController = config.animatorController;
			animator.Update(0f);
			animator.enabled = true;
			_observationState = ObservationState.Visible;
			_executionType = ExecutionType.NotStated;
		}

		public override void ExecuteCommands(MecanimControllerCmd commands, float duration, float time)
		{
			mecanimSync.EvaluateTimePoints(time, isNetworkSync: true);
			_executionType = ExecutionType.Driven;
			if (_updateMode == MecanimUpdateMode.OnDemand)
			{
				animator.Update(duration);
				this.onAnimatorUpdated();
			}
		}

		public override MecanimSnapshot GetSnapshot()
		{
			syncParameters.mecanimSyncData = mecanimSync.GetAnimatorSnapshot(isNetworkSync: true);
			return syncParameters;
		}

		public override void SetSnapshot(MecanimSnapshot parameters)
		{
			_executionType = ExecutionType.Synchronized;
			syncParameters = parameters;
			mecanimSync.SetAnimatorSnapshot(parameters.mecanimSyncData, true, _observationState == ObservationState.Visible);
			CallAnimatorStateSetEvent();
		}

		private void CallAnimatorUpdatedEvent()
		{
			if (_observationState == ObservationState.Visible)
			{
				this.onAnimatorUpdated();
			}
		}

		private void CallAnimatorStateSetEvent()
		{
			this.onAnimatorStateSet();
		}

		private void LateUpdate()
		{
			if (_updateMode == MecanimUpdateMode.GameLoop)
			{
				CallAnimatorUpdatedEvent();
				if (_executionType == ExecutionType.Driven)
				{
					CallAnimatorStateSetEvent();
				}
			}
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			MecanimSyncData animatorSnapshot = mecanimSync.GetAnimatorSnapshot(isNetworkSync: false);
			animator = GetAnimator(bipedMap);
			mecanimSync.Initialize(animator);
			mecanimSync.SetAnimatorSnapshot(animatorSnapshot, isNetworkSync: false, evaluateAnimator: true);
			animator.Update(0f);
		}

		private void OnOcclusionBecameInvisible()
		{
			if (_observationState != ObservationState.Disabled)
			{
				_observationState = ObservationState.Invisible;
				animator.enabled = false;
			}
		}

		private void OnOcclusionBecameVisible()
		{
			if (_observationState != ObservationState.Disabled)
			{
				_observationState = ObservationState.Visible;
				animator.enabled = true;
				if (_viewMode == ViewMode.FPS)
				{
					animator.SetFloat("ViewType", 0f);
				}
				else
				{
					animator.SetFloat("ViewType", 1f);
				}
			}
		}

		private Animator GetAnimator(BipedMap bipedMap)
		{
			Animator animator = bipedMap.GetComponent<Animator>();
			if (animator == null)
			{
				animator = bipedMap.gameObject.AddComponent<Animator>();
				animator.avatar = config.avatar;
				animator.runtimeAnimatorController = config.animatorController;
			}
			return animator;
		}

		public void ResetAnimatorController()
		{
			OnAnimatorResetEvent();
		}

		public void SwitchWeapon(int weaponId)
		{
			animator.SetInteger("WeaponNO", weaponId);
			animator.SetTrigger("TakeWeapon");
		}

		public void SetWeaponIdleNO(int weaponId)
		{
			animator.SetFloat("WeaponIdleNO", weaponId);
		}

		public void SetWeaponNO(int weaponId)
		{
			animator.SetInteger("WeaponNO", weaponId);
		}

		public void SetReloading()
		{
			animator.SetTrigger("Reload");
		}

		public void InsertRound()
		{
			animator.SetTrigger("InsertRound");
		}

		public void FinishReload()
		{
			animator.SetTrigger("FinishReload");
		}

		public void SetShootType(int shootType)
		{
			animator.SetInteger("ShootType", shootType);
		}

		public void SetShooting()
		{
			animator.ResetTrigger("FinishReload");
			animator.SetTrigger("Shoot");
		}

		public void SetMoveDir(Vector2 dir)
		{
			animator.SetFloat("Horizontal", dir.x);
			animator.SetFloat("Vertical", dir.y);
			animator.SetFloat("Magnitude", dir.magnitude);
		}

		public void SetTargetDirectionMagnitude(float magn)
		{
			animator.SetFloat("TargetDirMagn", magn);
		}

		public void SetJumpCoeff(float jumpCoeff)
		{
			animator.SetFloat("JumpCoeff", jumpCoeff);
		}

		public void SetStandCoeff(float coeff)
		{
			animator.SetFloat("StandType", coeff);
		}

		public void SetArmsOffsetCoeff(float armsOffsetCoeff)
		{
			animator.SetFloat("ArmsOffset", armsOffsetCoeff);
		}

		public void SetCrouchIdle(bool state)
		{
			animator.SetBool("ToCrouchIdle", state);
		}

		public void SetRotating(float ang)
		{
			animator.SetTrigger("ToRotate");
			animator.SetFloat("RotAngle", ang);
		}

		public void SetFPSViewMode()
		{
			_viewMode = ViewMode.FPS;
			animator.SetFloat("ViewType", 0f);
			animator.Update(0f);
		}

		public void SetTPSViewMode()
		{
			_viewMode = ViewMode.TPS;
			animator.SetFloat("ViewType", 1f);
			animator.Update(0f);
		}

		public void SetAiming()
		{
			animator.SetTrigger("Aim");
		}

		public void SetAimingState(bool isAimingState)
		{
			animator.SetBool("Aiming", isAimingState);
		}

		public void CancelAiming()
		{
			animator.SetTrigger("CancelAiming");
		}

		public void PlantBomb()
		{
			animator.SetTrigger("Plant");
		}

		public void SetRecoilProgress(float progress)
		{
			animator.SetFloat("Recoil", progress);
		}

		public bool IsSwitchingWeapon(string weaponName)
		{
			if (!animator.IsInTransition(1))
			{
				return animator.GetCurrentAnimatorStateInfo(1).IsName("Take_" + weaponName);
			}
			return animator.GetNextAnimatorStateInfo(1).IsName("Take_" + weaponName);
		}

		public bool IsReloadingWeapon(string weaponName)
		{
			if (!animator.IsInTransition(1))
			{
				return animator.GetCurrentAnimatorStateInfo(1).IsName("Reload_" + weaponName);
			}
			return animator.GetNextAnimatorStateInfo(1).IsName("Reload_" + weaponName);
		}

		public bool IsStartingWeaponSwitch(string weaponName)
		{
			if (animator.IsInTransition(1) && animator.GetNextAnimatorStateInfo(1).IsName("Take_" + weaponName))
			{
				return true;
			}
			return false;
		}

		public bool IsHoldingWeapon(string weaponName)
		{
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
			if (currentAnimatorStateInfo.IsName("Idle_" + weaponName) || currentAnimatorStateInfo.IsName("Reload_" + weaponName) || currentAnimatorStateInfo.IsName("Shoot_" + weaponName) || currentAnimatorStateInfo.IsName("Take_" + weaponName))
			{
				return true;
			}
			return false;
		}

		public bool IsIdlingWeapon(string weaponName)
		{
			if (_observationState != ObservationState.Visible)
			{
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
			if (!animator.IsInTransition(1) && currentAnimatorStateInfo.IsName("Idle_" + weaponName))
			{
				return true;
			}
			return false;
		}

		public MecanimLayerInfo GetAnimatorStateInfo(int layer)
		{
			if (_executionType == ExecutionType.Synchronized)
			{
				return mecanimSync.GetSynchronizedLayerInfo(layer);
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			_layerInfo.layerInd = layer;
			_layerInfo.stateNameHash = currentAnimatorStateInfo.shortNameHash;
			_layerInfo.stateNormalizedTime = currentAnimatorStateInfo.normalizedTime;
			_layerInfo.IsSynchronized = false;
			return _layerInfo;
		}

		public float GetWalkAnimationProgress()
		{
			return animator.GetFloat("IsRU");
		}

		public float GetLFootParameter()
		{
			return animator.GetFloat("LFoot");
		}

		private void OnAnimatorResetEvent()
		{
			IMecanimEvents[] mecanimEventsImpl = _mecanimEventsImpl;
			for (int i = 0; i < mecanimEventsImpl.Length; i++)
			{
				mecanimEventsImpl[i]?.OnAnimatorReset();
			}
		}
	}
}
