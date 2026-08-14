using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class UserConfigBinary : IMessage<UserConfigBinary>, IMessage, IEquatable<UserConfigBinary>, IDeepCloneable<UserConfigBinary>
	{
		private static readonly MessageParser<UserConfigBinary> _parser = new MessageParser<UserConfigBinary>(() => new UserConfigBinary());

		public const int ConfigFieldNumber = 1;

		private ByteString config_ = ByteString.Empty;

		[DebuggerNonUserCode]
		public static MessageParser<UserConfigBinary> Parser
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
				return CommonMessageReflection.Descriptor.MessageTypes[2];
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
		public ByteString Config
		{
			get
			{
				return config_;
			}
			set
			{
				config_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public UserConfigBinary()
		{
		}

		[DebuggerNonUserCode]
		public UserConfigBinary(UserConfigBinary other)
			: this()
		{
			config_ = other.config_;
		}

		[DebuggerNonUserCode]
		public UserConfigBinary Clone()
		{
			return new UserConfigBinary(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as UserConfigBinary);
		}

		[DebuggerNonUserCode]
		public bool Equals(UserConfigBinary other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Config != other.Config)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Config.Length != 0)
			{
				num ^= Config.GetHashCode();
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
			if (Config.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteBytes(Config);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Config.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(Config);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(UserConfigBinary other)
		{
			if (other != null && other.Config.Length != 0)
			{
				Config = other.Config;
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 10)
				{
					input.SkipLastField();
				}
				else
				{
					Config = input.ReadBytes();
				}
			}
		}
	}
}
