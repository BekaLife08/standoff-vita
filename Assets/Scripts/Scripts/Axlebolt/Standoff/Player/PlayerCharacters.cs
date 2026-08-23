using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[Serializable]
	public class PlayerCharacters
	{
		[SerializeField]
		private string character;

		[SerializeField]
		private string[] skins;

		public string Character
		{
			[CompilerGenerated]
			get
			{
				return character;
			}
		}

		public string[] Skins
		{
			[CompilerGenerated]
			get
			{
				return skins;
			}
		}
	}
}
