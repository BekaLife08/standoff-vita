using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Static
{
	[ExecuteInEditMode]
	public class StaticOcclusionPortalBox : MonoBehaviour
	{
		private readonly Color _color = new Color(0.61f, 0.87f, 0f, 0.5f);

		public Bounds Bounds
		{
			get;
			private set;
		}

		public void Init()
		{
			Bounds = new Bounds(base.transform.position, base.transform.localScale);
		}
	}
}
