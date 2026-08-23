using UnityEngine;

namespace Axlebolt.Standoff.Core
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if ((Object)_instance != (Object)null)
				{
					return _instance;
				}
				GameObject gameObject = new GameObject(typeof(T).Name);
				_instance = gameObject.AddComponent<T>();
				return _instance;
			}
		}

		protected virtual void OnDestroy()
		{
			_instance = (T)null;
		}
	}
}
