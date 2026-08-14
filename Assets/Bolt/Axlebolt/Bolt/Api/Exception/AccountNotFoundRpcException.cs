using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class AccountNotFoundRpcException : RpcException
	{
		public AccountNotFoundRpcException()
			: base(2005)
		{
		}
	}
}
