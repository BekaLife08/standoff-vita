using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Storage
{
	public class BoltStorageFileMapper : MessageMapper<Axlebolt.Bolt.Protobuf.Storage, BoltStorageFile>
	{
		public static readonly BoltStorageFileMapper Instance = new BoltStorageFileMapper();

		public override BoltStorageFile ToOriginal(Axlebolt.Bolt.Protobuf.Storage storage)
		{
			return new BoltStorageFile
			{
				Filename = storage.Filename,
				File = storage.File.ToByteArray()
			};
		}
	}
}
