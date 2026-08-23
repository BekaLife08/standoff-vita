using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Gun
{
	public class GunSoundTest : MonoBehaviour
	{
		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.S))
			{
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
