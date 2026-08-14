using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotEnoughRecipeComponentsRpcException : RpcException
	{
		public NotEnoughRecipeComponentsRpcException()
			: base(3403)
		{
		}
	}
}
