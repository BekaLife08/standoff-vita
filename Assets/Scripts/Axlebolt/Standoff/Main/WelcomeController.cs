using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axlebolt.Standoff.Main
{
	public class WelcomeController : MonoBehaviour
	{
		private void Start()
		{
			SceneManager.LoadScene("Main", LoadSceneMode.Single);
		}
	}
}
