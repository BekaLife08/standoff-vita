using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayerFriend : IMessage<PlayerFriend>, IMessage, IEquatable<PlayerFriend>, IDeepCloneable<PlayerFriend>
	{
		private static readonly MessageParser<PlayerFriend> _parser = new MessageParser<PlayerFriend>(() => new PlayerFriend());

		public const int PlayerFieldNumber = 1;

		private Player player_;

		public const int RelationshipStatusFieldNumber = 2;

		private RelationshipStatus relationshipStatus_ = RelationshipStatus.None;

		public const int LastRelationshipUpdateFieldNumber = 3;

		private long lastRelationshipUpdate_;

		[DebuggerNonUserCode]
		public static MessageParser<PlayerFriend> Parser
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
				return FriendsMessageReflection.Descriptor.MessageTypes[0];
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
		public RelationshipStatus RelationshipStatus
		{
			get
			{
				return relationshipStatus_;
			}
			set
			{
				relationshipStatus_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long LastRelationshipUpdate
		{
			get
			{
				return lastRelationshipUpdate_;
			}
			set
			{
				lastRelationshipUpdate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerFriend()
		{
		}

		[DebuggerNonUserCode]
		public PlayerFriend(PlayerFriend other)
			: this()
		{
			Player = ((other.player_ != null) ? other.Player.Clone() : null);
			relationshipStatus_ = other.relationshipStatus_;
			lastRelationshipUpdate_ = other.lastRelationshipUpdate_;
		}

		[DebuggerNonUserCode]
		public PlayerFriend Clone()
		{
			return new PlayerFriend(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerFriend);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerFriend other)
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
			if (RelationshipStatus != other.RelationshipStatus)
			{
				return false;
			}
			if (LastRelationshipUpdate != other.LastRelationshipUpdate)
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
			if (RelationshipStatus != 0)
			{
				num ^= RelationshipStatus.GetHashCode();
			}
			if (LastRelationshipUpdate != 0)
			{
				num ^= LastRelationshipUpdate.GetHashCode();
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
			if (RelationshipStatus != 0)
			{
				output.WriteRawTag(16);
				output.WriteEnum((int)RelationshipStatus);
			}
			if (LastRelationshipUpdate != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt64(LastRelationshipUpdate);
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
			if (RelationshipStatus != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)RelationshipStatus);
			}
			if (LastRelationshipUpdate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(LastRelationshipUpdate);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerFriend other)
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
			if (other.RelationshipStatus != 0)
			{
				RelationshipStatus = other.RelationshipStatus;
			}
			if (other.LastRelationshipUpdate != 0)
			{
				LastRelationshipUpdate = other.LastRelationshipUpdate;
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
					relationshipStatus_ = (RelationshipStatus)input.ReadEnum();
					break;
				case 24u:
					LastRelationshipUpdate = input.ReadInt64();
					break;
				}
			}
		}
	}
}
