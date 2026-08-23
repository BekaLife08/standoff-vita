using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[CreateAssetMenu(fileName = "CharacterImpactParams", menuName = "Standoff/Effects/Character", order = 0)]
	public class CharacterImpactParams : ScriptableObject
	{
		[SerializeField]
		private UVEffect _effect;

		[SerializeField]
		private bool[] _localPlayFlags;

		[SerializeField]
		private EffectDetails _effectDetails;

		public UVEffect Effect
		{
			[CompilerGenerated]
			get
			{
				return _effect;
			}
		}

		public bool[] LocalPlayFlags
		{
			[CompilerGenerated]
			get
			{
				return _localPlayFlags;
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
	}
}
