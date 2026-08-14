using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Filter : IMessage<Filter>, IMessage, IEquatable<Filter>, IDeepCloneable<Filter>
	{
		private static readonly MessageParser<Filter> _parser = new MessageParser<Filter>(() => new Filter());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int IntValueFieldNumber = 2;

		private int intValue_;

		public const int FloatValueFieldNumber = 3;

		private float floatValue_;

		public const int StringValueFieldNumber = 4;

		private string stringValue_ = "";

		public const int ComparisonFieldNumber = 5;

		private Comparison comparison_ = Comparison.EqualToOrLessThan;

		[DebuggerNonUserCode]
		public static MessageParser<Filter> Parser
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
				return CommonMessageReflection.Descriptor.MessageTypes[0];
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
		public string Name
		{
			get
			{
				return name_;
			}
			set
			{
				name_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int IntValue
		{
			get
			{
				return intValue_;
			}
			set
			{
				intValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float FloatValue
		{
			get
			{
				return floatValue_;
			}
			set
			{
				floatValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string StringValue
		{
			get
			{
				return stringValue_;
			}
			set
			{
				stringValue_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public Comparison Comparison
		{
			get
			{
				return comparison_;
			}
			set
			{
				comparison_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Filter()
		{
		}

		[DebuggerNonUserCode]
		public Filter(Filter other)
			: this()
		{
			name_ = other.name_;
			intValue_ = other.intValue_;
			floatValue_ = other.floatValue_;
			stringValue_ = other.stringValue_;
			comparison_ = other.comparison_;
		}

		[DebuggerNonUserCode]
		public Filter Clone()
		{
			return new Filter(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Filter);
		}

		[DebuggerNonUserCode]
		public bool Equals(Filter other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Name != other.Name)
			{
				return false;
			}
			if (IntValue != other.IntValue)
			{
				return false;
			}
			if (FloatValue != other.FloatValue)
			{
				return false;
			}
			if (StringValue != other.StringValue)
			{
				return false;
			}
			if (Comparison != other.Comparison)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (IntValue != 0)
			{
				num ^= IntValue.GetHashCode();
			}
			if (FloatValue != 0f)
			{
				num ^= FloatValue.GetHashCode();
			}
			if (StringValue.Length != 0)
			{
				num ^= StringValue.GetHashCode();
			}
			if (Comparison != 0)
			{
				num ^= Comparison.GetHashCode();
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
			if (Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Name);
			}
			if (IntValue != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(IntValue);
			}
			if (FloatValue != 0f)
			{
				output.WriteRawTag(29);
				output.WriteFloat(FloatValue);
			}
			if (StringValue.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(StringValue);
			}
			if (Comparison != 0)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)Comparison);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (IntValue != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(IntValue);
			}
			if (FloatValue != 0f)
			{
				num += 5;
			}
			if (StringValue.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(StringValue);
			}
			if (Comparison != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Comparison);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Filter other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.IntValue != 0)
				{
					IntValue = other.IntValue;
				}
				if (other.FloatValue != 0f)
				{
					FloatValue = other.FloatValue;
				}
				if (other.StringValue.Length != 0)
				{
					StringValue = other.StringValue;
				}
				if (other.Comparison != 0)
				{
					Comparison = other.Comparison;
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
				case 10u:
					Name = input.ReadString();
					break;
				case 16u:
					IntValue = input.ReadInt32();
					break;
				case 29u:
					FloatValue = input.ReadFloat();
					break;
				case 34u:
					StringValue = input.ReadString();
					break;
				case 40u:
					comparison_ = (Comparison)input.ReadEnum();
					break;
				}
			}
		}
	}
}
