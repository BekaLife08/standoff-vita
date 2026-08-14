using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Static
{
	[ExecuteInEditMode]
	public class StaticOcclusionAreaBox : MonoBehaviour
	{
		private readonly Color _color = new Color(1f, 0.5f, 0f, 0.5f);

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
