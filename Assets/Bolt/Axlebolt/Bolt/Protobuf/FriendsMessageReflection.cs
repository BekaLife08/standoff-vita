using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class FriendsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static FriendsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChVmcmllbmRzX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0GhRw" + "bGF5ZXJfbWVzc2FnZS5wcm90byKcAQoMUGxheWVyRnJpZW5kEikKBnBsYXll" + "chgBIAEoCzIZLmNvbS5heGxlYm9sdC5ib2x0LlBsYXllchJBChJyZWxhdGlv" + "bnNoaXBTdGF0dXMYAiABKA4yJS5jb20uYXhsZWJvbHQuYm9sdC5SZWxhdGlv" + "bnNoaXBTdGF0dXMSHgoWbGFzdFJlbGF0aW9uc2hpcFVwZGF0ZRgDIAEoAypw" + "ChJSZWxhdGlvbnNoaXBTdGF0dXMSCAoETm9uZRAAEgsKB0Jsb2NrZWQQARIU" + "ChBSZXF1ZXN0UmVjaXBpZW50EAISCgoGRnJpZW5kEAMSFAoQUmVxdWVzdElu" + "aXRpYXRvchAEEgsKB0lnbm9yZWQQBSpBCg1NZXNzYWdlU3RhdHVzEggKBFNl" + "bnQQABIMCghSZWNlaXZlZBABEggKBFJlYWQQAhIOCgpTZW50RmFpbGVkEANC" + "NQoaY29tLmF4bGVib2x0LmJvbHQucHJvdG9idWaqAhZBeGxlYm9sdC5Cb2x0" + "LlByb3RvYnVmYgZwcm90bzM=");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { PlayerMessageReflection.Descriptor }, new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(RelationshipStatus),
				typeof(MessageStatus)
			}, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(PlayerFriend), PlayerFriend.Parser, new string[3] { "Player", "RelationshipStatus", "LastRelationshipUpdate" }, null, null, null)
			}));
		}
	}
}
