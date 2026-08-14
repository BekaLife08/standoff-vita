using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class InventoryItemNotFoundRpcException : RpcException
	{
		public InventoryItemNotFoundRpcException()
			: base(3201)
		{
		}
	}
}
