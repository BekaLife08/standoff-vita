using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class WrongRecipeComponentsRpcException : RpcException
	{
		public WrongRecipeComponentsRpcException()
			: base(3402)
		{
		}
	}
}
