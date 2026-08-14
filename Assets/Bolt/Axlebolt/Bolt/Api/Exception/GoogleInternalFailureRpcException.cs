using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class GoogleInternalFailureRpcException : RpcException
	{
		public GoogleInternalFailureRpcException()
			: base(2104)
		{
		}
	}
}
