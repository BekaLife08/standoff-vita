using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Static
{
	[ExecuteInEditMode]
	public class StaticOcclusionArea : MonoBehaviour
	{
		private StaticOcclusionAreaBox[] _occlusionAreaBoxs;

		[SerializeField]
		private MeshRenderer[] _meshRenderers;

		public MeshRenderer[] MeshRenderers
		{
			[CompilerGenerated]
			get
			{
				return _meshRenderers;
			}
		}

		public StaticOcclusionPortal[] Portals
		{
			get;
			private set;
		}

		public void Init()
		{
			_occlusionAreaBoxs = GetComponentsInChildren<StaticOcclusionAreaBox>();
			StaticOcclusionAreaBox[] occlusionAreaBoxs = _occlusionAreaBoxs;
			foreach (StaticOcclusionAreaBox staticOcclusionAreaBox in occlusionAreaBoxs)
			{
				staticOcclusionAreaBox.Init();
			}
			_meshRenderers = ((_meshRenderers.Length != 0) ? (from meshRenderer in _meshRenderers
				where meshRenderer != null
				select meshRenderer).ToArray() : GetComponentsInChildren<MeshRenderer>());
			MeshRenderer[] meshRenderers = _meshRenderers;
			foreach (MeshRenderer meshRenderer2 in meshRenderers)
			{
				meshRenderer2.enabled = true;
			}
			Portals = (from portal in UnityEngine.Object.FindObjectsOfType<StaticOcclusionPortal>()
				where portal.Areas.Contains(this)
				select portal).ToArray();
		}

		public bool Contains(Vector3 point, out int boxIndex)
		{
			for (int i = 0; i < _occlusionAreaBoxs.Length; i++)
			{
				StaticOcclusionAreaBox staticOcclusionAreaBox = _occlusionAreaBoxs[i];
				if (staticOcclusionAreaBox.Bounds.Contains(point))
				{
					boxIndex = i;
					return true;
				}
			}
			boxIndex = -1;
			return false;
		}

		public bool Contains(Vector3 point, int boxIndex)
		{
			return boxIndex < _occlusionAreaBoxs.Length && _occlusionAreaBoxs[boxIndex].Bounds.Contains(point);
		}
	}
}
