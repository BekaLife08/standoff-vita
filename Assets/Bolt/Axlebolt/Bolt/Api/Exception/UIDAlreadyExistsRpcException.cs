using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class UIDAlreadyExistsRpcException : RpcException
	{
		public UIDAlreadyExistsRpcException()
			: base(1001)
		{
		}
	}
}
