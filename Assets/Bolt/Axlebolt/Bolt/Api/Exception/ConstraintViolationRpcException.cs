using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class ConstraintViolationRpcException : RpcException
	{
		public ConstraintViolationRpcException()
			: base(7001)
		{
		}
	}
}
