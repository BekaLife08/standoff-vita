using System;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class LocationUtils
	{
		public static void ComputeDistanceAndBearing(double lat1, double lon1, double lat2, double lon2, float[] results)
		{
			int num = 20;
			lat1 *= Math.PI / 180.0;
			lat2 *= Math.PI / 180.0;
			lon1 *= Math.PI / 180.0;
			lon2 *= Math.PI / 180.0;
			double num2 = 6378137.0;
			double num3 = 6356752.3142;
			double num4 = (num2 - num3) / num2;
			double num5 = (num2 * num2 - num3 * num3) / (num3 * num3);
			double num6 = lon2 - lon1;
			double num7 = 0.0;
			double num8 = Math.Atan((1.0 - num4) * Math.Tan(lat1));
			double num9 = Math.Atan((1.0 - num4) * Math.Tan(lat2));
			double num10 = Math.Cos(num8);
			double num11 = Math.Cos(num9);
			double num12 = Math.Sin(num8);
			double num13 = Math.Sin(num9);
			double num14 = num10 * num11;
			double num15 = num12 * num13;
			double num16 = 0.0;
			double num17 = 0.0;
			double num18 = 0.0;
			double num19 = 0.0;
			double num20 = 0.0;
			double num21 = 0.0;
			double num22 = 0.0;
			double num23 = 0.0;
			double num24 = num6;
			for (int i = 0; i < num; i++)
			{
				double num25 = num24;
				num22 = Math.Cos(num24);
				num23 = Math.Sin(num24);
				double num26 = num11 * num23;
				double num27 = num10 * num13 - num12 * num11 * num22;
				double d = num26 * num26 + num27 * num27;
				num21 = Math.Sqrt(d);
				num20 = num15 + num14 * num22;
				num16 = Math.Atan2(num21, num20);
				double num28 = (num21 != 0.0) ? (num14 * num23 / num21) : 0.0;
				num18 = 1.0 - num28 * num28;
				num19 = ((num18 != 0.0) ? (num20 - 2.0 * num15 / num18) : 0.0);
				double num29 = num18 * num5;
				num7 = 1.0 + num29 / 16384.0 * (4096.0 + num29 * (-768.0 + num29 * (320.0 - 175.0 * num29)));
				double num30 = num29 / 1024.0 * (256.0 + num29 * (-128.0 + num29 * (74.0 - 47.0 * num29)));
				double num31 = num4 / 16.0 * num18 * (4.0 + num4 * (4.0 - 3.0 * num18));
				double num32 = num19 * num19;
				num17 = num30 * num21 * (num19 + num30 / 4.0 * (num20 * (-1.0 + 2.0 * num32) - num30 / 6.0 * num19 * (-3.0 + 4.0 * num21 * num21) * (-3.0 + 4.0 * num32)));
				num24 = num6 + (1.0 - num31) * num4 * num28 * (num16 + num31 * num21 * (num19 + num31 * num20 * (-1.0 + 2.0 * num19 * num19)));
				double value = (num24 - num25) / num24;
				if (Math.Abs(value) < 1E-12)
				{
					break;
				}
			}
			float num33 = results[0] = (float)(num3 * num7 * (num16 - num17));
			if (results.Length > 1)
			{
				float num34 = (float)Math.Atan2(num11 * num23, num10 * num13 - num12 * num11 * num22);
				num34 = (results[1] = num34 * (180f / (float)Math.PI));
				if (results.Length > 2)
				{
					float num35 = (float)Math.Atan2(num10 * num23, (0.0 - num12) * num11 + num10 * num13 * num22);
					num35 = (results[2] = num35 * (180f / (float)Math.PI));
				}
			}
		}
	}
}
