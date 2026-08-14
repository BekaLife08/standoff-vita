using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Player : IMessage<Player>, IMessage, IEquatable<Player>, IDeepCloneable<Player>
	{
		private static readonly MessageParser<Player> _parser = new MessageParser<Player>(() => new Player());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int UidFieldNumber = 2;

		private string uid_ = "";

		public const int NameFieldNumber = 3;

		private string name_ = "";

		public const int AvatarIdFieldNumber = 4;

		private string avatarId_ = "";

		public const int TimeInGameFieldNumber = 5;

		private int timeInGame_;

		public const int PlayerStatusFieldNumber = 6;

		private PlayerStatus playerStatus_;

		public const int LogoutDateFieldNumber = 7;

		private long logoutDate_;

		[DebuggerNonUserCode]
		public static MessageParser<Player> Parser
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
				return PlayerMessageReflection.Descriptor.MessageTypes[2];
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
		public string Uid
		{
			get
			{
				return uid_;
			}
			set
			{
				uid_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public int TimeInGame
		{
			get
			{
				return timeInGame_;
			}
			set
			{
				timeInGame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerStatus PlayerStatus
		{
			get
			{
				return playerStatus_;
			}
			set
			{
				playerStatus_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long LogoutDate
		{
			get
			{
				return logoutDate_;
			}
			set
			{
				logoutDate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Player()
		{
		}

		[DebuggerNonUserCode]
		public Player(Player other)
			: this()
		{
			id_ = other.id_;
			uid_ = other.uid_;
			name_ = other.name_;
			avatarId_ = other.avatarId_;
			timeInGame_ = other.timeInGame_;
			PlayerStatus = ((other.playerStatus_ != null) ? other.PlayerStatus.Clone() : null);
			logoutDate_ = other.logoutDate_;
		}

		[DebuggerNonUserCode]
		public Player Clone()
		{
			return new Player(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Player);
		}

		[DebuggerNonUserCode]
		public bool Equals(Player other)
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
			if (Uid != other.Uid)
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
			if (TimeInGame != other.TimeInGame)
			{
				return false;
			}
			if (!object.Equals(PlayerStatus, other.PlayerStatus))
			{
				return false;
			}
			if (LogoutDate != other.LogoutDate)
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
			if (Uid.Length != 0)
			{
				num ^= Uid.GetHashCode();
			}
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (AvatarId.Length != 0)
			{
				num ^= AvatarId.GetHashCode();
			}
			if (TimeInGame != 0)
			{
				num ^= TimeInGame.GetHashCode();
			}
			if (playerStatus_ != null)
			{
				num ^= PlayerStatus.GetHashCode();
			}
			if (LogoutDate != 0)
			{
				num ^= LogoutDate.GetHashCode();
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
			if (Uid.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Uid);
			}
			if (Name.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(Name);
			}
			if (AvatarId.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(AvatarId);
			}
			if (TimeInGame != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(TimeInGame);
			}
			if (playerStatus_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(PlayerStatus);
			}
			if (LogoutDate != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt64(LogoutDate);
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
			if (Uid.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Uid);
			}
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (AvatarId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AvatarId);
			}
			if (TimeInGame != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(TimeInGame);
			}
			if (playerStatus_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(PlayerStatus);
			}
			if (LogoutDate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(LogoutDate);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Player other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.AvatarId.Length != 0)
			{
				AvatarId = other.AvatarId;
			}
			if (other.TimeInGame != 0)
			{
				TimeInGame = other.TimeInGame;
			}
			if (other.playerStatus_ != null)
			{
				if (playerStatus_ == null)
				{
					playerStatus_ = new PlayerStatus();
				}
				PlayerStatus.MergeFrom(other.PlayerStatus);
			}
			if (other.LogoutDate != 0)
			{
				LogoutDate = other.LogoutDate;
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
					Uid = input.ReadString();
					break;
				case 26u:
					Name = input.ReadString();
					break;
				case 34u:
					AvatarId = input.ReadString();
					break;
				case 40u:
					TimeInGame = input.ReadInt32();
					break;
				case 50u:
					if (playerStatus_ == null)
					{
						playerStatus_ = new PlayerStatus();
					}
					input.ReadMessage(playerStatus_);
					break;
				case 56u:
					LogoutDate = input.ReadInt64();
					break;
				}
			}
		}
	}
}
