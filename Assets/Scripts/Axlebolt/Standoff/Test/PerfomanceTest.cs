using Axlebolt.Standoff.Controls;
using UnityEngine;

namespace Axlebolt.Standoff.Test
{
	public class PerfomanceTest : MonoBehaviour
	{
		private void Start()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				PlayerControls.Instance.gameObject.AddComponent<RandomInputsSimulator>();
				PlayerControls.Instance.SourceType = PlayerControls.InputSourceType.InputSimulator;
			}
		}
	}
}
