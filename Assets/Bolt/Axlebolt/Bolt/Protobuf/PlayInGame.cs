using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayInGame : IMessage<PlayInGame>, IMessage, IEquatable<PlayInGame>, IDeepCloneable<PlayInGame>
	{
		private static readonly MessageParser<PlayInGame> _parser = new MessageParser<PlayInGame>(() => new PlayInGame());

		public const int GameCodeFieldNumber = 1;

		private string gameCode_ = "";

		public const int GameVersionFieldNumber = 2;

		private string gameVersion_ = "";

		public const int LobbyIdFieldNumber = 3;

		private string lobbyId_ = "";

		public const int PhotonGameFieldNumber = 4;

		private PhotonGame photonGame_;

		[DebuggerNonUserCode]
		public static MessageParser<PlayInGame> Parser
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
				return PlayerMessageReflection.Descriptor.MessageTypes[0];
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
		public string GameCode
		{
			get
			{
				return gameCode_;
			}
			set
			{
				gameCode_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string GameVersion
		{
			get
			{
				return gameVersion_;
			}
			set
			{
				gameVersion_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public PlayInGame()
		{
		}

		[DebuggerNonUserCode]
		public PlayInGame(PlayInGame other)
			: this()
		{
			gameCode_ = other.gameCode_;
			gameVersion_ = other.gameVersion_;
			lobbyId_ = other.lobbyId_;
			PhotonGame = ((other.photonGame_ != null) ? other.PhotonGame.Clone() : null);
		}

		[DebuggerNonUserCode]
		public PlayInGame Clone()
		{
			return new PlayInGame(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayInGame);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayInGame other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (GameCode != other.GameCode)
			{
				return false;
			}
			if (GameVersion != other.GameVersion)
			{
				return false;
			}
			if (LobbyId != other.LobbyId)
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
			if (GameCode.Length != 0)
			{
				num ^= GameCode.GetHashCode();
			}
			if (GameVersion.Length != 0)
			{
				num ^= GameVersion.GetHashCode();
			}
			if (LobbyId.Length != 0)
			{
				num ^= LobbyId.GetHashCode();
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
			if (GameCode.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(GameCode);
			}
			if (GameVersion.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(GameVersion);
			}
			if (LobbyId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(LobbyId);
			}
			if (photonGame_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(PhotonGame);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (GameCode.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(GameCode);
			}
			if (GameVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(GameVersion);
			}
			if (LobbyId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(LobbyId);
			}
			if (photonGame_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(PhotonGame);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayInGame other)
		{
			if (other == null)
			{
				return;
			}
			if (other.GameCode.Length != 0)
			{
				GameCode = other.GameCode;
			}
			if (other.GameVersion.Length != 0)
			{
				GameVersion = other.GameVersion;
			}
			if (other.LobbyId.Length != 0)
			{
				LobbyId = other.LobbyId;
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
					GameCode = input.ReadString();
					break;
				case 18u:
					GameVersion = input.ReadString();
					break;
				case 26u:
					LobbyId = input.ReadString();
					break;
				case 34u:
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
