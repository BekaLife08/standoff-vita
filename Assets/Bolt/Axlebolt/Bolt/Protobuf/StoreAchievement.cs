using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class StoreAchievement : IMessage<StoreAchievement>, IMessage, IEquatable<StoreAchievement>, IDeepCloneable<StoreAchievement>
	{
		private static readonly MessageParser<StoreAchievement> _parser = new MessageParser<StoreAchievement>(() => new StoreAchievement());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int AchievedFieldNumber = 2;

		private bool achieved_;

		[DebuggerNonUserCode]
		public static MessageParser<StoreAchievement> Parser
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
				return PlayerStatsMessageReflection.Descriptor.MessageTypes[4];
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
		public StoreAchievement()
		{
		}

		[DebuggerNonUserCode]
		public StoreAchievement(StoreAchievement other)
			: this()
		{
			name_ = other.name_;
			achieved_ = other.achieved_;
		}

		[DebuggerNonUserCode]
		public StoreAchievement Clone()
		{
			return new StoreAchievement(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as StoreAchievement);
		}

		[DebuggerNonUserCode]
		public bool Equals(StoreAchievement other)
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
			if (Achieved != other.Achieved)
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
			if (Achieved)
			{
				num ^= Achieved.GetHashCode();
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
			if (Achieved)
			{
				output.WriteRawTag(16);
				output.WriteBool(Achieved);
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
			if (Achieved)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(StoreAchievement other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.Achieved)
				{
					Achieved = other.Achieved;
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
				case 16u:
					Achieved = input.ReadBool();
					break;
				}
			}
		}
	}
}
