using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Inventory.Gun
{
	[CreateAssetMenu(fileName = "NewGun", menuName = "Standoff/Create Gun", order = 2)]
	public class GunParameters : WeaponParameters
	{
		[Space]
		[SerializeField]
		[Header("Gun Parameters")]
		private SightType _sightType;

		[SerializeField]
		private BulletTraceType _bulletTraceType;

		[SerializeField]
		private ReloadType _reloadType;

		[SerializeField]
		private AmmunitionType _ammunitionType;

		[SerializeField]
		private ShotgunShellParameters _shotgunShellParameters;

		[SerializeField]
		private float _scopeAimSensitivityMult;

		[SerializeField]
		private Ammunition _ammunition;

		[SerializeField]
		private Damage _damage;

		[SerializeField]
		private int _fireRate;

		[SerializeField]
		private int _recoilControl;

		[SerializeField]
		private RecoilParameters _recoilParameters;

		[SerializeField]
		[FormerlySerializedAs("AccuracyAdditiveCurve")]
		private AnimationCurve _accuracyAdditiveCurve;

		[SerializeField]
		private AnimationCurve _additiveNoAimDispertionCurve;

		[SerializeField]
		private float _recoilMultOnCrouch;

		[SerializeField]
		private float _accuracyMultOnCrouch;

		[SerializeField]
		private float _recoilAimMult;

		[SerializeField]
		private float _accuracyAimMult;

		[SerializeField]
		private float _recoilAimMultOnCrouch;

		[SerializeField]
		private float _accuracyAimMultOnCrouch;

		[SerializeField]
		private int _accurateRange;

		[SerializeField]
		private GunSightViewParameters _gunSightViewParameters;

		[Range(0f, 100f)]
		[SerializeField]
		private float _armorPenetration;

		[SerializeField]
		private int _penetrationPower;

		[SerializeField]
		private float _reloadDuration;

		[SerializeField]
		private float _roundInsertDuration;

		[SerializeField]
		private float _reloadStartOffset;

		[SerializeField]
		private float _roundInsertOffset;

		[SerializeField]
		private float _magazineInsertTime;

		[SerializeField]
		private float _takeDuration;

		[SerializeField]
		private float _preReloadTime;

		[SerializeField]
		private int _impulse;

		[SerializeField]
		private MuzzleFlashType _muzzleFlashType;

		public GunType GunType
		{
			[CompilerGenerated]
			get
			{
				return base.Id.GetGunType();
			}
		}

		public SightType SightType
		{
			[CompilerGenerated]
			get
			{
				return _sightType;
			}
		}

		public ReloadType ReloadType
		{
			[CompilerGenerated]
			get
			{
				return _reloadType;
			}
		}

		public AmmunitionType AmmunitionType
		{
			[CompilerGenerated]
			get
			{
				return _ammunitionType;
			}
		}

		public ShotgunShellParameters ShotgunShellParameters
		{
			[CompilerGenerated]
			get
			{
				return _shotgunShellParameters;
			}
		}

		public BulletTraceType BulletTraceType
		{
			[CompilerGenerated]
			get
			{
				return _bulletTraceType;
			}
		}

		public float ScopeAimSensitivityMult
		{
			[CompilerGenerated]
			get
			{
				return _scopeAimSensitivityMult;
			}
		}

		public Ammunition Ammunition
		{
			[CompilerGenerated]
			get
			{
				return _ammunition;
			}
		}

		public Damage Damage
		{
			[CompilerGenerated]
			get
			{
				return _damage;
			}
		}

		public int FireRate
		{
			[CompilerGenerated]
			get
			{
				return _fireRate;
			}
		}

		public RecoilParameters RecoilParameters
		{
			[CompilerGenerated]
			get
			{
				return _recoilParameters;
			}
		}

		public AnimationCurve AccuracyAdditiveCurve
		{
			[CompilerGenerated]
			get
			{
				return _accuracyAdditiveCurve;
			}
		}

		public AnimationCurve AdditiveNoAimDispertionCurve
		{
			[CompilerGenerated]
			get
			{
				return _additiveNoAimDispertionCurve;
			}
		}

		public float RecoilMultOnCrouch
		{
			[CompilerGenerated]
			get
			{
				return _recoilMultOnCrouch;
			}
		}

		public float AccuracyMultOnCrouch
		{
			[CompilerGenerated]
			get
			{
				return _accuracyMultOnCrouch;
			}
		}

		public float RecoilAimMult
		{
			[CompilerGenerated]
			get
			{
				return _recoilAimMult;
			}
		}

		public float AccuracyAimMult
		{
			[CompilerGenerated]
			get
			{
				return _accuracyAimMult;
			}
		}

		public float RecoilAimMultOnCrouch
		{
			[CompilerGenerated]
			get
			{
				return _recoilAimMultOnCrouch;
			}
		}

		public float AccuracyAimMultOnCrouch
		{
			[CompilerGenerated]
			get
			{
				return _accuracyAimMultOnCrouch;
			}
		}

		public int RecoilControl
		{
			[CompilerGenerated]
			get
			{
				return _recoilControl;
			}
		}

		public int AccurateRange
		{
			[CompilerGenerated]
			get
			{
				return _accurateRange;
			}
		}

		public GunSightViewParameters GunSightViewParameters
		{
			[CompilerGenerated]
			get
			{
				return _gunSightViewParameters;
			}
		}

		public float ArmorPenetration
		{
			[CompilerGenerated]
			get
			{
				return _armorPenetration;
			}
		}

		public int PenetrationPower
		{
			[CompilerGenerated]
			get
			{
				return _penetrationPower;
			}
		}

		public float ReloadDuration
		{
			[CompilerGenerated]
			get
			{
				return _reloadDuration;
			}
		}

		public float ReloadStartOffset
		{
			[CompilerGenerated]
			get
			{
				return _reloadStartOffset;
			}
		}

		public float RoundInsertOffset
		{
			[CompilerGenerated]
			get
			{
				return _roundInsertOffset;
			}
		}

		public float RoundInsertDuration
		{
			[CompilerGenerated]
			get
			{
				return _roundInsertDuration;
			}
		}

		public float MagazineInsertTime
		{
			[CompilerGenerated]
			get
			{
				return _magazineInsertTime;
			}
		}

		public float TakeDuration
		{
			[CompilerGenerated]
			get
			{
				return _takeDuration;
			}
		}

		public float PreReloadTime
		{
			[CompilerGenerated]
			get
			{
				return _preReloadTime;
			}
		}

		public int Impulse
		{
			[CompilerGenerated]
			get
			{
				return _impulse;
			}
		}

		public MuzzleFlashType MuzzleFlashType
		{
			[CompilerGenerated]
			get
			{
				return _muzzleFlashType;
			}
		}
	}
}
