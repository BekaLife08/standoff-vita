using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	[CreateAssetMenu(fileName = "Bomb", menuName = "Standoff/Create BombExtendedParameters")]
	public class BombExtendedParameters : ScriptableObject
	{
		[SerializeField]
		private ParticleSystem _explosionParticle;

		[SerializeField]
		private Vector3 _bombRotation;

		[SerializeField]
		private Vector3 _bombOffset;

		[SerializeField]
		private Vector3 _explosionParticleOffset;

		[SerializeField]
		private BombBeepStep[] _beepSteps;

		[SerializeField]
		private AudioClip _beepClip;

		[SerializeField]
		private AudioClip _finalClip;

		[SerializeField]
		private float _finalClipTime;

		[SerializeField]
		private AudioClip _explosionClip;

		[SerializeField]
		private float _plantedIndicatorBrightness;

		[SerializeField]
		private Flare _plantedIndicatorFlare;

		[SerializeField]
		private Flare _detonatedIndicatorFlare;

		[SerializeField]
		private float _indicatorBrigthnes;

		[SerializeField]
		private Flare _indicatorFlare;

		[SerializeField]
		private float _indicatorSignalInterval;

		public ParticleSystem ExplosionParticle
		{
			[CompilerGenerated]
			get
			{
				return _explosionParticle;
			}
		}

		public Vector3 BombRotation
		{
			[CompilerGenerated]
			get
			{
				return _bombRotation;
			}
		}

		public Vector3 BombOffset
		{
			[CompilerGenerated]
			get
			{
				return _bombOffset;
			}
		}

		public BombBeepStep[] BeepSteps
		{
			[CompilerGenerated]
			get
			{
				return _beepSteps;
			}
		}

		public AudioClip BeepClip
		{
			[CompilerGenerated]
			get
			{
				return _beepClip;
			}
		}

		public AudioClip FinalClip
		{
			[CompilerGenerated]
			get
			{
				return _finalClip;
			}
		}

		public float FinalClipTime
		{
			[CompilerGenerated]
			get
			{
				return _finalClipTime;
			}
		}

		public Vector3 ExplosionParticleOffset
		{
			[CompilerGenerated]
			get
			{
				return _explosionParticleOffset;
			}
		}

		public AudioClip ExplosionClip
		{
			[CompilerGenerated]
			get
			{
				return _explosionClip;
			}
		}

		public float PlantedIndicatorBrightness
		{
			[CompilerGenerated]
			get
			{
				return _plantedIndicatorBrightness;
			}
		}

		public Flare PlantedIndicatorFlare
		{
			[CompilerGenerated]
			get
			{
				return _plantedIndicatorFlare;
			}
		}

		public Flare DetonatedIndicatorFlare
		{
			[CompilerGenerated]
			get
			{
				return _detonatedIndicatorFlare;
			}
		}

		public float IndicatorBrigthnes
		{
			[CompilerGenerated]
			get
			{
				return _indicatorBrigthnes;
			}
		}

		public Flare IndicatorFlare
		{
			[CompilerGenerated]
			get
			{
				return _indicatorFlare;
			}
		}

		public float IndicatorSignalInterval
		{
			[CompilerGenerated]
			get
			{
				return _indicatorSignalInterval;
			}
		}
	}
}
