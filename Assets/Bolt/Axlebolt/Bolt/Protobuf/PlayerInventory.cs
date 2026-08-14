using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayerInventory : IMessage<PlayerInventory>, IMessage, IEquatable<PlayerInventory>, IDeepCloneable<PlayerInventory>
	{
		private static readonly MessageParser<PlayerInventory> _parser = new MessageParser<PlayerInventory>(() => new PlayerInventory());

		public const int CurrenciesFieldNumber = 1;

		private static readonly FieldCodec<CurrencyAmount> _repeated_currencies_codec = FieldCodec.ForMessage(10u, CurrencyAmount.Parser);

		private readonly RepeatedField<CurrencyAmount> currencies_ = new RepeatedField<CurrencyAmount>();

		public const int InventoryItemsFieldNumber = 2;

		private static readonly FieldCodec<InventoryItemStack> _repeated_inventoryItems_codec = FieldCodec.ForMessage(18u, InventoryItemStack.Parser);

		private readonly RepeatedField<InventoryItemStack> inventoryItems_ = new RepeatedField<InventoryItemStack>();

		[DebuggerNonUserCode]
		public static MessageParser<PlayerInventory> Parser
		{
			get
			{
				return _parser;
			}
		}

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return InventoryMessageReflection.Descriptor.MessageTypes[0];
			}
		}

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Descriptor;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CurrencyAmount> Currencies
		{
			get
			{
				return currencies_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<InventoryItemStack> InventoryItems
		{
			get
			{
				return inventoryItems_;
			}
		}

		[DebuggerNonUserCode]
		public PlayerInventory()
		{
		}

		[DebuggerNonUserCode]
		public PlayerInventory(PlayerInventory other)
			: this()
		{
			currencies_ = other.currencies_.Clone();
			inventoryItems_ = other.inventoryItems_.Clone();
		}

		[DebuggerNonUserCode]
		public PlayerInventory Clone()
		{
			return new PlayerInventory(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerInventory);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerInventory other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!currencies_.Equals(other.currencies_))
			{
				return false;
			}
			if (!inventoryItems_.Equals(other.inventoryItems_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= currencies_.GetHashCode();
			return num ^ inventoryItems_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			currencies_.WriteTo(output, _repeated_currencies_codec);
			inventoryItems_.WriteTo(output, _repeated_inventoryItems_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += currencies_.CalculateSize(_repeated_currencies_codec);
			return num + inventoryItems_.CalculateSize(_repeated_inventoryItems_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerInventory other)
		{
			if (other != null)
			{
				currencies_.Add(other.currencies_);
				inventoryItems_.Add(other.inventoryItems_);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				switch (num)
				{
				default:
					input.SkipLastField();
					break;
				case 10u:
					currencies_.AddEntriesFrom(input, _repeated_currencies_codec);
					break;
				case 18u:
					inventoryItems_.AddEntriesFrom(input, _repeated_inventoryItems_codec);
					break;
				}
			}
		}
	}
}
