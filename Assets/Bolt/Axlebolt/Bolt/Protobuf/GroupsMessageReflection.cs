using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class GroupsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static GroupsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChRncm91cHNfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQaFHBs" + "YXllcl9tZXNzYWdlLnByb3RvIl8KBUdyb3VwEgoKAmlkGAEgASgJEgwKBG5h" + "bWUYAiABKAkSEAoIYXZhdGFySWQYAyABKAkSKgoHcGxheWVycxgEIAMoCzIZ" + "LmNvbS5heGxlYm9sdC5ib2x0LlBsYXllckI1Chpjb20uYXhsZWJvbHQuYm9s" + "dC5wcm90b2J1ZqoCFkF4bGVib2x0LkJvbHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { PlayerMessageReflection.Descriptor }, new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(Group), Group.Parser, new string[4] { "Id", "Name", "AvatarId", "Players" }, null, null, null)
			}));
		}
	}
}
