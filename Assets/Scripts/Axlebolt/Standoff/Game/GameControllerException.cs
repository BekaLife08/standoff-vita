using System;

namespace Axlebolt.Standoff.Game
{
	public class GameControllerException : Exception
	{
		public GameControllerException(string message)
			: base(message)
		{
		}
	}
}
