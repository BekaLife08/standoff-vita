using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class LobbyNotFoundRpcException : RpcException
	{
		public LobbyNotFoundRpcException()
			: base(5001)
		{
		}
	}
}
