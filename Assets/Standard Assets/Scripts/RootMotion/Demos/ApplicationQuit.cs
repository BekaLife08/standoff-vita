using UnityEngine;

namespace RootMotion.Demos
{
	public class ApplicationQuit : MonoBehaviour
	{
		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
}
