using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class InventoryMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static InventoryMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChdpbnZlbnRvcnlfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQa" + "FmN1cnJlbmN5X21lc3NhZ2UucHJvdG8ihwEKD1BsYXllckludmVudG9yeRI1" + "CgpjdXJyZW5jaWVzGAEgAygLMiEuY29tLmF4bGVib2x0LmJvbHQuQ3VycmVu" + "Y3lBbW91bnQSPQoOaW52ZW50b3J5SXRlbXMYAiADKAsyJS5jb20uYXhsZWJv" + "bHQuYm9sdC5JbnZlbnRvcnlJdGVtU3RhY2sifwoSSW52ZW50b3J5SXRlbVN0" + "YWNrEg8KB3N0YWNrSWQYASABKAUSNwoNaW52ZW50b3J5SXRlbRgCIAEoCzIg" + "LmNvbS5heGxlYm9sdC5ib2x0LkludmVudG9yeUl0ZW0SEAoIcXVhbnRpdHkY" + "AyABKAUSDQoFZmxhZ3MYBCABKAUilAIKDUludmVudG9yeUl0ZW0SCgoCaWQY" + "ASABKAUSEwoLZGlzcGxheU5hbWUYAiABKAkSRAoKcHJvcGVydGllcxgDIAMo" + "CzIwLmNvbS5heGxlYm9sdC5ib2x0LkludmVudG9yeUl0ZW0uUHJvcGVydGll" + "c0VudHJ5EjMKCGJ1eVByaWNlGAQgAygLMiEuY29tLmF4bGVib2x0LmJvbHQu" + "Q3VycmVuY3lBbW91bnQSNAoJc2VsbFByaWNlGAUgAygLMiEuY29tLmF4bGVi" + "b2x0LmJvbHQuQ3VycmVuY3lBbW91bnQaMQoPUHJvcGVydGllc0VudHJ5EgsK" + "A2tleRgBIAEoCRINCgV2YWx1ZRgCIAEoCToCOAEiPQoTSW52ZW50b3J5SXRl" + "bUFtb3VudBIXCg9pbnZlbnRvcnlJdGVtSWQYASABKAUSDQoFdmFsdWUYAiAB" + "KAUihgEKDkV4Y2hhbmdlUmVzdWx0EjUKCmN1cnJlbmNpZXMYASADKAsyIS5j" + "b20uYXhsZWJvbHQuYm9sdC5DdXJyZW5jeUFtb3VudBI9Cg5pbnZlbnRvcnlJ" + "dGVtcxgCIAMoCzIlLmNvbS5heGxlYm9sdC5ib2x0LkludmVudG9yeUl0ZW1T" + "dGFja0I1Chpjb20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVib2x0" + "LkJvbHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { CurrencyMessageReflection.Descriptor }, new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[5]
			{
				new GeneratedClrTypeInfo(typeof(PlayerInventory), PlayerInventory.Parser, new string[2] { "Currencies", "InventoryItems" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(InventoryItemStack), InventoryItemStack.Parser, new string[4] { "StackId", "InventoryItem", "Quantity", "Flags" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(InventoryItem), InventoryItem.Parser, new string[5] { "Id", "DisplayName", "Properties", "BuyPrice", "SellPrice" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(InventoryItemAmount), InventoryItemAmount.Parser, new string[2] { "InventoryItemId", "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ExchangeResult), ExchangeResult.Parser, new string[2] { "Currencies", "InventoryItems" }, null, null, null)
			}));
		}
	}
}
