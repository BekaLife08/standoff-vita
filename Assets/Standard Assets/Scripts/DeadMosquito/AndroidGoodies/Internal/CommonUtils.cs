using System;
using System.IO;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class CommonUtils
	{
		private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static byte[] Encode(this Texture2D tex, ImageFormat format)
		{
			return (format != 0) ? tex.EncodeToJPG() : tex.EncodeToPNG();
		}

		public static Texture2D TextureFromFile(string path)
		{
			Texture2D texture2D = null;
			if (File.Exists(path))
			{
				byte[] data = File.ReadAllBytes(path);
				texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
			}
			return texture2D;
		}

		public static long ToMillisSinceEpoch(this DateTime date)
		{
			return (long)(date - Jan1st1970.ToLocalTime()).TotalMilliseconds;
		}

		public static DateTime DateTimeFromMillisSinceEpoch(long millis)
		{
			long num = millis / 1000;
			return Jan1st1970.AddSeconds(num);
		}
	}
}
