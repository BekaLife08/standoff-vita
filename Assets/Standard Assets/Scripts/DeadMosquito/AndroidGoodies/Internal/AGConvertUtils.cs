using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class AGConvertUtils
	{
		public static Dictionary<string, object> FromJavaMap(this AndroidJavaObject javaMap)
		{
			if (javaMap == null || javaMap.IsJavaNull())
			{
				return new Dictionary<string, object>();
			}
			int capacity = javaMap.CallInt("size");
			Dictionary<string, object> dictionary = new Dictionary<string, object>(capacity);
			AndroidJavaObject ajo = javaMap.CallAJO("keySet").CallAJO("iterator");
			while (ajo.CallBool("hasNext"))
			{
				string text = ajo.CallStr("next");
				AndroidJavaObject boxedValueAjo = javaMap.CallAJO("get", text);
				dictionary.Add(text, ParseJavaBoxedValue(boxedValueAjo));
			}
			javaMap.Dispose();
			return dictionary;
		}

		public static List<T> FromJavaList<T>(this AndroidJavaObject javaList)
		{
			if (javaList == null || javaList.IsJavaNull())
			{
				return new List<T>();
			}
			int num = javaList.CallInt("size");
			List<T> list = new List<T>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(javaList.Call<T>("get", new object[1]
				{
					i
				}));
			}
			javaList.Dispose();
			return list;
		}

		public static object ParseJavaBoxedValue(AndroidJavaObject boxedValueAjo)
		{
			if (boxedValueAjo == null || boxedValueAjo.IsJavaNull())
			{
				return null;
			}
			switch (boxedValueAjo.GetClassSimpleName())
			{
			case "Boolean":
				return boxedValueAjo.CallBool("booleanValue");
			case "Float":
				return boxedValueAjo.CallFloat("floatValue");
			case "Integer":
				return boxedValueAjo.CallInt("intValue");
			case "Long":
				return boxedValueAjo.CallLong("longValue");
			case "String":
				return boxedValueAjo.CallStr("toString");
			default:
				return boxedValueAjo;
			}
		}

		public static int ToAndroidColor(this Color32 color32)
		{
			return color32.r * 65536 + color32.g * 256 + color32.b;
		}
	}
}
