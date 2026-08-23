using System;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public abstract class ScenePhotonBehavior<T> : PhotonBehavior, IPrefabPoolElement where T : ScenePhotonBehavior<T>
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if ((UnityEngine.Object)_instance == (UnityEngine.Object)null)
				{
					throw new Exception(string.Format("{0} not initialized", "T"));
				}
				return _instance;
			}
		}

		public static event Action ReadyEvent;

		public static IEnumerator Init(object[] data)
		{
			while (true)
			{
				if (PhotonNetwork.isMasterClient)
				{
					string name = typeof(T).Name;
					PhotonNetwork.InstantiateSceneObject(name, Vector3.zero, Quaternion.identity, 0, data).GetRequireComponent<T>();
				}
				if ((UnityEngine.Object)_instance != (UnityEngine.Object)null)
				{
					break;
				}
				yield return null;
			}
		}

		public static bool IsInitialized()
		{
			return (UnityEngine.Object)_instance != (UnityEngine.Object)null;
		}

		public override void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			OnInstantiate(base.PhotonView.instantiationData);
			_instance = (T)this;
			if (ScenePhotonBehavior<T>.ReadyEvent != null)
			{
				ScenePhotonBehavior<T>.ReadyEvent();
			}
		}

		public abstract void OnInstantiate(object[] data);

		public abstract void OnReturnToPool();

		protected virtual void OnDestroy()
		{
			_instance = (T)null;
			ScenePhotonBehavior<T>.ReadyEvent = null;
		}
	}
}
