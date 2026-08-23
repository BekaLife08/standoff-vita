using Axlebolt.Standoff.Core;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class SkinnedMeshLodGroup : LodGroup
	{
		[SerializeField]
		[HideInInspector]
		private Transform[] _skinnedMeshLods;

		[SerializeField]
		[HideInInspector]
		private SkinnedMeshRenderer[] _skinnedMeshRenderers;

		public SkinnedMeshRenderer[] SkinnedMeshRenderers
		{
			[CompilerGenerated]
			get
			{
				return _skinnedMeshRenderers;
			}
		}

		private void Awake()
		{
			Transform[] skinnedMeshLods = _skinnedMeshLods;
			foreach (Transform itemTransform in skinnedMeshLods)
			{
				Singleton<Trash>.Instance.Drop(itemTransform);
			}
			SetLod(QualitySettings.maximumLODLevel);
		}

		internal override void InternalSetLod(int level)
		{
			Singleton<Trash>.Instance.Drop(_skinnedMeshLods[base.CurrentLevel]);
			Singleton<Trash>.Instance.Return(_skinnedMeshLods[level], base.transform);
			EnableSkinnedMeshRenderer();
		}

		public void DisableSkinnedMeshRenderer()
		{
			_skinnedMeshRenderers[base.CurrentLevel].enabled = false;
		}

		public void EnableSkinnedMeshRenderer()
		{
			_skinnedMeshRenderers[base.CurrentLevel].enabled = true;
		}
	}
}
