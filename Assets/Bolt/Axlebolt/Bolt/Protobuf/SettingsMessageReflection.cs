using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class SettingsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static SettingsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChZzZXR0aW5nc19tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9sdCJq" + "Cg1QbGF5ZXJTZXR0aW5nEgsKA2tleRgBIAEoCRIQCghpbnRWYWx1ZRgCIAEo" + "BRISCgpmbG9hdFZhbHVlGAMgASgCEhEKCWJvb2xWYWx1ZRgEIAEoCBITCgtz" + "dHJpbmdWYWx1ZRgFIAEoCUI1Chpjb20uYXhsZWJvbHQuYm9sdC5wcm90b2J1" + "ZqoCFkF4bGVib2x0LkJvbHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(PlayerSetting), PlayerSetting.Parser, new string[5] { "Key", "IntValue", "FloatValue", "BoolValue", "StringValue" }, null, null, null)
			}));
		}
	}
}
