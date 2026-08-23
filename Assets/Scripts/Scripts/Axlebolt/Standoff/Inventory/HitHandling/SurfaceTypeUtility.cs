using Axlebolt.Standoff.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	public class SurfaceTypeUtility
	{
		private static readonly Log Log;

		public const string TagPrefix = "SurfaceType/";

		private static readonly Dictionary<string, SurfaceType> EnumByTag;

		static SurfaceTypeUtility()
		{
			Log = Log.Create(typeof(HitCaster));
			EnumByTag = new Dictionary<string, SurfaceType>();
			foreach (SurfaceType item in Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>())
			{
				EnumByTag[GetTag(item)] = item;
			}
		}

		public static string GetTag(SurfaceType surfaceType)
		{
			return "SurfaceType/" + surfaceType;
		}

		public static SurfaceType FromTag(string surfaceType)
		{
			if (EnumByTag.TryGetValue(surfaceType, out SurfaceType value))
			{
				return value;
			}
			Log.Error($"Unsupported SurfaceType {surfaceType}");
			return SurfaceType.Unknown;
		}

		public static bool IsSupportedTag(string tag)
		{
			return EnumByTag.ContainsKey(tag);
		}
	}
}
