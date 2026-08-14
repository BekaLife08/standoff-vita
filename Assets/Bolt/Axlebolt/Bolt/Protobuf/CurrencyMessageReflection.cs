using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class CurrencyMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static CurrencyMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChZjdXJyZW5jeV9tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9sdCJM" + "CghDdXJyZW5jeRIKCgJpZBgBIAEoBRIVCg1leGNoYW5nZVJhdGlvGAIgASgC" + "Eh0KFWV4Y2hhbmdhYmxlQ3VycmVuY2llcxgDIAMoBSIzCg5DdXJyZW5jeUFt" + "b3VudBISCgpjdXJyZW5jeUlkGAEgASgFEg0KBXZhbHVlGAIgASgFQjUKGmNv" + "bS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Qcm90" + "b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(Currency), Currency.Parser, new string[3] { "Id", "ExchangeRatio", "ExchangableCurrencies" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(CurrencyAmount), CurrencyAmount.Parser, new string[2] { "CurrencyId", "Value" }, null, null, null)
			}));
		}
	}
}
