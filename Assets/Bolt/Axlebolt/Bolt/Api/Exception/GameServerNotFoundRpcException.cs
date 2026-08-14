using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class GameServerNotFoundRpcException : RpcException
	{
		public GameServerNotFoundRpcException()
			: base(6001)
		{
		}
	}
}
