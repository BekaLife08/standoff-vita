using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotInLobbyRpcException : RpcException
	{
		public NotInLobbyRpcException()
			: base(5004)
		{
		}
	}
}
