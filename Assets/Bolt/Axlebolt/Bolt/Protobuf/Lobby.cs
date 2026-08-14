using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Lobby : IMessage<Lobby>, IMessage, IEquatable<Lobby>, IDeepCloneable<Lobby>
	{
		private static readonly MessageParser<Lobby> _parser = new MessageParser<Lobby>(() => new Lobby());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int OwnerPlayerIdFieldNumber = 2;

		private string ownerPlayerId_ = "";

		public const int NameFieldNumber = 3;

		private string name_ = "";

		public const int LobbyTypeFieldNumber = 4;

		private LobbyType lobbyType_ = LobbyType.Private;

		public const int JoinableFieldNumber = 5;

		private bool joinable_;

		public const int MaxMembersFieldNumber = 6;

		private int maxMembers_;

		public const int DataFieldNumber = 7;

		private static readonly MapField<string, string>.Codec _map_data_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10u), FieldCodec.ForString(18u), 58u);

		private readonly MapField<string, string> data_ = new MapField<string, string>();

		public const int MembersFieldNumber = 8;

		private static readonly FieldCodec<PlayerFriend> _repeated_members_codec = FieldCodec.ForMessage(66u, PlayerFriend.Parser);

		private readonly RepeatedField<PlayerFriend> members_ = new RepeatedField<PlayerFriend>();

		public const int InvitesFieldNumber = 9;

		private static readonly FieldCodec<PlayerFriend> _repeated_invites_codec = FieldCodec.ForMessage(74u, PlayerFriend.Parser);

		private readonly RepeatedField<PlayerFriend> invites_ = new RepeatedField<PlayerFriend>();

		public const int GameServerFieldNumber = 10;

		private GameServer gameServer_;

		public const int PhotonGameFieldNumber = 11;

		private PhotonGame photonGame_;

		[DebuggerNonUserCode]
		public static MessageParser<Lobby> Parser
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
				return MatchmakingMessageReflection.Descriptor.MessageTypes[1];
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
		public string OwnerPlayerId
		{
			get
			{
				return ownerPlayerId_;
			}
			set
			{
				ownerPlayerId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public LobbyType LobbyType
		{
			get
			{
				return lobbyType_;
			}
			set
			{
				lobbyType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Joinable
		{
			get
			{
				return joinable_;
			}
			set
			{
				joinable_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int MaxMembers
		{
			get
			{
				return maxMembers_;
			}
			set
			{
				maxMembers_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, string> Data
		{
			get
			{
				return data_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PlayerFriend> Members
		{
			get
			{
				return members_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PlayerFriend> Invites
		{
			get
			{
				return invites_;
			}
		}

		[DebuggerNonUserCode]
		public GameServer GameServer
		{
			get
			{
				return gameServer_;
			}
			set
			{
				gameServer_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PhotonGame PhotonGame
		{
			get
			{
				return photonGame_;
			}
			set
			{
				photonGame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Lobby()
		{
		}

		[DebuggerNonUserCode]
		public Lobby(Lobby other)
			: this()
		{
			id_ = other.id_;
			ownerPlayerId_ = other.ownerPlayerId_;
			name_ = other.name_;
			lobbyType_ = other.lobbyType_;
			joinable_ = other.joinable_;
			maxMembers_ = other.maxMembers_;
			data_ = other.data_.Clone();
			members_ = other.members_.Clone();
			invites_ = other.invites_.Clone();
			GameServer = ((other.gameServer_ != null) ? other.GameServer.Clone() : null);
			PhotonGame = ((other.photonGame_ != null) ? other.PhotonGame.Clone() : null);
		}

		[DebuggerNonUserCode]
		public Lobby Clone()
		{
			return new Lobby(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Lobby);
		}

		[DebuggerNonUserCode]
		public bool Equals(Lobby other)
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
			if (OwnerPlayerId != other.OwnerPlayerId)
			{
				return false;
			}
			if (Name != other.Name)
			{
				return false;
			}
			if (LobbyType != other.LobbyType)
			{
				return false;
			}
			if (Joinable != other.Joinable)
			{
				return false;
			}
			if (MaxMembers != other.MaxMembers)
			{
				return false;
			}
			if (!Data.Equals(other.Data))
			{
				return false;
			}
			if (!members_.Equals(other.members_))
			{
				return false;
			}
			if (!invites_.Equals(other.invites_))
			{
				return false;
			}
			if (!object.Equals(GameServer, other.GameServer))
			{
				return false;
			}
			if (!object.Equals(PhotonGame, other.PhotonGame))
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
			if (OwnerPlayerId.Length != 0)
			{
				num ^= OwnerPlayerId.GetHashCode();
			}
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (LobbyType != 0)
			{
				num ^= LobbyType.GetHashCode();
			}
			if (Joinable)
			{
				num ^= Joinable.GetHashCode();
			}
			if (MaxMembers != 0)
			{
				num ^= MaxMembers.GetHashCode();
			}
			num ^= Data.GetHashCode();
			num ^= members_.GetHashCode();
			num ^= invites_.GetHashCode();
			if (gameServer_ != null)
			{
				num ^= GameServer.GetHashCode();
			}
			if (photonGame_ != null)
			{
				num ^= PhotonGame.GetHashCode();
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
			if (OwnerPlayerId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(OwnerPlayerId);
			}
			if (Name.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(Name);
			}
			if (LobbyType != 0)
			{
				output.WriteRawTag(32);
				output.WriteEnum((int)LobbyType);
			}
			if (Joinable)
			{
				output.WriteRawTag(40);
				output.WriteBool(Joinable);
			}
			if (MaxMembers != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(MaxMembers);
			}
			data_.WriteTo(output, _map_data_codec);
			members_.WriteTo(output, _repeated_members_codec);
			invites_.WriteTo(output, _repeated_invites_codec);
			if (gameServer_ != null)
			{
				output.WriteRawTag(82);
				output.WriteMessage(GameServer);
			}
			if (photonGame_ != null)
			{
				output.WriteRawTag(90);
				output.WriteMessage(PhotonGame);
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
			if (OwnerPlayerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(OwnerPlayerId);
			}
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (LobbyType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)LobbyType);
			}
			if (Joinable)
			{
				num += 2;
			}
			if (MaxMembers != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(MaxMembers);
			}
			num += data_.CalculateSize(_map_data_codec);
			num += members_.CalculateSize(_repeated_members_codec);
			num += invites_.CalculateSize(_repeated_invites_codec);
			if (gameServer_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(GameServer);
			}
			if (photonGame_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(PhotonGame);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Lobby other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.OwnerPlayerId.Length != 0)
			{
				OwnerPlayerId = other.OwnerPlayerId;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.LobbyType != 0)
			{
				LobbyType = other.LobbyType;
			}
			if (other.Joinable)
			{
				Joinable = other.Joinable;
			}
			if (other.MaxMembers != 0)
			{
				MaxMembers = other.MaxMembers;
			}
			data_.Add(other.data_);
			members_.Add(other.members_);
			invites_.Add(other.invites_);
			if (other.gameServer_ != null)
			{
				if (gameServer_ == null)
				{
					gameServer_ = new GameServer();
				}
				GameServer.MergeFrom(other.GameServer);
			}
			if (other.photonGame_ != null)
			{
				if (photonGame_ == null)
				{
					photonGame_ = new PhotonGame();
				}
				PhotonGame.MergeFrom(other.PhotonGame);
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
					OwnerPlayerId = input.ReadString();
					break;
				case 26u:
					Name = input.ReadString();
					break;
				case 32u:
					lobbyType_ = (LobbyType)input.ReadEnum();
					break;
				case 40u:
					Joinable = input.ReadBool();
					break;
				case 48u:
					MaxMembers = input.ReadInt32();
					break;
				case 58u:
					data_.AddEntriesFrom(input, _map_data_codec);
					break;
				case 66u:
					members_.AddEntriesFrom(input, _repeated_members_codec);
					break;
				case 74u:
					invites_.AddEntriesFrom(input, _repeated_invites_codec);
					break;
				case 82u:
					if (gameServer_ == null)
					{
						gameServer_ = new GameServer();
					}
					input.ReadMessage(gameServer_);
					break;
				case 90u:
					if (photonGame_ == null)
					{
						photonGame_ = new PhotonGame();
					}
					input.ReadMessage(photonGame_);
					break;
				}
			}
		}
	}
}
