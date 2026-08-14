using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotInviteOwnerRpcException : RpcException
	{
		public NotInviteOwnerRpcException()
			: base(5009)
		{
		}
	}
}
