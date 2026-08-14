using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class CommonMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static CommonMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChRjb21tb25fbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQihAEK" + "BkZpbHRlchIMCgRuYW1lGAEgASgJEhAKCGludFZhbHVlGAIgASgFEhIKCmZs" + "b2F0VmFsdWUYAyABKAISEwoLc3RyaW5nVmFsdWUYBCABKAkSMQoKY29tcGFy" + "aXNvbhgFIAEoDjIdLmNvbS5heGxlYm9sdC5ib2x0LkNvbXBhcmlzb24iKgoM" + "QXZhdGFyQmluYXJ5EgoKAmlkGAEgASgJEg4KBmF2YXRhchgCIAEoDCIiChBV" + "c2VyQ29uZmlnQmluYXJ5Eg4KBmNvbmZpZxgBIAEoDCJ5CgpEaWN0aW9uYXJ5" + "EjsKB2NvbnRlbnQYASADKAsyKi5jb20uYXhsZWJvbHQuYm9sdC5EaWN0aW9u" + "YXJ5LkNvbnRlbnRFbnRyeRouCgxDb250ZW50RW50cnkSCwoDa2V5GAEgASgJ" + "Eg0KBXZhbHVlGAIgASgJOgI4ASIyCgpHYW1lU2VydmVyEgoKAmlkGAEgASgJ" + "EgoKAmlwGAIgASgJEgwKBHBvcnQYAyABKAUiyAEKClBob3RvbkdhbWUSDgoG" + "cmVnaW9uGAEgASgJEg4KBnJvb21JZBgCIAEoCRISCgphcHBWZXJzaW9uGAMg" + "ASgJEk0KEGN1c3RvbVByb3BlcnRpZXMYBCADKAsyMy5jb20uYXhsZWJvbHQu" + "Ym9sdC5QaG90b25HYW1lLkN1c3RvbVByb3BlcnRpZXNFbnRyeRo3ChVDdXN0" + "b21Qcm9wZXJ0aWVzRW50cnkSCwoDa2V5GAEgASgJEg0KBXZhbHVlGAIgASgJ" + "OgI4ASqAAQoKQ29tcGFyaXNvbhIZChVFUVVBTF9UT19PUl9MRVNTX1RIQU4Q" + "ABINCglMRVNTX1RIQU4QARIJCgVFUVVBTBACEhAKDEdSRUFURVJfVEhBThAD" + "EhwKGEVRVUFMX1RPX09SX0dSRUFURVJfVEhBThAEEg0KCU5PVF9FUVVBTBAF" + "QjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9s" + "dC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[1] { typeof(Comparison) }, new GeneratedClrTypeInfo[6]
			{
				new GeneratedClrTypeInfo(typeof(Filter), Filter.Parser, new string[5] { "Name", "IntValue", "FloatValue", "StringValue", "Comparison" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(AvatarBinary), AvatarBinary.Parser, new string[2] { "Id", "Avatar" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(UserConfigBinary), UserConfigBinary.Parser, new string[1] { "Config" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Dictionary), Dictionary.Parser, new string[1] { "Content" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(GameServer), GameServer.Parser, new string[3] { "Id", "Ip", "Port" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PhotonGame), PhotonGame.Parser, new string[4] { "Region", "RoomId", "AppVersion", "CustomProperties" }, null, null, new GeneratedClrTypeInfo[1])
			}));
		}
	}
}
