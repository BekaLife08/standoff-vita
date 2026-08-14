using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class NotEnoughItemsRpcException : RpcException
	{
		public NotEnoughItemsRpcException()
			: base(3304)
		{
		}
	}
}
