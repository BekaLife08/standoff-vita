using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[CreateAssetMenu(fileName = "NewMaterial", menuName = "Standoff/Create Arms Material", order = 5)]
	public class ArmsMaterial : ScriptableObject
	{
		[SerializeField]
		[NotNull]
		private Material _material;

		[NotNull]
		public Material Material
		{
			[CompilerGenerated]
			get
			{
				return _material;
			}
		}
	}
}
