using System;

namespace Axlebolt.Bolt
{
	public class BoltApiException : Exception
	{
		public BoltApiException(string message)
			: base(message)
		{
		}
	}
}
