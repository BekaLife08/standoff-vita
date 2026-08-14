using Axlebolt.Networking;
using Axlebolt.Standoff.Assets.Scripts.Player;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.Networking;
using Axlebolt.Standoff.Player.Occlusion;
using Axlebolt.Standoff.Player.Ragdoll;
using Axlebolt.Standoff.Player.Weaponry;
using Axlebolt.Standoff.Sound;
using JetBrains.Annotations;
using Photon;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(PhotonView))]
	[RequireComponent(typeof(WeaponryController))]
	[RequireComponent(typeof(MecanimController))]
	[RequireComponent(typeof(MovementController))]
	[RequireComponent(typeof(ArmsAnimationController))]
	[RequireComponent(typeof(AimController))]
	public class PlayerController : PunBehaviour
	{
		public delegate void CharacterSkinChangedHander(BipedMap bipedMap);

		private static readonly Log Log = Log.Create(typeof(PlayerController));

		[SerializeField]
		private Transform _mainCameraHolder;

		[SerializeField]
		private GameObject _fpsCameraHolder;

		[SerializeField]
		private BipedMap _characterBiped;

		[SerializeField]
		private BipedMap _armsBiped;

		[SerializeField]
		private GameObject _fpsDirective;

		private PhotonView _photonView;

		private int _photonPlayerId;

		private PlayerSnapshot _lastReceivedSnapshot;

		private Transform _transformSubstitude;

		private Transform _transformPlaceholder;

		private TransformTR _fpsDirectiveTR = new TransformTR();

		private CharacterPlayer _characterPlayer;

		private PlayerMainCamera _mainCamera;

		private PlayerFPSCamera _fpsCamera;

		private Transform _transform;

		public Transform TransformPlaceholder
		{
			[CompilerGenerated]
			get
			{
				return _transformPlaceholder;
			}
		}

		public float LocalTime
		{
			get;
			private set;
		}

		public bool IsInitialized
		{
			get;
			private set;
		}

		public AimController AimController
		{
			get;
			private set;
		}

		public WeaponryController WeaponryController
		{
			get;
			private set;
		}

		public MecanimController MecanimController
		{
			get;
			private set;
		}

		public MovementController MovementController
		{
			get;
			private set;
		}

		public NetworkController NetworkController
		{
			get;
			private set;
		}

		public PlayerRagdollController CharacterRagdollController
		{
			get;
			private set;
		}

		public ArmsAnimationController ArmsAnimController
		{
			get;
			private set;
		}

		public PlayerHitController HitController
		{
			get;
			private set;
		}

		public PlayerOcclusionController PlayerOcclusionController
		{
			get;
			private set;
		}

		public AudioPlayer CharacterAudioPlayer
		{
			get;
			private set;
		}

		public Interpolator Interpolator
		{
			[CompilerGenerated]
			get
			{
				return _characterPlayer.GetInterpolator();
			}
		}

		public BipedMap BipedMap
		{
			get;
			private set;
		}

		public GameObject CharacterGO
		{
			[CompilerGenerated]
			get
			{
				return BipedMap.gameObject;
			}
		}

		public GameObject FpsCameraHolder
		{
			[CompilerGenerated]
			get
			{
				return _fpsCameraHolder;
			}
		}

		public Transform MainCameraHolder
		{
			[CompilerGenerated]
			get
			{
				return _mainCameraHolder;
			}
		}

		public BipedMap CharacterBiped
		{
			[CompilerGenerated]
			get
			{
				return _characterBiped;
			}
		}

		public bool IsSynchronized
		{
			get;
			private set;
		}

		public bool IsCharacterVisible
		{
			get;
			private set;
		}

		public Transform Transform
		{
			[CompilerGenerated]
			get
			{
				return _transform ?? (_transform = base.transform);
			}
		}

		public ViewMode ViewMode
		{
			get;
			private set;
		}

		public PhotonView PhotonView
		{
			[CompilerGenerated]
			get
			{
				return _photonView;
			}
		}

		public int PlayerId
		{
			[CompilerGenerated]
			get
			{
				return PhotonView.ownerId;
			}
		}

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return PhotonView.owner;
			}
		}

		public event CharacterSkinChangedHander CharacterSkinSetEvent = delegate
		{
		};

		public void PreInitialize(BipedMap characterBiped, BipedMap armsBiped)
		{
			PreInitialize(characterBiped, armsBiped, null);
		}

		public void PreInitialize([NotNull] BipedMap characterBiped, [NotNull] BipedMap armsBiped, PlayerHitboxConfig hitboxConfig)
		{
			if (characterBiped == null)
			{
				throw new ArgumentNullException("characterBiped");
			}
			if (armsBiped == null)
			{
				throw new ArgumentNullException("armsBiped");
			}
			IsInitialized = true;
			_characterBiped = characterBiped;
			_armsBiped = armsBiped;
			SetInitView(preInit: true);
			CharacterAudioPlayer = Singleton<AudioPlayerManager>.Instance.Create();
			CharacterAudioPlayer.Initialize(base.transform, new Vector3(0f, 1.5f, 0f), 20);
			_photonView = this.GetRequireComponent<PhotonView>();
			PlayerOcclusionController = GetComponent<PlayerOcclusionController>();
			PlayerOcclusionController.PreInitialize();
			PlayerOcclusionController.SetEnabled(isEnabled: false);
			PlayerOcclusionController.OnOcclusionBecameVisible += OnOcclusionBecameVisible;
			PlayerOcclusionController.OnOcclusionPostBecameVisible += OnOcclusionPostBecameVisible;
			PlayerOcclusionController.OnOcclusionBecameInvisible += OnOcclusionBecameInvisible;
			MovementController = this.GetRequireComponent<MovementController>();
			MovementController.PreInitialize();
			AimController = this.GetRequireComponent<AimController>();
			AimController.PreInitialize();
			MecanimController = this.GetRequireComponent<MecanimController>();
			MecanimController.PreInitialize();
			ArmsAnimController = this.GetRequireComponent<ArmsAnimationController>();
			ArmsAnimController.PreInitialize();
			WeaponryController = this.GetRequireComponent<WeaponryController>();
			WeaponryController.PreInitialize();
			CharacterRagdollController = base.gameObject.AddComponent<PlayerRagdollController>();
			CharacterRagdollController.PreInitialize();
			NetworkController = GetComponent<NetworkController>();
			_characterPlayer = base.gameObject.GetRequireComponent<CharacterPlayer>();
			_characterPlayer.PreInitialize();
			_transformSubstitude = new GameObject().transform;
			_transformSubstitude.name = "PlayerTransformSubstitude";
			_transformPlaceholder = base.transform;
			if (hitboxConfig != null)
			{
				HitController = base.gameObject.AddComponent<PlayerHitController>();
				HitController.PreInitialize(hitboxConfig);
			}
			base.enabled = true;
		}

		public void Initialize()
		{
			MovementController.Initialize();
			AimController.Initialize();
			MecanimController.Initialize();
			ArmsAnimController.Initialize();
			PlayerOcclusionController.Initialize();
			PlayerOcclusionController.SetEnabled(isEnabled: false);
			SetInitView(preInit: false);
			WeaponryController.Initialize();
			_characterPlayer.Initialize();
			SetCharacterVisible(isEnabled: false);
		}

		public void OnReturnToPool()
		{
			try
			{
				if (!_characterPlayer.enabled)
				{
					Singleton<PlayerManager>.Instance.OnLocalDestroyEvent(this);
				}
				else
				{
					Singleton<PlayerManager>.Instance.OnRemoteDestroyEvent(this, _photonPlayerId);
				}
			}
			finally
			{
				if (_mainCamera != null)
				{
					_mainCamera.DetachFromPlayer();
				}
				if (_fpsCamera != null)
				{
					_fpsCamera.DetachFromPlayer();
				}
				_characterPlayer.Clear();
				WeaponryController.OnReturnToPool();
			}
		}

		private void SetInitView(bool preInit)
		{
			if (preInit)
			{
				_characterBiped.transform.SetParent(base.transform);
				_armsBiped.transform.SetParent(base.transform);
				BipedMap = _characterBiped;
			}
			if (Singleton<Trash>.Instance.Contains(_characterBiped.transform, base.transform))
			{
				Singleton<Trash>.Instance.Return(_characterBiped.transform, base.transform);
			}
			_characterBiped.gameObject.SetActive(value: true);
			if (!Singleton<Trash>.Instance.Contains(_armsBiped.transform, base.transform))
			{
				Singleton<Trash>.Instance.Drop(_armsBiped.transform);
			}
			if (!preInit)
			{
				ViewMode = ViewMode.NotStated;
				BipedMap = null;
				SetTPSView();
			}
		}

		private void OnOcclusionBecameInvisible()
		{
			Singleton<Trash>.Instance.Drop(BipedMap.gameObject);
		}

		private void OnOcclusionBecameVisible()
		{
			Singleton<Trash>.Instance.Return(BipedMap.transform, base.transform);
		}

		private void OnOcclusionPostBecameVisible()
		{
			if (_lastReceivedSnapshot != null)
			{
				SetSnapsot(_lastReceivedSnapshot);
			}
		}

		public void SetFPSView()
		{
			if (ViewMode != ViewMode.FPS)
			{
				if (Singleton<Trash>.Instance.Contains(_fpsDirective.transform, base.transform))
				{
					_fpsDirective.transform.localEulerAngles = _fpsDirectiveTR.rot;
					_fpsDirective.transform.localPosition = _fpsDirectiveTR.pos;
				}
				ViewMode = ViewMode.FPS;
				SetCharacterSkin(_armsBiped);
				AimController.SetFPSView();
			}
		}

		public void SetTPSView()
		{
			if (ViewMode != ViewMode.TPS)
			{
				_fpsDirectiveTR.pos = _fpsDirective.transform.localPosition;
				_fpsDirectiveTR.rot = _fpsDirective.transform.localEulerAngles;
				ViewMode = ViewMode.TPS;
				SetCharacterSkin(_characterBiped);
				AimController.SetTPSView();
			}
		}

		public void SetCharacterSkin(BipedMap newBipedMap)
		{
			if (Singleton<Trash>.Instance.Contains(newBipedMap.transform, base.transform))
			{
				Singleton<Trash>.Instance.Return(newBipedMap.transform, base.transform);
			}
			newBipedMap.transform.SetParent(base.transform);
			newBipedMap.transform.localPosition = Vector3.zero;
			newBipedMap.transform.localRotation = Quaternion.identity;
			if (!newBipedMap.gameObject.activeSelf)
			{
				newBipedMap.gameObject.SetActive(value: true);
			}
			this.CharacterSkinSetEvent(newBipedMap);
			if (BipedMap != null && !Singleton<Trash>.Instance.Contains(BipedMap.transform, base.transform))
			{
				Singleton<Trash>.Instance.Drop(BipedMap.transform);
			}
			BipedMap = newBipedMap;
			if (ViewMode == ViewMode.FPS)
			{
				_armsBiped = newBipedMap;
			}
			if (ViewMode == ViewMode.TPS)
			{
				_characterBiped = newBipedMap;
			}
			SetCharacterVisible(IsCharacterVisible);
		}

		public void SetMainCamera([NotNull] Camera mainCamera)
		{
			if (mainCamera == null)
			{
				throw new ArgumentNullException("mainCamera");
			}
			_mainCamera = (mainCamera.GetComponent<PlayerMainCamera>() ?? mainCamera.gameObject.AddComponent<PlayerMainCamera>());
			_mainCamera.AttachToPlayer(this, _mainCameraHolder.transform);
		}

		public void ResetMainCamera()
		{
			_mainCamera = null;
		}

		public void SetFpsCamera([NotNull] Camera fpsCamera)
		{
			if (fpsCamera == null)
			{
				throw new ArgumentNullException("fpsCamera");
			}
			_fpsCamera = (fpsCamera.GetComponent<PlayerFPSCamera>() ?? fpsCamera.gameObject.AddComponent<PlayerFPSCamera>());
			_fpsCamera.AttachToPlayer(this, _fpsCameraHolder.transform);
		}

		public void ResetFpsCamera()
		{
			_fpsCamera = null;
		}

		public void SetInputs(PlayerInputs inputs, float deltaTime)
		{
			IsSynchronized = false;
			LocalTime += deltaTime;
			MovementControllerCmd movementControllerCmd = new MovementControllerCmd();
			movementControllerCmd.Horizontal = inputs.Horizontal;
			movementControllerCmd.Vertical = inputs.Vertical;
			movementControllerCmd.IsToJump = inputs.IsToJump;
			movementControllerCmd.IsCrouching = inputs.IsCrouching;
			MovementControllerCmd commands = movementControllerCmd;
			AimControlCmd aimControlCmd = new AimControlCmd();
			aimControlCmd.AimType = AimType.DeltaAngle;
			aimControlCmd.DeltaAimAngles = inputs.DeltaAimAngles;
			AimControlCmd commands2 = aimControlCmd;
			WeaponryControllerCmd weaponryControllerCmd = new WeaponryControllerCmd();
			weaponryControllerCmd.SlotIndex = inputs.SwitchToWeapon;
			weaponryControllerCmd.ToDrop = inputs.IsToDrop;
			weaponryControllerCmd.WeaponControllerCmd = new WeaponControllerCmd
			{
				ToFire = inputs.IsToFire,
				ToReload = inputs.IsToReload,
				ToAim = inputs.IsToAim,
				ToAction = inputs.IsToAction
			};
			WeaponryControllerCmd commands3 = weaponryControllerCmd;
			AimController.ExecuteCommands(commands2, deltaTime, LocalTime);
			MovementController.ExecuteCommands(commands, deltaTime, LocalTime);
			MecanimController.ExecuteCommands(new MecanimControllerCmd(), deltaTime, LocalTime);
			WeaponryController.ExecuteCommands(commands3, deltaTime, LocalTime);
		}

		public void SetSnapsot(PlayerSnapshot snapshot)
		{
			_lastReceivedSnapshot = snapshot;
			IsSynchronized = true;
			MecanimController.SetSnapshot(snapshot.mecanimSnapshot);
			AimController.SetSnapshot(snapshot.aimingSnapshot);
			MovementController.SetSnapshot(snapshot.movementSnapshot);
			WeaponryController.SetSnapshot(snapshot.WeaponrySnapshot);
		}

		public PlayerSnapshot GetSnapshot()
		{
			PlayerSnapshot playerSnapshot = new PlayerSnapshot();
			playerSnapshot.time = LocalTime;
			playerSnapshot.mecanimSnapshot = MecanimController.GetSnapshot();
			playerSnapshot.movementSnapshot = MovementController.GetSnapshot();
			playerSnapshot.aimingSnapshot = AimController.GetSnapshot();
			playerSnapshot.WeaponrySnapshot = WeaponryController.GetSnapshot();
			return playerSnapshot;
		}

		public override void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			_photonPlayerId = PhotonView.ownerId;
			_characterPlayer.enabled = !PhotonView.isMine;
			PlayerOcclusionController.SetEnabled(!PhotonView.isMine);
			if (!PhotonView.isMine)
			{
				Singleton<PlayerManager>.Instance.OnRemoteInstantiateEvent(this);
			}
		}

		internal void Instantiate(int health, int armor, bool hasHelmet)
		{
			if (!PhotonView.isMine)
			{
				Log.Error("Can't Instantiate remote player");
			}
			else if (GetHealth() > 0)
			{
				Log.Error("Can't instantiate player, already instantiated!");
			}
			else
			{
				PhotonView.RPC("InstantiateViaServer", PhotonTargets.AllBufferedViaServer, health, armor, hasHelmet);
			}
		}

		[PunRPC]
		private void InstantiateViaServer(int health, int armor, bool hasHelmet)
		{
			PhotonPlayer owner = PhotonView.owner;
			owner.SetHealth(health);
			owner.SetArmor(armor);
			owner.SetHelmet(hasHelmet);
			if (PhotonView.isMine)
			{
				OnLocalInstantiate();
			}
		}

		private void OnLocalInstantiate()
		{
			Singleton<PlayerManager>.Instance.OnLocalInstantiateEvent(this);
			NetworkController.OnInstantiated();
			WeaponryController.OnInstantiated();
			SetCharacterVisible(isEnabled: true);
		}

		public int GetHealth()
		{
			return _photonView.owner.GetHealth();
		}

		public int GetArmor()
		{
			return _photonView.owner.GetArmor();
		}

		public bool HasHelmet()
		{
			return _photonView.owner.HasHelmet();
		}

		public bool IsDeath()
		{
			return _photonView.owner.IsDead();
		}

		public void Die()
		{
			PhotonView.RPC("DieViaServer", PhotonTargets.AllViaServer);
		}

		[PunRPC]
		private void DieViaServer()
		{
			PhotonPlayer owner = PhotonView.owner;
			if (owner.IsLocal)
			{
				KillPlayer();
			}
			Singleton<HitManager>.Instance.Suicide(owner);
		}

		public void KillPlayer()
		{
			WeaponryController.DropAllWeapons();
			DestroyPlayer();
		}

		public void DestroyPlayer()
		{
			if (!PhotonView.isMine)
			{
				Log.Error("Can't destroy remote player");
			}
			else
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
		}

		public void SetCharacterVisible(bool isEnabled)
		{
			IsCharacterVisible = isEnabled;
			HitController.Trigger.enabled = isEnabled;
			WeaponryController.SetWeaponsVisible(isEnabled);
			SkinnedMeshLodGroup requireComponent = BipedMap.GetRequireComponent<SkinnedMeshLodGroup>();
			if (isEnabled)
			{
				requireComponent.EnableSkinnedMeshRenderer();
			}
			else
			{
				requireComponent.DisableSkinnedMeshRenderer();
			}
		}
	}
}
