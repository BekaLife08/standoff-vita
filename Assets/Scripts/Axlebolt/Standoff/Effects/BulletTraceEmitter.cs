using Axlebolt.Standoff.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class BulletTraceEmitter : AbstractEffectsEmitter<BulletTraceEmitter>
	{
		private BulletTraceEffectParams _effectParams;

		private GameObject _bulletTracerPrefab;

		private readonly List<BulletTracer> _bulletTracersList = new List<BulletTracer>();

		private readonly Queue<BulletTracer> _freeBulletTracersQueue = new Queue<BulletTracer>();

		public void Init()
		{
			_effectParams = ResourcesUtility.Load<BulletTraceEffectParams>("Effects/BulletTraceParams");
			_bulletTracerPrefab = _effectParams.Prefab;
			Init(_effectParams.EffectDetails);
		}

		protected override void InitPool(int poolSize)
		{
			for (int i = 0; i < poolSize; i++)
			{
				BulletTracer component = Object.Instantiate(_bulletTracerPrefab).GetComponent<BulletTracer>();
				component.transform.SetParent(base.transform);
				component.Init(_effectParams);
				_bulletTracersList.Add(component);
				_freeBulletTracersQueue.Enqueue(component);
			}
			foreach (BulletTracer bulletTracers in _bulletTracersList)
			{
				bulletTracers.Hide();
			}
		}

		protected override void ClearPool()
		{
			foreach (BulletTracer bulletTracers in _bulletTracersList)
			{
				UnityEngine.Object.Destroy(bulletTracers.gameObject);
			}
			_bulletTracersList.Clear();
			_freeBulletTracersQueue.Clear();
		}

		public void Emit(Vector3 startPosition, Vector3 endPosition, BulletTraceType traceType)
		{
			if (_bulletTracersList.Count != 0)
			{
				BulletTracer bulletTracer = ((_freeBulletTracersQueue.Count <= 0) ? null : _freeBulletTracersQueue.Dequeue()) ?? _bulletTracersList[0];
				bulletTracer.Trace(startPosition, endPosition, traceType);
			}
		}

		internal void ReturnToPool(BulletTracer bulletTracer)
		{
			_freeBulletTracersQueue.Enqueue(bulletTracer);
		}

		private void Update()
		{
			foreach (BulletTracer bulletTracers in _bulletTracersList)
			{
				bulletTracers.Evaluate();
			}
		}
	}
}
