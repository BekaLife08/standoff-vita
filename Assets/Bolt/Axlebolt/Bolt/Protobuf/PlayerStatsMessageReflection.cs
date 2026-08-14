using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class PlayerStatsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static PlayerStatsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChpwbGF5ZXJfc3RhdHNfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJv" + "bHQiaQoFU3RhdHMSKwoEc3RhdBgBIAMoCzIdLmNvbS5heGxlYm9sdC5ib2x0" + "LlBsYXllclN0YXQSMwoLYWNoaWV2ZW1lbnQYAiADKAsyHi5jb20uYXhsZWJv" + "bHQuYm9sdC5BY2hpZXZlbWVudCJ+CgpQbGF5ZXJTdGF0EgwKBG5hbWUYASAB" + "KAkSEAoIaW50VmFsdWUYAiABKAUSEgoKZmxvYXRWYWx1ZRgDIAEoAhIOCgZ3" + "aW5kb3cYBCABKAISLAoEdHlwZRgFIAEoDjIeLmNvbS5heGxlYm9sdC5ib2x0" + "LlN0YXREZWZUeXBlIpABCgtBY2hpZXZlbWVudBIMCgRuYW1lGAEgASgJEhMK" + "C2Rpc3BsYXlOYW1lGAIgASgJEhoKEmRpc3BsYXlEZXNjcmlwdGlvbhgDIAEo" + "CRISCgp1bmxvY2tUaW1lGAQgASgDEhAKCGFjaGlldmVkGAUgASgIEgwKBGlj" + "b24YBiABKAwSDgoGaGlkZGVuGAcgASgIIkUKD1N0b3JlUGxheWVyU3RhdBIM" + "CgRuYW1lGAEgASgJEhAKCHN0b3JlSW50GAIgASgFEhIKCnN0b3JlRmxvYXQY" + "AyABKAIiMgoQU3RvcmVBY2hpZXZlbWVudBIMCgRuYW1lGAEgASgJEhAKCGFj" + "aGlldmVkGAIgASgIKi4KC1N0YXREZWZUeXBlEgcKA0lOVBAAEgkKBUZMT0FU" + "EAESCwoHQVZHUkFURRACQjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVm" + "qgIWQXhsZWJvbHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[1] { typeof(StatDefType) }, new GeneratedClrTypeInfo[5]
			{
				new GeneratedClrTypeInfo(typeof(Stats), Stats.Parser, new string[2] { "Stat", "Achievement" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PlayerStat), PlayerStat.Parser, new string[5] { "Name", "IntValue", "FloatValue", "Window", "Type" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Achievement), Achievement.Parser, new string[7] { "Name", "DisplayName", "DisplayDescription", "UnlockTime", "Achieved", "Icon", "Hidden" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(StorePlayerStat), StorePlayerStat.Parser, new string[3] { "Name", "StoreInt", "StoreFloat" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(StoreAchievement), StoreAchievement.Parser, new string[2] { "Name", "Achieved" }, null, null, null)
			}));
		}
	}
}
