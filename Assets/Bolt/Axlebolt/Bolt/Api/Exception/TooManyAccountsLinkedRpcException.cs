using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class TooManyAccountsLinkedRpcException : RpcException
	{
		public TooManyAccountsLinkedRpcException()
			: base(2009)
		{
		}
	}
}
