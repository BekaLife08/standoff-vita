using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class IncorrectLobbyParamRpcException : RpcException
	{
		public IncorrectLobbyParamRpcException()
			: base(5007)
		{
		}
	}
}
