using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Percent : IMessage<Percent>, IMessage, IEquatable<Percent>, IDeepCloneable<Percent>
{
	private static readonly MessageParser<Percent> _parser = new MessageParser<Percent>(() => new Percent());

	private UnknownFieldSet _unknownFields;

	public const int IntValueFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_intValue_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? intValue_;

	public const int DoubleValueFieldNumber = 2;

	private static readonly FieldCodec<double?> _single_doubleValue_codec = FieldCodec.ForStructWrapper<double>(18u);

	private double? doubleValue_;

	[DebuggerNonUserCode]
	public static MessageParser<Percent> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[15];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? IntValue
	{
		get
		{
			return intValue_;
		}
		set
		{
			intValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double? DoubleValue
	{
		get
		{
			return doubleValue_;
		}
		set
		{
			doubleValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Percent()
	{
	}

	[DebuggerNonUserCode]
	public Percent(Percent other)
		: this()
	{
		IntValue = other.IntValue;
		DoubleValue = other.DoubleValue;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Percent Clone()
	{
		return new Percent(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Percent);
	}

	[DebuggerNonUserCode]
	public bool Equals(Percent other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IntValue != other.IntValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseNullableDoubleEqualityComparer.Equals(DoubleValue, other.DoubleValue))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (intValue_.HasValue)
		{
			num ^= IntValue.GetHashCode();
		}
		if (doubleValue_.HasValue)
		{
			num ^= ProtobufEqualityComparers.BitwiseNullableDoubleEqualityComparer.GetHashCode(DoubleValue);
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
		if (intValue_.HasValue)
		{
			_single_intValue_codec.WriteTagAndValue(output, IntValue);
		}
		if (doubleValue_.HasValue)
		{
			_single_doubleValue_codec.WriteTagAndValue(output, DoubleValue);
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
		if (intValue_.HasValue)
		{
			num += _single_intValue_codec.CalculateSizeWithTag(IntValue);
		}
		if (doubleValue_.HasValue)
		{
			num += _single_doubleValue_codec.CalculateSizeWithTag(DoubleValue);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Percent other)
	{
		if (other != null)
		{
			if (other.intValue_.HasValue && (!intValue_.HasValue || other.IntValue != 0))
			{
				IntValue = other.IntValue;
			}
			if (other.doubleValue_.HasValue && (!doubleValue_.HasValue || other.DoubleValue != 0.0))
			{
				DoubleValue = other.DoubleValue;
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
				int? num3 = _single_intValue_codec.Read(input);
				if (!intValue_.HasValue || num3 != 0)
				{
					IntValue = num3;
				}
				break;
			}
			case 18u:
			{
				double? num2 = _single_doubleValue_codec.Read(input);
				if (!doubleValue_.HasValue || num2 != 0.0)
				{
					DoubleValue = num2;
				}
				break;
			}
			}
		}
	}
}
