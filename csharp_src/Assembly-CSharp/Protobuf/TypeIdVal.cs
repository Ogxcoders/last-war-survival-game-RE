using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class TypeIdVal : IMessage<TypeIdVal>, IMessage, IEquatable<TypeIdVal>, IDeepCloneable<TypeIdVal>
{
	private static readonly MessageParser<TypeIdVal> _parser = new MessageParser<TypeIdVal>(() => new TypeIdVal());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_type_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? type_;

	public const int IdFieldNumber = 2;

	private static readonly FieldCodec<string> _single_id_codec = FieldCodec.ForClassWrapper<string>(18u);

	private string id_;

	public const int ValueFieldNumber = 3;

	private static readonly FieldCodec<long?> _single_value_codec = FieldCodec.ForStructWrapper<long>(26u);

	private long? value_;

	[DebuggerNonUserCode]
	public static MessageParser<TypeIdVal> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[10];

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
	public string Id
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
	public long? Value
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
	public TypeIdVal()
	{
	}

	[DebuggerNonUserCode]
	public TypeIdVal(TypeIdVal other)
		: this()
	{
		Type = other.Type;
		Id = other.Id;
		Value = other.Value;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TypeIdVal Clone()
	{
		return new TypeIdVal(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TypeIdVal);
	}

	[DebuggerNonUserCode]
	public bool Equals(TypeIdVal other)
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
		if (Id != other.Id)
		{
			return false;
		}
		if (Value != other.Value)
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
		if (id_ != null)
		{
			num ^= Id.GetHashCode();
		}
		if (value_.HasValue)
		{
			num ^= Value.GetHashCode();
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
		if (id_ != null)
		{
			_single_id_codec.WriteTagAndValue(output, Id);
		}
		if (value_.HasValue)
		{
			_single_value_codec.WriteTagAndValue(output, Value);
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
		if (id_ != null)
		{
			num += _single_id_codec.CalculateSizeWithTag(Id);
		}
		if (value_.HasValue)
		{
			num += _single_value_codec.CalculateSizeWithTag(Value);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TypeIdVal other)
	{
		if (other != null)
		{
			if (other.type_.HasValue && (!type_.HasValue || other.Type != 0))
			{
				Type = other.Type;
			}
			if (other.id_ != null && (id_ == null || other.Id != ""))
			{
				Id = other.Id;
			}
			if (other.value_.HasValue && (!value_.HasValue || other.Value != 0))
			{
				Value = other.Value;
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
				int? num3 = _single_type_codec.Read(input);
				if (!type_.HasValue || num3 != 0)
				{
					Type = num3;
				}
				break;
			}
			case 18u:
			{
				string text = _single_id_codec.Read(input);
				if (id_ == null || text != "")
				{
					Id = text;
				}
				break;
			}
			case 26u:
			{
				long? num2 = _single_value_codec.Read(input);
				if (!value_.HasValue || num2 != 0)
				{
					Value = num2;
				}
				break;
			}
			}
		}
	}
}
