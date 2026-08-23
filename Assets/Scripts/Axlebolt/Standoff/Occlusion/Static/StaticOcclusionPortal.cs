using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Static
{
	[ExecuteInEditMode]
	public class StaticOcclusionPortal : MonoBehaviour
	{
		[SerializeField]
		private StaticOcclusionArea[] _areas;

		[SerializeField]
		private MeshRenderer[] _meshRenderers;

		private StaticOcclusionPortalBox[] _boxs;

		public StaticOcclusionArea[] Areas
		{
			[CompilerGenerated]
			get
			{
				return _areas;
			}
		}

		public MeshRenderer[] MeshRenderers
		{
			[CompilerGenerated]
			get
			{
				return _meshRenderers;
			}
		}

		public void Init()
		{
			_boxs = GetComponentsInChildren<StaticOcclusionPortalBox>();
			StaticOcclusionPortalBox[] boxs = _boxs;
			foreach (StaticOcclusionPortalBox staticOcclusionPortalBox in boxs)
			{
				staticOcclusionPortalBox.Init();
			}
			if (_areas == null)
			{
				_areas = new StaticOcclusionArea[0];
			}
			_meshRenderers = (from meshRenderer in _meshRenderers
				where meshRenderer != null
				select meshRenderer).ToArray();
		}

		public bool Contains(Vector3 point, out int boxIndex)
		{
			for (int i = 0; i < _boxs.Length; i++)
			{
				StaticOcclusionPortalBox staticOcclusionPortalBox = _boxs[i];
				if (staticOcclusionPortalBox.Bounds.Contains(point))
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
			return boxIndex < _boxs.Length && _boxs[boxIndex].Bounds.Contains(point);
		}
	}
}
