using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Resource : IMessage<Resource>, IMessage, IEquatable<Resource>, IDeepCloneable<Resource>
{
	private static readonly MessageParser<Resource> _parser = new MessageParser<Resource>(() => new Resource());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_type_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? type_;

	public const int ValueFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_value_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? value_;

	public const int IdFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_id_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? id_;

	public const int NewValueFieldNumber = 4;

	private static readonly FieldCodec<long?> _single_newValue_codec = FieldCodec.ForStructWrapper<long>(34u);

	private long? newValue_;

	[DebuggerNonUserCode]
	public static MessageParser<Resource> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Value
	{
		get
		{
			return value_;
		}
		set
		{
			value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Id
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
	public long? NewValue
	{
		get
		{
			return newValue_;
		}
		set
		{
			newValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Resource()
	{
	}

	[DebuggerNonUserCode]
	public Resource(Resource other)
		: this()
	{
		Type = other.Type;
		Value = other.Value;
		Id = other.Id;
		NewValue = other.NewValue;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Resource Clone()
	{
		return new Resource(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Resource);
	}

	[DebuggerNonUserCode]
	public bool Equals(Resource other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (Value != other.Value)
		{
			return false;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (NewValue != other.NewValue)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (type_.HasValue)
		{
			num ^= Type.GetHashCode();
		}
		if (value_.HasValue)
		{
			num ^= Value.GetHashCode();
		}
		if (id_.HasValue)
		{
			num ^= Id.GetHashCode();
		}
		if (newValue_.HasValue)
		{
			num ^= NewValue.GetHashCode();
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
		if (type_.HasValue)
		{
			_single_type_codec.WriteTagAndValue(output, Type);
		}
		if (value_.HasValue)
		{
			_single_value_codec.WriteTagAndValue(output, Value);
		}
		if (id_.HasValue)
		{
			_single_id_codec.WriteTagAndValue(output, Id);
		}
		if (newValue_.HasValue)
		{
			_single_newValue_codec.WriteTagAndValue(output, NewValue);
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
		if (type_.HasValue)
		{
			num += _single_type_codec.CalculateSizeWithTag(Type);
		}
		if (value_.HasValue)
		{
			num += _single_value_codec.CalculateSizeWithTag(Value);
		}
		if (id_.HasValue)
		{
			num += _single_id_codec.CalculateSizeWithTag(Id);
		}
		if (newValue_.HasValue)
		{
			num += _single_newValue_codec.CalculateSizeWithTag(NewValue);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Resource other)
	{
		if (other != null)
		{
			if (other.type_.HasValue && (!type_.HasValue || other.Type != 0))
			{
				Type = other.Type;
			}
			if (other.value_.HasValue && (!value_.HasValue || other.Value != 0))
			{
				Value = other.Value;
			}
			if (other.id_.HasValue && (!id_.HasValue || other.Id != 0))
			{
				Id = other.Id;
			}
			if (other.newValue_.HasValue && (!newValue_.HasValue || other.NewValue != 0))
			{
				NewValue = other.NewValue;
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
			{
				int? num5 = _single_type_codec.Read(input);
				if (!type_.HasValue || num5 != 0)
				{
					Type = num5;
				}
				break;
			}
			case 18u:
			{
				int? num3 = _single_value_codec.Read(input);
				if (!value_.HasValue || num3 != 0)
				{
					Value = num3;
				}
				break;
			}
			case 26u:
			{
				int? num4 = _single_id_codec.Read(input);
				if (!id_.HasValue || num4 != 0)
				{
					Id = num4;
				}
				break;
			}
			case 34u:
			{
				long? num2 = _single_newValue_codec.Read(input);
				if (!newValue_.HasValue || num2 != 0)
				{
					NewValue = num2;
				}
				break;
			}
			}
		}
	}
}
