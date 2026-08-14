using System;

namespace Axlebolt.Standoff.Common
{
	public class ResourceNotFoundException : Exception
	{
		public ResourceNotFoundException(string path)
			: base($"Resource {path} not found")
		{
		}
	}
}
