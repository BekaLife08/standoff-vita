using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClanMember : IMessage<ClanMember>, IMessage, IEquatable<ClanMember>, IDeepCloneable<ClanMember>
	{
		private static readonly MessageParser<ClanMember> _parser = new MessageParser<ClanMember>(() => new ClanMember());

		public const int PlayerFieldNumber = 1;

		private Player player_;

		public const int OnlineFieldNumber = 2;

		private bool online_;

		public const int RoleFieldNumber = 3;

		private ClanMemberRole role_;

		[DebuggerNonUserCode]
		public static MessageParser<ClanMember> Parser
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
				return ClanMessageReflection.Descriptor.MessageTypes[1];
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
		public Player Player
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
		public bool Online
		{
			get
			{
				return online_;
			}
			set
			{
				online_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ClanMemberRole Role
		{
			get
			{
				return role_;
			}
			set
			{
				role_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ClanMember()
		{
		}

		[DebuggerNonUserCode]
		public ClanMember(ClanMember other)
			: this()
		{
			Player = ((other.player_ != null) ? other.Player.Clone() : null);
			online_ = other.online_;
			Role = ((other.role_ != null) ? other.Role.Clone() : null);
		}

		[DebuggerNonUserCode]
		public ClanMember Clone()
		{
			return new ClanMember(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClanMember);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClanMember other)
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
			if (Online != other.Online)
			{
				return false;
			}
			if (!object.Equals(Role, other.Role))
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
			if (Online)
			{
				num ^= Online.GetHashCode();
			}
			if (role_ != null)
			{
				num ^= Role.GetHashCode();
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
			if (Online)
			{
				output.WriteRawTag(16);
				output.WriteBool(Online);
			}
			if (role_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(Role);
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
			if (Online)
			{
				num += 2;
			}
			if (role_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Role);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClanMember other)
		{
			if (other == null)
			{
				return;
			}
			if (other.player_ != null)
			{
				if (player_ == null)
				{
					player_ = new Player();
				}
				Player.MergeFrom(other.Player);
			}
			if (other.Online)
			{
				Online = other.Online;
			}
			if (other.role_ != null)
			{
				if (role_ == null)
				{
					role_ = new ClanMemberRole();
				}
				Role.MergeFrom(other.Role);
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
						player_ = new Player();
					}
					input.ReadMessage(player_);
					break;
				case 16u:
					Online = input.ReadBool();
					break;
				case 26u:
					if (role_ == null)
					{
						role_ = new ClanMemberRole();
					}
					input.ReadMessage(role_);
					break;
				}
			}
		}
	}
}
