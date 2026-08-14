using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class GameNotFoundRpcException : RpcException
	{
		public GameNotFoundRpcException()
			: base(4002)
		{
		}
	}
}
