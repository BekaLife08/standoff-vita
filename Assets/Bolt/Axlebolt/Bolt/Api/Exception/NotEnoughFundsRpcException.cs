using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotEnoughFundsRpcException : RpcException
	{
		public NotEnoughFundsRpcException()
			: base(3303)
		{
		}
	}
}
