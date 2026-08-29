using Axlebolt.Common.States;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Inventory.HitHandling;
using Axlebolt.Standoff.Inventory.Weapon;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Mecanim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Gun
{
	[RequireComponent(typeof(WeaponMap))]
	public class GunController : WeaponController
	{
		public enum StateType
		{
			Reloading,
			PreReload,
			Switching,
			TryToSwitch,
			Ready
		}

		public enum AimingMode
		{
			Aiming,
			AimingReload,
			NotAiming,
			StartingAiming,
			FinishingAiming
		}

		private static readonly Log Log = Log.Create(typeof(GunController));

		private float _fireInterval;

		private float _lastFiredTime;

		private int _audioSourceIndex;

		private bool _wasShoot;

		private SafeShort _capacity;

		private SafeShort _magazineCapacity;

		private RecoilData _recoilData;

		private RecoilControl _recoilControl;

		private float _lastHandledShotTime;

		private GunSnapshot _syncData = new GunSnapshot();

		private readonly StateSimple<StateType> _state = new StateSimple<StateType>();

		private readonly StateSimple<AimingMode> _aimingMode = new StateSimple<AimingMode>();

		private readonly List<ShotData> _shotBuffer = new List<ShotData>();

		private readonly List<ShotData> _unhandledShotBuffer = new List<ShotData>();

		private const int MaxUnhandledShotBufferLength = 10;

		private Transform _cameraTransform;

		private Transform _muzzlePoint;

		private readonly float _switchAnimConfirmDuration = 0.5f;

		private readonly float _reloadAnimConfirmDuration = 0.5f;

		private float _accuracyAdditive = 1f;

		private AccuracyData _accuracyData = new AccuracyData();

		private float _recoilMult;

		private float _accuracyMult;

		private float _reloadCycle;

		private bool _isRoundTaken;

		public float TimeFired
		{
			get;
			private set;
		}

		public int ShotId
		{
			get;
			private set;
		} = -1;


		public bool InfinityAmmo
		{
			get;
			set;
		}

		public float RecoilProgress
		{
			get
			{
				if (_recoilControl != null)
				{
					return _recoilControl.Progress;
				}
				return 0f;
			}
		}

		public GunParameters GunParameters
		{
			get;
			private set;
		}

		public GunSightViewControl SightViewControl
		{
			get;
			private set;
		}

		public AimingMode CurrentAimingMode
		{
			[CompilerGenerated]
			get
			{
				return _aimingMode.curState;
			}
		}

		public override WeaponType WeaponType
		{
			[CompilerGenerated]
			get
			{
				return WeaponType.Gun;
			}
		}

		public short Capacity
		{
			get
			{
				return (!InfinityAmmo) ? _capacity.Value : GunParameters.Ammunition.Capacity;
			}
			set
			{
				if (!InfinityAmmo)
				{
					_capacity.Value = value;
				}
			}
		}

		public short MagazineCapacity
		{
			get
			{
				return _magazineCapacity.Value;
			}
			set
			{
				_magazineCapacity.Value = value;
			}
		}

		internal override void PreInitialize(WeaponParameters weaponParameters, WeaponAnimationParameters animationParameters)
		{
			if (!(weaponParameters is GunParameters))
			{
				throw new ArgumentException("Invalid weaponParameters, expected Weapon actual is " + weaponParameters);
			}
			base.PreInitialize(weaponParameters, animationParameters);
			GunParameters = (GunParameters)weaponParameters;
			_fireInterval = 60f / (float)GunParameters.FireRate;
			_magazineCapacity = new SafeShort(GunParameters.Ammunition.MagazineCapacity);
			_capacity = new SafeShort(GunParameters.Ammunition.Capacity);
			TimeFired = 0f;
			_recoilControl = new RecoilControl(GunParameters);
			SightViewControl = new GunSightViewControl(GunParameters);
			AnimationController.PreInitialize(this);
			ConfigureMuzzlePoint();
		}

		internal override void Initialize(InventoryItemId skinId)
		{
			base.Initialize(skinId);
			_magazineCapacity = new SafeShort(GunParameters.Ammunition.MagazineCapacity);
			_capacity = new SafeShort(GunParameters.Ammunition.Capacity);
			_recoilControl = new RecoilControl(GunParameters);
			TimeFired = 0f;
			_shotBuffer.Clear();
			_unhandledShotBuffer.Clear();
			_lastHandledShotTime = -1f;
			_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
			_accuracyMult = 1f;
			_recoilMult = 1f;
		}

		private void ConfigureMuzzlePoint()
		{
			if (base.WeaponMap.MuzzlePoint == null)
			{
				Log.Error($"Weapon {base.gameObject.name} is not valid, {WeaponMap.WeaponPart.MuzzlePoint} not found");
			}
			else
			{
				_muzzlePoint = base.WeaponMap.MuzzlePoint.transform;
			}
		}

		private bool CanShoot()
		{
			if (GunParameters.ReloadType == ReloadType.Magazine && _state.curState == StateType.Ready)
			{
				return true;
			}
			if (GunParameters.ReloadType == ReloadType.SingleRound && MagazineCapacity > 0)
			{
				_state.SetState(StateType.Ready, base.LocalTime);
				return true;
			}
			return false;
		}

		public override void ExecuteCommands(WeaponControllerCmd commands, float duration, float time)
		{
			base.ExecuteCommands(commands, duration, time);
			LocalUpdate();
			if (commands.ToFire && CanShoot())
			{
				Fire();
			}
			if (commands.ToReload && CanReload())
			{
				_state.SetState(StateType.PreReload, base.LocalTime);
			}
			LocalPostUpdate();
		}

		private bool CanReload()
		{
			return _state.curState == StateType.Ready && Capacity > 0 && MagazineCapacity < GunParameters.Ammunition.MagazineCapacity && (GunParameters.SightType != SightType.CollimatorSight || (GunParameters.SightType == SightType.CollimatorSight && (_aimingMode.curState == AimingMode.Aiming || _aimingMode.curState == AimingMode.NotAiming)));
		}

		private bool IsCollimatorAiming()
		{
			return GunParameters.SightType == SightType.CollimatorSight && _aimingMode.curState != AimingMode.NotAiming;
		}

		public override WeaponSnapshot GetSnapshot()
		{
			_syncData.MagazineAmmo = MagazineCapacity;
			_syncData.RestAmmo = Capacity;
			_syncData.ShootDataList.Clear();
			_syncData.ShootDataList.AddRange(_shotBuffer);
			_shotBuffer.Clear();
			return _syncData;
		}

		private void ExpandUnhandledShotBuffer(List<ShotData> newShotDataList)
		{
			foreach (ShotData newShotData in newShotDataList)
			{
				if (!_unhandledShotBuffer.Contains(newShotData))
				{
					_unhandledShotBuffer.Add(newShotData);
				}
			}
			if (_unhandledShotBuffer.Count > 20)
			{
				_unhandledShotBuffer.RemoveRange(0, _unhandledShotBuffer.Count - 20);
			}
		}

		public override void SetSnapshot(WeaponSnapshot parameters)
		{
			base.SetSnapshot(parameters);
			_syncData = (GunSnapshot)parameters;
			MagazineCapacity = _syncData.MagazineAmmo;
			Capacity = _syncData.RestAmmo;
			ExpandUnhandledShotBuffer(_syncData.ShootDataList);
			List<ShotData> unhandledShots = GetUnhandledShots(_unhandledShotBuffer, _syncData.Time);
			if (unhandledShots.Count > 0)
			{
				foreach (ShotData item in unhandledShots)
				{
					if (item.BulletCastDataList.Length > 0)
					{
						CastBullets(item.BulletCastDataList[0].StartPosition, item.BulletCastDataList[0].Direction, item.BulletCastDataList.ToList(), isLocal: false);
					}
					PlayShootEffect(isLocal: false);
				}
				_lastHandledShotTime = unhandledShots[unhandledShots.Count - 1].ShootTime;
			}
		}

		private List<ShotData> GetUnhandledShots(List<ShotData> shootList, float time)
		{
			List<ShotData> list = new List<ShotData>();
			for (int i = 0; i < shootList.Count; i++)
			{
				if (_lastHandledShotTime < shootList[i].ShootTime && shootList[i].ShootTime <= time)
				{
					list.Add(shootList[i]);
				}
			}
			return list;
		}

		private void Reload()
		{
			_reloadCycle = 1f;
			_isRoundTaken = true;
			MecanimController.SetReloading();
		}

		private void PlayShotSound()
		{
			_lastFiredTime = base.LocalTime;
			if (ShotAudioData.AudioSources == null || ShotAudioData.AudioSources.Count == 0)
			{
				if (ShotAudioData.ShootClip != null)
				{
					AudioSource.PlayClipAtPoint(ShotAudioData.ShootClip, base.transform.position, ShotAudioData.ShootVolume);
				}
				return;
			}
			_audioSourceIndex %= ShotAudioData.AudioSources.Count;
			var src = ShotAudioData.AudioSources[_audioSourceIndex];
			if (src != null && ShotAudioData.ShootClip != null)
			{
				src.spatialBlend = 1f;
				src.maxDistance = 100f;
				src.PlayOneShot(ShotAudioData.ShootClip);
			}
			_audioSourceIndex++;
		}

		private List<BulletCastData> GetShotgunShots(List<ShotgunShellParameters.ShotDispertion> dispertionCircles, Vector3 originPoint, Vector3 direction, Vector3 relativeXAxis, Vector3 relativeYAxis, float angleDispertion)
		{
			List<BulletCastData> list = new List<BulletCastData>();
			float min = 0f;
			foreach (ShotgunShellParameters.ShotDispertion dispertionCircle in dispertionCircles)
			{
				for (int i = 0; i < dispertionCircle.Sectors; i++)
				{
					float d = UnityEngine.Random.Range(min, dispertionCircle.RadiusRatio);
					if (dispertionCircle.Sectors <= 0)
					{
						UnityEngine.Debug.LogError("Shot dispertion sector count must be > 0");
						break;
					}
					float num = 360f / (float)dispertionCircle.Sectors;
					float num2 = (float)i / (float)dispertionCircle.Sectors;
					float num3 = 360f * num2;
					float angle = UnityEngine.Random.Range(num3, num3 + num);
					Vector3 a = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
					a *= d;
					a *= angleDispertion;
					Vector3 point = Quaternion.AngleAxis(a.y, relativeYAxis) * direction;
					point = Quaternion.AngleAxis(0f - a.x, relativeXAxis) * point;
					list.Add(new BulletCastData
					{
						StartPosition = originPoint,
						Direction = point
					});
				}
				min = dispertionCircle.RadiusRatio;
			}
			return list;
		}

		private void GetRelativeDispertion(ref RecoilData recoilData, float accuracyAdditive, float angleDispertion)
		{
			float max = 1f;
			float angle = UnityEngine.Random.Range(0f, 360f);
			Vector3 a = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
			a *= UnityEngine.Random.Range(0f, max);
			a *= angleDispertion * _accuracyMult + accuracyAdditive;
			recoilData.XDeviation *= _recoilMult;
			recoilData.YDeviation *= _recoilMult;
			recoilData.XDeviation += a.x;
			recoilData.YDeviation += a.y;
		}

		private void Fire()
		{
			float num = base.LocalTime - TimeFired;
			if (!(num < _fireInterval) && MagazineCapacity > 0)
			{
				ShotId++;
				MagazineCapacity--;
				if (TimeFired + _fireInterval > base.LocalTime - base.DeltaTime)
				{
					TimeFired += _fireInterval;
				}
				else
				{
					TimeFired = base.LocalTime;
				}
				_recoilData = _recoilControl.GetNextShot(false, TimeFired);
				if (GunParameters.AmmunitionType == AmmunitionType.ShotgunShell)
				{
					GetRelativeDispertion(ref _recoilData, _accuracyAdditive, 0f);
				}
				else
				{
					GetRelativeDispertion(ref _recoilData, _accuracyAdditive, _recoilData.AngleDispertion);
				}
				Vector3 forward = _cameraTransform.forward;
				Vector3 right = _cameraTransform.right;
				Vector3 up = _cameraTransform.up;
				forward = Quaternion.AngleAxis(_recoilData.YDeviation, up) * forward;
				forward = Quaternion.AngleAxis(0f - _recoilData.XDeviation, right) * forward;
				Vector3 position = _cameraTransform.position;
				List<BulletCastData> list = new List<BulletCastData>();
				if (GunParameters.AmmunitionType == AmmunitionType.Cartridge)
				{
					list.Add(new BulletCastData
					{
						StartPosition = position,
						Direction = forward
					});
				}
				if (GunParameters.AmmunitionType == AmmunitionType.ShotgunShell)
				{
					list = GetShotgunShots(GunParameters.ShotgunShellParameters.ShotDispertionCircles, position, forward, right, up, _recoilData.AngleDispertion * _accuracyMult);
				}
				CastBullets(position, forward, list, isLocal: true);
				PlayShootEffect(isLocal: true);
				AnimationController.OnWeaponShoot(base.LocalTime);
				if (GunParameters.SightType != SightType.CollimatorSight || (GunParameters.SightType == SightType.CollimatorSight && _aimingMode.curState == AimingMode.NotAiming))
				{
					MecanimController.SetShooting();
				}
				ShotData shotData = new ShotData();
				shotData.ShootTime = _lastFiredTime;
				shotData.BulletCastDataList = list.ToArray();
				_shotBuffer.Add(shotData);
			}
		}

		private void PlayShootEffect(bool isLocal)
		{
			PlayShotSound();
			if (_viewMode == ViewMode.TPS)
			{
				base.OnCharacterSpineRotationApplied += delegate
				{
					EmitMuzzleFlash(isLocal);
				};
			}
			else
			{
				EmitMuzzleFlash(isLocal);
			}
			if (isLocal)
			{
				Singleton<WeaponManager>.Instance.OnShootEvent(this);
			}
		}

		private void CastBullets(Vector3 originPoint, Vector3 originDirection, List<BulletCastData> bulletCastDataList, bool isLocal)
		{
			WeaponHitParameters parameters = WeaponHitParameters.Create(base.WeaponId, base.SkinId, GunParameters.PenetrationPower, GunParameters.ArmorPenetration, GunParameters.Impulse, GunParameters.Damage);
			List<HitCasterResult> list = new List<HitCasterResult>();
			Dictionary<PlayerHitController, List<BulletHitData>> dictionary = new Dictionary<PlayerHitController, List<BulletHitData>>();
			foreach (BulletCastData bulletCastData in bulletCastDataList)
			{
				list.Clear();
				Vector3 endPosition = HitCaster.CastHit(bulletCastData.StartPosition, bulletCastData.Direction, parameters, isLocal, list);
				foreach (HitCasterResult item in list)
				{
					if (!dictionary.ContainsKey(item.PlayerHitController))
					{
						dictionary[item.PlayerHitController] = new List<BulletHitData>();
					}
					dictionary[item.PlayerHitController].Add(item.BulletHitData);
				}
				Singleton<BulletTraceEmitter>.Instance.Emit(bulletCastData.StartPosition, endPosition, GunParameters.BulletTraceType);
			}
			foreach (PlayerHitController key in dictionary.Keys)
			{
				List<BulletHitData> list2 = dictionary[key];
				HitData hitData = new HitData();
				hitData.Direction = originDirection;
				hitData.WeaponId = base.WeaponId;
				hitData.SkinId = base.SkinId;
				hitData.Hits = list2.ToArray();
				HitData hitData2 = hitData;
				key.Hit(hitData2);
			}
		}

		private void EmitMuzzleFlash(bool isLocal)
		{
			Singleton<MuzzleFlashEmitter>.Instance.Emit(GunParameters.MuzzleFlashType, _muzzlePoint.position, _muzzlePoint.forward, isLocal);
		}

		public override void SetAsDefault(float time)
		{
			base.SetAsDefault(time);
			_state.SetState(StateType.TryToSwitch, base.LocalTime);
			SetDefaultAimingState();
		}

		public override void SetPlayer(PlayerController playerController)
		{
			base.SetPlayer(playerController);
			base.transform.SetParent(playerController.BipedMap.RightHand);
		}

		private void SetDefaultAimingState()
		{
			if (GunParameters.SightType == SightType.CollimatorSight)
			{
				_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
				base.WeaponMap.SightLense.SetActive(value: true);
				base.WeaponMap.SightReticle.SetActive(value: false);
				base.WeaponMap.Sight.SetActive(value: true);
			}
			if (GunParameters.SightType == SightType.SniperScope)
			{
				_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
			}
		}

		private bool IsFiring()
		{
			return base.LocalTime - TimeFired < _fireInterval;
		}

		private void SetAiming()
		{
			PlayerController.FpsCameraHolder.SetActive(value: false);
		}

		private void SetNotAiming()
		{
			PlayerController.FpsCameraHolder.SetActive(value: true);
		}

		private void EnableCollimatorSightParts(float progress)
		{
			if (progress > GunParameters.GunSightViewParameters.SightEnableNt)
			{
				base.WeaponMap.Sight.SetActive(value: true);
			}
			if (progress > GunParameters.GunSightViewParameters.SightLenseEnableNt)
			{
				base.WeaponMap.SightLense.SetActive(value: true);
			}
		}

		private void DisableCollimatorSightParts(float progress)
		{
			if (progress > GunParameters.GunSightViewParameters.SightDisbleNt)
			{
				base.WeaponMap.Sight.SetActive(value: false);
			}
			if (progress > GunParameters.GunSightViewParameters.SightLenseDisbleNt)
			{
				base.WeaponMap.SightLense.SetActive(value: false);
			}
		}

		private void AimingControl()
		{
			if (GunParameters.SightType == SightType.SniperScope)
			{
				if (_aimingMode.curState == AimingMode.Aiming)
				{
					if (Commands.ToAim)
					{
						_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
						SetNotAiming();
					}
					if (IsFiring() || _state.curState != StateType.Ready)
					{
						_aimingMode.SetState(AimingMode.AimingReload, base.LocalTime);
						SetNotAiming();
					}
					return;
				}
				if (_aimingMode.curState == AimingMode.AimingReload)
				{
					if (!IsFiring() && _state.curState == StateType.Ready)
					{
						_aimingMode.SetState(AimingMode.Aiming, base.LocalTime);
						SetAiming();
					}
					return;
				}
				if (_aimingMode.curState == AimingMode.NotAiming && _state.curState == StateType.Ready && !IsFiring() && Commands.ToAim)
				{
					_aimingMode.SetState(AimingMode.Aiming, base.LocalTime);
					SetAiming();
				}
			}
			if (GunParameters.SightType != SightType.CollimatorSight)
			{
				return;
			}
			if (_aimingMode.curState == AimingMode.Aiming)
			{
				float progress = 1f;
				SightViewControl.Evaluate(base.LocalTime, base.DeltaTime, progress, GunSightViewControl.States.Neared);
				DisableCollimatorSightParts(progress);
				if (_state.curState != StateType.Ready)
				{
					SetDefaultAimingState();
					SightViewControl.Reset();
					MecanimController.SetAimingState(isAimingState: false);
				}
				if (Commands.ToAim)
				{
					_aimingMode.SetState(AimingMode.FinishingAiming, base.LocalTime);
					SightViewControl.FinishAiming(base.LocalTime);
					MecanimController.CancelAiming();
				}
			}
			else if (_aimingMode.curState == AimingMode.StartingAiming)
			{
				float num = (base.LocalTime - _aimingMode.timeSwitched) / GunParameters.GunSightViewParameters.AimingStartDuration;
				DisableCollimatorSightParts(num);
				SightViewControl.Evaluate(base.LocalTime, base.DeltaTime, num, GunSightViewControl.States.Neared);
				if (num > 1f)
				{
					_aimingMode.SetState(AimingMode.Aiming, base.LocalTime);
				}
			}
			else if (_aimingMode.curState == AimingMode.FinishingAiming)
			{
				float num2 = (base.LocalTime - _aimingMode.timeSwitched) / GunParameters.GunSightViewParameters.AimingEndDuration;
				EnableCollimatorSightParts(num2);
				SightViewControl.Evaluate(base.LocalTime, base.DeltaTime, 1f - num2, GunSightViewControl.States.Default);
				if (num2 > 1f)
				{
					_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
					MecanimController.SetAimingState(isAimingState: false);
				}
			}
			else if (_aimingMode.curState == AimingMode.NotAiming)
			{
				float progress2 = 0f;
				EnableCollimatorSightParts(progress2);
				SightViewControl.Evaluate(base.LocalTime, base.DeltaTime, progress2, GunSightViewControl.States.Default);
				if (Commands.ToAim && _state.curState == StateType.Ready)
				{
					_aimingMode.SetState(AimingMode.StartingAiming, base.LocalTime);
					MecanimController.SetAimingState(isAimingState: true);
					MecanimController.SetAiming();
					SightViewControl.StartAiming(base.LocalTime);
				}
			}
		}

		private void AccuracyControl()
		{
			float actual = PlayerController.MovementController.translationData.standTypeCoeff.actual;
			float magnitude = PlayerController.MovementController.translationData.velocity.magnitude;
			if (actual > 0.5f)
			{
				if (IsCollimatorAiming())
				{
					_recoilMult = Mathf.Lerp(_recoilMult, GunParameters.RecoilAimMult, base.DeltaTime * 20f);
					_accuracyMult = Mathf.Lerp(_accuracyMult, GunParameters.AccuracyAimMult, base.DeltaTime * 20f);
				}
				else
				{
					_recoilMult = Mathf.Lerp(_recoilMult, 1f, base.DeltaTime * 20f);
					_accuracyMult = Mathf.Lerp(_accuracyMult, 1f, base.DeltaTime * 20f);
				}
			}
			else if (IsCollimatorAiming())
			{
				_recoilMult = Mathf.Lerp(_recoilMult, GunParameters.RecoilAimMultOnCrouch, base.DeltaTime * 20f);
				_accuracyMult = Mathf.Lerp(_accuracyMult, GunParameters.AccuracyAimMultOnCrouch, base.DeltaTime * 20f);
			}
			else
			{
				_recoilMult = Mathf.Lerp(_recoilMult, GunParameters.RecoilMultOnCrouch, base.DeltaTime * 20f);
				_accuracyMult = Mathf.Lerp(_accuracyMult, GunParameters.AccuracyMultOnCrouch, base.DeltaTime * 20f);
			}
			bool isPistol = GunParameters.FireRate < 500 && GunParameters.AmmunitionType == AmmunitionType.Cartridge;
			if (magnitude > 0.5f)
			{
				float speedRatio = Mathf.Clamp01(magnitude / 10f);
				float movePenalty = isPistol ? (1f + speedRatio * 0.15f) : (1f + speedRatio * 0.5f);
				_accuracyMult *= movePenalty;
				_recoilMult *= (1f + speedRatio * 0.25f);
			}
			_accuracyAdditive = Mathf.Lerp(_accuracyAdditive, GunParameters.AccuracyAdditiveCurve.Evaluate(magnitude) * _accuracyMult, base.DeltaTime * 20f);
			if (GunParameters.SightType == SightType.CollimatorSight && _aimingMode.curState == AimingMode.NotAiming)
			{
				_accuracyAdditive = Mathf.Lerp(_accuracyAdditive, GunParameters.AdditiveNoAimDispertionCurve.Evaluate(magnitude) * _accuracyMult, 20f);
			}
			if (GunParameters.SightType == SightType.SniperScope && _aimingMode.curState == AimingMode.NotAiming)
			{
				_accuracyAdditive = GunParameters.AdditiveNoAimDispertionCurve.Evaluate(magnitude);
			}
		}

		private void MagazineGunReloadControl()
		{
			if (base.LocalTime - _state.timeSwitched > GunParameters.ReloadDuration)
			{
				_state.SetState(StateType.Ready, base.LocalTime);
			}
			else if (base.LocalTime - _state.timeSwitched < _reloadAnimConfirmDuration && !MecanimController.IsReloadingWeapon(base.WeaponName))
			{
				MecanimController.SetWeaponNO(base.WeaponNumId);
				MecanimController.SetReloading();
			}
			if (base.LocalTime - _state.timeSwitched > GunParameters.MagazineInsertTime)
			{
				Capacity += MagazineCapacity;
				MagazineCapacity = 0;
				if (Capacity <= GunParameters.Ammunition.MagazineCapacity)
				{
					MagazineCapacity = Capacity;
					Capacity = 0;
				}
				else
				{
					MagazineCapacity = GunParameters.Ammunition.MagazineCapacity;
					Capacity -= GunParameters.Ammunition.MagazineCapacity;
				}
			}
		}

		private void SingleRoundGunReloadControl()
		{
			float num = base.LocalTime - _state.timeSwitched - GunParameters.ReloadStartOffset - _reloadCycle * GunParameters.RoundInsertDuration;
			if (num > GunParameters.RoundInsertOffset && _isRoundTaken)
			{
				MagazineCapacity++;
				Capacity--;
				_isRoundTaken = false;
				if (Capacity == 0 || MagazineCapacity == GunParameters.Ammunition.MagazineCapacity)
				{
					_state.SetState(StateType.Ready, base.LocalTime);
					MecanimController.FinishReload();
					return;
				}
			}
			if (num > 0f)
			{
				_reloadCycle += 1f;
				MecanimController.InsertRound();
				_isRoundTaken = true;
			}
		}

		private void GunStateControl()
		{
			if (_state.curState == StateType.PreReload && base.LocalTime - TimeFired > GunParameters.PreReloadTime)
			{
				Reload();
				_state.SetState(StateType.Reloading, base.LocalTime);
			}
			if (_state.curState == StateType.Reloading)
			{
				if (GunParameters.ReloadType == ReloadType.Magazine)
				{
					MagazineGunReloadControl();
				}
				if (GunParameters.ReloadType == ReloadType.SingleRound)
				{
					SingleRoundGunReloadControl();
				}
				return;
			}
			if (_state.curState == StateType.TryToSwitch)
			{
				_state.SetState(StateType.Switching, base.LocalTime);
				return;
			}
			if (_state.curState == StateType.Switching)
			{
				if (base.LocalTime - _state.timeSwitched > GunParameters.TakeDuration)
				{
					_state.SetState(StateType.Ready, base.LocalTime);
				}
				else if (base.LocalTime - _state.timeSwitched < _switchAnimConfirmDuration && !MecanimController.IsSwitchingWeapon(base.WeaponName))
				{
					MecanimController.SwitchWeapon(base.WeaponNumId);
				}
			}
			if (_state.curState == StateType.Ready && MagazineCapacity <= 0 && Capacity > 0)
			{
				if (IsCollimatorAiming())
				{
					Reload();
					_state.SetState(StateType.Reloading, base.LocalTime);
				}
				else
				{
					_state.SetState(StateType.PreReload, base.LocalTime);
				}
			}
		}

		private void LocalUpdate()
		{
			AccuracyControl();
			GunStateControl();
			if (GunParameters.SightType == SightType.SniperScope || GunParameters.SightType == SightType.CollimatorSight)
			{
				AimingControl();
			}
		}

		public override AccuracyData GetAccuracyData()
		{
			return _accuracyData;
		}

		private void LocalPostUpdate()
		{
			MecanimController.SetRecoilProgress(AnimationController.EvaluateRecoilAnimation(base.LocalTime));
			_cameraTransform = PlayerController.MainCameraHolder.transform;
			_cameraTransform.localPosition = Vector3.zero;
			_cameraTransform.localRotation = Quaternion.identity;
			RecoilData currentDeviation = _recoilControl.GetCurrentDeviation(base.LocalTime);
			currentDeviation.XDeviation *= _recoilMult;
			currentDeviation.YDeviation *= _recoilMult;
			_accuracyData.RecoilAngle = Mathf.Max(currentDeviation.XDeviation, currentDeviation.YDeviation);
			_accuracyData.AccuracyAngle = currentDeviation.AngleDispertion + _accuracyAdditive;
			_cameraTransform.localRotation *= Quaternion.AngleAxis(currentDeviation.YDeviation * GunParameters.RecoilParameters.cameraDeviationCoeff, Vector3.up);
			_cameraTransform.localRotation *= Quaternion.AngleAxis((0f - currentDeviation.XDeviation) * GunParameters.RecoilParameters.cameraDeviationCoeff, Vector3.right);
			Transform cameraTransform = _cameraTransform;
			Vector3 localEulerAngles = _cameraTransform.localEulerAngles;
			float x = localEulerAngles.x;
			Vector3 localEulerAngles2 = _cameraTransform.localEulerAngles;
			cameraTransform.localEulerAngles = new Vector3(x, localEulerAngles2.y, 0f);
		}

		private void MeshCombineSplitTracing()
		{
			if (MecanimController.IsIdlingWeapon(base.WeaponName))
			{
				base.LodGroup.CombineMesh();
				if (!base.LodGroup.Combined)
				{
					base.LodGroup.CombineMesh();
				}
			}
			else
			{
				base.LodGroup.SplitMesh();
				if (base.LodGroup.Combined)
				{
					base.LodGroup.SplitMesh();
				}
			}
		}

		public override void OnAnimatorStateSet()
		{
			MecanimLayerInfo currentCharacterWeaponAnimationState = GetCurrentCharacterWeaponAnimationState();
			AnimationController.Play(currentCharacterWeaponAnimationState.stateNameHash, currentCharacterWeaponAnimationState.stateNormalizedTime, currentCharacterWeaponAnimationState.IsSynchronized, PlayerController.PlayerOcclusionController.IsVisible, base.DeltaTime);
			MeshCombineSplitTracing();
		}
	}
}
