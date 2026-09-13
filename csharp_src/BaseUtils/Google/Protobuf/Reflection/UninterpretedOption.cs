using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class UninterpretedOption : IMessage<UninterpretedOption>, IMessage, IEquatable<UninterpretedOption>, IDeepCloneable<UninterpretedOption>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class NamePart : IMessage<NamePart>, IMessage, IEquatable<NamePart>, IDeepCloneable<NamePart>
		{
			private static readonly MessageParser<NamePart> _parser = new MessageParser<NamePart>(() => new NamePart());

			private UnknownFieldSet _unknownFields;

			private int _hasBits0;

			public const int NamePart_FieldNumber = 1;

			private static readonly string NamePart_DefaultValue = "";

			private string namePart_;

			public const int IsExtensionFieldNumber = 2;

			private static readonly bool IsExtensionDefaultValue = false;

			private bool isExtension_;

			[DebuggerNonUserCode]
			public static MessageParser<NamePart> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => UninterpretedOption.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => Descriptor;

			[DebuggerNonUserCode]
			public string NamePart_
			{
				get
				{
					return namePart_ ?? NamePart_DefaultValue;
				}
				set
				{
					namePart_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public bool HasNamePart_ => namePart_ != null;

			[DebuggerNonUserCode]
			public bool IsExtension
			{
				get
				{
					if ((_hasBits0 & 1) != 0)
					{
						return isExtension_;
					}
					return IsExtensionDefaultValue;
				}
				set
				{
					_hasBits0 |= 1;
					isExtension_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool HasIsExtension => (_hasBits0 & 1) != 0;

			[DebuggerNonUserCode]
			public NamePart()
			{
			}

			[DebuggerNonUserCode]
			public NamePart(NamePart other)
				: this()
			{
				_hasBits0 = other._hasBits0;
				namePart_ = other.namePart_;
				isExtension_ = other.isExtension_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public NamePart Clone()
			{
				return new NamePart(this);
			}

			[DebuggerNonUserCode]
			public void ClearNamePart_()
			{
				namePart_ = null;
			}

			[DebuggerNonUserCode]
			public void ClearIsExtension()
			{
				_hasBits0 &= -2;
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as NamePart);
			}

			[DebuggerNonUserCode]
			public bool Equals(NamePart other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (NamePart_ != other.NamePart_)
				{
					return false;
				}
				if (IsExtension != other.IsExtension)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (HasNamePart_)
				{
					num ^= NamePart_.GetHashCode();
				}
				if (HasIsExtension)
				{
					num ^= IsExtension.GetHashCode();
				}
				if (_unknownFields != null)
				{
					num ^= _unknownFields.GetHashCode();
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
				if (HasNamePart_)
				{
					output.WriteRawTag(10);
					output.WriteString(NamePart_);
				}
				if (HasIsExtension)
				{
					output.WriteRawTag(16);
					output.WriteBool(IsExtension);
				}
				if (_unknownFields != null)
				{
					_unknownFields.WriteTo(output);
				}
			}

			[DebuggerNonUserCode]
			public int CalculateSize()
			{
				int num = 0;
				if (HasNamePart_)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(NamePart_);
				}
				if (HasIsExtension)
				{
					num += 2;
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(NamePart other)
			{
				if (other != null)
				{
					if (other.HasNamePart_)
					{
						NamePart_ = other.NamePart_;
					}
					if (other.HasIsExtension)
					{
						IsExtension = other.IsExtension;
					}
					_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
						_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
						break;
					case 10u:
						NamePart_ = input.ReadString();
						break;
					case 16u:
						IsExtension = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<UninterpretedOption> _parser = new MessageParser<UninterpretedOption>(() => new UninterpretedOption());

	private UnknownFieldSet _unknownFields;

	private int _hasBits0;

	public const int NameFieldNumber = 2;

	private static readonly FieldCodec<Types.NamePart> _repeated_name_codec = FieldCodec.ForMessage(18u, Types.NamePart.Parser);

	private readonly RepeatedField<Types.NamePart> name_ = new RepeatedField<Types.NamePart>();

	public const int IdentifierValueFieldNumber = 3;

	private static readonly string IdentifierValueDefaultValue = "";

	private string identifierValue_;

	public const int PositiveIntValueFieldNumber = 4;

	private static readonly ulong PositiveIntValueDefaultValue = 0uL;

	private ulong positiveIntValue_;

	public const int NegativeIntValueFieldNumber = 5;

	private static readonly long NegativeIntValueDefaultValue = 0L;

	private long negativeIntValue_;

	public const int DoubleValueFieldNumber = 6;

	private static readonly double DoubleValueDefaultValue = 0.0;

	private double doubleValue_;

	public const int StringValueFieldNumber = 7;

	private static readonly ByteString StringValueDefaultValue = ByteString.Empty;

	private ByteString stringValue_;

	public const int AggregateValueFieldNumber = 8;

	private static readonly string AggregateValueDefaultValue = "";

	private string aggregateValue_;

	[DebuggerNonUserCode]
	public static MessageParser<UninterpretedOption> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<Types.NamePart> Name => name_;

	[DebuggerNonUserCode]
	public string IdentifierValue
	{
		get
		{
			return identifierValue_ ?? IdentifierValueDefaultValue;
		}
		set
		{
			identifierValue_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasIdentifierValue => identifierValue_ != null;

	[DebuggerNonUserCode]
	public ulong PositiveIntValue
	{
		get
		{
			if ((_hasBits0 & 1) != 0)
			{
				return positiveIntValue_;
			}
			return PositiveIntValueDefaultValue;
		}
		set
		{
			_hasBits0 |= 1;
			positiveIntValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasPositiveIntValue => (_hasBits0 & 1) != 0;

	[DebuggerNonUserCode]
	public long NegativeIntValue
	{
		get
		{
			if ((_hasBits0 & 2) != 0)
			{
				return negativeIntValue_;
			}
			return NegativeIntValueDefaultValue;
		}
		set
		{
			_hasBits0 |= 2;
			negativeIntValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasNegativeIntValue => (_hasBits0 & 2) != 0;

	[DebuggerNonUserCode]
	public double DoubleValue
	{
		get
		{
			if ((_hasBits0 & 4) != 0)
			{
				return doubleValue_;
			}
			return DoubleValueDefaultValue;
		}
		set
		{
			_hasBits0 |= 4;
			doubleValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasDoubleValue => (_hasBits0 & 4) != 0;

	[DebuggerNonUserCode]
	public ByteString StringValue
	{
		get
		{
			return stringValue_ ?? StringValueDefaultValue;
		}
		set
		{
			stringValue_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasStringValue => stringValue_ != null;

	[DebuggerNonUserCode]
	public string AggregateValue
	{
		get
		{
			return aggregateValue_ ?? AggregateValueDefaultValue;
		}
		set
		{
			aggregateValue_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasAggregateValue => aggregateValue_ != null;

	[DebuggerNonUserCode]
	public UninterpretedOption()
	{
	}

	[DebuggerNonUserCode]
	public UninterpretedOption(UninterpretedOption other)
		: this()
	{
		_hasBits0 = other._hasBits0;
		name_ = other.name_.Clone();
		identifierValue_ = other.identifierValue_;
		positiveIntValue_ = other.positiveIntValue_;
		negativeIntValue_ = other.negativeIntValue_;
		doubleValue_ = other.doubleValue_;
		stringValue_ = other.stringValue_;
		aggregateValue_ = other.aggregateValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UninterpretedOption Clone()
	{
		return new UninterpretedOption(this);
	}

	[DebuggerNonUserCode]
	public void ClearIdentifierValue()
	{
		identifierValue_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearPositiveIntValue()
	{
		_hasBits0 &= -2;
	}

	[DebuggerNonUserCode]
	public void ClearNegativeIntValue()
	{
		_hasBits0 &= -3;
	}

	[DebuggerNonUserCode]
	public void ClearDoubleValue()
	{
		_hasBits0 &= -5;
	}

	[DebuggerNonUserCode]
	public void ClearStringValue()
	{
		stringValue_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearAggregateValue()
	{
		aggregateValue_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UninterpretedOption);
	}

	[DebuggerNonUserCode]
	public bool Equals(UninterpretedOption other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!name_.Equals(other.name_))
		{
			return false;
		}
		if (IdentifierValue != other.IdentifierValue)
		{
			return false;
		}
		if (PositiveIntValue != other.PositiveIntValue)
		{
			return false;
		}
		if (NegativeIntValue != other.NegativeIntValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DoubleValue, other.DoubleValue))
		{
			return false;
		}
		if (StringValue != other.StringValue)
		{
			return false;
		}
		if (AggregateValue != other.AggregateValue)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= name_.GetHashCode();
		if (HasIdentifierValue)
		{
			num ^= IdentifierValue.GetHashCode();
		}
		if (HasPositiveIntValue)
		{
			num ^= PositiveIntValue.GetHashCode();
		}
		if (HasNegativeIntValue)
		{
			num ^= NegativeIntValue.GetHashCode();
		}
		if (HasDoubleValue)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DoubleValue);
		}
		if (HasStringValue)
		{
			num ^= StringValue.GetHashCode();
		}
		if (HasAggregateValue)
		{
			num ^= AggregateValue.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
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
		name_.WriteTo(output, _repeated_name_codec);
		if (HasIdentifierValue)
		{
			output.WriteRawTag(26);
			output.WriteString(IdentifierValue);
		}
		if (HasPositiveIntValue)
		{
			output.WriteRawTag(32);
			output.WriteUInt64(PositiveIntValue);
		}
		if (HasNegativeIntValue)
		{
			output.WriteRawTag(40);
			output.WriteInt64(NegativeIntValue);
		}
		if (HasDoubleValue)
		{
			output.WriteRawTag(49);
			output.WriteDouble(DoubleValue);
		}
		if (HasStringValue)
		{
			output.WriteRawTag(58);
			output.WriteBytes(StringValue);
		}
		if (HasAggregateValue)
		{
			output.WriteRawTag(66);
			output.WriteString(AggregateValue);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += name_.CalculateSize(_repeated_name_codec);
		if (HasIdentifierValue)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(IdentifierValue);
		}
		if (HasPositiveIntValue)
		{
			num += 1 + CodedOutputStream.ComputeUInt64Size(PositiveIntValue);
		}
		if (HasNegativeIntValue)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(NegativeIntValue);
		}
		if (HasDoubleValue)
		{
			num += 9;
		}
		if (HasStringValue)
		{
			num += 1 + CodedOutputStream.ComputeBytesSize(StringValue);
		}
		if (HasAggregateValue)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AggregateValue);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UninterpretedOption other)
	{
		if (other != null)
		{
			name_.Add(other.name_);
			if (other.HasIdentifierValue)
			{
				IdentifierValue = other.IdentifierValue;
			}
			if (other.HasPositiveIntValue)
			{
				PositiveIntValue = other.PositiveIntValue;
			}
			if (other.HasNegativeIntValue)
			{
				NegativeIntValue = other.NegativeIntValue;
			}
			if (other.HasDoubleValue)
			{
				DoubleValue = other.DoubleValue;
			}
			if (other.HasStringValue)
			{
				StringValue = other.StringValue;
			}
			if (other.HasAggregateValue)
			{
				AggregateValue = other.AggregateValue;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 18u:
				name_.AddEntriesFrom(input, _repeated_name_codec);
				break;
			case 26u:
				IdentifierValue = input.ReadString();
				break;
			case 32u:
				PositiveIntValue = input.ReadUInt64();
				break;
			case 40u:
				NegativeIntValue = input.ReadInt64();
				break;
			case 49u:
				DoubleValue = input.ReadDouble();
				break;
			case 58u:
				StringValue = input.ReadBytes();
				break;
			case 66u:
				AggregateValue = input.ReadString();
				break;
			}
		}
	}
}
