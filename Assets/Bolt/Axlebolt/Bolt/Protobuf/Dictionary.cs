using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Dictionary : IMessage<Dictionary>, IMessage, IEquatable<Dictionary>, IDeepCloneable<Dictionary>
	{
		private static readonly MessageParser<Dictionary> _parser = new MessageParser<Dictionary>(() => new Dictionary());

		public const int ContentFieldNumber = 1;

		private static readonly MapField<string, string>.Codec _map_content_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10u), FieldCodec.ForString(18u), 10u);

		private readonly MapField<string, string> content_ = new MapField<string, string>();

		[DebuggerNonUserCode]
		public static MessageParser<Dictionary> Parser
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
				return CommonMessageReflection.Descriptor.MessageTypes[3];
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
		public MapField<string, string> Content
		{
			get
			{
				return content_;
			}
		}

		[DebuggerNonUserCode]
		public Dictionary()
		{
		}

		[DebuggerNonUserCode]
		public Dictionary(Dictionary other)
			: this()
		{
			content_ = other.content_.Clone();
		}

		[DebuggerNonUserCode]
		public Dictionary Clone()
		{
			return new Dictionary(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Dictionary);
		}

		[DebuggerNonUserCode]
		public bool Equals(Dictionary other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!Content.Equals(other.Content))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			return num ^ Content.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			content_.WriteTo(output, _map_content_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			return num + content_.CalculateSize(_map_content_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Dictionary other)
		{
			if (other != null)
			{
				content_.Add(other.content_);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 10)
				{
					input.SkipLastField();
				}
				else
				{
					content_.AddEntriesFrom(input, _map_content_codec);
				}
			}
		}
	}
}
