using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGBattery
	{
		private const string BatteryManagerEXTRA_LEVEL = "level";

		private const string BatteryManagerEXTRA_SCALE = "scale";

		public static float GetBatteryChargeLevel()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0f;
			}
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.IntentFilter", "android.intent.action.BATTERY_CHANGED"))
			{
				using (AndroidJavaObject androidJavaObject2 = AGUtils.Activity.CallAJO("registerReceiver", null, androidJavaObject))
				{
					int num = androidJavaObject2.Call<int>("getIntExtra", new object[2]
					{
						"level",
						-1
					});
					int num2 = androidJavaObject2.Call<int>("getIntExtra", new object[2]
					{
						"scale",
						-1
					});
					if (num == -1 || num2 == -1)
					{
						return 50f;
					}
					return (float)num / (float)num2 * 100f;
				}
			}
		}
	}
}
