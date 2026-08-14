using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("StorageRemoteService")]
	public class StorageRemoteService : RpcService
	{
		public StorageRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("writeFile")]
		public void WriteFile(string filename, byte[] file, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { filename, file }, ct);
		}

		[Rpc("readFile")]
		public byte[] ReadFile(string filename, CancellationToken ct = default(CancellationToken))
		{
			return (byte[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { filename }, ct);
		}

		[Rpc("readAllFiles")]
		public Axlebolt.Bolt.Protobuf.Storage[] ReadAllFiles(CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Storage[])Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("deleteFile")]
		public void DeleteFile(string filename, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { filename }, ct);
		}
	}
}
