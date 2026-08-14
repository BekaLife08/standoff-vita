using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CurrencyAmount : IMessage<CurrencyAmount>, IMessage, IEquatable<CurrencyAmount>, IDeepCloneable<CurrencyAmount>
	{
		private static readonly MessageParser<CurrencyAmount> _parser = new MessageParser<CurrencyAmount>(() => new CurrencyAmount());

		public const int CurrencyIdFieldNumber = 1;

		private int currencyId_;

		public const int ValueFieldNumber = 2;

		private int value_;

		[DebuggerNonUserCode]
		public static MessageParser<CurrencyAmount> Parser
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
				return CurrencyMessageReflection.Descriptor.MessageTypes[1];
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
		public int CurrencyId
		{
			get
			{
				return currencyId_;
			}
			set
			{
				currencyId_ = value;
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
		public CurrencyAmount()
		{
		}

		[DebuggerNonUserCode]
		public CurrencyAmount(CurrencyAmount other)
			: this()
		{
			currencyId_ = other.currencyId_;
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public CurrencyAmount Clone()
		{
			return new CurrencyAmount(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CurrencyAmount);
		}

		[DebuggerNonUserCode]
		public bool Equals(CurrencyAmount other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (CurrencyId != other.CurrencyId)
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
			if (CurrencyId != 0)
			{
				num ^= CurrencyId.GetHashCode();
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
			if (CurrencyId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(CurrencyId);
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
			if (CurrencyId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(CurrencyId);
			}
			if (Value != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Value);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CurrencyAmount other)
		{
			if (other != null)
			{
				if (other.CurrencyId != 0)
				{
					CurrencyId = other.CurrencyId;
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
					CurrencyId = input.ReadInt32();
					break;
				case 16u:
					Value = input.ReadInt32();
					break;
				}
			}
		}
	}
}
