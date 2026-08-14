using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemAmount : IMessage<InventoryItemAmount>, IMessage, IEquatable<InventoryItemAmount>, IDeepCloneable<InventoryItemAmount>
	{
		private static readonly MessageParser<InventoryItemAmount> _parser = new MessageParser<InventoryItemAmount>(() => new InventoryItemAmount());

		public const int InventoryItemIdFieldNumber = 1;

		private int inventoryItemId_;

		public const int ValueFieldNumber = 2;

		private int value_;

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemAmount> Parser
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
				return InventoryMessageReflection.Descriptor.MessageTypes[3];
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
		public int InventoryItemId
		{
			get
			{
				return inventoryItemId_;
			}
			set
			{
				inventoryItemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Value
		{
			get
			{
				return value_;
			}
			set
			{
				value_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItemAmount()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemAmount(InventoryItemAmount other)
			: this()
		{
			inventoryItemId_ = other.inventoryItemId_;
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public InventoryItemAmount Clone()
		{
			return new InventoryItemAmount(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemAmount);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemAmount other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (InventoryItemId != other.InventoryItemId)
			{
				return false;
			}
			if (Value != other.Value)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (InventoryItemId != 0)
			{
				num ^= InventoryItemId.GetHashCode();
			}
			if (Value != 0)
			{
				num ^= Value.GetHashCode();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (InventoryItemId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(InventoryItemId);
			}
			if (Value != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (InventoryItemId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(InventoryItemId);
			}
			if (Value != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Value);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemAmount other)
		{
			if (other != null)
			{
				if (other.InventoryItemId != 0)
				{
					InventoryItemId = other.InventoryItemId;
				}
				if (other.Value != 0)
				{
					Value = other.Value;
				}
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
				case 8u:
					InventoryItemId = input.ReadInt32();
					break;
				case 16u:
					Value = input.ReadInt32();
					break;
				}
			}
		}
	}
}
