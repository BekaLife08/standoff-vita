using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Defuse
{
	[Serializable]
	public class DefuseAudioClips
	{
		[SerializeField]
		private AudioClip _ctWin;

		[SerializeField]
		private AudioClip _trWin;

		[SerializeField]
		private AudioClip _bombHasBeenPlanted;

		[SerializeField]
		private AudioClip _bombHasBeenDefused;

		[SerializeField]
		private AudioClip _plantingTheBomb;

		[SerializeField]
		private AudioClip _defusingTheBomb;

		public AudioClip CtWin
		{
			[CompilerGenerated]
			get
			{
				return _ctWin;
			}
		}

		public AudioClip TrWin
		{
			[CompilerGenerated]
			get
			{
				return _trWin;
			}
		}

		public AudioClip BombHasBeenPlanted
		{
			[CompilerGenerated]
			get
			{
				return _bombHasBeenPlanted;
			}
		}

		public AudioClip BombHasBeenDefused
		{
			[CompilerGenerated]
			get
			{
				return _bombHasBeenDefused;
			}
		}

		public AudioClip PlantingTheBomb
		{
			[CompilerGenerated]
			get
			{
				return _plantingTheBomb;
			}
		}

		public AudioClip DefusingTheBomb
		{
			[CompilerGenerated]
			get
			{
				return _defusingTheBomb;
			}
		}
	}
}
