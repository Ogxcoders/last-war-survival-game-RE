using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class DescriptorProto : IMessage<DescriptorProto>, IMessage, IEquatable<DescriptorProto>, IDeepCloneable<DescriptorProto>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ExtensionRange : IMessage<ExtensionRange>, IMessage, IEquatable<ExtensionRange>, IDeepCloneable<ExtensionRange>
		{
			private static readonly MessageParser<ExtensionRange> _parser = new MessageParser<ExtensionRange>(() => new ExtensionRange());

			private UnknownFieldSet _unknownFields;

			private int _hasBits0;

			public const int StartFieldNumber = 1;

			private static readonly int StartDefaultValue = 0;

			private int start_;

			public const int EndFieldNumber = 2;

			private static readonly int EndDefaultValue = 0;

			private int end_;

			public const int OptionsFieldNumber = 3;

			private ExtensionRangeOptions options_;

			[DebuggerNonUserCode]
			public static MessageParser<ExtensionRange> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => DescriptorProto.Descriptor.NestedTypes[0];

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
			public ExtensionRangeOptions Options
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
			public ExtensionRange()
			{
			}

			[DebuggerNonUserCode]
			public ExtensionRange(ExtensionRange other)
				: this()
			{
				_hasBits0 = other._hasBits0;
				start_ = other.start_;
				end_ = other.end_;
				options_ = (other.HasOptions ? other.options_.Clone() : null);
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ExtensionRange Clone()
			{
				return new ExtensionRange(this);
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
			public void ClearOptions()
			{
				options_ = null;
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ExtensionRange);
			}

			[DebuggerNonUserCode]
			public bool Equals(ExtensionRange other)
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
				if (!object.Equals(Options, other.Options))
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
				if (HasOptions)
				{
					num ^= Options.GetHashCode();
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
				if (HasOptions)
				{
					output.WriteRawTag(26);
					output.WriteMessage(Options);
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
				if (HasOptions)
				{
					num += 1 + CodedOutputStream.ComputeMessageSize(Options);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ExtensionRange other)
			{
				if (other == null)
				{
					return;
				}
				if (other.HasStart)
				{
					Start = other.Start;
				}
				if (other.HasEnd)
				{
					End = other.End;
				}
				if (other.HasOptions)
				{
					if (!HasOptions)
					{
						Options = new ExtensionRangeOptions();
					}
					Options.MergeFrom(other.Options);
				}
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
					case 8u:
						Start = input.ReadInt32();
						break;
					case 16u:
						End = input.ReadInt32();
						break;
					case 26u:
						if (!HasOptions)
						{
							Options = new ExtensionRangeOptions();
						}
						input.ReadMessage(Options);
						break;
					}
				}
			}
		}

		public sealed class ReservedRange : IMessage<ReservedRange>, IMessage, IEquatable<ReservedRange>, IDeepCloneable<ReservedRange>
		{
			private static readonly MessageParser<ReservedRange> _parser = new MessageParser<ReservedRange>(() => new ReservedRange());

			private UnknownFieldSet _unknownFields;

			private int _hasBits0;

			public const int StartFieldNumber = 1;

			private static readonly int StartDefaultValue = 0;

			private int start_;

			public const int EndFieldNumber = 2;

			private static readonly int EndDefaultValue = 0;

			private int end_;

			[DebuggerNonUserCode]
			public static MessageParser<ReservedRange> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => DescriptorProto.Descriptor.NestedTypes[1];

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
			public ReservedRange()
			{
			}

			[DebuggerNonUserCode]
			public ReservedRange(ReservedRange other)
				: this()
			{
				_hasBits0 = other._hasBits0;
				start_ = other.start_;
				end_ = other.end_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ReservedRange Clone()
			{
				return new ReservedRange(this);
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
				return Equals(other as ReservedRange);
			}

			[DebuggerNonUserCode]
			public bool Equals(ReservedRange other)
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
			public void MergeFrom(ReservedRange other)
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

	private static readonly MessageParser<DescriptorProto> _parser = new MessageParser<DescriptorProto>(() => new DescriptorProto());

	private UnknownFieldSet _unknownFields;

	public const int NameFieldNumber = 1;

	private static readonly string NameDefaultValue = "";

	private string name_;

	public const int FieldFieldNumber = 2;

	private static readonly FieldCodec<FieldDescriptorProto> _repeated_field_codec = FieldCodec.ForMessage(18u, FieldDescriptorProto.Parser);

	private readonly RepeatedField<FieldDescriptorProto> field_ = new RepeatedField<FieldDescriptorProto>();

	public const int ExtensionFieldNumber = 6;

	private static readonly FieldCodec<FieldDescriptorProto> _repeated_extension_codec = FieldCodec.ForMessage(50u, FieldDescriptorProto.Parser);

	private readonly RepeatedField<FieldDescriptorProto> extension_ = new RepeatedField<FieldDescriptorProto>();

	public const int NestedTypeFieldNumber = 3;

	private static readonly FieldCodec<DescriptorProto> _repeated_nestedType_codec = FieldCodec.ForMessage(26u, Parser);

	private readonly RepeatedField<DescriptorProto> nestedType_ = new RepeatedField<DescriptorProto>();

	public const int EnumTypeFieldNumber = 4;

	private static readonly FieldCodec<EnumDescriptorProto> _repeated_enumType_codec = FieldCodec.ForMessage(34u, EnumDescriptorProto.Parser);

	private readonly RepeatedField<EnumDescriptorProto> enumType_ = new RepeatedField<EnumDescriptorProto>();

	public const int ExtensionRangeFieldNumber = 5;

	private static readonly FieldCodec<Types.ExtensionRange> _repeated_extensionRange_codec = FieldCodec.ForMessage(42u, Types.ExtensionRange.Parser);

	private readonly RepeatedField<Types.ExtensionRange> extensionRange_ = new RepeatedField<Types.ExtensionRange>();

	public const int OneofDeclFieldNumber = 8;

	private static readonly FieldCodec<OneofDescriptorProto> _repeated_oneofDecl_codec = FieldCodec.ForMessage(66u, OneofDescriptorProto.Parser);

	private readonly RepeatedField<OneofDescriptorProto> oneofDecl_ = new RepeatedField<OneofDescriptorProto>();

	public const int OptionsFieldNumber = 7;

	private MessageOptions options_;

	public const int ReservedRangeFieldNumber = 9;

	private static readonly FieldCodec<Types.ReservedRange> _repeated_reservedRange_codec = FieldCodec.ForMessage(74u, Types.ReservedRange.Parser);

	private readonly RepeatedField<Types.ReservedRange> reservedRange_ = new RepeatedField<Types.ReservedRange>();

	public const int ReservedNameFieldNumber = 10;

	private static readonly FieldCodec<string> _repeated_reservedName_codec = FieldCodec.ForString(82u);

	private readonly RepeatedField<string> reservedName_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<DescriptorProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[2];

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
	public RepeatedField<FieldDescriptorProto> Field => field_;

	[DebuggerNonUserCode]
	public RepeatedField<FieldDescriptorProto> Extension => extension_;

	[DebuggerNonUserCode]
	public RepeatedField<DescriptorProto> NestedType => nestedType_;

	[DebuggerNonUserCode]
	public RepeatedField<EnumDescriptorProto> EnumType => enumType_;

	[DebuggerNonUserCode]
	public RepeatedField<Types.ExtensionRange> ExtensionRange => extensionRange_;

	[DebuggerNonUserCode]
	public RepeatedField<OneofDescriptorProto> OneofDecl => oneofDecl_;

	[DebuggerNonUserCode]
	public MessageOptions Options
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
	public RepeatedField<Types.ReservedRange> ReservedRange => reservedRange_;

	[DebuggerNonUserCode]
	public RepeatedField<string> ReservedName => reservedName_;

	[DebuggerNonUserCode]
	public DescriptorProto()
	{
	}

	[DebuggerNonUserCode]
	public DescriptorProto(DescriptorProto other)
		: this()
	{
		name_ = other.name_;
		field_ = other.field_.Clone();
		extension_ = other.extension_.Clone();
		nestedType_ = other.nestedType_.Clone();
		enumType_ = other.enumType_.Clone();
		extensionRange_ = other.extensionRange_.Clone();
		oneofDecl_ = other.oneofDecl_.Clone();
		options_ = (other.HasOptions ? other.options_.Clone() : null);
		reservedRange_ = other.reservedRange_.Clone();
		reservedName_ = other.reservedName_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DescriptorProto Clone()
	{
		return new DescriptorProto(this);
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
		return Equals(other as DescriptorProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(DescriptorProto other)
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
		if (!field_.Equals(other.field_))
		{
			return false;
		}
		if (!extension_.Equals(other.extension_))
		{
			return false;
		}
		if (!nestedType_.Equals(other.nestedType_))
		{
			return false;
		}
		if (!enumType_.Equals(other.enumType_))
		{
			return false;
		}
		if (!extensionRange_.Equals(other.extensionRange_))
		{
			return false;
		}
		if (!oneofDecl_.Equals(other.oneofDecl_))
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
		num ^= field_.GetHashCode();
		num ^= extension_.GetHashCode();
		num ^= nestedType_.GetHashCode();
		num ^= enumType_.GetHashCode();
		num ^= extensionRange_.GetHashCode();
		num ^= oneofDecl_.GetHashCode();
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
		field_.WriteTo(output, _repeated_field_codec);
		nestedType_.WriteTo(output, _repeated_nestedType_codec);
		enumType_.WriteTo(output, _repeated_enumType_codec);
		extensionRange_.WriteTo(output, _repeated_extensionRange_codec);
		extension_.WriteTo(output, _repeated_extension_codec);
		if (HasOptions)
		{
			output.WriteRawTag(58);
			output.WriteMessage(Options);
		}
		oneofDecl_.WriteTo(output, _repeated_oneofDecl_codec);
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
		num += field_.CalculateSize(_repeated_field_codec);
		num += extension_.CalculateSize(_repeated_extension_codec);
		num += nestedType_.CalculateSize(_repeated_nestedType_codec);
		num += enumType_.CalculateSize(_repeated_enumType_codec);
		num += extensionRange_.CalculateSize(_repeated_extensionRange_codec);
		num += oneofDecl_.CalculateSize(_repeated_oneofDecl_codec);
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
	public void MergeFrom(DescriptorProto other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HasName)
		{
			Name = other.Name;
		}
		field_.Add(other.field_);
		extension_.Add(other.extension_);
		nestedType_.Add(other.nestedType_);
		enumType_.Add(other.enumType_);
		extensionRange_.Add(other.extensionRange_);
		oneofDecl_.Add(other.oneofDecl_);
		if (other.HasOptions)
		{
			if (!HasOptions)
			{
				Options = new MessageOptions();
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
				field_.AddEntriesFrom(input, _repeated_field_codec);
				break;
			case 26u:
				nestedType_.AddEntriesFrom(input, _repeated_nestedType_codec);
				break;
			case 34u:
				enumType_.AddEntriesFrom(input, _repeated_enumType_codec);
				break;
			case 42u:
				extensionRange_.AddEntriesFrom(input, _repeated_extensionRange_codec);
				break;
			case 50u:
				extension_.AddEntriesFrom(input, _repeated_extension_codec);
				break;
			case 58u:
				if (!HasOptions)
				{
					Options = new MessageOptions();
				}
				input.ReadMessage(Options);
				break;
			case 66u:
				oneofDecl_.AddEntriesFrom(input, _repeated_oneofDecl_codec);
				break;
			case 74u:
				reservedRange_.AddEntriesFrom(input, _repeated_reservedRange_codec);
				break;
			case 82u:
				reservedName_.AddEntriesFrom(input, _repeated_reservedName_codec);
				break;
			}
		}
	}
}
