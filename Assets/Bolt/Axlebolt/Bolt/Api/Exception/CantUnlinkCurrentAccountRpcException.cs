using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class CantUnlinkCurrentAccountRpcException : RpcException
	{
		public CantUnlinkCurrentAccountRpcException()
			: base(2007)
		{
		}
	}
}
