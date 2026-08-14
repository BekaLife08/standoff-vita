using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Storage : IMessage<Storage>, IMessage, IEquatable<Storage>, IDeepCloneable<Storage>
	{
		private static readonly MessageParser<Storage> _parser = new MessageParser<Storage>(() => new Storage());

		public const int FilenameFieldNumber = 1;

		private string filename_ = "";

		public const int FileFieldNumber = 2;

		private ByteString file_ = ByteString.Empty;

		[DebuggerNonUserCode]
		public static MessageParser<Storage> Parser
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
				return StorageMessageReflection.Descriptor.MessageTypes[0];
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
		public string Filename
		{
			get
			{
				return filename_;
			}
			set
			{
				filename_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ByteString File
		{
			get
			{
				return file_;
			}
			set
			{
				file_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public Storage()
		{
		}

		[DebuggerNonUserCode]
		public Storage(Storage other)
			: this()
		{
			filename_ = other.filename_;
			file_ = other.file_;
		}

		[DebuggerNonUserCode]
		public Storage Clone()
		{
			return new Storage(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Storage);
		}

		[DebuggerNonUserCode]
		public bool Equals(Storage other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Filename != other.Filename)
			{
				return false;
			}
			if (File != other.File)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Filename.Length != 0)
			{
				num ^= Filename.GetHashCode();
			}
			if (File.Length != 0)
			{
				num ^= File.GetHashCode();
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
			if (Filename.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Filename);
			}
			if (File.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteBytes(File);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Filename.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Filename);
			}
			if (File.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(File);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Storage other)
		{
			if (other != null)
			{
				if (other.Filename.Length != 0)
				{
					Filename = other.Filename;
				}
				if (other.File.Length != 0)
				{
					File = other.File;
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
					Filename = input.ReadString();
					break;
				case 18u:
					File = input.ReadBytes();
					break;
				}
			}
		}
	}
}
