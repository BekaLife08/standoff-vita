using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class LobbyInvite : IMessage<LobbyInvite>, IMessage, IEquatable<LobbyInvite>, IDeepCloneable<LobbyInvite>
	{
		private static readonly MessageParser<LobbyInvite> _parser = new MessageParser<LobbyInvite>(() => new LobbyInvite());

		public const int LobbyIdFieldNumber = 1;

		private string lobbyId_ = "";

		public const int InviteCreatorFieldNumber = 2;

		private PlayerFriend inviteCreator_;

		public const int DateFieldNumber = 3;

		private long date_;

		[DebuggerNonUserCode]
		public static MessageParser<LobbyInvite> Parser
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
				return MatchmakingMessageReflection.Descriptor.MessageTypes[2];
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
		public string LobbyId
		{
			get
			{
				return lobbyId_;
			}
			set
			{
				lobbyId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public PlayerFriend InviteCreator
		{
			get
			{
				return inviteCreator_;
			}
			set
			{
				inviteCreator_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Date
		{
			get
			{
				return date_;
			}
			set
			{
				date_ = value;
			}
		}

		[DebuggerNonUserCode]
		public LobbyInvite()
		{
		}

		[DebuggerNonUserCode]
		public LobbyInvite(LobbyInvite other)
			: this()
		{
			lobbyId_ = other.lobbyId_;
			InviteCreator = ((other.inviteCreator_ != null) ? other.InviteCreator.Clone() : null);
			date_ = other.date_;
		}

		[DebuggerNonUserCode]
		public LobbyInvite Clone()
		{
			return new LobbyInvite(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as LobbyInvite);
		}

		[DebuggerNonUserCode]
		public bool Equals(LobbyInvite other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (LobbyId != other.LobbyId)
			{
				return false;
			}
			if (!object.Equals(InviteCreator, other.InviteCreator))
			{
				return false;
			}
			if (Date != other.Date)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (LobbyId.Length != 0)
			{
				num ^= LobbyId.GetHashCode();
			}
			if (inviteCreator_ != null)
			{
				num ^= InviteCreator.GetHashCode();
			}
			if (Date != 0)
			{
				num ^= Date.GetHashCode();
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
			if (LobbyId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(LobbyId);
			}
			if (inviteCreator_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(InviteCreator);
			}
			if (Date != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt64(Date);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (LobbyId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(LobbyId);
			}
			if (inviteCreator_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(InviteCreator);
			}
			if (Date != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Date);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(LobbyInvite other)
		{
			if (other == null)
			{
				return;
			}
			if (other.LobbyId.Length != 0)
			{
				LobbyId = other.LobbyId;
			}
			if (other.inviteCreator_ != null)
			{
				if (inviteCreator_ == null)
				{
					inviteCreator_ = new PlayerFriend();
				}
				InviteCreator.MergeFrom(other.InviteCreator);
			}
			if (other.Date != 0)
			{
				Date = other.Date;
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
					LobbyId = input.ReadString();
					break;
				case 18u:
					if (inviteCreator_ == null)
					{
						inviteCreator_ = new PlayerFriend();
					}
					input.ReadMessage(inviteCreator_);
					break;
				case 24u:
					Date = input.ReadInt64();
					break;
				}
			}
		}
	}
}
