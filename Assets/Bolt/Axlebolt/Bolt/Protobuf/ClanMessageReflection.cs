using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class ClanMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static ClanMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChJjbGFuX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0GhRwbGF5" + "ZXJfbWVzc2FnZS5wcm90byJQCgRDbGFuEgwKBG5hbWUYASABKAkSCwoDdGFn" + "GAIgASgJEi0KCGNsYW5UeXBlGAMgASgOMhsuY29tLmF4bGVib2x0LmJvbHQu" + "Q2xhblR5cGUieAoKQ2xhbk1lbWJlchIpCgZwbGF5ZXIYASABKAsyGS5jb20u" + "YXhsZWJvbHQuYm9sdC5QbGF5ZXISDgoGb25saW5lGAIgASgIEi8KBHJvbGUY" + "AyABKAsyIS5jb20uYXhsZWJvbHQuYm9sdC5DbGFuTWVtYmVyUm9sZSJvCg5D" + "bGFuTWVtYmVyUm9sZRIMCgRuYW1lGAEgASgJEg0KBWxldmVsGAIgASgFEkAK" + "C3Blcm1pc3Npb25zGAMgAygOMisuY29tLmF4bGVib2x0LmJvbHQuQ2xhbk1l" + "bWJlclJvbGVQZXJtaXNzaW9uKjwKCENsYW5UeXBlEgoKBkNMT1NFRBAAEg8K" + "C0lOVklURV9PTkxZEAESEwoPQU5ZT05FX0NBTl9KT0lOEAIq4gEKGENsYW5N" + "ZW1iZXJSb2xlUGVybWlzc2lvbhIYChRDSEFOR0VfQ0xBTl9TRVRUSU5HUxAA" + "EhEKDUFDQ0VQVF9NRU1CRVIQARIRCg1JTlZJVEVfTUVNQkVSEAISFAoQS0lD" + "S19NRU1CRVJfTEVTUxADEhUKEUtJQ0tfTUVNQkVSX0VRVUFMEAQSFAoQQVNT" + "SUdOX1JPTEVfTEVTUxAFEhUKEUFTU0lHTl9ST0xFX0VRVUFMEAYSFgoSQ1JF" + "QVRFX0NMQU5fQkFUVExFEAcSFAoQSk9JTl9DTEFOX0JBVFRMRRAIQjUKGmNv" + "bS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Qcm90" + "b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { PlayerMessageReflection.Descriptor }, new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(ClanType),
				typeof(ClanMemberRolePermission)
			}, new GeneratedClrTypeInfo[3]
			{
				new GeneratedClrTypeInfo(typeof(Clan), Clan.Parser, new string[3] { "Name", "Tag", "ClanType" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanMember), ClanMember.Parser, new string[3] { "Player", "Online", "Role" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanMemberRole), ClanMemberRole.Parser, new string[3] { "Name", "Level", "Permissions" }, null, null, null)
			}));
		}
	}
}
