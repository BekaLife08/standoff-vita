using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Group : IMessage<Group>, IMessage, IEquatable<Group>, IDeepCloneable<Group>
	{
		private static readonly MessageParser<Group> _parser = new MessageParser<Group>(() => new Group());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int NameFieldNumber = 2;

		private string name_ = "";

		public const int AvatarIdFieldNumber = 3;

		private string avatarId_ = "";

		public const int PlayersFieldNumber = 4;

		private static readonly FieldCodec<Player> _repeated_players_codec = FieldCodec.ForMessage(34u, Player.Parser);

		private readonly RepeatedField<Player> players_ = new RepeatedField<Player>();

		[DebuggerNonUserCode]
		public static MessageParser<Group> Parser
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
				return GroupsMessageReflection.Descriptor.MessageTypes[0];
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
		public string AvatarId
		{
			get
			{
				return avatarId_;
			}
			set
			{
				avatarId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Player> Players
		{
			get
			{
				return players_;
			}
		}

		[DebuggerNonUserCode]
		public Group()
		{
		}

		[DebuggerNonUserCode]
		public Group(Group other)
			: this()
		{
			id_ = other.id_;
			name_ = other.name_;
			avatarId_ = other.avatarId_;
			players_ = other.players_.Clone();
		}

		[DebuggerNonUserCode]
		public Group Clone()
		{
			return new Group(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Group);
		}

		[DebuggerNonUserCode]
		public bool Equals(Group other)
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
			if (Name != other.Name)
			{
				return false;
			}
			if (AvatarId != other.AvatarId)
			{
				return false;
			}
			if (!players_.Equals(other.players_))
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
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (AvatarId.Length != 0)
			{
				num ^= AvatarId.GetHashCode();
			}
			return num ^ players_.GetHashCode();
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
			if (Name.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Name);
			}
			if (AvatarId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(AvatarId);
			}
			players_.WriteTo(output, _repeated_players_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Id);
			}
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (AvatarId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AvatarId);
			}
			return num + players_.CalculateSize(_repeated_players_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Group other)
		{
			if (other != null)
			{
				if (other.Id.Length != 0)
				{
					Id = other.Id;
				}
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.AvatarId.Length != 0)
				{
					AvatarId = other.AvatarId;
				}
				players_.Add(other.players_);
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
					Name = input.ReadString();
					break;
				case 26u:
					AvatarId = input.ReadString();
					break;
				case 34u:
					players_.AddEntriesFrom(input, _repeated_players_codec);
					break;
				}
			}
		}
	}
}
