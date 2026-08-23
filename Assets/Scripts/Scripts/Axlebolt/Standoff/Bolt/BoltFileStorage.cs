using Axlebolt.Bolt;
using Axlebolt.Bolt.Storage;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Axlebolt.Standoff.Bolt
{
	public class BoltFileStorage : IFileStorage
	{
		private static readonly Log Log = Log.Create(typeof(BoltFileStorage));

		public T Load<T>(string key, T defaultValue = default(T))
		{
			byte[] array = BoltService<BoltStorageService>.Instance.ReadFile(key);
			if (Log.DebugEnabled)
			{
				Log.Debug($"Load file {key} ({array?.Length})");
			}
			if (array == null || array.Length == 0)
			{
				return defaultValue;
			}
			string @string = Encoding.UTF8.GetString(array);
			return JsonUtility.FromJson<T>(@string);
		}

		public IEnumerator Save<T>(string key, T value)
		{
			string json = JsonUtility.ToJson(value);
			byte[] bytes = Encoding.UTF8.GetBytes(json);
			if (Log.DebugEnabled)
			{
				Log.Debug($"Save file {key} ({bytes.Length} bytes)");
			}
			Task task = BoltService<BoltStorageService>.Instance.WriteFile(key, bytes);
			if (!task.IsCompleted)
			{
				yield return null;
			}
			if (!task.IsFaulted)
			{
				if (Log.DebugEnabled)
				{
					Log.Debug($"Files {key} successfully saved");
				}
				yield break;
			}
			if (Log.DebugEnabled)
			{
				Log.Debug($"Files {key} save failed");
			}
			LogException(task.Exception);
		}

		private void LogException(AggregateException ex)
		{
			if (ex.InnerException is ConnectionFailedException)
			{
				Log.Debug(ex);
			}
			else
			{
				Log.Error(ex.InnerException);
			}
		}
	}
}
