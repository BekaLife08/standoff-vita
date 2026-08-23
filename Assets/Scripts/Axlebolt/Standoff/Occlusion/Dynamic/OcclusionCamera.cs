using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Dynamic
{
	[RequireComponent(typeof(Camera))]
	public class OcclusionCamera : MonoBehaviour
	{
		private void Awake()
		{
			Singleton<OcclusionControl>.Instance.Init(this.GetRequireComponent<Camera>());
		}
	}
}
