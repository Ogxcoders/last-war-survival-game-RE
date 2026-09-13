using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class EnumDescriptorProto : IMessage<EnumDescriptorProto>, IMessage, IEquatable<EnumDescriptorProto>, IDeepCloneable<EnumDescriptorProto>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class EnumReservedRange : IMessage<EnumReservedRange>, IMessage, IEquatable<EnumReservedRange>, IDeepCloneable<EnumReservedRange>
		{
			private static readonly MessageParser<EnumReservedRange> _parser = new MessageParser<EnumReservedRange>(() => new EnumReservedRange());

			private UnknownFieldSet _unknownFields;

			private int _hasBits0;

			public const int StartFieldNumber = 1;

			private static readonly int StartDefaultValue = 0;

			private int start_;

			public const int EndFieldNumber = 2;

			private static readonly int EndDefaultValue = 0;

			private int end_;

			[DebuggerNonUserCode]
			public static MessageParser<EnumReservedRange> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnumDescriptorProto.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => Descriptor;

			[DebuggerNonUserCode]
			public int Start
			{
				get
				{
					if ((_hasBits0 & 1) != 0)
					{
						return start_;
					}
					return StartDefaultValue;
				}
				set
				{
					_hasBits0 |= 1;
					start_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool HasStart => (_hasBits0 & 1) != 0;

			[DebuggerNonUserCode]
			public int End
			{
				get
				{
					if ((_hasBits0 & 2) != 0)
					{
						return end_;
					}
					return EndDefaultValue;
				}
				set
				{
					_hasBits0 |= 2;
					end_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool HasEnd => (_hasBits0 & 2) != 0;

			[DebuggerNonUserCode]
			public EnumReservedRange()
			{
			}

			[DebuggerNonUserCode]
			public EnumReservedRange(EnumReservedRange other)
				: this()
			{
				_hasBits0 = other._hasBits0;
				start_ = other.start_;
				end_ = other.end_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public EnumReservedRange Clone()
			{
				return new EnumReservedRange(this);
			}

			[DebuggerNonUserCode]
			public void ClearStart()
			{
				_hasBits0 &= -2;
			}

			[DebuggerNonUserCode]
			public void ClearEnd()
			{
				_hasBits0 &= -3;
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as EnumReservedRange);
			}

			[DebuggerNonUserCode]
			public bool Equals(EnumReservedRange other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Start != other.Start)
				{
					return false;
				}
				if (End != other.End)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (HasStart)
				{
					num ^= Start.GetHashCode();
				}
				if (HasEnd)
				{
					num ^= End.GetHashCode();
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
				if (HasStart)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Start);
				}
				if (HasEnd)
				{
					output.WriteRawTag(16);
					output.WriteInt32(End);
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
				if (HasStart)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Start);
				}
				if (HasEnd)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(End);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(EnumReservedRange other)
			{
				if (other != null)
				{
					if (other.HasStart)
					{
						Start = other.Start;
					}
					if (other.HasEnd)
					{
						End = other.End;
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
					case 8u:
						Start = input.ReadInt32();
						break;
					case 16u:
						End = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnumDescriptorProto> _parser = new MessageParser<EnumDescriptorProto>(() => new EnumDescriptorProto());

	private UnknownFieldSet _unknownFields;

	public const int NameFieldNumber = 1;

	private static readonly string NameDefaultValue = "";

	private string name_;

	public const int ValueFieldNumber = 2;

	private static readonly FieldCodec<EnumValueDescriptorProto> _repeated_value_codec = FieldCodec.ForMessage(18u, EnumValueDescriptorProto.Parser);

	private readonly RepeatedField<EnumValueDescriptorProto> value_ = new RepeatedField<EnumValueDescriptorProto>();

	public const int OptionsFieldNumber = 3;

	private EnumOptions options_;

	public const int ReservedRangeFieldNumber = 4;

	private static readonly FieldCodec<Types.EnumReservedRange> _repeated_reservedRange_codec = FieldCodec.ForMessage(34u, Types.EnumReservedRange.Parser);

	private readonly RepeatedField<Types.EnumReservedRange> reservedRange_ = new RepeatedField<Types.EnumReservedRange>();

	public const int ReservedNameFieldNumber = 5;

	private static readonly FieldCodec<string> _repeated_reservedName_codec = FieldCodec.ForString(42u);

	private readonly RepeatedField<string> reservedName_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<EnumDescriptorProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_ ?? NameDefaultValue;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasName => name_ != null;

	[DebuggerNonUserCode]
	public RepeatedField<EnumValueDescriptorProto> Value => value_;

	[DebuggerNonUserCode]
	public EnumOptions Options
	{
		get
		{
			return options_;
		}
		set
		{
			options_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasOptions => options_ != null;

	[DebuggerNonUserCode]
	public RepeatedField<Types.EnumReservedRange> ReservedRange => reservedRange_;

	[DebuggerNonUserCode]
	public RepeatedField<string> ReservedName => reservedName_;

	[DebuggerNonUserCode]
	public EnumDescriptorProto()
	{
	}

	[DebuggerNonUserCode]
	public EnumDescriptorProto(EnumDescriptorProto other)
		: this()
	{
		name_ = other.name_;
		value_ = other.value_.Clone();
		options_ = (other.HasOptions ? other.options_.Clone() : null);
		reservedRange_ = other.reservedRange_.Clone();
		reservedName_ = other.reservedName_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnumDescriptorProto Clone()
	{
		return new EnumDescriptorProto(this);
	}

	[DebuggerNonUserCode]
	public void ClearName()
	{
		name_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearOptions()
	{
		options_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnumDescriptorProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnumDescriptorProto other)
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
		if (!value_.Equals(other.value_))
		{
			return false;
		}
		if (!object.Equals(Options, other.Options))
		{
			return false;
		}
		if (!reservedRange_.Equals(other.reservedRange_))
		{
			return false;
		}
		if (!reservedName_.Equals(other.reservedName_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HasName)
		{
			num ^= Name.GetHashCode();
		}
		num ^= value_.GetHashCode();
		if (HasOptions)
		{
			num ^= Options.GetHashCode();
		}
		num ^= reservedRange_.GetHashCode();
		num ^= reservedName_.GetHashCode();
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
		if (HasName)
		{
			output.WriteRawTag(10);
			output.WriteString(Name);
		}
		value_.WriteTo(output, _repeated_value_codec);
		if (HasOptions)
		{
			output.WriteRawTag(26);
			output.WriteMessage(Options);
		}
		reservedRange_.WriteTo(output, _repeated_reservedRange_codec);
		reservedName_.WriteTo(output, _repeated_reservedName_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (HasName)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		num += value_.CalculateSize(_repeated_value_codec);
		if (HasOptions)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Options);
		}
		num += reservedRange_.CalculateSize(_repeated_reservedRange_codec);
		num += reservedName_.CalculateSize(_repeated_reservedName_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnumDescriptorProto other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HasName)
		{
			Name = other.Name;
		}
		value_.Add(other.value_);
		if (other.HasOptions)
		{
			if (!HasOptions)
			{
				Options = new EnumOptions();
			}
			Options.MergeFrom(other.Options);
		}
		reservedRange_.Add(other.reservedRange_);
		reservedName_.Add(other.reservedName_);
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				Name = input.ReadString();
				break;
			case 18u:
				value_.AddEntriesFrom(input, _repeated_value_codec);
				break;
			case 26u:
				if (!HasOptions)
				{
					Options = new EnumOptions();
				}
				input.ReadMessage(Options);
				break;
			case 34u:
				reservedRange_.AddEntriesFrom(input, _repeated_reservedRange_codec);
				break;
			case 42u:
				reservedName_.AddEntriesFrom(input, _repeated_reservedName_codec);
				break;
			}
		}
	}
}
