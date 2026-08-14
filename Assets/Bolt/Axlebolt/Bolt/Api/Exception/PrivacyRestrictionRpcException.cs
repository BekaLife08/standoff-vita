using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class PrivacyRestrictionRpcException : RpcException
	{
		public PrivacyRestrictionRpcException()
			: base(2001)
		{
		}
	}
}
