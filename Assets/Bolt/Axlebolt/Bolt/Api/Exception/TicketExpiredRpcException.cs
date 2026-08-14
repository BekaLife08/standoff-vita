using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class TicketExpiredRpcException : RpcException
	{
		public TicketExpiredRpcException()
			: base(2003)
		{
		}
	}
}
