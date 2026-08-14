using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayerStatus : IMessage<PlayerStatus>, IMessage, IEquatable<PlayerStatus>, IDeepCloneable<PlayerStatus>
	{
		private static readonly MessageParser<PlayerStatus> _parser = new MessageParser<PlayerStatus>(() => new PlayerStatus());

		public const int PlayerIdFieldNumber = 1;

		private string playerId_ = "";

		public const int PlayInGameFieldNumber = 2;

		private PlayInGame playInGame_;

		public const int OnlineStatusFieldNumber = 3;

		private OnlineStatus onlineStatus_ = OnlineStatus.StateOffline;

		[DebuggerNonUserCode]
		public static MessageParser<PlayerStatus> Parser
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
				return PlayerMessageReflection.Descriptor.MessageTypes[1];
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
		public string PlayerId
		{
			get
			{
				return playerId_;
			}
			set
			{
				playerId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public PlayInGame PlayInGame
		{
			get
			{
				return playInGame_;
			}
			set
			{
				playInGame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnlineStatus OnlineStatus
		{
			get
			{
				return onlineStatus_;
			}
			set
			{
				onlineStatus_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PlayerStatus()
		{
		}

		[DebuggerNonUserCode]
		public PlayerStatus(PlayerStatus other)
			: this()
		{
			playerId_ = other.playerId_;
			PlayInGame = ((other.playInGame_ != null) ? other.PlayInGame.Clone() : null);
			onlineStatus_ = other.onlineStatus_;
		}

		[DebuggerNonUserCode]
		public PlayerStatus Clone()
		{
			return new PlayerStatus(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerStatus);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerStatus other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (PlayerId != other.PlayerId)
			{
				return false;
			}
			if (!object.Equals(PlayInGame, other.PlayInGame))
			{
				return false;
			}
			if (OnlineStatus != other.OnlineStatus)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (PlayerId.Length != 0)
			{
				num ^= PlayerId.GetHashCode();
			}
			if (playInGame_ != null)
			{
				num ^= PlayInGame.GetHashCode();
			}
			if (OnlineStatus != 0)
			{
				num ^= OnlineStatus.GetHashCode();
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
			if (PlayerId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(PlayerId);
			}
			if (playInGame_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(PlayInGame);
			}
			if (OnlineStatus != 0)
			{
				output.WriteRawTag(24);
				output.WriteEnum((int)OnlineStatus);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (PlayerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PlayerId);
			}
			if (playInGame_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(PlayInGame);
			}
			if (OnlineStatus != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)OnlineStatus);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerStatus other)
		{
			if (other == null)
			{
				return;
			}
			if (other.PlayerId.Length != 0)
			{
				PlayerId = other.PlayerId;
			}
			if (other.playInGame_ != null)
			{
				if (playInGame_ == null)
				{
					playInGame_ = new PlayInGame();
				}
				PlayInGame.MergeFrom(other.PlayInGame);
			}
			if (other.OnlineStatus != 0)
			{
				OnlineStatus = other.OnlineStatus;
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
					PlayerId = input.ReadString();
					break;
				case 18u:
					if (playInGame_ == null)
					{
						playInGame_ = new PlayInGame();
					}
					input.ReadMessage(playInGame_);
					break;
				case 24u:
					onlineStatus_ = (OnlineStatus)input.ReadEnum();
					break;
				}
			}
		}
	}
}
