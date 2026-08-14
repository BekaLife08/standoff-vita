using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Stats : IMessage<Stats>, IMessage, IEquatable<Stats>, IDeepCloneable<Stats>
	{
		private static readonly MessageParser<Stats> _parser = new MessageParser<Stats>(() => new Stats());

		public const int StatFieldNumber = 1;

		private static readonly FieldCodec<PlayerStat> _repeated_stat_codec = FieldCodec.ForMessage(10u, PlayerStat.Parser);

		private readonly RepeatedField<PlayerStat> stat_ = new RepeatedField<PlayerStat>();

		public const int AchievementFieldNumber = 2;

		private static readonly FieldCodec<Achievement> _repeated_achievement_codec = FieldCodec.ForMessage(18u, Axlebolt.Bolt.Protobuf.Achievement.Parser);

		private readonly RepeatedField<Achievement> achievement_ = new RepeatedField<Achievement>();

		[DebuggerNonUserCode]
		public static MessageParser<Stats> Parser
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
				return PlayerStatsMessageReflection.Descriptor.MessageTypes[0];
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
		public RepeatedField<PlayerStat> Stat
		{
			get
			{
				return stat_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<Achievement> Achievement
		{
			get
			{
				return achievement_;
			}
		}

		[DebuggerNonUserCode]
		public Stats()
		{
		}

		[DebuggerNonUserCode]
		public Stats(Stats other)
			: this()
		{
			stat_ = other.stat_.Clone();
			achievement_ = other.achievement_.Clone();
		}

		[DebuggerNonUserCode]
		public Stats Clone()
		{
			return new Stats(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Stats);
		}

		[DebuggerNonUserCode]
		public bool Equals(Stats other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!stat_.Equals(other.stat_))
			{
				return false;
			}
			if (!achievement_.Equals(other.achievement_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= stat_.GetHashCode();
			return num ^ achievement_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			stat_.WriteTo(output, _repeated_stat_codec);
			achievement_.WriteTo(output, _repeated_achievement_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += stat_.CalculateSize(_repeated_stat_codec);
			return num + achievement_.CalculateSize(_repeated_achievement_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Stats other)
		{
			if (other != null)
			{
				stat_.Add(other.stat_);
				achievement_.Add(other.achievement_);
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
					stat_.AddEntriesFrom(input, _repeated_stat_codec);
					break;
				case 18u:
					achievement_.AddEntriesFrom(input, _repeated_achievement_codec);
					break;
				}
			}
		}
	}
}
