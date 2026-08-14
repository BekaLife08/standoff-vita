using Axlebolt.Standoff.Core;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class PrefsStorage : IFileStorage
	{
		private static readonly Log Log = Log.Create(typeof(PrefsStorage));

		public T Load<T>(string key, T defaultValue = default(T))
		{
			string @string = PlayerPrefs.GetString(key, null);
			if (Log.DebugEnabled)
			{
				Log.Debug($"Load {key} {@string}");
			}
			return (!string.IsNullOrEmpty(@string)) ? JsonUtility.FromJson<T>(@string) : defaultValue;
		}

		public IEnumerator Save<T>(string key, T value)
		{
			string json = JsonUtility.ToJson(value);
			if (Log.DebugEnabled)
			{
				Log.Debug($"Save {key} {json}");
			}
			PlayerPrefs.SetString(key, json);
			PlayerPrefs.Save();
			yield return null;
		}
	}
}
