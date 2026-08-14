using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class StorePlayerStat : IMessage<StorePlayerStat>, IMessage, IEquatable<StorePlayerStat>, IDeepCloneable<StorePlayerStat>
	{
		private static readonly MessageParser<StorePlayerStat> _parser = new MessageParser<StorePlayerStat>(() => new StorePlayerStat());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int StoreIntFieldNumber = 2;

		private int storeInt_;

		public const int StoreFloatFieldNumber = 3;

		private float storeFloat_;

		[DebuggerNonUserCode]
		public static MessageParser<StorePlayerStat> Parser
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
				return PlayerStatsMessageReflection.Descriptor.MessageTypes[3];
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
		public int StoreInt
		{
			get
			{
				return storeInt_;
			}
			set
			{
				storeInt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float StoreFloat
		{
			get
			{
				return storeFloat_;
			}
			set
			{
				storeFloat_ = value;
			}
		}

		[DebuggerNonUserCode]
		public StorePlayerStat()
		{
		}

		[DebuggerNonUserCode]
		public StorePlayerStat(StorePlayerStat other)
			: this()
		{
			name_ = other.name_;
			storeInt_ = other.storeInt_;
			storeFloat_ = other.storeFloat_;
		}

		[DebuggerNonUserCode]
		public StorePlayerStat Clone()
		{
			return new StorePlayerStat(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as StorePlayerStat);
		}

		[DebuggerNonUserCode]
		public bool Equals(StorePlayerStat other)
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
			if (StoreInt != other.StoreInt)
			{
				return false;
			}
			if (StoreFloat != other.StoreFloat)
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
			if (StoreInt != 0)
			{
				num ^= StoreInt.GetHashCode();
			}
			if (StoreFloat != 0f)
			{
				num ^= StoreFloat.GetHashCode();
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
			if (StoreInt != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(StoreInt);
			}
			if (StoreFloat != 0f)
			{
				output.WriteRawTag(29);
				output.WriteFloat(StoreFloat);
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
			if (StoreInt != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(StoreInt);
			}
			if (StoreFloat != 0f)
			{
				num += 5;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(StorePlayerStat other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.StoreInt != 0)
				{
					StoreInt = other.StoreInt;
				}
				if (other.StoreFloat != 0f)
				{
					StoreFloat = other.StoreFloat;
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
					StoreInt = input.ReadInt32();
					break;
				case 29u:
					StoreFloat = input.ReadFloat();
					break;
				}
			}
		}
	}
}
