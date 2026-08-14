using System;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	[Serializable]
	public class MeshLodLayerInfo
	{
		[SerializeField]
		[HideInInspector]
		private MeshLodInfo[] _lodsInfo = new MeshLodInfo[0];

		public MeshLodInfo[] LodsInfo => _lodsInfo;
	}
}
