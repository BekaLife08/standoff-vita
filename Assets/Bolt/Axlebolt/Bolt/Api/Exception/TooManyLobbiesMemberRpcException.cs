using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class TooManyLobbiesMemberRpcException : RpcException
	{
		public TooManyLobbiesMemberRpcException()
			: base(5008)
		{
		}
	}
}
