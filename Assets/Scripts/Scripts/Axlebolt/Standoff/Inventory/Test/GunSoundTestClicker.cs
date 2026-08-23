using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Test
{
	public class GunSoundTestClicker : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _audioSource;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				_audioSource.Play();
			}
		}
	}
}
