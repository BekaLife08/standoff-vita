using UnityEngine;

namespace RootMotion
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T sInstance;

		public static T instance => sInstance;

		protected virtual void Awake()
		{
			if ((Object)sInstance != (Object)null)
			{
				UnityEngine.Debug.LogError(base.name + "error: already initialized", this);
			}
			sInstance = (T)this;
		}
	}
}
