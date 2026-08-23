using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public abstract class BaseController<TSnapshot, TCmdType> : MonoBehaviour, IPlayerComponent
	{
		private Transform _transform;

		public Transform Transform
		{
			[CompilerGenerated]
			get
			{
				return _transform ?? (_transform = base.transform);
			}
		}

		public abstract void PreInitialize();

		public abstract void Initialize();

		public virtual void OnInstantiated()
		{
		}

		public abstract void SetSnapshot(TSnapshot parameters);

		public abstract TSnapshot GetSnapshot();

		public abstract void ExecuteCommands(TCmdType commands, float duration, float time);

		public virtual void OnReturnToPool()
		{
		}
	}
}
