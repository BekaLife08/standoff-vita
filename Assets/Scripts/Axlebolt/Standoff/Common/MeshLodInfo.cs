using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	[Serializable]
	public class MeshLodInfo
	{
		[SerializeField]
		private Mesh _mesh;

		[SerializeField]
		private Mesh[] _childMeshes;

		[SerializeField]
		private Mesh _combinedMesh;

		public Mesh Mesh
		{
			[CompilerGenerated]
			get
			{
				return _mesh;
			}
		}

		public Mesh[] ChildMeshes
		{
			[CompilerGenerated]
			get
			{
				return _childMeshes;
			}
		}

		public Mesh CombinedMesh
		{
			[CompilerGenerated]
			get
			{
				return _combinedMesh;
			}
		}
	}
}
