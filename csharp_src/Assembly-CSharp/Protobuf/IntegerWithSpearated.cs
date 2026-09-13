using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class IntegerWithSpearated : IMessage<IntegerWithSpearated>, IMessage, IEquatable<IntegerWithSpearated>, IDeepCloneable<IntegerWithSpearated>
{
	private static readonly MessageParser<IntegerWithSpearated> _parser = new MessageParser<IntegerWithSpearated>(() => new IntegerWithSpearated());

	private UnknownFieldSet _unknownFields;

	public const int ValueFieldNumber = 1;

	private static readonly FieldCodec<long?> _single_value_codec = FieldCodec.ForStructWrapper<long>(10u);

	private long? value_;

	[DebuggerNonUserCode]
	public static MessageParser<IntegerWithSpearated> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[30];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public IntegerWithSpearated()
	{
	}

	[DebuggerNonUserCode]
	public IntegerWithSpearated(IntegerWithSpearated other)
		: this()
	{
		Value = other.Value;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntegerWithSpearated Clone()
	{
		return new IntegerWithSpearated(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntegerWithSpearated);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntegerWithSpearated other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
	public void MergeFrom(IntegerWithSpearated other)
	{
		if (other != null)
		{
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				continue;
			}
			long? num2 = _single_value_codec.Read(input);
			if (!value_.HasValue || num2 != 0)
			{
				Value = num2;
			}
		}
	}
}
