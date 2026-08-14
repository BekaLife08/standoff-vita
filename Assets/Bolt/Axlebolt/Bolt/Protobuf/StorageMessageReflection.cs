using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class StorageMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static StorageMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChVzdG9yYWdlX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0IikK" + "B1N0b3JhZ2USEAoIZmlsZW5hbWUYASABKAkSDAoEZmlsZRgCIAEoDEI1Chpj" + "b20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVib2x0LkJvbHQuUHJv" + "dG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(Storage), Storage.Parser, new string[2] { "Filename", "File" }, null, null, null)
			}));
		}
	}
}
