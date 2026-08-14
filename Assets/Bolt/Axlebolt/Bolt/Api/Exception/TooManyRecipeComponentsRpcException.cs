using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class TooManyRecipeComponentsRpcException : RpcException
	{
		public TooManyRecipeComponentsRpcException()
			: base(3404)
		{
		}
	}
}
