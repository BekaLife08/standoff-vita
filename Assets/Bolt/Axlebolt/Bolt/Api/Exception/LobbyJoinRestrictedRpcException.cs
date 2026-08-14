using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class LobbyJoinRestrictedRpcException : RpcException
	{
		public LobbyJoinRestrictedRpcException()
			: base(5005)
		{
		}
	}
}
