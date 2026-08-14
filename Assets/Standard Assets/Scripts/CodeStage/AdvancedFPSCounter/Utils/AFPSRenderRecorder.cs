using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.Utils
{
	[DisallowMultipleComponent]
	public class AFPSRenderRecorder : MonoBehaviour
	{
		private bool recording;

		private float renderTime;

		private void OnPreCull()
		{
			if (!recording && !(AFPSCounter.Instance == null))
			{
				recording = true;
				renderTime = Time.realtimeSinceStartup;
			}
		}

		private void OnPostRender()
		{
			if (recording && !(AFPSCounter.Instance == null))
			{
				recording = false;
				renderTime = Time.realtimeSinceStartup - renderTime;
				AFPSCounter.Instance.fpsCounter.AddRenderTime(renderTime * 1000f);
			}
		}
	}
}
