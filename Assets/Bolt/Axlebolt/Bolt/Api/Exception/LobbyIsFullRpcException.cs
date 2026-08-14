using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class LobbyIsFullRpcException : RpcException
	{
		public LobbyIsFullRpcException()
			: base(5006)
		{
		}
	}
}
