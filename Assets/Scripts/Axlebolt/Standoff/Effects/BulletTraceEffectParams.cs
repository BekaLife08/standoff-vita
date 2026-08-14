using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[CreateAssetMenu(fileName = "BulletTraceParams", menuName = "Standoff/Effects/BulletTraceParams", order = 3)]
	public class BulletTraceEffectParams : ScriptableObject
	{
		[Serializable]
		public class TracerParams
		{
			public BulletTraceType TraceType;

			public float ScaleXY;

			public float ScaleZ;

			public float Speed;

			public float Offset;
		}

		[SerializeField]
		private GameObject _prefab;

		[SerializeField]
		private EffectDetails _effectDetails;

		[SerializeField]
		private List<TracerParams> _tracerParamsList;

		public GameObject Prefab
		{
			[CompilerGenerated]
			get
			{
				return _prefab;
			}
		}

		public EffectDetails EffectDetails
		{
			[CompilerGenerated]
			get
			{
				return _effectDetails;
			}
		}

		public TracerParams GetTracerParams(BulletTraceType traceType)
		{
			return _tracerParamsList.Find((TracerParams x) => x.TraceType == traceType);
		}
	}
}
