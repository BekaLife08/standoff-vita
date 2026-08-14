using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClanMemberRole : IMessage<ClanMemberRole>, IMessage, IEquatable<ClanMemberRole>, IDeepCloneable<ClanMemberRole>
	{
		private static readonly MessageParser<ClanMemberRole> _parser = new MessageParser<ClanMemberRole>(() => new ClanMemberRole());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int LevelFieldNumber = 2;

		private int level_;

		public const int PermissionsFieldNumber = 3;

		private static readonly FieldCodec<ClanMemberRolePermission> _repeated_permissions_codec = FieldCodec.ForEnum(26u, (ClanMemberRolePermission x) => (int)x, (int x) => (ClanMemberRolePermission)x);

		private readonly RepeatedField<ClanMemberRolePermission> permissions_ = new RepeatedField<ClanMemberRolePermission>();

		[DebuggerNonUserCode]
		public static MessageParser<ClanMemberRole> Parser
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
				return ClanMessageReflection.Descriptor.MessageTypes[2];
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
		public int Level
		{
			get
			{
				return level_;
			}
			set
			{
				level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ClanMemberRolePermission> Permissions
		{
			get
			{
				return permissions_;
			}
		}

		[DebuggerNonUserCode]
		public ClanMemberRole()
		{
		}

		[DebuggerNonUserCode]
		public ClanMemberRole(ClanMemberRole other)
			: this()
		{
			name_ = other.name_;
			level_ = other.level_;
			permissions_ = other.permissions_.Clone();
		}

		[DebuggerNonUserCode]
		public ClanMemberRole Clone()
		{
			return new ClanMemberRole(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClanMemberRole);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClanMemberRole other)
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
			if (Level != other.Level)
			{
				return false;
			}
			if (!permissions_.Equals(other.permissions_))
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
			if (Level != 0)
			{
				num ^= Level.GetHashCode();
			}
			return num ^ permissions_.GetHashCode();
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
			if (Level != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Level);
			}
			permissions_.WriteTo(output, _repeated_permissions_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (Level != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Level);
			}
			return num + permissions_.CalculateSize(_repeated_permissions_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClanMemberRole other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.Level != 0)
				{
					Level = other.Level;
				}
				permissions_.Add(other.permissions_);
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
					Level = input.ReadInt32();
					break;
				case 24u:
				case 26u:
					permissions_.AddEntriesFrom(input, _repeated_permissions_codec);
					break;
				}
			}
		}
	}
}
