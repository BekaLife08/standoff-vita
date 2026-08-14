using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Achievement : IMessage<Achievement>, IMessage, IEquatable<Achievement>, IDeepCloneable<Achievement>
	{
		private static readonly MessageParser<Achievement> _parser = new MessageParser<Achievement>(() => new Achievement());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int DisplayNameFieldNumber = 2;

		private string displayName_ = "";

		public const int DisplayDescriptionFieldNumber = 3;

		private string displayDescription_ = "";

		public const int UnlockTimeFieldNumber = 4;

		private long unlockTime_;

		public const int AchievedFieldNumber = 5;

		private bool achieved_;

		public const int IconFieldNumber = 6;

		private ByteString icon_ = ByteString.Empty;

		public const int HiddenFieldNumber = 7;

		private bool hidden_;

		[DebuggerNonUserCode]
		public static MessageParser<Achievement> Parser
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
				return PlayerStatsMessageReflection.Descriptor.MessageTypes[2];
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
		public string DisplayName
		{
			get
			{
				return displayName_;
			}
			set
			{
				displayName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string DisplayDescription
		{
			get
			{
				return displayDescription_;
			}
			set
			{
				displayDescription_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public long UnlockTime
		{
			get
			{
				return unlockTime_;
			}
			set
			{
				unlockTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool Achieved
		{
			get
			{
				return achieved_;
			}
			set
			{
				achieved_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ByteString Icon
		{
			get
			{
				return icon_;
			}
			set
			{
				icon_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool Hidden
		{
			get
			{
				return hidden_;
			}
			set
			{
				hidden_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Achievement()
		{
		}

		[DebuggerNonUserCode]
		public Achievement(Achievement other)
			: this()
		{
			name_ = other.name_;
			displayName_ = other.displayName_;
			displayDescription_ = other.displayDescription_;
			unlockTime_ = other.unlockTime_;
			achieved_ = other.achieved_;
			icon_ = other.icon_;
			hidden_ = other.hidden_;
		}

		[DebuggerNonUserCode]
		public Achievement Clone()
		{
			return new Achievement(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Achievement);
		}

		[DebuggerNonUserCode]
		public bool Equals(Achievement other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Name != other.Name)
			{
				return false;
			}
			if (DisplayName != other.DisplayName)
			{
				return false;
			}
			if (DisplayDescription != other.DisplayDescription)
			{
				return false;
			}
			if (UnlockTime != other.UnlockTime)
			{
				return false;
			}
			if (Achieved != other.Achieved)
			{
				return false;
			}
			if (Icon != other.Icon)
			{
				return false;
			}
			if (Hidden != other.Hidden)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (DisplayName.Length != 0)
			{
				num ^= DisplayName.GetHashCode();
			}
			if (DisplayDescription.Length != 0)
			{
				num ^= DisplayDescription.GetHashCode();
			}
			if (UnlockTime != 0)
			{
				num ^= UnlockTime.GetHashCode();
			}
			if (Achieved)
			{
				num ^= Achieved.GetHashCode();
			}
			if (Icon.Length != 0)
			{
				num ^= Icon.GetHashCode();
			}
			if (Hidden)
			{
				num ^= Hidden.GetHashCode();
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
			if (Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Name);
			}
			if (DisplayName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(DisplayName);
			}
			if (DisplayDescription.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(DisplayDescription);
			}
			if (UnlockTime != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt64(UnlockTime);
			}
			if (Achieved)
			{
				output.WriteRawTag(40);
				output.WriteBool(Achieved);
			}
			if (Icon.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteBytes(Icon);
			}
			if (Hidden)
			{
				output.WriteRawTag(56);
				output.WriteBool(Hidden);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (DisplayName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(DisplayName);
			}
			if (DisplayDescription.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(DisplayDescription);
			}
			if (UnlockTime != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(UnlockTime);
			}
			if (Achieved)
			{
				num += 2;
			}
			if (Icon.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(Icon);
			}
			if (Hidden)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Achievement other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.DisplayName.Length != 0)
				{
					DisplayName = other.DisplayName;
				}
				if (other.DisplayDescription.Length != 0)
				{
					DisplayDescription = other.DisplayDescription;
				}
				if (other.UnlockTime != 0)
				{
					UnlockTime = other.UnlockTime;
				}
				if (other.Achieved)
				{
					Achieved = other.Achieved;
				}
				if (other.Icon.Length != 0)
				{
					Icon = other.Icon;
				}
				if (other.Hidden)
				{
					Hidden = other.Hidden;
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
					Name = input.ReadString();
					break;
				case 18u:
					DisplayName = input.ReadString();
					break;
				case 26u:
					DisplayDescription = input.ReadString();
					break;
				case 32u:
					UnlockTime = input.ReadInt64();
					break;
				case 40u:
					Achieved = input.ReadBool();
					break;
				case 50u:
					Icon = input.ReadBytes();
					break;
				case 56u:
					Hidden = input.ReadBool();
					break;
				}
			}
		}
	}
}
