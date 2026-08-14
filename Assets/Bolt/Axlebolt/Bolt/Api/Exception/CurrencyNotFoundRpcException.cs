using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class CurrencyNotFoundRpcException : RpcException
	{
		public CurrencyNotFoundRpcException()
			: base(3301)
		{
		}
	}
}
