using System;

namespace Axlebolt.Standoff.Photon
{
	public class PhotonException : Exception
	{
		public PhotonException()
		{
		}

		public PhotonException(string message)
			: base(message)
		{
		}
	}
}
