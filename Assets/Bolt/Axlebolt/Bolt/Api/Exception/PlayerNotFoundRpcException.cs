using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class PlayerNotFoundRpcException : RpcException
	{
		public PlayerNotFoundRpcException()
			: base(3101)
		{
		}
	}
}
