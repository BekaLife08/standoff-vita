using Axlebolt.Standoff.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class PhotonPrefabPool : IPunPrefabPool
	{
		private static readonly Log Log = Log.Create(typeof(PhotonPrefabPool));

		private readonly Dictionary<string, IPrefabPoolElement> _prefabTypes = new Dictionary<string, IPrefabPoolElement>();

		public virtual GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
		{
			Log.Debug($"Get from pool {prefabId}");
			if (_prefabTypes.ContainsKey(prefabId))
			{
				GameObject gameObject = PhotonNetwork.PrefabCache[prefabId];
				gameObject.gameObject.SetActive(value: true);
				return gameObject;
			}
			return null;
		}

		public virtual void Register<T>() where T : IPrefabPoolElement
		{
			Type typeFromHandle = typeof(T);
			string name = typeFromHandle.Name;
			Log.Debug($"Register prefab {name}");
			GameObject gameObject = new GameObject(name, typeFromHandle, typeof(PhotonView));
			_prefabTypes.Add(name, gameObject.GetRequireComponent<IPrefabPoolElement>());
			PhotonNetwork.PrefabCache[name] = gameObject;
			gameObject.gameObject.SetActive(value: false);
		}

		public virtual void Destroy(GameObject gameObject)
		{
			Log.Debug($"Return to pool {gameObject.name}");
			if (_prefabTypes.ContainsKey(gameObject.name))
			{
				PhotonNetwork.PrefabCache[gameObject.name].gameObject.SetActive(value: false);
				_prefabTypes[gameObject.name].OnReturnToPool();
			}
		}
	}
}
