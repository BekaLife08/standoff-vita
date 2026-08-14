using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public abstract class SimpleEffectsEmitter<T, TK> : AbstractEffectsEmitter<T> where T : MonoBehaviour
	{
		protected class Attr
		{
			public readonly TK Type;

			public readonly ParticleSystem Particles;

			public Attr(TK type, ParticleSystem particles)
			{
				Type = type;
				Particles = particles;
			}
		}

		protected static readonly Log Log = Log.Create(typeof(T));

		protected Dictionary<TK, Queue<ParticleSystem>> Pool = new Dictionary<TK, Queue<ParticleSystem>>();

		protected readonly Queue<Attr> LocalInstances = new Queue<Attr>();

		protected readonly Queue<Attr> OtherInstances = new Queue<Attr>();

		protected bool Initialized;

		protected int PoolSize;

		protected override void InitPool(int poolSize)
		{
			PoolSize = poolSize;
			TK[] types = GetTypes();
			foreach (TK val in types)
			{
				Pool[val] = CreatePool(val, poolSize);
			}
			Initialized = true;
		}

		protected abstract TK[] GetTypes();

		protected abstract ParticleSystem GetParticles(TK type);

		protected override void ClearPool()
		{
			foreach (KeyValuePair<TK, Queue<ParticleSystem>> item in Pool)
			{
				foreach (ParticleSystem item2 in item.Value)
				{
					Object.Destroy(item2.gameObject);
				}
			}
			foreach (Attr localInstance in LocalInstances)
			{
				Object.Destroy(localInstance.Particles.gameObject);
			}
			foreach (Attr otherInstance in OtherInstances)
			{
				Object.Destroy(otherInstance.Particles.gameObject);
			}
			Pool.Clear();
			LocalInstances.Clear();
			OtherInstances.Clear();
		}

		private Queue<ParticleSystem> CreatePool(TK type, int poolSize)
		{
			Queue<ParticleSystem> queue = new Queue<ParticleSystem>();
			ParticleSystem particles = GetParticles(type);
			for (int i = 0; i < poolSize; i++)
			{
				ParticleSystem particleSystem = Object.Instantiate(particles, base.transform);
				OnParticlesCreated(type, particleSystem);
				particleSystem.gameObject.SetActive(false);
				queue.Enqueue(particleSystem);
			}
			return queue;
		}

		protected virtual void OnParticlesCreated(TK type, ParticleSystem particles)
		{
		}

		protected void Emit(TK type, Vector3 position, Vector3 direction, bool isLocal)
		{
			if (!Initialized)
			{
				Log.Error(string.Format("{0} not initialized", GetType().Name));
			}
			else
			{
				if (PoolSize == 0)
				{
					return;
				}
				if (LocalInstances.Count + OtherInstances.Count >= PoolSize)
				{
					if (OtherInstances.Count > 0)
					{
						ReturnToPool(OtherInstances.Dequeue());
					}
					else
					{
						ReturnToPool(LocalInstances.Dequeue());
					}
				}
				if (Pool[type].Count > 0)
				{
					ParticleSystem particles = Pool[type].Dequeue();
					Create(type, particles, position, direction, isLocal);
				}
				else
				{
					Log.Error("Pool size exceeded, it is impossible!!!");
				}
			}
		}

		private void ReturnToPool(Attr attr)
		{
			if (attr.Particles.isPlaying)
			{
				attr.Particles.Stop(true);
			}
			attr.Particles.gameObject.SetActive(false);
			Pool[attr.Type].Enqueue(attr.Particles);
		}

		private void Create(TK type, ParticleSystem particles, Vector3 position, Vector3 direction, bool isLocal)
		{
			particles.transform.position = position;
			particles.transform.rotation = Quaternion.LookRotation(direction);
			particles.gameObject.SetActive(true);
			particles.Play(true);
			if (isLocal)
			{
				LocalInstances.Enqueue(new Attr(type, particles));
			}
			else
			{
				OtherInstances.Enqueue(new Attr(type, particles));
			}
		}

		private void Update()
		{
			Update(OtherInstances);
			Update(LocalInstances);
		}

		private void Update(Queue<Attr> instances)
		{
			while (instances.Count > 0)
			{
				Attr attr = instances.Peek();
				ParticleSystem particles = attr.Particles;
				if (particles.isPlaying)
				{
					break;
				}
				instances.Dequeue();
				ReturnToPool(attr);
			}
		}
	}
}
