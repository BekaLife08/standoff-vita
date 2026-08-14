using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GameServerDetails : IMessage<GameServerDetails>, IMessage, IEquatable<GameServerDetails>, IDeepCloneable<GameServerDetails>
	{
		private static readonly MessageParser<GameServerDetails> _parser = new MessageParser<GameServerDetails>(() => new GameServerDetails());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int GameServerFieldNumber = 2;

		private GameServer gameServer_;

		public const int MapFieldNumber = 3;

		private string map_ = "";

		public const int CurrentPlayersFieldNumber = 4;

		private int currentPlayers_;

		public const int MaxPlayersFieldNumber = 5;

		private int maxPlayers_;

		public const int BotPlayersFieldNumber = 6;

		private int botPlayers_;

		public const int RequirePasswordFieldNumber = 7;

		private bool requirePassword_;

		public const int VersionFieldNumber = 8;

		private string version_ = "";

		public const int SuccessfulResponseFieldNumber = 9;

		private bool successfulResponse_;

		public const int DoNotRefreshFieldNumber = 10;

		private bool doNotRefresh_;

		[DebuggerNonUserCode]
		public static MessageParser<GameServerDetails> Parser
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
				return MatchmakingMessageReflection.Descriptor.MessageTypes[0];
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
		public string Map
		{
			get
			{
				return map_;
			}
			set
			{
				map_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int CurrentPlayers
		{
			get
			{
				return currentPlayers_;
			}
			set
			{
				currentPlayers_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int MaxPlayers
		{
			get
			{
				return maxPlayers_;
			}
			set
			{
				maxPlayers_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int BotPlayers
		{
			get
			{
				return botPlayers_;
			}
			set
			{
				botPlayers_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool RequirePassword
		{
			get
			{
				return requirePassword_;
			}
			set
			{
				requirePassword_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Version
		{
			get
			{
				return version_;
			}
			set
			{
				version_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool SuccessfulResponse
		{
			get
			{
				return successfulResponse_;
			}
			set
			{
				successfulResponse_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool DoNotRefresh
		{
			get
			{
				return doNotRefresh_;
			}
			set
			{
				doNotRefresh_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GameServerDetails()
		{
		}

		[DebuggerNonUserCode]
		public GameServerDetails(GameServerDetails other)
			: this()
		{
			id_ = other.id_;
			GameServer = ((other.gameServer_ != null) ? other.GameServer.Clone() : null);
			map_ = other.map_;
			currentPlayers_ = other.currentPlayers_;
			maxPlayers_ = other.maxPlayers_;
			botPlayers_ = other.botPlayers_;
			requirePassword_ = other.requirePassword_;
			version_ = other.version_;
			successfulResponse_ = other.successfulResponse_;
			doNotRefresh_ = other.doNotRefresh_;
		}

		[DebuggerNonUserCode]
		public GameServerDetails Clone()
		{
			return new GameServerDetails(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GameServerDetails);
		}

		[DebuggerNonUserCode]
		public bool Equals(GameServerDetails other)
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
			if (!object.Equals(GameServer, other.GameServer))
			{
				return false;
			}
			if (Map != other.Map)
			{
				return false;
			}
			if (CurrentPlayers != other.CurrentPlayers)
			{
				return false;
			}
			if (MaxPlayers != other.MaxPlayers)
			{
				return false;
			}
			if (BotPlayers != other.BotPlayers)
			{
				return false;
			}
			if (RequirePassword != other.RequirePassword)
			{
				return false;
			}
			if (Version != other.Version)
			{
				return false;
			}
			if (SuccessfulResponse != other.SuccessfulResponse)
			{
				return false;
			}
			if (DoNotRefresh != other.DoNotRefresh)
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
			if (gameServer_ != null)
			{
				num ^= GameServer.GetHashCode();
			}
			if (Map.Length != 0)
			{
				num ^= Map.GetHashCode();
			}
			if (CurrentPlayers != 0)
			{
				num ^= CurrentPlayers.GetHashCode();
			}
			if (MaxPlayers != 0)
			{
				num ^= MaxPlayers.GetHashCode();
			}
			if (BotPlayers != 0)
			{
				num ^= BotPlayers.GetHashCode();
			}
			if (RequirePassword)
			{
				num ^= RequirePassword.GetHashCode();
			}
			if (Version.Length != 0)
			{
				num ^= Version.GetHashCode();
			}
			if (SuccessfulResponse)
			{
				num ^= SuccessfulResponse.GetHashCode();
			}
			if (DoNotRefresh)
			{
				num ^= DoNotRefresh.GetHashCode();
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
			if (gameServer_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(GameServer);
			}
			if (Map.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(Map);
			}
			if (CurrentPlayers != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(CurrentPlayers);
			}
			if (MaxPlayers != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(MaxPlayers);
			}
			if (BotPlayers != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(BotPlayers);
			}
			if (RequirePassword)
			{
				output.WriteRawTag(56);
				output.WriteBool(RequirePassword);
			}
			if (Version.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(Version);
			}
			if (SuccessfulResponse)
			{
				output.WriteRawTag(72);
				output.WriteBool(SuccessfulResponse);
			}
			if (DoNotRefresh)
			{
				output.WriteRawTag(80);
				output.WriteBool(DoNotRefresh);
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
			if (gameServer_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(GameServer);
			}
			if (Map.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Map);
			}
			if (CurrentPlayers != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPlayers);
			}
			if (MaxPlayers != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(MaxPlayers);
			}
			if (BotPlayers != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(BotPlayers);
			}
			if (RequirePassword)
			{
				num += 2;
			}
			if (Version.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Version);
			}
			if (SuccessfulResponse)
			{
				num += 2;
			}
			if (DoNotRefresh)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GameServerDetails other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.gameServer_ != null)
			{
				if (gameServer_ == null)
				{
					gameServer_ = new GameServer();
				}
				GameServer.MergeFrom(other.GameServer);
			}
			if (other.Map.Length != 0)
			{
				Map = other.Map;
			}
			if (other.CurrentPlayers != 0)
			{
				CurrentPlayers = other.CurrentPlayers;
			}
			if (other.MaxPlayers != 0)
			{
				MaxPlayers = other.MaxPlayers;
			}
			if (other.BotPlayers != 0)
			{
				BotPlayers = other.BotPlayers;
			}
			if (other.RequirePassword)
			{
				RequirePassword = other.RequirePassword;
			}
			if (other.Version.Length != 0)
			{
				Version = other.Version;
			}
			if (other.SuccessfulResponse)
			{
				SuccessfulResponse = other.SuccessfulResponse;
			}
			if (other.DoNotRefresh)
			{
				DoNotRefresh = other.DoNotRefresh;
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
					if (gameServer_ == null)
					{
						gameServer_ = new GameServer();
					}
					input.ReadMessage(gameServer_);
					break;
				case 26u:
					Map = input.ReadString();
					break;
				case 32u:
					CurrentPlayers = input.ReadInt32();
					break;
				case 40u:
					MaxPlayers = input.ReadInt32();
					break;
				case 48u:
					BotPlayers = input.ReadInt32();
					break;
				case 56u:
					RequirePassword = input.ReadBool();
					break;
				case 66u:
					Version = input.ReadString();
					break;
				case 72u:
					SuccessfulResponse = input.ReadBool();
					break;
				case 80u:
					DoNotRefresh = input.ReadBool();
					break;
				}
			}
		}
	}
}
