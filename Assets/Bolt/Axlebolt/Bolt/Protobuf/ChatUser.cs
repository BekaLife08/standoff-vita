using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ChatUser : IMessage<ChatUser>, IMessage, IEquatable<ChatUser>, IDeepCloneable<ChatUser>
	{
		private static readonly MessageParser<ChatUser> _parser = new MessageParser<ChatUser>(() => new ChatUser());

		public const int PlayerFieldNumber = 1;

		private PlayerFriend player_;

		public const int GroupFieldNumber = 2;

		private Group group_;

		public const int MessageFieldNumber = 3;

		private string message_ = "";

		public const int TimestampFieldNumber = 4;

		private long timestamp_;

		public const int UnreadMsgsCountFieldNumber = 5;

		private int unreadMsgsCount_;

		[DebuggerNonUserCode]
		public static MessageParser<ChatUser> Parser
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
				return ChatMessageReflection.Descriptor.MessageTypes[0];
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
		public PlayerFriend Player
		{
			get
			{
				return player_;
			}
			set
			{
				player_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Group Group
		{
			get
			{
				return group_;
			}
			set
			{
				group_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Message
		{
			get
			{
				return message_;
			}
			set
			{
				message_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public long Timestamp
		{
			get
			{
				return timestamp_;
			}
			set
			{
				timestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int UnreadMsgsCount
		{
			get
			{
				return unreadMsgsCount_;
			}
			set
			{
				unreadMsgsCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChatUser()
		{
		}

		[DebuggerNonUserCode]
		public ChatUser(ChatUser other)
			: this()
		{
			Player = ((other.player_ != null) ? other.Player.Clone() : null);
			Group = ((other.group_ != null) ? other.Group.Clone() : null);
			message_ = other.message_;
			timestamp_ = other.timestamp_;
			unreadMsgsCount_ = other.unreadMsgsCount_;
		}

		[DebuggerNonUserCode]
		public ChatUser Clone()
		{
			return new ChatUser(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ChatUser);
		}

		[DebuggerNonUserCode]
		public bool Equals(ChatUser other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Player, other.Player))
			{
				return false;
			}
			if (!object.Equals(Group, other.Group))
			{
				return false;
			}
			if (Message != other.Message)
			{
				return false;
			}
			if (Timestamp != other.Timestamp)
			{
				return false;
			}
			if (UnreadMsgsCount != other.UnreadMsgsCount)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (player_ != null)
			{
				num ^= Player.GetHashCode();
			}
			if (group_ != null)
			{
				num ^= Group.GetHashCode();
			}
			if (Message.Length != 0)
			{
				num ^= Message.GetHashCode();
			}
			if (Timestamp != 0)
			{
				num ^= Timestamp.GetHashCode();
			}
			if (UnreadMsgsCount != 0)
			{
				num ^= UnreadMsgsCount.GetHashCode();
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
			if (player_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(Player);
			}
			if (group_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Group);
			}
			if (Message.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(Message);
			}
			if (Timestamp != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt64(Timestamp);
			}
			if (UnreadMsgsCount != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(UnreadMsgsCount);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (player_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Player);
			}
			if (group_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Group);
			}
			if (Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Message);
			}
			if (Timestamp != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Timestamp);
			}
			if (UnreadMsgsCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(UnreadMsgsCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ChatUser other)
		{
			if (other == null)
			{
				return;
			}
			if (other.player_ != null)
			{
				if (player_ == null)
				{
					player_ = new PlayerFriend();
				}
				Player.MergeFrom(other.Player);
			}
			if (other.group_ != null)
			{
				if (group_ == null)
				{
					group_ = new Group();
				}
				Group.MergeFrom(other.Group);
			}
			if (other.Message.Length != 0)
			{
				Message = other.Message;
			}
			if (other.Timestamp != 0)
			{
				Timestamp = other.Timestamp;
			}
			if (other.UnreadMsgsCount != 0)
			{
				UnreadMsgsCount = other.UnreadMsgsCount;
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
					if (player_ == null)
					{
						player_ = new PlayerFriend();
					}
					input.ReadMessage(player_);
					break;
				case 18u:
					if (group_ == null)
					{
						group_ = new Group();
					}
					input.ReadMessage(group_);
					break;
				case 26u:
					Message = input.ReadString();
					break;
				case 32u:
					Timestamp = input.ReadInt64();
					break;
				case 40u:
					UnreadMsgsCount = input.ReadInt32();
					break;
				}
			}
		}
	}
}
