using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class ResourcesUtility
	{
		private static readonly Log Log = Log.Create(typeof(ResourcesUtility));

		public static T Load<T>([NotNull] string path) where T : UnityEngine.Object
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug($"Loading resource {path}");
			}
			T val = Resources.Load<T>(path);
			if ((UnityEngine.Object)val == (UnityEngine.Object)null)
			{
				throw new ResourceNotFoundException(path);
			}
			if (Log.DebugEnabled)
			{
				Log.Debug($"Resource {path} successfully loaded");
			}
			return val;
		}

		public static T[] LoadAll<T>(string path) where T : UnityEngine.Object
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug($"Loading resources {path}");
			}
			T[] result = Resources.LoadAll<T>(path);
			if (Log.DebugEnabled)
			{
				Log.Debug($"Resources {path} successfully loaded");
			}
			return result;
		}

		public static void Unload([NotNull] UnityEngine.Object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug($"Unloading resource {o}");
			}
			Resources.UnloadAsset(o);
		}
	}
}
