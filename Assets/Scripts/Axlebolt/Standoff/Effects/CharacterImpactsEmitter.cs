using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class CharacterImpactsEmitter : AbstractEffectsEmitter<CharacterImpactsEmitter>
	{
		private static readonly Log Log = Log.Create(typeof(CharacterImpactsEmitter));

		private CharacterImpactParams _characterImpactParams;

		private bool _initialized;

		private int _poolSize;

		private UVEffectEmitter _emitter;

		private readonly Queue<int> _pool = new Queue<int>();

		private readonly Queue<int> _localInstances = new Queue<int>();

		private readonly Queue<int> _otherInstances = new Queue<int>();

		public void Init()
		{
			_characterImpactParams = ResourcesUtility.Load<CharacterImpactParams>("Effects/CharacterImpactParams");
			Init(_characterImpactParams.EffectDetails);
			_initialized = true;
		}

		protected override void ClearPool()
		{
			if (_emitter != null)
			{
				UnityEngine.Object.Destroy(_emitter.gameObject);
				_emitter = null;
			}
			_pool.Clear();
			_localInstances.Clear();
			_otherInstances.Clear();
		}

		protected override void InitPool(int poolSize)
		{
			_poolSize = poolSize;
			if (_poolSize > 0)
			{
				_emitter = UVEffectEmitter.Create(_characterImpactParams.Effect, poolSize);
				for (int i = 0; i < _poolSize; i++)
				{
					_pool.Enqueue(i);
				}
			}
		}

		public void Emit(Vector3 position, Vector3 bulletDirection, bool isLocal)
		{
			if (!_initialized)
			{
				Log.Error("CharacterImpactsEmitter is not initialized");
			}
			else if (_poolSize != 0)
			{
				if (_localInstances.Count + _otherInstances.Count < _poolSize)
				{
					int index = _pool.Dequeue();
					CreateParticles(index, position, bulletDirection, isLocal);
				}
				else if (_otherInstances.Count > 0)
				{
					int index2 = _otherInstances.Dequeue();
					CreateParticles(index2, position, bulletDirection, isLocal);
				}
				else if (_localInstances.Count > 0)
				{
					int index3 = _localInstances.Dequeue();
					CreateParticles(index3, position, bulletDirection, isLocal);
				}
				else
				{
					Log.Error("Pool size exceeded, it is impossible!!!");
				}
			}
		}

		private void CreateParticles(int index, Vector3 position, Vector3 bulletDirection, bool isLocal)
		{
			bool[] playFlags = (!isLocal) ? null : _characterImpactParams.LocalPlayFlags;
			_emitter.Emit(index, position, Quaternion.LookRotation(-bulletDirection) * Quaternion.Euler(0f, 0f, Random.Range(-180, 180)), playFlags);
			if (isLocal)
			{
				_localInstances.Enqueue(index);
			}
			else
			{
				_otherInstances.Enqueue(index);
			}
		}

		private void Update()
		{
			UpdateQueue(_localInstances);
			UpdateQueue(_otherInstances);
		}

		private void UpdateQueue(Queue<int> instances)
		{
			while (instances.Count > 0)
			{
				int num = instances.Peek();
				if (_emitter.IsPlaying(num))
				{
					break;
				}
				instances.Dequeue();
				_pool.Enqueue(num);
			}
		}
	}
}
