using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class BlockedPlayerRpcException : RpcException
	{
		public BlockedPlayerRpcException()
			: base(3102)
		{
		}
	}
}
