using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayerSetting : IMessage<PlayerSetting>, IMessage, IEquatable<PlayerSetting>, IDeepCloneable<PlayerSetting>
	{
		private static readonly MessageParser<PlayerSetting> _parser = new MessageParser<PlayerSetting>(() => new PlayerSetting());

		public const int KeyFieldNumber = 1;

		private string key_ = "";

		public const int IntValueFieldNumber = 2;

		private int intValue_;

		public const int FloatValueFieldNumber = 3;

		private float floatValue_;

		public const int BoolValueFieldNumber = 4;

		private bool boolValue_;

		public const int StringValueFieldNumber = 5;

		private string stringValue_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<PlayerSetting> Parser
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
				return SettingsMessageReflection.Descriptor.MessageTypes[0];
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
		public string Key
		{
			get
			{
				return key_;
			}
			set
			{
				key_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public bool BoolValue
		{
			get
			{
				return boolValue_;
			}
			set
			{
				boolValue_ = value;
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
		public PlayerSetting()
		{
		}

		[DebuggerNonUserCode]
		public PlayerSetting(PlayerSetting other)
			: this()
		{
			key_ = other.key_;
			intValue_ = other.intValue_;
			floatValue_ = other.floatValue_;
			boolValue_ = other.boolValue_;
			stringValue_ = other.stringValue_;
		}

		[DebuggerNonUserCode]
		public PlayerSetting Clone()
		{
			return new PlayerSetting(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerSetting);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerSetting other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Key != other.Key)
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
			if (BoolValue != other.BoolValue)
			{
				return false;
			}
			if (StringValue != other.StringValue)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Key.Length != 0)
			{
				num ^= Key.GetHashCode();
			}
			if (IntValue != 0)
			{
				num ^= IntValue.GetHashCode();
			}
			if (FloatValue != 0f)
			{
				num ^= FloatValue.GetHashCode();
			}
			if (BoolValue)
			{
				num ^= BoolValue.GetHashCode();
			}
			if (StringValue.Length != 0)
			{
				num ^= StringValue.GetHashCode();
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
			if (Key.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Key);
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
			if (BoolValue)
			{
				output.WriteRawTag(32);
				output.WriteBool(BoolValue);
			}
			if (StringValue.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(StringValue);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Key.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Key);
			}
			if (IntValue != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(IntValue);
			}
			if (FloatValue != 0f)
			{
				num += 5;
			}
			if (BoolValue)
			{
				num += 2;
			}
			if (StringValue.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(StringValue);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerSetting other)
		{
			if (other != null)
			{
				if (other.Key.Length != 0)
				{
					Key = other.Key;
				}
				if (other.IntValue != 0)
				{
					IntValue = other.IntValue;
				}
				if (other.FloatValue != 0f)
				{
					FloatValue = other.FloatValue;
				}
				if (other.BoolValue)
				{
					BoolValue = other.BoolValue;
				}
				if (other.StringValue.Length != 0)
				{
					StringValue = other.StringValue;
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
					Key = input.ReadString();
					break;
				case 16u:
					IntValue = input.ReadInt32();
					break;
				case 29u:
					FloatValue = input.ReadFloat();
					break;
				case 32u:
					BoolValue = input.ReadBool();
					break;
				case 42u:
					StringValue = input.ReadString();
					break;
				}
			}
		}
	}
}
