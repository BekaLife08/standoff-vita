using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayerStat : IMessage<PlayerStat>, IMessage, IEquatable<PlayerStat>, IDeepCloneable<PlayerStat>
	{
		private static readonly MessageParser<PlayerStat> _parser = new MessageParser<PlayerStat>(() => new PlayerStat());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int IntValueFieldNumber = 2;

		private int intValue_;

		public const int FloatValueFieldNumber = 3;

		private float floatValue_;

		public const int WindowFieldNumber = 4;

		private float window_;

		public const int TypeFieldNumber = 5;

		private StatDefType type_ = StatDefType.Int;

		[DebuggerNonUserCode]
		public static MessageParser<PlayerStat> Parser
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
				return PlayerStatsMessageReflection.Descriptor.MessageTypes[1];
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
		public float Window
		{
			get
			{
				return window_;
			}
			set
			{
				window_ = value;
			}
		}

		[DebuggerNonUserCode]
		public StatDefType Type
		{
			get
			{
				return type_;
			}
			set
			{
				type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerStat()
		{
		}

		[DebuggerNonUserCode]
		public PlayerStat(PlayerStat other)
			: this()
		{
			name_ = other.name_;
			intValue_ = other.intValue_;
			floatValue_ = other.floatValue_;
			window_ = other.window_;
			type_ = other.type_;
		}

		[DebuggerNonUserCode]
		public PlayerStat Clone()
		{
			return new PlayerStat(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerStat);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerStat other)
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
			if (Window != other.Window)
			{
				return false;
			}
			if (Type != other.Type)
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
			if (Window != 0f)
			{
				num ^= Window.GetHashCode();
			}
			if (Type != 0)
			{
				num ^= Type.GetHashCode();
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
			if (Window != 0f)
			{
				output.WriteRawTag(37);
				output.WriteFloat(Window);
			}
			if (Type != 0)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)Type);
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
			if (Window != 0f)
			{
				num += 5;
			}
			if (Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerStat other)
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
				if (other.Window != 0f)
				{
					Window = other.Window;
				}
				if (other.Type != 0)
				{
					Type = other.Type;
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
				case 37u:
					Window = input.ReadFloat();
					break;
				case 40u:
					type_ = (StatDefType)input.ReadEnum();
					break;
				}
			}
		}
	}
}
