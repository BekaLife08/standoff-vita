using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class InvalidArgumentRpcException : RpcException
	{
		public InvalidArgumentRpcException()
			: base(4001)
		{
		}
	}
}
