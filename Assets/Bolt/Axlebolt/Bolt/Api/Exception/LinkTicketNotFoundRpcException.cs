using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class LinkTicketNotFoundRpcException : RpcException
	{
		public LinkTicketNotFoundRpcException()
			: base(2006)
		{
		}
	}
}
