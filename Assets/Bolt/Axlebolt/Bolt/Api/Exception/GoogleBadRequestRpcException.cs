using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class GoogleBadRequestRpcException : RpcException
	{
		public GoogleBadRequestRpcException()
			: base(2103)
		{
		}
	}
}
