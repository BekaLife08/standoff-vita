using System;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal static class Check
	{
		public delegate bool Condition();

		public static class Argument
		{
			public static void IsNotNull(object argument, string argumentName, string message = "")
			{
				if (string.IsNullOrEmpty(message))
				{
					message = $"[{argumentName}] must not be null or empty";
				}
				if (argument == null)
				{
					throw new ArgumentNullException(argumentName, message);
				}
			}

			public static void IsStrNotNullOrEmpty(string argument, string argumentName, string message = null)
			{
				if (string.IsNullOrEmpty(argument))
				{
					if (message == null)
					{
						message = $"[{argumentName}] must not be null or empty";
					}
					throw new ArgumentException(message, argumentName);
				}
			}

			public static void IsNotNegative(int argument, string argumentName)
			{
				if (argument < 0)
				{
					throw new ArgumentOutOfRangeException(argumentName, argumentName + " must not be negative.");
				}
			}
		}

		public static void IsTrue(Condition condition, string message = "")
		{
			if (!condition())
			{
				throw new ArgumentException(message);
			}
		}
	}
}
