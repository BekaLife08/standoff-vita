using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotLobbyMemberRpcException : RpcException
	{
		public NotLobbyMemberRpcException()
			: base(5003)
		{
		}
	}
}
