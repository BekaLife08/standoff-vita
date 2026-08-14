using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GameServer : IMessage<GameServer>, IMessage, IEquatable<GameServer>, IDeepCloneable<GameServer>
	{
		private static readonly MessageParser<GameServer> _parser = new MessageParser<GameServer>(() => new GameServer());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int IpFieldNumber = 2;

		private string ip_ = "";

		public const int PortFieldNumber = 3;

		private int port_;

		[DebuggerNonUserCode]
		public static MessageParser<GameServer> Parser
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
				return CommonMessageReflection.Descriptor.MessageTypes[4];
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
		public string Ip
		{
			get
			{
				return ip_;
			}
			set
			{
				ip_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int Port
		{
			get
			{
				return port_;
			}
			set
			{
				port_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GameServer()
		{
		}

		[DebuggerNonUserCode]
		public GameServer(GameServer other)
			: this()
		{
			id_ = other.id_;
			ip_ = other.ip_;
			port_ = other.port_;
		}

		[DebuggerNonUserCode]
		public GameServer Clone()
		{
			return new GameServer(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GameServer);
		}

		[DebuggerNonUserCode]
		public bool Equals(GameServer other)
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
			if (Ip != other.Ip)
			{
				return false;
			}
			if (Port != other.Port)
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
			if (Ip.Length != 0)
			{
				num ^= Ip.GetHashCode();
			}
			if (Port != 0)
			{
				num ^= Port.GetHashCode();
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
			if (Ip.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Ip);
			}
			if (Port != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Port);
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
			if (Ip.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Ip);
			}
			if (Port != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Port);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GameServer other)
		{
			if (other != null)
			{
				if (other.Id.Length != 0)
				{
					Id = other.Id;
				}
				if (other.Ip.Length != 0)
				{
					Ip = other.Ip;
				}
				if (other.Port != 0)
				{
					Port = other.Port;
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
					Id = input.ReadString();
					break;
				case 18u:
					Ip = input.ReadString();
					break;
				case 24u:
					Port = input.ReadInt32();
					break;
				}
			}
		}
	}
}
