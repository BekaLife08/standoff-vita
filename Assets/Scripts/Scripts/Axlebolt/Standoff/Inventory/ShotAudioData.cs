using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class ShotAudioData
	{
		public List<AudioSource> AudioSources = new List<AudioSource>();

		public AudioClip ShootClip;

		public float ShootVolume;
	}
}
