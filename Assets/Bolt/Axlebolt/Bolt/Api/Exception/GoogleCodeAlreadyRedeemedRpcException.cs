using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class GoogleCodeAlreadyRedeemedRpcException : RpcException
	{
		public GoogleCodeAlreadyRedeemedRpcException()
			: base(2102)
		{
		}
	}
}
