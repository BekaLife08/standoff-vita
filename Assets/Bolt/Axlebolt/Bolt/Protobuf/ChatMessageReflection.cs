using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class ChatMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static ChatMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChJjaGF0X21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0GhVmcmll" + "bmRzX21lc3NhZ2UucHJvdG8aFGdyb3Vwc19tZXNzYWdlLnByb3RvIqEBCghD" + "aGF0VXNlchIvCgZwbGF5ZXIYASABKAsyHy5jb20uYXhsZWJvbHQuYm9sdC5Q" + "bGF5ZXJGcmllbmQSJwoFZ3JvdXAYAiABKAsyGC5jb20uYXhsZWJvbHQuYm9s" + "dC5Hcm91cBIPCgdtZXNzYWdlGAMgASgJEhEKCXRpbWVzdGFtcBgEIAEoAxIX" + "Cg91bnJlYWRNc2dzQ291bnQYBSABKAUiUwoLVXNlck1lc3NhZ2USEAoIc2Vu" + "ZGVySWQYASABKAkSDwoHbWVzc2FnZRgCIAEoCRIRCgl0aW1lc3RhbXAYAyAB" + "KAMSDgoGaXNSZWFkGAQgASgIQjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3Rv" + "YnVmqgIWQXhsZWJvbHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[2]
			{
				FriendsMessageReflection.Descriptor,
				GroupsMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(ChatUser), ChatUser.Parser, new string[5] { "Player", "Group", "Message", "Timestamp", "UnreadMsgsCount" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(UserMessage), UserMessage.Parser, new string[4] { "SenderId", "Message", "Timestamp", "IsRead" }, null, null, null)
			}));
		}
	}
}
