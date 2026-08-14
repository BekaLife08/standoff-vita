using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class PlayerIsBannedRpcException : RpcException
	{
		public PlayerIsBannedRpcException()
			: base(9999)
		{
		}
	}
}
