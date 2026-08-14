using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class WrongCurrencyRpcException : RpcException
	{
		public WrongCurrencyRpcException()
			: base(3302)
		{
		}
	}
}
