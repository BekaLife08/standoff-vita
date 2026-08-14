using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class InventoryItemStackNotFoundRpcException : RpcException
	{
		public InventoryItemStackNotFoundRpcException()
			: base(3202)
		{
		}
	}
}
