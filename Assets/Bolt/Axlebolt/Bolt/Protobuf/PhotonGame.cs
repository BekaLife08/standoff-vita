using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PhotonGame : IMessage<PhotonGame>, IMessage, IEquatable<PhotonGame>, IDeepCloneable<PhotonGame>
	{
		private static readonly MessageParser<PhotonGame> _parser = new MessageParser<PhotonGame>(() => new PhotonGame());

		public const int RegionFieldNumber = 1;

		private string region_ = "";

		public const int RoomIdFieldNumber = 2;

		private string roomId_ = "";

		public const int AppVersionFieldNumber = 3;

		private string appVersion_ = "";

		public const int CustomPropertiesFieldNumber = 4;

		private static readonly MapField<string, string>.Codec _map_customProperties_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10u), FieldCodec.ForString(18u), 34u);

		private readonly MapField<string, string> customProperties_ = new MapField<string, string>();

		[DebuggerNonUserCode]
		public static MessageParser<PhotonGame> Parser
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
				return CommonMessageReflection.Descriptor.MessageTypes[5];
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
		public string Region
		{
			get
			{
				return region_;
			}
			set
			{
				region_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string RoomId
		{
			get
			{
				return roomId_;
			}
			set
			{
				roomId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string AppVersion
		{
			get
			{
				return appVersion_;
			}
			set
			{
				appVersion_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, string> CustomProperties
		{
			get
			{
				return customProperties_;
			}
		}

		[DebuggerNonUserCode]
		public PhotonGame()
		{
		}

		[DebuggerNonUserCode]
		public PhotonGame(PhotonGame other)
			: this()
		{
			region_ = other.region_;
			roomId_ = other.roomId_;
			appVersion_ = other.appVersion_;
			customProperties_ = other.customProperties_.Clone();
		}

		[DebuggerNonUserCode]
		public PhotonGame Clone()
		{
			return new PhotonGame(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PhotonGame);
		}

		[DebuggerNonUserCode]
		public bool Equals(PhotonGame other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Region != other.Region)
			{
				return false;
			}
			if (RoomId != other.RoomId)
			{
				return false;
			}
			if (AppVersion != other.AppVersion)
			{
				return false;
			}
			if (!CustomProperties.Equals(other.CustomProperties))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Region.Length != 0)
			{
				num ^= Region.GetHashCode();
			}
			if (RoomId.Length != 0)
			{
				num ^= RoomId.GetHashCode();
			}
			if (AppVersion.Length != 0)
			{
				num ^= AppVersion.GetHashCode();
			}
			return num ^ CustomProperties.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (Region.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Region);
			}
			if (RoomId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(RoomId);
			}
			if (AppVersion.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(AppVersion);
			}
			customProperties_.WriteTo(output, _map_customProperties_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Region.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Region);
			}
			if (RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(RoomId);
			}
			if (AppVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AppVersion);
			}
			return num + customProperties_.CalculateSize(_map_customProperties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PhotonGame other)
		{
			if (other != null)
			{
				if (other.Region.Length != 0)
				{
					Region = other.Region;
				}
				if (other.RoomId.Length != 0)
				{
					RoomId = other.RoomId;
				}
				if (other.AppVersion.Length != 0)
				{
					AppVersion = other.AppVersion;
				}
				customProperties_.Add(other.customProperties_);
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
					Region = input.ReadString();
					break;
				case 18u:
					RoomId = input.ReadString();
					break;
				case 26u:
					AppVersion = input.ReadString();
					break;
				case 34u:
					customProperties_.AddEntriesFrom(input, _map_customProperties_codec);
					break;
				}
			}
		}
	}
}
