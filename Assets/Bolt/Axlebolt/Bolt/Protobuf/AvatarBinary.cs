using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class AvatarBinary : IMessage<AvatarBinary>, IMessage, IEquatable<AvatarBinary>, IDeepCloneable<AvatarBinary>
	{
		private static readonly MessageParser<AvatarBinary> _parser = new MessageParser<AvatarBinary>(() => new AvatarBinary());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int AvatarFieldNumber = 2;

		private ByteString avatar_ = ByteString.Empty;

		[DebuggerNonUserCode]
		public static MessageParser<AvatarBinary> Parser
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
				return CommonMessageReflection.Descriptor.MessageTypes[1];
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
		public string Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ByteString Avatar
		{
			get
			{
				return avatar_;
			}
			set
			{
				avatar_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public AvatarBinary()
		{
		}

		[DebuggerNonUserCode]
		public AvatarBinary(AvatarBinary other)
			: this()
		{
			id_ = other.id_;
			avatar_ = other.avatar_;
		}

		[DebuggerNonUserCode]
		public AvatarBinary Clone()
		{
			return new AvatarBinary(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as AvatarBinary);
		}

		[DebuggerNonUserCode]
		public bool Equals(AvatarBinary other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (Avatar != other.Avatar)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id.Length != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (Avatar.Length != 0)
			{
				num ^= Avatar.GetHashCode();
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
			if (Id.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Id);
			}
			if (Avatar.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteBytes(Avatar);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Id);
			}
			if (Avatar.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(Avatar);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AvatarBinary other)
		{
			if (other != null)
			{
				if (other.Id.Length != 0)
				{
					Id = other.Id;
				}
				if (other.Avatar.Length != 0)
				{
					Avatar = other.Avatar;
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
					Id = input.ReadString();
					break;
				case 18u:
					Avatar = input.ReadBytes();
					break;
				}
			}
		}
	}
}
