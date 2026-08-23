using System.Collections;
using Axlebolt.Standoff.Core;
using Photon;
using UnityEngine;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncOperation<T> : PunBehaviour where T : PunBehaviour
	{
		internal static readonly Log Log = Log.Create(typeof(T));

		internal AsyncResult Result;

		public static T Create()
		{
			GameObject gameObject = new GameObject(typeof(T).Name);
			T val = gameObject.AddComponent<T>();
			PhotonNetworkExtension.MessageListeners.Add(val);
			return val;
		}

		internal void Done()
		{
			try
			{
				if (!Result.Success)
				{
					throw Result.Exception;
				}
			}
			finally
			{
				Object.Destroy(base.gameObject);
			}
		}

		internal IEnumerator WaitForResult()
		{
			while (Result == null)
			{
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
		}
	}
}
