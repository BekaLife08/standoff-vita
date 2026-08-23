using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Effects
{
	[CreateAssetMenu(fileName = "SurfaceImpactParams", menuName = "Standoff/Effects/Surface", order = 1)]
	public class SurfaceImpactParams : ScriptableObject
	{
		[SerializeField]
		private int _maxDecalsCount;

		[SerializeField]
		private EffectDetails _effectDetails;

		[SerializeField]
		private Material _bulletHoleMaterial;

		[SerializeField]
		[FormerlySerializedAs("_params")]
		private GunImpactParam[] _gunParams;

		[SerializeField]
		private KnifeImpactParam _knifeParams;

		public int MaxDecalsCount
		{
			[CompilerGenerated]
			get
			{
				return _maxDecalsCount;
			}
		}

		public EffectDetails EffectDetails
		{
			[CompilerGenerated]
			get
			{
				return _effectDetails;
			}
		}

		public Material BulletHoleMaterial
		{
			[CompilerGenerated]
			get
			{
				return _bulletHoleMaterial;
			}
		}

		public GunImpactParam[] GunParams
		{
			[CompilerGenerated]
			get
			{
				return _gunParams;
			}
		}

		public KnifeImpactParam KnifeParams
		{
			[CompilerGenerated]
			get
			{
				return _knifeParams;
			}
		}
	}
}
