using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Sound;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[RequireComponent(typeof(MeshLodGroup))]
	[RequireComponent(typeof(WeaponAnimationController))]
	[RequireComponent(typeof(WeaponMap))]
	public abstract class WeaponController : MonoBehaviour
	{
		public enum HandleState
		{
			Primary,
			Secondary,
			NotStated
		}

		internal const int FpsLodLayer = 0;

		internal const int TpsLodLayer = 1;

		internal GameObject WeaponHierarchy;

		internal List<AudioSource> AnimationAudioSources = new List<AudioSource>();

		protected ShotAudioData ShotAudioData;

		internal PlayerController PlayerController;

		internal MecanimController MecanimController;

		internal WeaponAnimationController AnimationController;

		internal WeaponAnimationParameters AnimationParameters;

		protected ViewMode _viewMode;

		private Transform _transform;

		protected WeaponControllerCmd Commands;

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

		public byte InventoryInstanceId
		{
			get;
		}

		public MeshLodGroup LodGroup
		{
			get;
			private set;
		}

		public HandleState HandlingState
		{
			get;
			protected set;
		} = HandleState.NotStated;


		public WeaponId WeaponId
		{
			[CompilerGenerated]
			get
			{
				return WeaponParameters.Id;
			}
		}

		public byte WeaponNumId
		{
			[CompilerGenerated]
			get
			{
				return (byte)WeaponParameters.Id;
			}
		}

		public string WeaponName
		{
			[CompilerGenerated]
			get
			{
				return WeaponParameters.name;
			}
		}

		public InventoryItemId SkinId
		{
			get;
			private set;
		}

		public short SkinNumId
		{
			[CompilerGenerated]
			get
			{
				return (short)SkinId;
			}
		}

		public WeaponParameters WeaponParameters
		{
			get;
			private set;
		}

		public WeaponMap WeaponMap
		{
			get;
			private set;
		}

		public abstract WeaponType WeaponType
		{
			get;
		}

		public Transform Transform
		{
			[CompilerGenerated]
			get
			{
				return _transform ?? (_transform = base.transform);
			}
		}

		public byte SlotIndex
		{
			get;
			private set;
		}

		protected event Action OnCharacterSpineRotationApplied = delegate
		{
		};

		protected WeaponController()
		{
			InventoryInstanceId = 0;
		}

		internal MecanimLayerInfo GetCurrentCharacterWeaponAnimationState()
		{
			return MecanimController.GetAnimatorStateInfo(1);
		}

		internal virtual void PreInitialize(WeaponParameters weaponParameters, WeaponAnimationParameters animationParameters)
		{
			SlotIndex = weaponParameters.Id.GetSlotIndex();
			LodGroup = GetComponent<MeshLodGroup>();
			LodGroup.Initialize();
			WeaponMap = GetComponent<WeaponMap>();
			WeaponMap.Initialize();
			WeaponParameters = weaponParameters;
			AnimationParameters = animationParameters;
			AnimationController = GetComponent<WeaponAnimationController>();
			WeaponHierarchy = base.gameObject;
			AnimationController.PreInitialize(this);
			ShotAudioData = new ShotAudioData
			{
				ShootClip = AnimationParameters.shotSound,
				ShootVolume = AnimationParameters.shotSoundVolume
			};
		}

		internal virtual void Initialize(InventoryItemId skinId)
		{
			SkinId = skinId;
			AnimationController.SetActive(isActive: true);
			LocalTime = 0f;
			DeltaTime = 1E-05f;
		}

		public virtual void SetPlayer(PlayerController playerController)
		{
			ClearAudioSources();
			ShotAudioData.AudioSources = playerController.CharacterAudioPlayer.GetAudioSources(AnimationParameters.audioSourcesCount);
			foreach (AudioSource audioSource in ShotAudioData.AudioSources)
			{
				audioSource.volume = ShotAudioData.ShootVolume;
			}
			AnimationAudioSources = playerController.CharacterAudioPlayer.GetAudioSources(AnimationParameters.animationAudioSourcesCount);
			HandlingState = HandleState.NotStated;
			PlayerController = playerController;
			LocalTime = playerController.LocalTime;
			MecanimController = playerController.MecanimController;
			AnimationController.OnPlayerSet(playerController);
			SetViewMode(playerController.AimController.ViewMode);
		}

		public void OnCharacterSkinSet(BipedMap bipedMap)
		{
			AnimationController.OnCharacterSkinSet(bipedMap);
		}

		internal virtual void SetViewMode(ViewMode viewMode)
		{
			_viewMode = viewMode;
			switch (viewMode)
			{
			case ViewMode.FPS:
				WeaponMap.SetLayer(8);
				LodGroup.SetLayer(0);
				foreach (AudioSource animationAudioSource in AnimationAudioSources)
				{
					animationAudioSource.spatialBlend = 0f;
				}
				foreach (AudioSource audioSource in ShotAudioData.AudioSources)
				{
					audioSource.spatialBlend = 0f;
				}
				break;
			case ViewMode.TPS:
				WeaponMap.SetLayer(0);
				LodGroup.SetLayer(1);
				foreach (AudioSource animationAudioSource2 in AnimationAudioSources)
				{
					animationAudioSource2.spatialBlend = 1f;
				}
				foreach (AudioSource audioSource2 in ShotAudioData.AudioSources)
				{
					audioSource2.spatialBlend = 1f;
				}
				break;
			default:
				throw new NotSupportedException("Unsupported view mode " + viewMode);
			}
		}

		public virtual void SetAsDefault(float time)
		{
			LocalTime = time;
			HandlingState = HandleState.Primary;
			AnimationController.OnBecomePrimary();
			base.gameObject.SetActive(value: true);
		}

		public virtual void SetAsSecondary()
		{
			HandlingState = HandleState.Secondary;
			base.gameObject.SetActive(value: false);
		}

		public virtual AccuracyData GetAccuracyData()
		{
			return new AccuracyData();
		}

		public virtual void OnCharacterSpinePostRotation()
		{
			this.OnCharacterSpineRotationApplied();
			this.OnCharacterSpineRotationApplied = delegate
			{
			};
		}

		public virtual void OnAnimatorStateSet()
		{
			MecanimLayerInfo currentCharacterWeaponAnimationState = GetCurrentCharacterWeaponAnimationState();
			AnimationController.Play(currentCharacterWeaponAnimationState.stateNameHash, currentCharacterWeaponAnimationState.stateNormalizedTime, currentCharacterWeaponAnimationState.IsSynchronized, PlayerController.PlayerOcclusionController.IsVisible, Time.deltaTime);
		}

		public virtual void ClearAudioSources()
		{
			if (ShotAudioData.AudioSources.Count > 0)
			{
				ShotAudioData.AudioSources[0].gameObject.GetComponent<AudioPlayer>().ReturnAudioSources(ShotAudioData.AudioSources);
				ShotAudioData.AudioSources.Clear();
			}
			if (AnimationAudioSources.Count > 0)
			{
				AnimationAudioSources[0].gameObject.GetComponent<AudioPlayer>().ReturnAudioSources(AnimationAudioSources);
				AnimationAudioSources.Clear();
			}
		}

		public virtual void OnReturnToPool()
		{
			ClearAudioSources();
		}

		public abstract WeaponSnapshot GetSnapshot();

		public virtual void SetSnapshot(WeaponSnapshot parameters)
		{
			if (LocalTime < 1E-05f)
			{
				LocalTime = parameters.Time;
				DeltaTime = 1E-05f;
			}
			else
			{
				DeltaTime = parameters.Time - LocalTime;
				LocalTime = parameters.Time;
			}
		}

		public virtual void ExecuteCommands(WeaponControllerCmd commands, float duration, float time)
		{
			Commands = commands;
			LocalTime = time;
			DeltaTime = duration;
		}

		public virtual void SetVisible(bool isVisible)
		{
			if (isVisible)
			{
				if (HandlingState == HandleState.Primary)
				{
					base.gameObject.SetActive(value: true);
				}
			}
			else
			{
				base.gameObject.SetActive(value: false);
			}
		}

		public bool IsVisible()
		{
			return base.gameObject.activeSelf;
		}

		public virtual bool IsDroppable()
		{
			return true;
		}
	}
}
