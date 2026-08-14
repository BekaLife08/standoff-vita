using DeadMosquito.AndroidGoodies.Internal;
using System;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGVibrator
	{
		private const int REPEAT = -1;

		public static bool HasVibrator()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			try
			{
				return AGSystemService.VibratorService.Call<bool>("hasVibrator", Array.Empty<object>());
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static void Vibrate(long durationInMillisec)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGSystemService.VibratorService.Call("vibrate", durationInMillisec);
			}
		}

		public static void VibratePattern(long[] pattern)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGSystemService.VibratorService.Call("vibrate", pattern, -1);
			}
		}
	}
}
