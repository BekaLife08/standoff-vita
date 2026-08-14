using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class GoogleInvalidCodeRpcException : RpcException
	{
		public GoogleInvalidCodeRpcException()
			: base(2101)
		{
		}
	}
}
