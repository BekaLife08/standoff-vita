using DeadMosquito.AndroidGoodies.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGSharedPrefs
	{
		public const int MODE_PRIVATE = 0;

		public const int MODE_WORLD_READABLE = 1;

		public const int MODE_WORLD_WRITEABLE = 2;

		public static bool SetBool(string preferenceFileKey, string key, bool value, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			AndroidJavaObject editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		public static bool SetFloat(string preferenceFileKey, string key, float value, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			AndroidJavaObject editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		public static bool SetInt(string preferenceFileKey, string key, int value, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			try
			{
				AndroidJavaObject editor = PutValue(preferenceFileKey, key, value, mode);
				return Commit(editor);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return false;
			}
		}

		public static bool SetLong(string preferenceFileKey, string key, long value, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			AndroidJavaObject editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		public static bool SetString(string preferenceFileKey, string key, string value, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			AndroidJavaObject editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		public static bool Remove(string preferenceFileKey, string key, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			using (AndroidJavaObject androidJavaObject = GetEditorInternal(preferenceFileKey, mode))
			{
				androidJavaObject.CallAJO("remove", key);
				return Commit(androidJavaObject);
			}
		}

		public static bool Clear(string preferenceFileKey, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			using (AndroidJavaObject androidJavaObject = GetEditorInternal(preferenceFileKey, mode))
			{
				androidJavaObject.CallAJO("clear");
				return Commit(androidJavaObject);
			}
		}

		public static Dictionary<string, object> GetAll(string preferenceFileKey, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return new Dictionary<string, object>();
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			try
			{
				using (AndroidJavaObject ajo = GetSharedPrefs(preferenceFileKey, mode))
				{
					AndroidJavaObject javaMap = ajo.CallAJO("getAll");
					return javaMap.FromJavaMap();
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return null;
			}
		}

		public static bool GetBool(string preferenceFileKey, string key, bool defaultValue, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		public static float GetFloat(string preferenceFileKey, string key, float defaultValue, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0f;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		public static int GetInt(string preferenceFileKey, string key, int defaultValue, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		public static long GetLong(string preferenceFileKey, string key, long defaultValue, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0L;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		public static string GetString(string preferenceFileKey, string key, string defaultValue, int mode = 0)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		private static AndroidJavaObject GetSharedPrefs(string preferenceFileKey, int mode = 0)
		{
			return AGUtils.Activity.CallAJO("getSharedPreferences", preferenceFileKey, mode);
		}

		private static AndroidJavaObject GetPrefs(int mode = 0)
		{
			return AGUtils.Activity.CallAJO("getPreferences", mode);
		}

		private static AndroidJavaObject GetEditor(AndroidJavaObject prefs)
		{
			return prefs.CallAJO("edit");
		}

		private static AndroidJavaObject GetEditorInternal(string perferenceFileKey, int mode = 0)
		{
			return GetEditor(GetSharedPrefs(perferenceFileKey, mode));
		}

		private static bool Commit(AndroidJavaObject editor)
		{
			return editor.CallBool("commit");
		}

		private static AndroidJavaObject PutValue<T>(string preferenceFileKey, string key, T value, int mode = 0)
		{
			AndroidJavaObject editorInternal = GetEditorInternal(preferenceFileKey, mode);
			if (typeof(T) == typeof(bool))
			{
				editorInternal.CallAJO("putBoolean", key, value);
			}
			else if (typeof(T) == typeof(float))
			{
				editorInternal.CallAJO("putFloat", key, value);
			}
			else if (typeof(T) == typeof(int))
			{
				editorInternal.CallAJO("putInt", key, value);
			}
			else if (typeof(T) == typeof(long))
			{
				editorInternal.CallAJO("putLong", key, value);
			}
			else if (typeof(T) == typeof(string))
			{
				editorInternal.CallAJO("putString", key, value);
			}
			else
			{
				UnityEngine.Debug.LogError(typeof(T) + " is not supported");
			}
			return editorInternal;
		}

		private static T GetValue<T>(string preferenceFileKey, string key, T defaultValue, int mode = 0)
		{
			try
			{
				using (AndroidJavaObject androidJavaObject = GetSharedPrefs(preferenceFileKey, mode))
				{
					T result = defaultValue;
					if (typeof(T) == typeof(bool))
					{
						result = androidJavaObject.Call<T>("getBoolean", new object[2]
						{
							key,
							defaultValue
						});
					}
					else if (typeof(T) == typeof(float))
					{
						result = androidJavaObject.Call<T>("getFloat", new object[2]
						{
							key,
							defaultValue
						});
					}
					else if (typeof(T) == typeof(int))
					{
						result = androidJavaObject.Call<T>("getInt", new object[2]
						{
							key,
							defaultValue
						});
					}
					else if (typeof(T) == typeof(long))
					{
						result = androidJavaObject.Call<T>("getLong", new object[2]
						{
							key,
							defaultValue
						});
					}
					else if (typeof(T) == typeof(string))
					{
						result = androidJavaObject.Call<T>("getString", new object[2]
						{
							key,
							defaultValue
						});
					}
					else
					{
						UnityEngine.Debug.LogError(typeof(T) + " is not supported");
					}
					return result;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return defaultValue;
			}
		}
	}
}
