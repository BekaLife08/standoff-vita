using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[CreateAssetMenu(fileName = "MuzzleFlashParams", menuName = "Standoff/Effects/MuzleFlash", order = 2)]
	public class MuzzleFlashParams : ScriptableObject
	{
		[SerializeField]
		private EffectDetails _effectDetails;

		[SerializeField]
		private MuzzleFlashParam[] _params;

		public EffectDetails EffectDetails
		{
			[CompilerGenerated]
			get
			{
				return _effectDetails;
			}
		}

		public MuzzleFlashParam[] Params
		{
			[CompilerGenerated]
			get
			{
				return _params;
			}
		}
	}
}
