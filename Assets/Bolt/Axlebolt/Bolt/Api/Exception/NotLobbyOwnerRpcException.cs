using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotLobbyOwnerRpcException : RpcException
	{
		public NotLobbyOwnerRpcException()
			: base(5002)
		{
		}
	}
}
