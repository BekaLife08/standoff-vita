using System;

public class StringUtils
{
	public static string FormatTwoDigit(double seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		return TwoDigit(timeSpan.Minutes) + ":" + TwoDigit(timeSpan.Seconds);
	}

	public static string FormatWithDots(double seconds)
	{
		return TimeSpan.FromSeconds(seconds).Seconds + "...";
	}

	public static string TwoDigit(int time)
	{
		if (time > 9)
		{
			return time + string.Empty;
		}
		return "0" + time;
	}
}
