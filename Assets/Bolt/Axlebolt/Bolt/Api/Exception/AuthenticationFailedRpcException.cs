using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class AuthenticationFailedRpcException : RpcException
	{
		public AuthenticationFailedRpcException()
			: base(2008)
		{
		}
	}
}
