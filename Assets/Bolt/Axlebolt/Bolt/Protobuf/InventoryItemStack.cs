using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemStack : IMessage<InventoryItemStack>, IMessage, IEquatable<InventoryItemStack>, IDeepCloneable<InventoryItemStack>
	{
		private static readonly MessageParser<InventoryItemStack> _parser = new MessageParser<InventoryItemStack>(() => new InventoryItemStack());

		public const int StackIdFieldNumber = 1;

		private int stackId_;

		public const int InventoryItemFieldNumber = 2;

		private InventoryItem inventoryItem_;

		public const int QuantityFieldNumber = 3;

		private int quantity_;

		public const int FlagsFieldNumber = 4;

		private int flags_;

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemStack> Parser
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
				return InventoryMessageReflection.Descriptor.MessageTypes[1];
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
		public int StackId
		{
			get
			{
				return stackId_;
			}
			set
			{
				stackId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItem InventoryItem
		{
			get
			{
				return inventoryItem_;
			}
			set
			{
				inventoryItem_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Quantity
		{
			get
			{
				return quantity_;
			}
			set
			{
				quantity_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Flags
		{
			get
			{
				return flags_;
			}
			set
			{
				flags_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItemStack()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemStack(InventoryItemStack other)
			: this()
		{
			stackId_ = other.stackId_;
			InventoryItem = ((other.inventoryItem_ != null) ? other.InventoryItem.Clone() : null);
			quantity_ = other.quantity_;
			flags_ = other.flags_;
		}

		[DebuggerNonUserCode]
		public InventoryItemStack Clone()
		{
			return new InventoryItemStack(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemStack);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemStack other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (StackId != other.StackId)
			{
				return false;
			}
			if (!object.Equals(InventoryItem, other.InventoryItem))
			{
				return false;
			}
			if (Quantity != other.Quantity)
			{
				return false;
			}
			if (Flags != other.Flags)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (StackId != 0)
			{
				num ^= StackId.GetHashCode();
			}
			if (inventoryItem_ != null)
			{
				num ^= InventoryItem.GetHashCode();
			}
			if (Quantity != 0)
			{
				num ^= Quantity.GetHashCode();
			}
			if (Flags != 0)
			{
				num ^= Flags.GetHashCode();
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
			if (StackId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(StackId);
			}
			if (inventoryItem_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(InventoryItem);
			}
			if (Quantity != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Quantity);
			}
			if (Flags != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(Flags);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (StackId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(StackId);
			}
			if (inventoryItem_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(InventoryItem);
			}
			if (Quantity != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
			}
			if (Flags != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Flags);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemStack other)
		{
			if (other == null)
			{
				return;
			}
			if (other.StackId != 0)
			{
				StackId = other.StackId;
			}
			if (other.inventoryItem_ != null)
			{
				if (inventoryItem_ == null)
				{
					inventoryItem_ = new InventoryItem();
				}
				InventoryItem.MergeFrom(other.InventoryItem);
			}
			if (other.Quantity != 0)
			{
				Quantity = other.Quantity;
			}
			if (other.Flags != 0)
			{
				Flags = other.Flags;
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
					StackId = input.ReadInt32();
					break;
				case 18u:
					if (inventoryItem_ == null)
					{
						inventoryItem_ = new InventoryItem();
					}
					input.ReadMessage(inventoryItem_);
					break;
				case 24u:
					Quantity = input.ReadInt32();
					break;
				case 32u:
					Flags = input.ReadInt32();
					break;
				}
			}
		}
	}
}
