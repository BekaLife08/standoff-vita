using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotEnoughPermissionRpcException : RpcException
	{
		public NotEnoughPermissionRpcException()
			: base(2002)
		{
		}
	}
}
