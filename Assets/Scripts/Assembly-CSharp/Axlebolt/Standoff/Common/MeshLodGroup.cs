using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class MeshLodGroup : LodGroup
	{
		private static readonly Log Log = Log.Create(typeof(MeshLodGroup));

		[HideInInspector]
		[SerializeField]
		private MeshLodLayerInfo[] _layersInfo = new MeshLodLayerInfo[0];

		[SerializeField]
		[HideInInspector]
		private MeshFilter _activeLod;

		[HideInInspector]
		[SerializeField]
		private MeshFilter[] _activeLodChildren;

		[SerializeField]
		private MeshRenderer[] _meshRenderers;

		[HideInInspector]
		[SerializeField]
		private bool _initialized;

		public bool Combined { get; private set; }

		public MeshRenderer[] MeshRenderers
		{
			get
			{
				CheckInitialized();
				return _meshRenderers;
			}
		}

		public bool Initialized
		{
			[CompilerGenerated]
			get
			{
				return _initialized;
			}
		}

		public MeshLodGroup()
		{
			Combined = false;
		}

		public void Initialize()
		{
			if (_layersInfo.Length == 0)
			{
				Log.Error(string.Format("LOD's are empty {0}", base.name));
				return;
			}
			if (_initialized)
			{
				throw new InvalidOperationException(string.Format("MeshLodGroup {0} already initialized", base.gameObject.name));
			}
			Init();
			SetLod(QualitySettings.maximumLODLevel);
		}

		private void Init()
		{
			Transform lod = LodUtility.GetLod(base.transform, 0, 0);
			Queue<GameObject> queue = new Queue<GameObject>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (!(lod == child) && LodUtility.IsLod(child))
				{
					queue.Enqueue(child.gameObject);
				}
			}
			while (queue.Count > 0)
			{
				GameObject obj = queue.Dequeue();
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(obj);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
			}
			if (lod == null)
			{
				Log.Error("LOD0 not found", this);
				return;
			}
			HierarchyUpdate(lod);
			_initialized = true;
		}

		private void HierarchyUpdate(Transform lodTransform)
		{
			MeshFilter component = lodTransform.GetComponent<MeshFilter>();
			if (component == null)
			{
				Log.Error("MeshFilter component not found in LOD0", this);
				return;
			}
			base.transform.localPosition = lodTransform.localPosition;
			base.transform.localRotation = lodTransform.localRotation;
			MoveOtherChilds(lodTransform);
			_activeLod = base.gameObject.AddComponent<MeshFilter>();
			_meshRenderers = new MeshRenderer[lodTransform.childCount + 1];
			_meshRenderers[0] = base.gameObject.AddComponent<MeshRenderer>();
			_meshRenderers[0].sharedMaterials = lodTransform.GetComponent<MeshRenderer>().sharedMaterials;
			_activeLod.sharedMesh = component.sharedMesh;
			_activeLodChildren = new MeshFilter[lodTransform.childCount];
			for (int i = 0; i < lodTransform.childCount; i++)
			{
				Transform child = component.transform.GetChild(i);
				_activeLodChildren[i] = child.GetComponent<MeshFilter>();
				_meshRenderers[i + 1] = child.GetComponent<MeshRenderer>();
			}
			while (component.transform.childCount > 0)
			{
				component.transform.GetChild(0).SetParent(base.transform, false);
			}
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(lodTransform.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(lodTransform.gameObject);
			}
		}

		private void MoveOtherChilds(Transform lodTransform)
		{
			int num = 0;
			while (num < lodTransform.childCount)
			{
				Transform child = lodTransform.GetChild(num);
				if (child.GetComponent<MeshFilter>() == null)
				{
					child.SetParent(base.transform, false);
				}
				else
				{
					num++;
				}
			}
		}

		internal override void InternalSetLod(int level)
		{
			CheckInitialized();
			MeshLodInfo meshLodInfo = _layersInfo[base.CurrentLayer].LodsInfo[level];
			if (Combined)
			{
				_activeLod.sharedMesh = meshLodInfo.CombinedMesh;
				return;
			}
			_activeLod.sharedMesh = meshLodInfo.Mesh;
			int num = Math.Min(_activeLodChildren.Length, meshLodInfo.ChildMeshes.Length);
			for (int i = 0; i < num; i++)
			{
				_activeLodChildren[i].sharedMesh = meshLodInfo.ChildMeshes[i];
			}
		}

		public void CombineMesh()
		{
			CheckInitialized();
			if (!Combined)
			{
				for (int i = 1; i < _meshRenderers.Length; i++)
				{
					_meshRenderers[i].enabled = false;
				}
				Combined = true;
				SetLod(base.CurrentLevel);
			}
		}

		public void SplitMesh()
		{
			CheckInitialized();
			if (Combined)
			{
				for (int i = 1; i < _meshRenderers.Length; i++)
				{
					_meshRenderers[i].enabled = true;
				}
				Combined = false;
				SetLod(base.CurrentLevel);
			}
		}

		public void SetLodActive(bool active)
		{
			if (Combined)
			{
				_activeLod.gameObject.SetActive(active);
				return;
			}
			_activeLod.gameObject.SetActive(active);
			MeshFilter[] activeLodChildren = _activeLodChildren;
			foreach (MeshFilter meshFilter in activeLodChildren)
			{
				meshFilter.gameObject.SetActive(active);
			}
		}

		public void SetMaterial(Material material)
		{
			MeshRenderer[] meshRenderers = MeshRenderers;
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				meshRenderer.sharedMaterials = new Material[1] { material };
			}
		}

		private void CheckInitialized()
		{
			if (!_initialized)
			{
				throw new InvalidOperationException(string.Format("MeshLodGroup {0} is not intialized", base.gameObject.name));
			}
		}
	}
}
