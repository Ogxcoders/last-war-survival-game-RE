using System;
using System.Diagnostics;

namespace Google.Protobuf.Reflection;

public sealed class FieldDescriptorProto : IMessage<FieldDescriptorProto>, IMessage, IEquatable<FieldDescriptorProto>, IDeepCloneable<FieldDescriptorProto>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public enum Type
		{
			[OriginalName("TYPE_DOUBLE")]
			Double = 1,
			[OriginalName("TYPE_FLOAT")]
			Float,
			[OriginalName("TYPE_INT64")]
			Int64,
			[OriginalName("TYPE_UINT64")]
			Uint64,
			[OriginalName("TYPE_INT32")]
			Int32,
			[OriginalName("TYPE_FIXED64")]
			Fixed64,
			[OriginalName("TYPE_FIXED32")]
			Fixed32,
			[OriginalName("TYPE_BOOL")]
			Bool,
			[OriginalName("TYPE_STRING")]
			String,
			[OriginalName("TYPE_GROUP")]
			Group,
			[OriginalName("TYPE_MESSAGE")]
			Message,
			[OriginalName("TYPE_BYTES")]
			Bytes,
			[OriginalName("TYPE_UINT32")]
			Uint32,
			[OriginalName("TYPE_ENUM")]
			Enum,
			[OriginalName("TYPE_SFIXED32")]
			Sfixed32,
			[OriginalName("TYPE_SFIXED64")]
			Sfixed64,
			[OriginalName("TYPE_SINT32")]
			Sint32,
			[OriginalName("TYPE_SINT64")]
			Sint64
		}

		public enum Label
		{
			[OriginalName("LABEL_OPTIONAL")]
			Optional = 1,
			[OriginalName("LABEL_REQUIRED")]
			Required,
			[OriginalName("LABEL_REPEATED")]
			Repeated
		}
	}

	private static readonly MessageParser<FieldDescriptorProto> _parser = new MessageParser<FieldDescriptorProto>(() => new FieldDescriptorProto());

	private UnknownFieldSet _unknownFields;

	private int _hasBits0;

	public const int NameFieldNumber = 1;

	private static readonly string NameDefaultValue = "";

	private string name_;

	public const int NumberFieldNumber = 3;

	private static readonly int NumberDefaultValue = 0;

	private int number_;

	public const int LabelFieldNumber = 4;

	private static readonly Types.Label LabelDefaultValue = Types.Label.Optional;

	private Types.Label label_;

	public const int TypeFieldNumber = 5;

	private static readonly Types.Type TypeDefaultValue = Types.Type.Double;

	private Types.Type type_;

	public const int TypeNameFieldNumber = 6;

	private static readonly string TypeNameDefaultValue = "";

	private string typeName_;

	public const int ExtendeeFieldNumber = 2;

	private static readonly string ExtendeeDefaultValue = "";

	private string extendee_;

	public const int DefaultValueFieldNumber = 7;

	private static readonly string DefaultValueDefaultValue = "";

	private string defaultValue_;

	public const int OneofIndexFieldNumber = 9;

	private static readonly int OneofIndexDefaultValue = 0;

	private int oneofIndex_;

	public const int JsonNameFieldNumber = 10;

	private static readonly string JsonNameDefaultValue = "";

	private string jsonName_;

	public const int OptionsFieldNumber = 8;

	private FieldOptions options_;

	[DebuggerNonUserCode]
	public static MessageParser<FieldDescriptorProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[4];

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
	public int Number
	{
		get
		{
			if ((_hasBits0 & 1) != 0)
			{
				return number_;
			}
			return NumberDefaultValue;
		}
		set
		{
			_hasBits0 |= 1;
			number_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasNumber => (_hasBits0 & 1) != 0;

	[DebuggerNonUserCode]
	public Types.Label Label
	{
		get
		{
			if ((_hasBits0 & 2) != 0)
			{
				return label_;
			}
			return LabelDefaultValue;
		}
		set
		{
			_hasBits0 |= 2;
			label_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasLabel => (_hasBits0 & 2) != 0;

	[DebuggerNonUserCode]
	public Types.Type Type
	{
		get
		{
			if ((_hasBits0 & 4) != 0)
			{
				return type_;
			}
			return TypeDefaultValue;
		}
		set
		{
			_hasBits0 |= 4;
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasType => (_hasBits0 & 4) != 0;

	[DebuggerNonUserCode]
	public string TypeName
	{
		get
		{
			return typeName_ ?? TypeNameDefaultValue;
		}
		set
		{
			typeName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasTypeName => typeName_ != null;

	[DebuggerNonUserCode]
	public string Extendee
	{
		get
		{
			return extendee_ ?? ExtendeeDefaultValue;
		}
		set
		{
			extendee_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasExtendee => extendee_ != null;

	[DebuggerNonUserCode]
	public string DefaultValue
	{
		get
		{
			return defaultValue_ ?? DefaultValueDefaultValue;
		}
		set
		{
			defaultValue_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasDefaultValue => defaultValue_ != null;

	[DebuggerNonUserCode]
	public int OneofIndex
	{
		get
		{
			if ((_hasBits0 & 8) != 0)
			{
				return oneofIndex_;
			}
			return OneofIndexDefaultValue;
		}
		set
		{
			_hasBits0 |= 8;
			oneofIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasOneofIndex => (_hasBits0 & 8) != 0;

	[DebuggerNonUserCode]
	public string JsonName
	{
		get
		{
			return jsonName_ ?? JsonNameDefaultValue;
		}
		set
		{
			jsonName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasJsonName => jsonName_ != null;

	[DebuggerNonUserCode]
	public FieldOptions Options
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
	public FieldDescriptorProto()
	{
	}

	[DebuggerNonUserCode]
	public FieldDescriptorProto(FieldDescriptorProto other)
		: this()
	{
		_hasBits0 = other._hasBits0;
		name_ = other.name_;
		number_ = other.number_;
		label_ = other.label_;
		type_ = other.type_;
		typeName_ = other.typeName_;
		extendee_ = other.extendee_;
		defaultValue_ = other.defaultValue_;
		oneofIndex_ = other.oneofIndex_;
		jsonName_ = other.jsonName_;
		options_ = (other.HasOptions ? other.options_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FieldDescriptorProto Clone()
	{
		return new FieldDescriptorProto(this);
	}

	[DebuggerNonUserCode]
	public void ClearName()
	{
		name_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearNumber()
	{
		_hasBits0 &= -2;
	}

	[DebuggerNonUserCode]
	public void ClearLabel()
	{
		_hasBits0 &= -3;
	}

	[DebuggerNonUserCode]
	public void ClearType()
	{
		_hasBits0 &= -5;
	}

	[DebuggerNonUserCode]
	public void ClearTypeName()
	{
		typeName_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearExtendee()
	{
		extendee_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearDefaultValue()
	{
		defaultValue_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearOneofIndex()
	{
		_hasBits0 &= -9;
	}

	[DebuggerNonUserCode]
	public void ClearJsonName()
	{
		jsonName_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearOptions()
	{
		options_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FieldDescriptorProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(FieldDescriptorProto other)
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
		if (Number != other.Number)
		{
			return false;
		}
		if (Label != other.Label)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (TypeName != other.TypeName)
		{
			return false;
		}
		if (Extendee != other.Extendee)
		{
			return false;
		}
		if (DefaultValue != other.DefaultValue)
		{
			return false;
		}
		if (OneofIndex != other.OneofIndex)
		{
			return false;
		}
		if (JsonName != other.JsonName)
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
		if (HasName)
		{
			num ^= Name.GetHashCode();
		}
		if (HasNumber)
		{
			num ^= Number.GetHashCode();
		}
		if (HasLabel)
		{
			num ^= Label.GetHashCode();
		}
		if (HasType)
		{
			num ^= Type.GetHashCode();
		}
		if (HasTypeName)
		{
			num ^= TypeName.GetHashCode();
		}
		if (HasExtendee)
		{
			num ^= Extendee.GetHashCode();
		}
		if (HasDefaultValue)
		{
			num ^= DefaultValue.GetHashCode();
		}
		if (HasOneofIndex)
		{
			num ^= OneofIndex.GetHashCode();
		}
		if (HasJsonName)
		{
			num ^= JsonName.GetHashCode();
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
		if (HasName)
		{
			output.WriteRawTag(10);
			output.WriteString(Name);
		}
		if (HasExtendee)
		{
			output.WriteRawTag(18);
			output.WriteString(Extendee);
		}
		if (HasNumber)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Number);
		}
		if (HasLabel)
		{
			output.WriteRawTag(32);
			output.WriteEnum((int)Label);
		}
		if (HasType)
		{
			output.WriteRawTag(40);
			output.WriteEnum((int)Type);
		}
		if (HasTypeName)
		{
			output.WriteRawTag(50);
			output.WriteString(TypeName);
		}
		if (HasDefaultValue)
		{
			output.WriteRawTag(58);
			output.WriteString(DefaultValue);
		}
		if (HasOptions)
		{
			output.WriteRawTag(66);
			output.WriteMessage(Options);
		}
		if (HasOneofIndex)
		{
			output.WriteRawTag(72);
			output.WriteInt32(OneofIndex);
		}
		if (HasJsonName)
		{
			output.WriteRawTag(82);
			output.WriteString(JsonName);
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
		if (HasName)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (HasNumber)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Number);
		}
		if (HasLabel)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)Label);
		}
		if (HasType)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
		}
		if (HasTypeName)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TypeName);
		}
		if (HasExtendee)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Extendee);
		}
		if (HasDefaultValue)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DefaultValue);
		}
		if (HasOneofIndex)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OneofIndex);
		}
		if (HasJsonName)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(JsonName);
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
	public void MergeFrom(FieldDescriptorProto other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HasName)
		{
			Name = other.Name;
		}
		if (other.HasNumber)
		{
			Number = other.Number;
		}
		if (other.HasLabel)
		{
			Label = other.Label;
		}
		if (other.HasType)
		{
			Type = other.Type;
		}
		if (other.HasTypeName)
		{
			TypeName = other.TypeName;
		}
		if (other.HasExtendee)
		{
			Extendee = other.Extendee;
		}
		if (other.HasDefaultValue)
		{
			DefaultValue = other.DefaultValue;
		}
		if (other.HasOneofIndex)
		{
			OneofIndex = other.OneofIndex;
		}
		if (other.HasJsonName)
		{
			JsonName = other.JsonName;
		}
		if (other.HasOptions)
		{
			if (!HasOptions)
			{
				Options = new FieldOptions();
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
			case 10u:
				Name = input.ReadString();
				break;
			case 18u:
				Extendee = input.ReadString();
				break;
			case 24u:
				Number = input.ReadInt32();
				break;
			case 32u:
				Label = (Types.Label)input.ReadEnum();
				break;
			case 40u:
				Type = (Types.Type)input.ReadEnum();
				break;
			case 50u:
				TypeName = input.ReadString();
				break;
			case 58u:
				DefaultValue = input.ReadString();
				break;
			case 66u:
				if (!HasOptions)
				{
					Options = new FieldOptions();
				}
				input.ReadMessage(Options);
				break;
			case 72u:
				OneofIndex = input.ReadInt32();
				break;
			case 82u:
				JsonName = input.ReadString();
				break;
			}
		}
	}
}
