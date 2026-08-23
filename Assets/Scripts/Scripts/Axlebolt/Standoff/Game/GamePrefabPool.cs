using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class GamePrefabPool : PhotonPrefabPool
	{
		private static readonly Log Log = Log.Create(typeof(GamePrefabPool));

		private readonly HashSet<string> _playePrefabIds = new HashSet<string>();

		public GamePrefabPool()
		{
			foreach (string id in Singleton<PlayerManager>.Instance.GetIds())
			{
				PhotonNetwork.PrefabCache[id] = Singleton<PlayerManager>.Instance.PlayerPrefab.gameObject;
				_playePrefabIds.Add(id);
				Log.Debug($"Player {id} registered in PhotonCache");
			}
		}

		public override GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
		{
			return (!_playePrefabIds.Contains(prefabId)) ? base.Instantiate(prefabId, position, rotation) : Singleton<PlayerManager>.Instance.GetFromPool(prefabId, position, rotation);
		}

		public override void Destroy(GameObject gameObject)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"Return to pool {gameObject.name}");
			}
			if (_playePrefabIds.Contains(gameObject.name))
			{
				Singleton<PlayerManager>.Instance.ReturnToPool(gameObject);
			}
			else
			{
				base.Destroy(gameObject);
			}
		}
	}
}
