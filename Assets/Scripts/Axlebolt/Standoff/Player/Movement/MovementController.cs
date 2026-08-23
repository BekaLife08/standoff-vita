using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement.States;
using Axlebolt.Standoff.Player.Occlusion;
using Axlebolt.Standoff.Player.Weaponry;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement
{
	[RequireComponent(typeof(MecanimController))]
	[RequireComponent(typeof(PlayerController))]
	public class MovementController : BaseController<MovementSnapshot, MovementControllerCmd>, IInventoryEvents
	{
		public delegate void MoveEvent(MovementController moveModule);

		public delegate void MoveLandEvent(MovementController moveModule, float fallDuration);

		private static readonly Log Log = Log.Create(typeof(MovementController));

		private PlayerController _playerController;

		private PlayerOcclusionController _occlusionController;

		private TranslationStatesMap _statesMap;

		public PlayerTranslationParameters translationParameters;

		public PlayerTranslationData translationData = new PlayerTranslationData();

		private MovementSnapshot _syncTranslationData = new MovementSnapshot();

		[HideInInspector]
		public Transform characterTransform;

		public Transform MovementDirector
		{
			get;
			private set;
		}

		public MovementControllerCmd CmdParameters
		{
			get;
			private set;
		} = new MovementControllerCmd();


		public float DeltaTime
		{
			get;
			private set;
		} = 1f;


		public float LocalTime
		{
			get;
			private set;
		}

		public CharacterController CharacterController
		{
			get;
			private set;
		}

		public Trigger CharacterControllerTrigger
		{
			get;
			private set;
		}

		public IMovementEvents[] MovementEventsImpls
		{
			get;
			private set;
		}

		public MecanimController MecanimController
		{
			get;
			private set;
		}

		public Vector3 Velocity
		{
			get
			{
				if (translationData == null)
				{
					UnityEngine.Debug.LogError("MovementController Not Initialized");
				}
				return translationData.velocity;
			}
		}

		public event MoveEvent OnMovementInit = delegate
		{
		};

		public event MoveEvent OnJump = delegate
		{
		};

		public event MoveLandEvent OnLand = delegate
		{
		};

		public event MoveEvent OnCrouch = delegate
		{
		};

		public event MoveEvent OnStand = delegate
		{
		};

		public MovementController()
		{
			LocalTime = 0f;
		}

		private void InitializeParameters()
		{
			CurvesContainer.RegisterCurve(translationParameters.crouchParameters.standToCrouchCurve);
			CurvesContainer.RegisterCurve(translationParameters.jumpParameters.jumpCurve);
			CurvesContainer.RegisterCurve(translationParameters.jumpParameters.landCurve);
			CurvesContainer.RegisterCurve(translationParameters.jumpParameters.minSpeedCurve);
			CurvesContainer.RegisterCurve(translationParameters.generalCurveTypes.curveLinear01);
			CurvesContainer.RegisterCurve(translationParameters.generalCurveTypes.curveLinear11);
			translationData.jumpTypeCoeff.actual = 0f;
			translationData.standTypeCoeff.actual = 1f;
		}

		public override void PreInitialize()
		{
			InitializeParameters();
			_playerController = GetComponent<PlayerController>();
			_playerController.CharacterSkinSetEvent += CharacterSkinSet;
			MecanimController = GetComponent<MecanimController>();
			_occlusionController = GetComponent<PlayerOcclusionController>();
			MovementDirector = base.transform;
			MovementDirector.position = base.transform.position;
			MovementDirector.rotation = base.transform.rotation;
			CharacterController = GetComponent<CharacterController>();
			CharacterController.center = GetComponent<CharacterController>().center;
			CharacterController.radius = GetComponent<CharacterController>().radius;
			CharacterController.height = GetComponent<CharacterController>().height;
			CharacterControllerTrigger = new GameObject().AddComponent<Trigger>();
			CharacterControllerTrigger.transform.SetParent(MovementDirector);
			CharacterControllerTrigger.transform.localPosition = Vector3.zero;
			CharacterControllerTrigger.transform.localScale = new Vector3(1f, 1f, 1f);
			CharacterControllerTrigger.transform.localEulerAngles = Vector3.zero;
			CharacterControllerTrigger.gameObject.name = "CharacterControllerTrigger";
			CharacterControllerTrigger.gameObject.layer = 9;
			CapsuleCollider capsuleCollider = CharacterControllerTrigger.gameObject.AddComponent<CapsuleCollider>();
			capsuleCollider.height = CharacterController.height;
			capsuleCollider.radius = CharacterController.radius;
			capsuleCollider.center = CharacterController.center + Vector3.down * 0.2f;
			capsuleCollider.isTrigger = true;
			characterTransform = _playerController.CharacterGO.transform;
			MovementEventsImpls = GetComponents<IMovementEvents>();
			translationData.prevPosition = (translationData.currentPosition = CharacterController.transform.position);
			IMovementEvents[] movementEventsImpls = MovementEventsImpls;
			foreach (IMovementEvents movementEvents in movementEventsImpls)
			{
				IMovementEvents movementEvents2 = movementEvents;
				OnCrouch += movementEvents2.OnCrouch;
				IMovementEvents movementEvents3 = movementEvents;
				OnJump += movementEvents3.OnJump;
				IMovementEvents movementEvents4 = movementEvents;
				OnStand += movementEvents4.OnStand;
				IMovementEvents movementEvents5 = movementEvents;
				OnLand += movementEvents5.OnLand;
				IMovementEvents movementEvents6 = movementEvents;
				OnMovementInit += movementEvents6.OnMovementInit;
			}
			this.OnMovementInit(this);
			this.OnStand(this);
			_statesMap = new TranslationStatesMap(this);
		}

		public override void Initialize()
		{
			translationData.Reset();
			translationData.prevPosition = (translationData.currentPosition = CharacterController.transform.position);
			_statesMap.Initialize();
			_statesMap.SetState(TranslationStatesEnum.Walk);
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			characterTransform = bipedMap.transform;
		}

		public override void SetSnapshot(MovementSnapshot param)
		{
			if (float.IsNaN(param.currentPosition.x) || float.IsNaN(param.currentPosition.y) || float.IsNaN(param.currentPosition.z))
			{
				if (Log.DebugEnabled)
				{
					Log.Error("SetSnapshot position is NaN");
				}
				return;
			}
			_syncTranslationData = param;
			translationData.currentPosition = param.currentPosition;
			if (!_occlusionController.IsActive || (_occlusionController.IsActive && _occlusionController.IsVisible))
			{
				characterTransform.localEulerAngles = param.characterRotation;
			}
			CharacterController.transform.position = translationData.currentPosition;
			translationData.velocity = param.velocity;
			translationData.IsVelocitySynchronized = true;
		}

		public override MovementSnapshot GetSnapshot()
		{
			_syncTranslationData.currentPosition = translationData.currentPosition;
			_syncTranslationData.characterRotation = characterTransform.localEulerAngles;
			_syncTranslationData.velocity = translationData.velocity;
			if ((float.IsNaN(_syncTranslationData.currentPosition.x) || float.IsNaN(_syncTranslationData.currentPosition.y) || float.IsNaN(_syncTranslationData.currentPosition.z)) && Log.DebugEnabled)
			{
				Log.Error("GetSnapshot position is NaN");
			}
			return _syncTranslationData;
		}

		public override void ExecuteCommands(MovementControllerCmd commands, float duration, float time)
		{
			CmdParameters = commands;
			DeltaTime = duration;
			LocalTime = time;
			MovementTranslationControl();
			_statesMap.LateUpdateState();
			translationData.IsVelocitySynchronized = false;
		}

		private void MovementTranslationControl()
		{
			float horizontal = CmdParameters.Horizontal;
			float vertical = CmdParameters.Vertical;
			Vector2 targetRelativeDirection = new Vector2(horizontal, vertical);
			if (targetRelativeDirection.magnitude < 0.3f)
			{
			}
			if (targetRelativeDirection.magnitude > 1f)
			{
				targetRelativeDirection.Normalize();
			}
			translationData.targetRelativeDirection = targetRelativeDirection;
			_statesMap.UpdateState();
			CharacterController.Move(translationData.targetDeltaPosition);
			MecanimController.SetMoveDir(new Vector2(translationData.currentRelativeDirectionMecanim.x, translationData.currentRelativeDirectionMecanim.y));
			MecanimController.SetJumpCoeff(translationData.jumpTypeCoeff.actual);
			MecanimController.SetStandCoeff(translationData.standTypeCoeff.actual);
			MecanimController.SetTargetDirectionMagnitude(translationData.targetMecanimDirectionMagnitude);
			translationData.prevDeltaTime = DeltaTime;
			translationData.prevPosition = translationData.currentPosition;
			translationData.currentPosition = CharacterController.transform.position;
			translationData.prevDeltaPosition = translationData.targetDeltaPosition;
		}

		public void OnCharacterSkinChanged(BipedMap bipedMap)
		{
		}

		public void OnViewModeChanged(ViewMode viewMode)
		{
		}

		public void OnWeaponSet(WeaponController weapon)
		{
			translationData.WeaponSpeedMult = (float)weapon.WeaponParameters.MovementRate / 250f;
		}

		public void OnNewWeaponSet(WeaponController weapon)
		{
		}

		public void OnDropWeapon(WeaponController weapon)
		{
		}
	}
}
