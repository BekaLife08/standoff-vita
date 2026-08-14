using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class ExchangeRecipeNotFoundRpcException : RpcException
	{
		public ExchangeRecipeNotFoundRpcException()
			: base(3401)
		{
		}
	}
}
