using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Sound
{
	public class AudioPlayerManager : Singleton<AudioPlayerManager>
	{
		private void Awake()
		{
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
		}

		public AudioPlayer Create()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "AudioPlayer";
			GameObject gameObject2 = gameObject;
			gameObject2.transform.SetParent(base.transform);
			return gameObject2.AddComponent<AudioPlayer>();
		}
	}
}
