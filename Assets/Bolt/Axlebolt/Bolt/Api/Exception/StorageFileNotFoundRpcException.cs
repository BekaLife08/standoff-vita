using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class StorageFileNotFoundRpcException : RpcException
	{
		public StorageFileNotFoundRpcException()
			: base(9998)
		{
		}
	}
}
