using System.Collections;
using UnityEngine;

namespace I2
{
	public class CoroutineManager : MonoBehaviour
	{
		private static CoroutineManager mInstance;

		public static CoroutineManager pInstance
		{
			get
			{
				if (mInstance == null)
				{
					GameObject gameObject = new GameObject("_Coroutiner");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					mInstance = gameObject.AddComponent<CoroutineManager>();
					if (Application.isPlaying)
					{
						Object.DontDestroyOnLoad(gameObject);
					}
				}
				return mInstance;
			}
		}

		public static Coroutine Start(IEnumerator coroutine)
		{
			return pInstance.StartCoroutine(coroutine);
		}
	}
}
