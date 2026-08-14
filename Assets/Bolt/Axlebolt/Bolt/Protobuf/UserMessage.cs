using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class UserMessage : IMessage<UserMessage>, IMessage, IEquatable<UserMessage>, IDeepCloneable<UserMessage>
	{
		private static readonly MessageParser<UserMessage> _parser = new MessageParser<UserMessage>(() => new UserMessage());

		public const int SenderIdFieldNumber = 1;

		private string senderId_ = "";

		public const int MessageFieldNumber = 2;

		private string message_ = "";

		public const int TimestampFieldNumber = 3;

		private long timestamp_;

		public const int IsReadFieldNumber = 4;

		private bool isRead_;

		[DebuggerNonUserCode]
		public static MessageParser<UserMessage> Parser
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
				return ChatMessageReflection.Descriptor.MessageTypes[1];
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
		public string SenderId
		{
			get
			{
				return senderId_;
			}
			set
			{
				senderId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public bool IsRead
		{
			get
			{
				return isRead_;
			}
			set
			{
				isRead_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserMessage()
		{
		}

		[DebuggerNonUserCode]
		public UserMessage(UserMessage other)
			: this()
		{
			senderId_ = other.senderId_;
			message_ = other.message_;
			timestamp_ = other.timestamp_;
			isRead_ = other.isRead_;
		}

		[DebuggerNonUserCode]
		public UserMessage Clone()
		{
			return new UserMessage(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as UserMessage);
		}

		[DebuggerNonUserCode]
		public bool Equals(UserMessage other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (SenderId != other.SenderId)
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
			if (IsRead != other.IsRead)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (SenderId.Length != 0)
			{
				num ^= SenderId.GetHashCode();
			}
			if (Message.Length != 0)
			{
				num ^= Message.GetHashCode();
			}
			if (Timestamp != 0)
			{
				num ^= Timestamp.GetHashCode();
			}
			if (IsRead)
			{
				num ^= IsRead.GetHashCode();
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
			if (SenderId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(SenderId);
			}
			if (Message.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Message);
			}
			if (Timestamp != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt64(Timestamp);
			}
			if (IsRead)
			{
				output.WriteRawTag(32);
				output.WriteBool(IsRead);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (SenderId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(SenderId);
			}
			if (Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Message);
			}
			if (Timestamp != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Timestamp);
			}
			if (IsRead)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(UserMessage other)
		{
			if (other != null)
			{
				if (other.SenderId.Length != 0)
				{
					SenderId = other.SenderId;
				}
				if (other.Message.Length != 0)
				{
					Message = other.Message;
				}
				if (other.Timestamp != 0)
				{
					Timestamp = other.Timestamp;
				}
				if (other.IsRead)
				{
					IsRead = other.IsRead;
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
					SenderId = input.ReadString();
					break;
				case 18u:
					Message = input.ReadString();
					break;
				case 24u:
					Timestamp = input.ReadInt64();
					break;
				case 32u:
					IsRead = input.ReadBool();
					break;
				}
			}
		}
	}
}
