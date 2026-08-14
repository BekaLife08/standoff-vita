using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Clan : IMessage<Clan>, IMessage, IEquatable<Clan>, IDeepCloneable<Clan>
	{
		private static readonly MessageParser<Clan> _parser = new MessageParser<Clan>(() => new Clan());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int TagFieldNumber = 2;

		private string tag_ = "";

		public const int ClanTypeFieldNumber = 3;

		private ClanType clanType_ = ClanType.Closed;

		[DebuggerNonUserCode]
		public static MessageParser<Clan> Parser
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
				return ClanMessageReflection.Descriptor.MessageTypes[0];
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
		public string Tag
		{
			get
			{
				return tag_;
			}
			set
			{
				tag_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ClanType ClanType
		{
			get
			{
				return clanType_;
			}
			set
			{
				clanType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Clan()
		{
		}

		[DebuggerNonUserCode]
		public Clan(Clan other)
			: this()
		{
			name_ = other.name_;
			tag_ = other.tag_;
			clanType_ = other.clanType_;
		}

		[DebuggerNonUserCode]
		public Clan Clone()
		{
			return new Clan(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Clan);
		}

		[DebuggerNonUserCode]
		public bool Equals(Clan other)
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
			if (Tag != other.Tag)
			{
				return false;
			}
			if (ClanType != other.ClanType)
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
			if (Tag.Length != 0)
			{
				num ^= Tag.GetHashCode();
			}
			if (ClanType != 0)
			{
				num ^= ClanType.GetHashCode();
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
			if (Tag.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Tag);
			}
			if (ClanType != 0)
			{
				output.WriteRawTag(24);
				output.WriteEnum((int)ClanType);
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
			if (Tag.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Tag);
			}
			if (ClanType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)ClanType);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Clan other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.Tag.Length != 0)
				{
					Tag = other.Tag;
				}
				if (other.ClanType != 0)
				{
					ClanType = other.ClanType;
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
				case 18u:
					Tag = input.ReadString();
					break;
				case 24u:
					clanType_ = (ClanType)input.ReadEnum();
					break;
				}
			}
		}
	}
}
