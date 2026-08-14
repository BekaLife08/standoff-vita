using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Currency : IMessage<Currency>, IMessage, IEquatable<Currency>, IDeepCloneable<Currency>
	{
		private static readonly MessageParser<Currency> _parser = new MessageParser<Currency>(() => new Currency());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int ExchangeRatioFieldNumber = 2;

		private float exchangeRatio_;

		public const int ExchangableCurrenciesFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_exchangableCurrencies_codec = FieldCodec.ForInt32(26u);

		private readonly RepeatedField<int> exchangableCurrencies_ = new RepeatedField<int>();

		[DebuggerNonUserCode]
		public static MessageParser<Currency> Parser
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
				return CurrencyMessageReflection.Descriptor.MessageTypes[0];
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
		public int Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float ExchangeRatio
		{
			get
			{
				return exchangeRatio_;
			}
			set
			{
				exchangeRatio_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> ExchangableCurrencies
		{
			get
			{
				return exchangableCurrencies_;
			}
		}

		[DebuggerNonUserCode]
		public Currency()
		{
		}

		[DebuggerNonUserCode]
		public Currency(Currency other)
			: this()
		{
			id_ = other.id_;
			exchangeRatio_ = other.exchangeRatio_;
			exchangableCurrencies_ = other.exchangableCurrencies_.Clone();
		}

		[DebuggerNonUserCode]
		public Currency Clone()
		{
			return new Currency(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Currency);
		}

		[DebuggerNonUserCode]
		public bool Equals(Currency other)
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
			if (ExchangeRatio != other.ExchangeRatio)
			{
				return false;
			}
			if (!exchangableCurrencies_.Equals(other.exchangableCurrencies_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (ExchangeRatio != 0f)
			{
				num ^= ExchangeRatio.GetHashCode();
			}
			return num ^ exchangableCurrencies_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Id);
			}
			if (ExchangeRatio != 0f)
			{
				output.WriteRawTag(21);
				output.WriteFloat(ExchangeRatio);
			}
			exchangableCurrencies_.WriteTo(output, _repeated_exchangableCurrencies_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			if (ExchangeRatio != 0f)
			{
				num += 5;
			}
			return num + exchangableCurrencies_.CalculateSize(_repeated_exchangableCurrencies_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Currency other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				if (other.ExchangeRatio != 0f)
				{
					ExchangeRatio = other.ExchangeRatio;
				}
				exchangableCurrencies_.Add(other.exchangableCurrencies_);
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
				case 8u:
					Id = input.ReadInt32();
					break;
				case 21u:
					ExchangeRatio = input.ReadFloat();
					break;
				case 24u:
				case 26u:
					exchangableCurrencies_.AddEntriesFrom(input, _repeated_exchangableCurrencies_codec);
					break;
				}
			}
		}
	}
}
